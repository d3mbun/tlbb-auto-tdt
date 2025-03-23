namespace _i
{
    class TIEUDAO
    {
        public static int Id = 14;
        public static string Name => "Lăng Ba Động";
        // npc kỵ
        public static NPC CauDoc = new NPC()
        {
            Id = 16,
            X = 65,
            Y = 55,
            Map = Id,
            Name = "Cẩu Độc"
        };

        // npc bái sư
        public static NPC ToTinhHa = new NPC()
        {
            Id = 0,
            X = 126,
            Y = 145,
            Map = Id,
            Name = "Tô Tinh Hà"
        };

        // npc nhiệm vụ
        public static NPC TanQuan = new NPC()
        {
            Id = 13,
            X = 119,
            Y = 152,
            Map = Id,
            Name = "Tần Quán"
        };

        // npc tâm pháp
        public static NPC KhangQuangLang = new NPC()
        {
            Id = 1,
            X = 125,
            Y = 142,
            Map = Id,
            Name = "Khang Quảng Lăng"
        };
    }
}
