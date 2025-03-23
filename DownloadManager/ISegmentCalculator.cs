using System;
using System.Collections.Generic;
using System.Text;

namespace Update
{
    interface ISegmentCalculator
    {
        CalculatedSegment[] GetSegments(int segmentCount, RemoteFileInfo fileSize);
    }
}
