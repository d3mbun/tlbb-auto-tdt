namespace _i
{
    class DUONGMON
    {
        public static int Id = 615;
        public static string Name => "Đường Gia Bảo";
        // mpc kỵ
        public static NPC DuongChuCo = new NPC()
        {
            Id = 4,
            X = 170,
            Y = 32,
            Map = Id,
            Name = "Đường Chử Cơ"
        };

        // npc bái sư
        public static NPC DuongXichPhong = new NPC()
        {
            Id = 0,
            X = 78,
            Y = 35,
            Map = Id,
            Name = "Đường Xích Phong"
        };        

        // npc nhiệm vụ
        public static NPC DuongThanhThu = new NPC()
        {
            Id = 2,
            X = 100,
            Y = 64,
            Map = Id,
            Name = "Đường Thanh Thu"
        };

        // npc tâm pháp
        public static NPC DuongNhacXung = new NPC()
        {
            Id = 1,
            X = 38,
            Y = 75,
            Map = Id,
            Name = "Đường Nhạc Xung"
        };
    }
}
