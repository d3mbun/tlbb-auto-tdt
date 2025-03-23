using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class YENTUO
    {
        public static int Id = 236;

        public static string Name => "Yến Tử Ổ";

        public static NPC HoDienBao = new NPC()
        {
            Id = 2635,
            X = 79,
            Y = 202,
            Map = Id,
            Name = "Hô Diên Báo"
        };

        public static NPC TienHoanhVu = new NPC()
        {
            Id = 2640,
            X = 217,
            Y = 150,
            Map = Id,
            Name = "Tiền Hoành Vũ"
        };

        public static NPC MoDungPhuc = new NPC()
        {
            Id = 2929,
            X = 63,
            Y = 67,
            Map = Id,
            Name = "Mộ Dung Phục"
        };

        public static NPC HoaHachCan = new NPC()
        {
            Id = 2641,
            X = 181,
            Y = 89,
            Map = Id,
            Name = "Hoa Hách Cấn"
        };


        public static NPC HoaHachCanTinhKiem = new NPC()
        {
            Id = 5036,
            X = 180,
            Y = 90,
            Map = Id,
            Name = "Hoa Hách Cấn"
        };
    }
}
