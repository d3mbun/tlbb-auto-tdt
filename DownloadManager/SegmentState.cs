using System;
using System.Collections.Generic;
using System.Text;

namespace Update
{
    enum SegmentState
    {
        Idle,
        Connecting,
        Downloading,
        Paused,
        Finished,
        Error,
    }
}
