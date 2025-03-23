namespace _i
{
    class CAIBANG
    {
        public static int Id => 10;

        public static string Name => "Cái Bang Tổng Đà";

        // npc kỵ
        public static NPC LyNhatViet = new NPC()
        {
            Id = 4,
            X = 54,
            Y = 86,
            Map = Id,
            Name = "Lý Nhật Việt"
        };

        // npc bái sư
        public static NPC TranCoNhan = new NPC()
        {
            Id = 23,
            X = 92,
            Y = 99,
            Map = Id,
            Name = "Trần Cô Nhạn"
        };

        // npc nhiệm vụ
        public static NPC HongThong = new NPC()
        {
            Id = 16,
            X = 92,
            Y = 77,
            Map = Id,
            Name = "Hồng Thông"
        };

        // npc tâm pháp
        public static NPC HeTamKi = new NPC()
        {
            Id = 0,
            X = 94,
            Y = 99,
            Map = Id,
            Name = "Hề Tam Kì"
        };        
    }
}
