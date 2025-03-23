namespace _i
{
    class MINHGIAO
    {
        public static int Id = 11;
        public static string Name => "Quang Minh Điện";
        // npc kỵ
        public static NPC DangNguyenGiac = new NPC()
        {
            Id = 9,
            X = 65,
            Y = 116,
            Map = Id,
            Name = "Đặng Nguyên Giác"
        };

        // npc bái sư
        public static NPC LaSuTuong = new NPC()
        {
            Id = 11,
            X = 108,
            Y = 56,
            Map = Id,
            Name = "Lã Sư Tương"
        };

        // npc nhiệm vụ
        public static NPC LamNham = new NPC()
        {
            Id = 1,
            X = 98,
            Y = 105,
            Map = Id,
            Name = "Lâm Nham"
        };

        // npc tâm pháp
        public static NPC BangVanXuan = new NPC()
        {
            Id = 12,
            X = 109,
            Y = 59,
            Map = Id,
            Name = "Bàng Vạn Xuân"
        };     
    }
}
