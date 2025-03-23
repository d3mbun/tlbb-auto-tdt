namespace _i
{
    class NGAMY
    {
        public static int Id = 15;
        public static string Name => "Nga Mi Sơn";
        // npc kỵ
        public static NPC TieuTuongNgoc = new NPC()
        {
            Id = 9,
            X = 146,
            Y = 54,
            Map = Id,
            Name = "Tiêu Tương Ngọc"
        };

        // npc bái sư
        public static NPC LyThapNhiNuong = new NPC()
        {
            Id = 1,
            X = 96,
            Y = 52,
            Map = Id,
            Name = "Lý Thập Nhị Nương"
        };

        // npc nhiệm vụ
        public static NPC ManhLong = new NPC()
        {
            Id = 7,
            X = 96,
            Y = 87,
            Map = Id,
            Name = "Mãnh Long"
        };

        // npc tâm pháp
        public static NPC ThoiLucHoa = new NPC()
        {
            Id = 2,
            X = 98,
            Y = 52,
            Map = Id,
            Name = "Thôi Lục Hoa"
        };       
    }
}
