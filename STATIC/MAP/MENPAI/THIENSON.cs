namespace _i
{
    class THIENSON
    {
        public static int Id = 17;
        public static string Name => "Thiên Sơn";
        // npc kỵ
        public static NPC NhamPhiHong = new NPC()
        {
            Id = 6,
            X = 39,
            Y = 71,
            Map = Id,
            Name = "Nhậm Phi Hồng"
        };

        // npc bái sư
        public static NPC MaiKiem = new NPC()
        {
            Id = 0,
            X = 92,
            Y = 45,
            Map = Id,
            Name = "Mai Kiếm"
        };

        // npc nhiệm vụ
        public static NPC PhuManNghi = new NPC()
        {
            Id = 2,
            X = 95,
            Y = 61,
            Map = Id,
            Name = "Phù Mẫn Nghi"
        };

        // npc tâm pháp
        public static NPC LanKiem = new NPC()
        {
            Id = 13,
            X = 89,
            Y = 45,
            Map = Id,
            Name = "Lan Kiếm"
        };      
    }
}
