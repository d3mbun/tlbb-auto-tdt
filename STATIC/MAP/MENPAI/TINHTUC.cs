namespace _i
{
    class TINHTUC
    {
        public static int Id = 16;
        public static string Name => "Tinh Túc Hải";
        // npc kỵ
        public static NPC ThienUngTu = new NPC()
        {
            Id = 8,
            X = 98,
            Y = 47,
            Map = Id,
            Name = "Thiên Ưng Tử"
        };

        // npc bái sư
        public static NPC HanTheTrung = new NPC()
        {
            Id = 1,
            X = 96,
            Y = 75,
            Map = Id,
            Name = "Hàn Thế Trung"
        };

        // npc nhiệm vụ
        public static NPC VuongNgan = new NPC()
        {
            Id = 9,
            X = 96,
            Y = 93,
            Map = Id,
            Name = "Vương Ngạn"
        };

        // npc tâm pháp
        public static NPC ThiToan = new NPC()
        {
            Id = 6,
            X = 87,
            Y = 70,
            Map = Id,
            Name = "Thi Toàn"
        };
    }
}
