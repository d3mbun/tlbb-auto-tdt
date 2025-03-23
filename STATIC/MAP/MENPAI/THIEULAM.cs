namespace _i
{
    class THIEULAM
    {
        public static int Id = 9;
        public static string Name => "Thiếu Lâm Tự";
        // npc kỵ
        public static NPC HuyenSinh = new NPC()
        {
            Id = 6,
            X = 61,
            Y = 82,
            Map = Id,
            Name = "Huyền Sinh"
        };

        // npc bái sư
        public static NPC HuyenTich = new NPC()
        {
            Id = 4,
            X = 90,
            Y = 72,
            Map = Id,
            Name = "Huyền Tịch"
        };

        // npc nhiệm vụ
        public static NPC TuePhuong = new NPC()
        {
            Id = 9,
            X = 96,
            Y = 82,
            Map = Id,
            Name = "Tuệ Phương"
        };

        // npc tâm pháp
        public static NPC HuyenNan = new NPC()
        {
            Id = 5,
            X = 92,
            Y = 71,
            Map = Id,
            Name = "Huyền Nạn"
        };       
    }
}
