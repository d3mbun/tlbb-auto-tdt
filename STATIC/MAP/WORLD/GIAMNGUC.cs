using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class GIAMNGUC
    {
        public static int Id = 194;
        public static string Name => "Giám ngục";
        public static NPC TruongChinhQuy = new NPC()
        {
            Id = 0,
            X = 45,
            Y = 31,
            Map = Id,
            Name = "Trương Chính Quý"
        };
    }
}
