// Unity MIDI Input plug-in / C# interface
// By Keijiro Takahashi, 2013
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MidiInput : MonoBehaviour
{
    #region Static interface
    // Common instance.
    static MidiInput instance;
    
    // Filter mode IDs.
    public enum Filter {
        Realtime,
        Fast,
        Slow
    }
    
    // Provides the channel list.
    public static int[] Channels {
        get {
            int[] channels = new int[instance.knobs.Count];
            instance.knobs.Keys.CopyTo (channels, 0);
            return channels;
        }
    }
    
    // Get the CC value.
    public static float GetKnob (int channel, Filter filter = Filter.Realtime)
    {
        if (instance.knobs.ContainsKey (channel)) {
            return instance.knobs [channel].filteredValues [(int)filter];
        } else {
            return 0.0f;
        }
    }
    #endregion

    #region MIDI message sturcture
    public struct MidiMessage
    {
        // MIDI source (endpoint) ID.
        public uint source;
        
        // MIDI status byte.
        public byte status;
        
        // MIDI data bytes.
        public byte data1;
        public byte data2;
        
        public MidiMessage (ulong data)
        {
            source = (uint)(data & 0xffffffffUL);
            status = (byte)((data >> 32) & 0xff);
            data1 = (byte)((data >> 40) & 0xff);
            data2 = (byte)((data >> 48) & 0xff);
        }
        
        public override string ToString ()
        {
            return string.Format ("s({0:X2}) d({1:X2},{2:X2}) from {3:X8}", status, data1, data2, source);
        }
    }
    #endregion

    #region Internal data structure
    // CC channel (knob) information.
    class Knob
    {
        public float[] filteredValues;

        public Knob (float initial)
        {
            filteredValues = new float[3];
            filteredValues [0] = filteredValues [1] = filteredValues [2] = initial;
        }

        public void Update (float value)
        {
            filteredValues [0] = value;
        }

        public void UpdateFilter (float fastFilterCoeff, float slowFilterCoeff)
        {
            filteredValues [1] = filteredValues [0] - (filteredValues [0] - filteredValues [1]) * fastFilterCoeff;
            filteredValues [2] = filteredValues [0] - (filteredValues [0] - filteredValues [2]) * slowFilterCoeff;
        }
    }

    // Channel number to knob mapping.
    Dictionary<int, Knob> knobs;
    #endregion

    #region Editor supports
    #if UNITY_EDITOR
    // Incoming message history.
    Queue<MidiMessage> messageHistory;
    public Queue<MidiMessage> History {
        get { return messageHistory; }
    }
    #endif
    #endregion

    #region Public properties
    public float sensibilityFast = 20.0f;
    public float sensibilitySlow = 8.0f;
    #endregion

    #region Monobehaviour functions
    void Awake ()
    {
        instance = this;
        knobs = new Dictionary<int, Knob> ();
        #if UNITY_EDITOR
        messageHistory = new Queue<MidiMessage> ();
        #endif
    }

    void Update ()
    {
        // Calculate the filter coefficients.
        var fastFilterCoeff = Mathf.Exp (-sensibilityFast * Time.deltaTime);
        var slowFilterCoeff = Mathf.Exp (-sensibilitySlow * Time.deltaTime);

        // Update the filtered value.
        foreach (var k in knobs.Values) {
            k.UpdateFilter (fastFilterCoeff, slowFilterCoeff);
        }

        // Process the message queue.
        while (true) {
            // Pop from the queue.
            var data = DequeueIncomingData ();
            if (data == 0) {
                break;
            }

            // Parse the message.
            var message = new MidiMessage (data);

            // CC message?
            if (message.status == 0xb0) {
                // Normalize the value.
                var value = 1.0f / 127 * message.data2;

                // Update the channel if it already exists, or add a new channel.
                if (knobs.ContainsKey (message.data1)) {
                    knobs [message.data1].Update (value);
                } else {
                    knobs [message.data1] = new Knob (value);
                }
            }

            #if UNITY_EDITOR
            // Record the message history.
            messageHistory.Enqueue (message);
            #endif
        }

        #if UNITY_EDITOR
        // Truncate the history.
        while (messageHistory.Count > 8) {
            messageHistory.Dequeue ();
        }
        #endif
    }
    #endregion

    #region Native module interface
    [DllImport ("UnityMIDIReceiver", EntryPoint="UnityMIDIReceiver_CountEndpoints")]
    public static extern int CountEndpoints ();
    
    [DllImport ("UnityMIDIReceiver", EntryPoint="UnityMIDIReceiver_GetEndpointIDAtIndex")]
    public static extern uint GetEndpointIdAtIndex (int index);
    
    [DllImport ("UnityMIDIReceiver", EntryPoint="UnityMIDIReceiver_DequeueIncomingData")]
    public static extern ulong DequeueIncomingData ();
    
    [DllImport ("UnityMIDIReceiver")]
    private static extern System.IntPtr UnityMIDIReceiver_GetEndpointName (uint id);
    
    public static string GetEndpointName (uint id)
    {
        return Marshal.PtrToStringAnsi (UnityMIDIReceiver_GetEndpointName (id));
    }
    #endregion
}
