MidiInput for Unity
===================

**MidiInput** is a native code plug-in for Unity. It allows Unity to communicate with
external MIDI controllers.

Demo
----

You can find two demo projects in the [test branch]
(https://github.com/keijiro/unity-midi-input/tree/test).

#### Note

The first demo "Note" shows how to get key presses and releases from MidiInput.
The size of the cubes are relative to velocity of a key press.

![Screenshot](http://keijiro.github.io/unity-midi-input/screenshot1.png)

#### CC

The second demo "CC" shows how to get CC (control change) data from MidiInput.

![Screenshot](http://keijiro.github.io/unity-midi-input/screenshot2.png)

System requirement
------------------

- Currently supports only the desktop platforms (Windows and Mac OS X).
- Requires Unity Pro to enable native plug-in feature.

Setting up
----------

1. Drag and drop the contents of this repository into the Project view.
2. Add the **MidiInput** script component to a game object.
3. Open Script Execution Order Settings (Edit -> Project Settings -> Script Execution
   Order) and set the **MidiInput** script to the highest priority.

Function reference
------------------

#### static float GetKey ( noteNumber )

If the key specified with noteNumber has been pressed down, returns its velocity
(greater than 0.0f, and less or equal to 1.0f). If the key isn't pressed,
returns 0.0f.

#### static bool GetKeyDown ( noteNumber )

Returns true during the frame the key was pressed down.

#### static bool GetKeyUp ( noteNumber )

Returns true during the frame the key was released.

#### static float GetKnob ( channel, filter )

Returns the current value of the specified CC channel.

#### static int [] KonbChannels

Provides the CC channel list. It contains the channel numbers which has alraedy
sent data to the host.

Filters for CC input
--------------------

You can specify one of the three filter types below when retrieving a CC value with
the GetKnob function.

- Filter.Realtime - no filter. It returns the last received value.
- Filter.Fast - light low-pass filter. It suits fast-moving objects.
- Filter.Slow - heavy low-pass filter. Very smooth but not responsive.

You can adjust the sensibility of each filter in the inspector.

See also
--------

This project was derived from [unity-midi-receiver]
(https://github.com/keijiro/unity-midi-receiver).
You can see the details of the implementation there.

License
-------

Copyright (C) 2013 Keijiro Takahashi

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
