namespace _i
{
    class VODANG
    {
        public static int Id = 12;
        public static string Name => "Võ Đang Sơn";
        // npc kỵ
        public static NPC TruongQuanMo = new NPC()
        {
            Id = 12,
            X = 101,
            Y = 115,
            Map = Id,
            Name = "Trương Quân Mộ"
        };

        // npc bái sư
        public static NPC TruongHuyenTo = new NPC()
        {
            Id = 0,
            X = 78,
            Y = 86,
            Map = Id,
            Name = "Trương Huyền Tố"
        };

        // npc nhiệm vụ
        public static NPC TruongTrungHanh = new NPC()
        {
            Id = 9,
            X = 78,
            Y = 95,
            Map = Id,
            Name = "Trương Trung Hành"
        };

        // npc tâm pháp
        public static NPC DuVienSon = new NPC()
        {
            Id = 1,
            X = 83,
            Y = 85,
            Map = Id,
            Name = "Du Viễn Sơn"
        };       
    }
}
