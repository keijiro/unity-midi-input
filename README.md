MidiInput for Unity
===================

**MidiInput** is a native code plug-in for Unity. It interfaces with MIDI input devices.
You can use it to control Unity apps with physical MIDI controllers.

This project was derived from [unity-midi-receiver]
(https://github.com/keijiro/unity-midi-receiver) project.

Setting up
----------

1. Drag and drop the contents of this repository into the Project view.
2. Add the **MidiInput** script component to a game object.

Usage
-----

There is only one public function and one public property.

#### static float GetKnob ( channel, filter )

Provides the current CC value from the specified CC channel.

#### static int [] Channels

The CC channel list. It contains the channel numbers which has alraedy sent any data to the host.

Filters for CC input
--------------------

You can specify the filter type when retrieving the CC value with the GetKnob function.
There are three filter types.

- Filter.Realtime - no filter. It returns the last received value.
- Filter.Fast - light low-pass filter. It suits fast-moving objects.
- Filter.Slow - heavy low-pass filter. Very smooth but not responsive.
 
Current limitation
------------------

You can only receive CC data. Note data support is TBA.


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
