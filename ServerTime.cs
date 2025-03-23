using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace _i
{
    class ServerTime
    {
        public DateTime Now
        {
            get
            {
                if (baseTime != null)
                    return baseTime.Add(swatch.Elapsed);
                else
                    return DateTime.Now;
            }
            set
            {
                baseTime = value;
                swatch.Reset();
                swatch.Start();
            }
        }

        public DateTime baseTime;
        public Stopwatch swatch = Stopwatch.StartNew();
    }
}
