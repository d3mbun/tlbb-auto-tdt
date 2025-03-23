namespace _i
{
    class MODUNG
    {
        public static int Id = 284;
        public static string Name => "Mộ Dung Sơn Trang";
        // npc kỵ
        public static NPC PhongThienLy = new NPC()
        {
            Id = 15,
            X = 25,
            Y = 167,
            Map = Id,
            Name = "Phong Thiên Lý"
        };

        // npc bái sư
        public static NPC MoDungKiet = new NPC()
        {
            Id = 13,
            X = 48,
            Y = 144,
            Map = Id,
            Name = "Mộ Dung Kiệt"
        };

        // npc nhiệm vụ
        public static NPC MoDungThang = new NPC()
        {
            Id = 9,
            X = 69,
            Y = 126,
            Map = Id,
            Name = "Mộ Dung Thắng"
        };

        // npc tâm pháp
        public static NPC MoDungThanhSon = new NPC()
        {
            Id = 14,
            X = 48,
            Y = 135,
            Map = Id,
            Name = "Mộ Dung Thanh Sơn"
        };
    }
}
