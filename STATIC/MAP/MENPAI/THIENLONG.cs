namespace _i
{
    class THIENLONG
    {
        public static int Id = 13;
        public static string Name => "Thiên Long Tự";
        // npc kỵ
        public static NPC DuongBachNguu = new NPC()
        {
            Id = 20,
            X = 147,
            Y = 96,
            Map = Id,
            Name = "Dương Bạch Ngưu"
        };

        // npc bái sư
        public static NPC BanNhan = new NPC()
        {
            Id = 0,
            X = 96,
            Y = 66,
            Map = Id,
            Name = "Bản Nhân"
        };

        // npc nhiệm vụ
        public static NPC BanPham = new NPC()
        {
            Id = 4,
            X = 96,
            Y = 89,
            Map = Id,
            Name = "Bản Phàm"
        };

        // npc tâm pháp
        public static NPC BanQuan = new NPC()
        {
            Id = 1,
            X = 98,
            Y = 67,
            Map = Id,
            Name = "Bản Quán"
        };        
    }
}
