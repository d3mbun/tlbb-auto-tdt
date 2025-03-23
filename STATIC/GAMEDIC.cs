using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class GAMEDIC
    {
        public static Dictionary<string, int> Ngoc = new Dictionary<string, int>()
        {
        };


        public static Dictionary<int, string> CuaRaPhuBan = new Dictionary<int, string>()
        {
            {MAP.ThanhThuSonPhuBan,"238,235" },
            {MAP.HuyenVuDaoPhuBan,"70,67" },
            {MAP.HuyetMo,"51,48" },
            {MAP.BienGioiTongLieu,"31,12" },
            {MAP.TrucLam,"107,112" },
            {MAP.YenTuO,"64,21" },
            {MAP.TangKinhCac,"70,20"},
            
          
            {MAP.TacKhauDoanhDia,"86,116" },
            {MAP.NongTruongDaTru,"29,104"},

            {MAP.MoDungPhuBan,"160,169"},
            {MAP.DuongMonPhuBan,"173,170"},
            {MAP.TinhTucPhuBan,"96,142"},
            {MAP.TieuDaoPhuBan,"44,129"},
            {MAP.ThieuLamPhuBan,"96,158"},
            {MAP.ThienSonPhuBan,"95,148"},
            {MAP.NgaMyPhuBan,"89,146" },
	        {MAP.VoDangPhuBan,"95,192"},
            {MAP.MinhGiaoPhuBan,"98,159"},
            {MAP.CaiBangPhuBan,"91,159"},
            {MAP.ThienLongPhuBan,"96,142"},
            {MAP.DaoHoaPhuBan,"274,178"},
        };


        public static List<string> NgocThuocTinh3 = new List<string>() 
        { 
            "Lục Tinh Thạch (Cấp 3)",
            "Hồng Tinh Thạch (Cấp 3)",
            "Lam Tinh Thạch (Cấp 3)",
            "Hoàng Tinh Thạch (Cấp 3)",
            "Bích Tỷ (Cấp 3)",
            "Nguyệt Quang Thạch (Cấp 3)",
            "Hạo Thạch (Cấp 3)",
            "Hoàng Ngọc (Cấp 3)",
        };


        public static List<string> MoDungBacPoint = new List<string>()
        {
            "118,42",
            "126,42",
            "130,47",
            "126,52",
            "118,52",
            "114,47",
        };

        public static List<string> PhucDiaLyThuThuy = new List<string>()
        {
            "144,55",
            "137,55",
            "142,70",
            "133,61",
            "136,69",
            "147,66",
        };


        public static List<string> YenTuO = new List<string>()
        {
            "44,75",
            "56,75",
            "46,69",
            "54,69",
            "46,81",
            "54,81"
        };

        public static List<string> PhucDiaLyDongLao = new List<string>()
        {
            "210,124",
            "207,120",
            "209,115",
            "214,114",
            "217,118",
            "216,123",
        };

        public static List<string> PhucDiaHuTruc { get; set; } = new List<string>()
        {
            "78,176",
            "84,175",
            "88,179",
            "86,185",
            "80,186",
            "76,182",
        };

        public static List<string> TamThanLietHai { get; set; } = new List<string>()
        {
            "47,110",
            "49,116",
            "52,106",
            "55,118",
            "57,108",
            "59,113",
        };

        public static List<string> TamThanNhiepLinh { get; set; } = new List<string>()
        {
            "135,148",
            "135,156",
            "131,157",
            "128,154",
            "131,148",
            "137,152",
        };

        public static List<string> TamThanLuyenNguc { get; set; } = new List<string>()
        {
            "130,63",
            "129,69",
            "127,61",
            "123,70",
            "122,62",
            "121,67",
        };


        public static List<string> TanMangThanPhu { get; set; } = new List<string>()
        {
            "Shenqi_1_9",
            "Shenqi_1_10",
            "Shenqi_1_11",
            "Shenqi_1_12",
            "Shenqi_1_13",
            "Shenqi_1_14",
            "Shenqi_1_15",
            "CircularTaskTool45_14",
        };

        public static List<string> TrangBiTranThu75 { get; set; } = new List<string>()
        {
            "Thương Lang KNT Trảo - Dũng",
            "Thương Lang KNT Trảo - Giảo",
            "Thương Lang KNT Trảo - Thận",
            "Hoàng Tước HTT Trảo - Khiếp",
            "Hoàng Tước HTT Trảo - Giảo",
            "Hoàng Tước HTT Trảo - Thận",
            "Ô Đồn Vọng Nhật Trảo-Trung",
            "Ô Đồn Vọng Nhật Trảo-Thận",
            "Bạch Thố Ẩn LT Trảo - Thận",
            "Thương Lang KNT Khôi - Dũng",
            "Thương Lang KNT Khôi - Giảo",
            "Thương Lang KNT Khôi - Thận",
            "Hoàng Tước HTT Khôi - Khiếp",
            "Hoàng Tước HTT Khôi - Giảo",
            "Hoàng Tước HTT Khôi - Thận",
            "Ô Đồn Vọng Nhật Khôi-Trung",
            "Ô Đồn Vọng Nhật Khôi-Thận",
            "Bạch Thố Ẩn LT Khôi - Thận",
            "Thương Lang KNT Giáp - Dũng",
            "Thương Lang KNT Giáp - Giảo",
            "Thương Lang KNT Giáp - Thận",
            "Hoàng Tước HTT Giáp - Khiếp",
            "Hoàng Tước HTT Giáp - Giảo",
            "Hoàng Tước HTT Giáp - Thận",
            "Ô Đồn Vọng Nhật Giáp-Trung",
            "Ô Đồn Vọng Nhật Giáp-Thận",
            "Bạch Thố Ẩn LT Giáp - Thận",
            "Thương Lang KNT Hoàn - Dũng",
            "Thương Lang KNT Hoàn - Giảo",
            "Thương Lang KNT Hoàn - Thận",
            "Hoàng Tước HTT Hoàn - Khiếp",
            "Hoàng Tước HTT Hoàn - Giảo",
            "Hoàng Tước HTT Hoàn - Thận",
            "Ô Đồn Vọng Nhật Hoàn-Trung",
            "Ô Đồn Vọng Nhật Hoàn-Thận",
            "Bạch Thố Ẩn LT Hoàn - Thận",
            "Thương Lang KNT Sức - Dũng",
            "Thương Lang KNT Sức - Giảo",
            "Thương Lang KNT Sức - Thận",
            "Hoàng Tước HTT Sức - Khiếp",
            "Hoàng Tước HTT Sức - Giảo",
            "Hoàng Tước HTT Sức - Thận",
            "Ô Đồn Vọng Nhật Sức-Trung",
            "Ô Đồn Vọng Nhật Sức-Thận",
            "Bạch Thố Ẩn LT Sức - Thận"
        };



        public static HashSet<string> NhiemVuKNBKhoa = new HashSet<string>()
        {
            "Lưu Kim Võ Lâm Ấn Đại Lý Sát Tinh", // quỷ cốc            
            "Lưu Kim Võ Lâm Ấn Như Hổ Thiêm Dực", // đường môn
            "Lưu Kim Võ Lâm Ấn Thần Dũng Võ Hồn", // mộ dung
            "Lưu Kim Võ Lâm Ấn Tiềm Tâm Tu Luyện", // tinh túc
            "Khoa Ngân Võ Lâm Ấn Túc Cầu Thịnh Hội",
            "Khoa Ngân Võ Lâm Ấn Tàng Kinh Các Nguy Cơ",
            "Khoa Ngân Võ Lâm Ấn Phồn Mang Tào Vận",
            "Quả Ngân Võ Lâm Ấn Bang HộiBuôn Bán",
            "Thanh Đồng Ấn-Tương Trợ Sư Môn",
            "Thanh Đồng Ấn-Trừ Ác",
            "Thanh Đồng Ấn-Trừng Hung",
            "Sinh Tài Chi Đạo",
        };


        public static HashSet<string> NhiemVuEXP = new HashSet<string>()
        {
            "Đại Lý Kỳ Thánh",
            "Tô Châu Kỳ Thánh",
            "Lạc Dương Kỳ Thánh",
            "Hắc Bạch",
            "#{YD_100806_52}",          
            "Ván Cờ Sinh Tử",

            "Đánh Lén Môn Phái",
            "#{YD_100806_64}",
            "Giang hồ tà đạo",

            "Tặc Binh Xâm Nhập",
            "#{YD_100806_56}",
            "Ác Tặc Tạo Phản",

            "#{LSHDR_150203_93}",
            "Đầu Sỏ Tam Quan",
            "#{LSHDR_150203_15}",
            "Vào Tam Tài Trận",
        };


        public static HashSet<string> NgoChanNguyen = new HashSet<string>()
        {
            "#{ZYRW_120522_66}",//"Ngộ chân nguyên",
            "Ngộ chân nguyên",
            "#{ZYRW_120522_67}",//"Chân nguyên hiện thế",
            "Chân nguyên hiện thế",
            "#{ZYRW_120522_68}",//"Tìm kiếm chân nguyên",
            "Tìm kiếm chân nguyên",
            "#{ZYRW_120522_69}", // "Chân nguyên chi đạo",
            "Chân nguyên chi đạo",
            "#{ZYRW_120522_70}",
            "Huyền âm giải chân nguyên",
           
        };

        public static List<string> NhiemVuChienBi = new List<string>()
        {
            "Diệt Thủ Vệ",
            "#{MZZBQ_150811_331}",
            "#{MZZBQ_150811_334}", // dotkhothoc
            "#{MZZBQ_150811_332}", //xaydungvuongky
            "#{MZZBQ_150811_335}", //guithukhuyenhang
            "#{MZZBQ_150811_330}", // camco
            "#{MZZBQ_150811_333}"
        };

        public static List<string> NhiemVuThienSu = new List<string>()
        {
            "Hồi Báo Thiên Sư (1)",
            "Thiên sư kỳ đãi (1)",
            "Thiên sư kỳ đãi (2)",
            "Thiên sư kỳ đãi (3)",
            "Thiên sư kỳ đãi (4)",
            "Thiên sư kỳ đãi (5)",
            "Thiên sư kỳ đãi (6)",
            "Thiên sư kỳ đãi (7)",
            "Thiên sư kỳ đãi (8)",
            "Thiên sư kỳ đãi (9)",
            "Thiên sư kỳ đãi (10)",
            "Hồi Báo Thiên Sư (1)",
        };

        public static List<string> CotTruyen = new List<string>()
        {
            "Đèn nhà ai nấy sáng",
            "Áo xanh lỗi lạc hành núi hiểm",
            "Vi Tiếu Chi Lữ",
            "Ác Quán Mãn Doanh",
            "Gặp nhau tại Vạn Kiếp Cốc",
            "Đại chiến Vạn Kiếp Cốc",
            "Đại tiến quân",
            "Hổ khiếu long ngâm",
        };

        public static Dictionary<string, string> DicCotTruyen = new Dictionary<string, string>()
        {
         {   "Juqing_Can_001","Đèn nhà ai nấy sáng"},
{            "Juqing_Can_002", "Áo xanh lỗi lạc hành núi hiểm"},
{            "Juqing_Can_003", "Vi Tiếu Chi Lữ"},
{            "Juqing_Can_004", "Ác Quán Mãn Doanh"},
{            "Juqing_Can_005", "Gặp nhau tại Vạn Kiếp Cốc"},
{            "Juqing_Can_006", "Đại chiến Vạn Kiếp Cốc"},
{            "Juqing_Can_007", "Đại tiến quân"},
{            "Juqing_Can_008", "Hổ khiếu long ngâm"},
{            "Juqing_Can_009", "Cô Tô Mộ Dung"},
{            "Juqing_Can_010", "Hoàn Thị Thủy Các"},//Độc chiến song hùng
{            "Juqing_Can_011", "Độc chiến song hùng"},//Đêm dài lắm mộng
{            "Juqing_Can_012", "Đêm dài lắm mộng"},//Nhất phẩm đường
{            "Juqing_Can_013", "Nhất phẩm đường"},//Nhất phẩm đường
{            "Juqing_Can_014", "Chỉ Điểm Quần Hào"},//Nhất phẩm đường
{            "Juqing_Can_015", "Bi Tô Thanh Phong"},//Nhất phẩm đường
{            "Juqing_Can_016", "Tứ hải là nhà"},//Nhất phẩm đường
{            "Juqing_Can_017", "Quần Long Vô Thủ"},//Nhất phẩm đường
{            "Juqing_Can_018", "Cùng Tiến Cùng Lùi"},//Nhất phẩm đường
{            "Juqing_Can_019", "Thiên hạ võ công xuất Thiếu Lâm"},//Nhất phẩm đường
{            "Juqing_Can_020", "Tụ Hiền Trang"},//Nhất phẩm đường
{            "Juqing_Can_021", "Đỉnh thiên lập địa"},//Nhất phẩm đường
{            "Juqing_Can_046", "Kỳ sau gặp mặt"},//Nhất phẩm đường
{            "Juqing_Can_022", "Dù vạn người, ta vẫn cứ tiến"},//Nhất phẩm đường
{            "Juqing_Can_023", "Huyết Chiến Tụ Hiền Trang"},//Nhất phẩm đường
{            "Juqing_Can_024", "Bảo vệ Mã Phu Nhân"},//Nhất phẩm đường
{            "Juqing_Can_025", "Nhất Bàn Tản Sa"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_026", "Tới núi Thương Mang"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_027", "Thiên thời không bằng địa lợi"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_028", "Cần Vương"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_029", "Bày mưu tính kế"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_030", "Tứ Diện Sở Ca"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_031", "Kim Qua Đãng Khấu Ngao Binh"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_032", "Lục Quân Tị Dịch"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_033", "Trân Long Kỳ Hội"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_034", "Tịnh hầu quế âm"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_035", "Thua thắng thành bại ai biết được"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_036", "Chọn ngày tái chiến"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_037", "Đóng cửa bắt trộm"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_038", "Hội Thi Túc Cầu"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_039", "Nhất kiếm thượng thiên sơn"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_040", "Dù nguy mà yên"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_041", "Y tiếu nhân gian vạn sự"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_042", "Hoàn Phụng Không quy nguyệt dạ hồn"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_043", "Bày tiệc rượu hỏi Quân Tam Ngữ"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_044", "Tình yêu không phải là mơ"},//Nhất phẩm đường//Tới núi Thương Mang
{            "Juqing_Can_045", "Bên Nhau Trọn Đời"},//Nhất phẩm đường//Tới núi Thương Mang
{            "CJG_101231_115", "Cái Bang Nghi Vấn Ban Đầu"},//Nhất phẩm đường//Tới núi Thương Mang
{            "CJG_101231_116", "Trừ độc vật giải nguy Cái Bang"},//Nhất phẩm đường//Tới núi Thương Mang
{            "CJG_101231_117", "Ẩn danh do thám Cái Bang"},//Nhất phẩm đường//Tới núi Thương Mang
{            "CJG_101231_118", "Cùng lên Thiếu Lâm"},//Nhất phẩm đường//Tới núi Thương Mang
{            "CJG_101231_119", "Đến Thiếu Lâm trừ gian kết nghĩa"},//Nhất phẩm đường//Tới núi Thương Mang
//{            "Juqing_Can_041", "Y tiếu nhân gian vạn sự"},//Nhất phẩm đường//Tới núi Thương Mang
//Thiên thời không bằng địa lợi
        };

        public static Dictionary<string, string> Replace = new Dictionary<string, string>()
        {
            {"#{MISSIONNAME_JUQING_1}","Hội Thi Túc Cầu" },
            { "#{CJG_101231_122}","Trừ độc vật giải nguy Cái Bang" },
            { "#{CJG_101231_123}","Ẩn danh do thám Cái Bang" },
            { "#{CJG_101231_124}","Cùng lên Thiếu Lâm" },
            { "#{CJG_101231_125}","Đến Thiếu Lâm trừ gian kết nghĩa" },
        };

        public static Dictionary<int, string> MapNameId = new Dictionary<int, string>()
        {
           {0,"Lạc Dương"},
            {1,"Tô Châu"},
            {2,"Đại Lý"},
            {3,"Tung Sơn"},
            {4,"Thái Hồ"},
            {5,"Kính Hồ"},
            {6,"Vô Lượng Sơn"},
            {7,"Kiếm Các"},
            {8,"Đôn Hoàng"},
            {9,"Thiếu Lâm Tự"},
            {10,"Cái Bang Tổng Đà "},
            {11,"Quang Minh Điện"},
            {12,"Võ Đang Sơn"},
            {13,"Thiên Long Tự "},
            {14,"Lăng Ba Động "},
            {15,"Nga Mi Sơn"},
            {16,"Tinh Túc Hải "},
            {17,"Thiên Sơn"},
            {18,"Nhạn Nam"},
            {19,"Nhạn Bắc "},
            {20,"Thảo Nguyên"},
            {21,"Liêu Tây "},
            {22,"Trường Bạch Sơn "},
            {23,"Hoàng Long Phủ "},
            {24,"Nhĩ Hải "},
            {25,"Thương Sơn "},
            {26,"Thạch Lâm "},
            {27,"Ngọc Khê "},
            {28,"Nam Chiêu "},
            {29,"Miêu Cương "},
            {30,"Tây Hồ"},
            {31,"Long Tuyền "},
            {32,"Võ Di "},
            {33,"Mai Lĩnh "},
            {34,"Nam Vực"},
            {35,"Quỳnh Châu"},
            {36,"Tụ Hiền Trang "},
            {37,"Yến Tử ‘ "},
            {38,"Nhất phẩm đường "},
            {39,"Dự Lưu 1"},
            {40,"Dự Lưu 2"},
            {41,"Dự lưu 3"},
            {42,"Phản quân doanh địa"},
            {43,"Kỉ niệm Lạc Dương"},
            {44,"Kỉ niệm Tô Châu"},
            {45,"Đại Lý tròn một năm"},
            {46,"Kỉ niệm Lâu Lan"},
            {47,"Mộc Nhân Hạng"},
            {61,"Trân Long Kỳ Cuộc"},
            {66,"Thủy lao"},
            {71,"Đại Lý 2"},
            {72,"Đại Lý 3"},
            {73,"Vô Lượng Sơn 2"},
            {74,"Vô Lượng Sơn 3"},
            {75,"Kiếm Các 2"},
            {76,"Kiếm Các 3"},
            {77,"Địa phủ"},
            {78,"Sa mạc yếu trại"},
            {82,"Lôi đài"},
            {92,"Lôi đài"},
            {102,"Quang Minh động"},
            {103,"Đáy cốc Tiêu Dao"},
            {104,"Linh Tính Phong"},
            {105,"Cái Bang Tửu Diếu"},
            {106,"Đào Hoa Trận"},
            {107,"Tháp Lâm"},
            {108,"Ngũ Thần Động"},
            {109,"Chiết Mai Phong"},
            {110,"Huyệt mộ"},
            {111,"Chân tháp"},
            {112,"Huyền Vũ Đảo"},
            {113,"Tụ Hiền Trang "},
            {114,"Yến Tử ‘ "},
            {115,"Nhất phẩm đường "},
            {116,"Lôi Cổ Sơn"},
            {117,"Lôi Cổ Sơn"},
            {118,"Vạn Kiếp Cốc"},
            {119,"Vạn Kiếp Cốc"},
            {120,"Thương Mang Sơn "},
            {121,"Thương Mang Sơn "},
            {122,"Tiểu Mộc Nhân Hạng"},
            {123,"Hậu Hoa Viên"},
            {124,"Chợ"},
            {125,"Khoáng trường"},
            {126,"Nông trại"},
            {127,"Thư Phòng"},
            {128,"Thị tập"},
            {129,"Nhạn Nam sơn động"},
            {130,"Nhạn Bắc sơn động"},
            {131,"Tây Hồ sơn động"},
            {132,"Thương Sơn sơn động"},
            {133,"Quang Minh động"},
            {134,"Đáy cốc Tiêu Dao"},
            {135,"Linh Tính Phong"},
            {136,"Cái Bang Tửu Diếu"},
            {137,"Đào Hoa Trận"},
            {138,"Tháp Lâm"},
            {139,"Ngũ Thần Động"},
            {140,"Chiết Mai Phong"},
            {141,"Chân tháp"},
            {142,"Quang Minh động"},
            {143,"Đáy cốc Tiêu Dao"},
            {144,"Linh Tính Phong"},
            {145,"Cái Bang Tửu Diếu"},
            {146,"Đào Hoa Trận"},
            {147,"Tháp Lâm"},
            {148,"Ngũ Thần Động"},
            {149,"Chiết Mai Phong"},
            {150,"Chân tháp"},
            {151,"Thị tập"},
            {152,"Thị tập"},
            {153,"Công địa"},
            {154,"Thao Trường"},
            {155,"Nha Môn"},
            {156,"Lôi đài"},
            {157,"Lôi đài"},
            {158,"Lôi đài"},
            {159,"Lôi đài"},
            {160,"Lôi đài"},
            {161,"Lôi đài"},
            {162,"Lôi đài"},
            {163,"Lôi đài"},
            {164,"Dạ Tây Hồ"},
            {165,"Tàng Kinh Các"},
            {166,"Bảo Tàng Động Tầng 1 "},
            {167,"Khảo trường"},
            {168,"Hoa Sơn tuyệt đỉnh"},
            {169,"Bảo Tàng Động Tầng 2 "},
            {170,"Tặc Khấu doanh địa"},
            {171,"Tàng Kinh Các"},
            {172,"Tàng Kinh Các"},
            {173,"Thiếu Lâm Tự"},
            {174,"Cái Bang Tổng Đà "},
            {175,"Quang Minh Điện"},
            {176,"Võ Đang Sơn"},
            {177,"Thiên Long Tự "},
            {178,"Lăng Ba Động "},
            {179,"Nga Mi Sơn"},
            {180,"Tinh Túc Hải "},
            {181,"Thiên Sơn"},
            {182,"Thiếu Lâm Tự"},
            {183,"Cái Bang Tổng Đà "},
            {184,"Quang Minh Điện"},
            {185,"Võ Đang Sơn"},
            {186,"Thiên Long Tự "},
            {187,"Lăng Ba Động "},
            {188,"Nga Mi Sơn"},
            {189,"Tinh Túc Hải "},
            {190,"Thiên Sơn"},
            {191,"Bảo Tàng Động Tầng 3 "},
            {192,"Bảo Tàng Động Tầng 4 "},
            {193,"Bảo Tàng Động tầng 5 "},
            {194,"Giám ngục"},
            {195,"Tỷ Võ Hội Trường"},
            {196,"Lễ đường phổ thông"},
            {197,"Lễ đường cao cấp"},
            {198,"Lễ đường hào hoa"},
            {199,"Thảo Liệu Trường"},
            {200,"Miêu Nhân Động"},
            {201,"Thánh Thú Sơn"},
            {202,"Yến Vương Cổ Mộ Tầng 1"},
            {203,"Yến Vương Cổ Mộ Tầng 2"},
            {204,"Yến Vương Cổ Mộ Tầng 3"},
            {205,"Yến Vương Cổ Mộ Tầng 4"},
            {206,"Yến Vương Cổ Mộ Tầng 5"},
            {207,"Yến Vương Cổ Mộ Tầng 6"},
            {208,"Yến Vương Cổ Mộ Tầng 7"},
            {209,"Yến Vương Cổ Mộ Tầng 8"},
            {210,"Yến Vương Cổ Mộ Tầng 9"},
            {211,"Bến Tàu Sơn Động"},
            {212,"Kiếm Gia"},
            {213,"Ma Nhai Động"},
            {214,"Dã Nhân Câu"},
            {215,"Ôn Tuyền Động"},
            {216,"Hoàng Long Động"},
            {217,"Thủy Kính Hồ"},
            {218,"Tiên Vương Phần"},
            {219,"Thiên Khanh Thụ Động"},
            {220,"Đào Hoa Nguyên"},
            {221,"Hải Tặc Động"},
            {222,"Tuyết Lang Hồ"},
            {223,"Phụng Hoàng Cổ Trấn"},
            {224,"Tiền Trang"},
            {225,"Tiểu Mộc Nhân Cảng 2"},
            {226,"Tiểu Mộc Nhân Cảng 3"},
            {227,"Hậu Hoa Viên 2"},
            {228,"Hậu Hoa Viên 3"},
            {229,"Ngân Ngai Tuyết Nguyên"},
            {230,"Kính Hồ Phỉ Trại"},
            {231,"Nông trường Dã Trư"},
            {232,"Phó bản Thánh Thú Sơn"},
            {233,"Mẫu Đơn Uyển"},
            {234,"Tung Sơn phong thiền đài"},
            {235,"Diêm Hồ"},
            {236,"Yến Tử ‘ "},
            {237,"Bạch sa diêm khanh"},
            {238,"Đệ nhất khu nghỉ ngơi tại Lạc Dương"},
            {239,"Đệ nhị khu nghỉ ngơi tại Lạc Dương"},
            {240,"Khu nghỉ ngơi tại Đại Lý"},
            {241,"Khu nghỉ ngơi tại Tô Châu"},
            {242,"Lạc Dương"},
            {243,"Hàn Ngọc Cốc"},
            {244,"Hỏa Diệm Sơn"},
            {245,"Cao Xương"},
            {246,"Lâu Lan"},
            {247,"Tháp Lý Mộc "},
            {248,"Côn Lôn Sơn"},
            {249,"Đại Uyển"},
            {250,"Tát Mã Nhĩ Hãn"},
            {251,"Hỏa Diệm Cốc"},
            {252,"Cao Xương Mê Cung"},
            {253,"Tháp Khắc Lạp Mã Can"},
            {254,"Côn Lôn Phúc Địa"},
            {255,"Hãn Huyết Lĩnh"},
            {256,"Thánh hỏa cung"},
            {257,"Sân bóng đá"},
            {258,"Sân bóng rổ"},
            {259,"Sân bóng bàn"},
            {260,"Thúc Hà Cổ Trấn"},
            {261,"Phiêu Miễu Phong"},
            {262,"Tần Hoàng Địa Cung Tầng 1"},
            {263,"Tần Hoàng Địa Cung Tầng 2"},
            {264,"Tần Hoàng Địa Cung Tầng 3"},
            {268,"Huyền Vũ Đảo"},
            {269,"Lâu Lan Bảo Tàng Động"},
            {270,"Trác Lộc"},
            {271,"Lượng Mã Dịch"},
            {272,"Tàng Kinh Các"},
            {273,"Thiên Kiếp Lâu Tầng 1"},
            {274,"Thiên Kiếp Lâu Tầng 2"},
            {275,"Thiên Kiếp Lâu Tầng 3"},
            {276,"Thiên Kiếp Lâu Tầng 4"},
            {277,"Thiên Kiếp Lâu Tầng 5"},
            {278,"Thiên Kiếp Lâu Tầng 6"},
            {279,"Thiên Kiếp Lâu Tầng 7"},
            {280,"Phụng Hoàng Cổ Thành"},
            {281,"Phụng Hoàng Lăng Mộ"},
            {282,"Thanh Nguyên"},
            {283,"Thanh Nguyên Sơn Động"},
            {284,"Mộ Dung Sơn Trang"},
            {285,"Tàng Thư Thủy Các"},
            {286,"Tàng Thư Thủy Các"},
            {287,"Tàng Thư Thủy Các"},
            {288,"Mộ Dung Sơn Trang"},
            {289,"Mộ Dung Sơn Trang"},
            {290,"Đỉnh Hoa Sơn"},
            {291,"Tứ Tuyệt Trang"},
            {292,"Tần Hoàng Địa Cung Tầng 4"},
            {293,"Niêm Hoa Mộc Nhân Trận"},
            {294,"Binh Thánh Kỳ Trận"},
            {295,"Thông Thiên Tháp Địa Cung"},
            {296,"Thông Thiên Tháp Tầng 1"},
            {297,"Thông Thiên Tháp Tầng 2"},
            {298,"Thông Thiên Tháp Tầng 3"},
            {299,"Đỉnh Thông Thiên Tháp"},
            {300,"Thiên Long Huyễn Cảnh"},
            {301,"Phòng dự bị chiến đấu"},
            {302,"Chiến Trường Tranh Bá Toàn Cầu"},
            {303,"Chung kết-Sân 1"},
            {304,"Chung kết-Sân 2"},
            {305,"Chung kết-Sân 3"},
            {306,"Chung kết-Sân 4"},
            {501,"Tái Ngoại 1"},
            {502,"Tái Ngoại 2"},
            {503,"Tái Ngoại 3"},
            {504,"Tái Ngoại 4"},
            {505,"Tái Ngoại 5"},
            {506,"Lĩnh Nam 1"},
            {507,"Lĩnh Nam 2"},
            {508,"Lĩnh Nam 3"},
            {509,"Lĩnh Nam 4"},
            {510,"Lĩnh Nam 5"},
            {511,"Hàng Châu 1"},
            {512,"Hàng Châu 2"},
            {513,"Hàng Châu 3"},
            {514,"Hàng Châu 4"},
            {515,"Hàng Châu 5"},
            {516,"Hải Khẩu 1"},
            {517,"Hải Khẩu 2"},
            {518,"Hải Khẩu 3"},
            {519,"Hải Khẩu 4"},
            {520,"Hải Khẩu 5"},
            {521,"Điền Vực 1"},
            {522,"Điền Vực 2"},
            {523,"Điền Vực 3"},
            {524,"Điền Vực 4"},
            {525,"Điền Vực 5"},
            {526,"Thạch Thành 1"},
            {527,"Thạch Thành 2"},
            {528,"Thạch Thành 3"},
            {529,"Thạch Thành 4"},
            {530,"Thạch Thành 5"},
            {531,"Miêu Vực 1"},
            {532,"Miêu Vực 2"},
            {533,"Miêu Vực 3"},
            {534,"Miêu Vực 4"},
            {535,"Miêu Vực 5"},
            {536,"Liêu Địa 1"},
            {537,"Liêu Địa 2"},
            {538,"Liêu Địa 3"},
            {539,"Liêu Địa 4"},
            {540,"Liêu Địa 5"},
            {541,"Nhạn Môn 1"},
            {542,"Nhạn Môn 2"},
            {543,"Nhạn Môn 3"},
            {544,"Nhạn Môn 4"},
            {545,"Nhạn Môn 5"},
            {546,"Lôi Đài Sinh Tử"},
            {547,"Con Giáp Lôi Đài"},
            {548,"Chiến Trường Tống Liêu"},
            {549,"Phòng thông tin Chiến Trường Tống Liêu"},
            {550,"Trân Long Kỳ Cuộc"},
            {551,"Tặc Khấu doanh địa"},
            {552,"Thiếu Lâm Tự"},
            {553,"Cái Bang Tổng Đà "},
            {554,"Quang Minh Điện"},
            {555,"Võ Đang Sơn"},
            {556,"Thiên Long Tự "},
            {557,"Lăng Ba Động "},
            {558,"Nga Mi Sơn"},
            {559,"Tinh Túc Hải "},
            {560,"Thiên Sơn"},
            {561,"Mộ Dung Sơn Trang"},
            {565,"Thiếu Thất Sơn (câu chuyện)"},
            {566,"Thiếu Thất Sơn"},
            {567,"Cuộc chiến Tranh Bá liên máy chủ"},
            {568,"Chung kết-Sân 1"},
            {569,"Lâm Hải Khê Cốc"},
            {570,"Quân Thiên Vương Lăng"},
            {571,"La Phù Vương Lăng"},
            {572,"Triều Kinh Vương Lăng"},
            {573,"Mạc Nam Thanh Nguyên"},
            {574,"Vong Xuyên Hoa Hải"},
            {575,"Thiên Kỳ Nam Hoài"},
            {576,"Tàng Kinh Các"},
            {577,"Giám ngục"},
            {578,"Địa phủ"},
            {579,"Tân Lạc Dương"},
            {580,"Phụng Minh Trấn"},
            {581,"Quân Thiên Thành"},
            {582,"La Phù Thành"},
            {583,"Triều Kinh Thành"},
            {584,"Thiếu Lâm Thí Luyện"},
            {585,"Cái Bang Thí Luyện"},
            {586,"Minh Giáo Thí Luyện"},
            {587,"Võ Đang Thí Luyện"},
            {588,"Thiên Long Thí Luyện"},
            {589,"Tiêu Dao Thí Luyện"},
            {590,"Nga Mi Thí Luyện"},
            {591,"Tinh Túc Thí Luyện"},
            {592,"Thiên Sơn Thí Luyện"},
            {593,"Mộ Dung Thí Luyện"},
            {594,"Chung kết-Sân 2"},
            {595,"Chung kết-Sân 3"},
            {596,"Chung kết-Sân 4"},
            {597,"Đại Địa Đồ KVK"},
            {598,"Thiên Hoàng Địa Cung"},
            {599,"Lôi đài"},
            {600,"Phụng Minh Vương Lăng"},
            {601,"Quang Minh động"},
            {602,"Đáy cốc Tiêu Dao"},
            {603,"Linh Tính Phong"},
            {604,"Cái Bang Tửu Diếu"},
            {605,"Đào Hoa Trận"},
            {606,"Tháp Lâm"},
            {607,"Ngũ Thần Động"},
            {608,"Chiết Mai Phong"},
            {609,"Chân tháp"},
            {610,"Tàng Thư Thủy Các"},
            {611,"Huyền Hải"},
            {612,"Đại Côn Di Hài"},
            {613,"Thủy Nguyệt Động Thiên"},
            {614,"Hư Không Huyễn Cảnh"},
            {615,"Đường Gia Bảo"},
            {616,"Đường Gia Bảo"},
            {617,"Diễn Võ Trường"},
            {618,"Đường Gia Bảo"},
            {619,"Đường Môn Thí Luyện"},
            {620,"Thiên Hạ Đệ Nhất Lôi Đài"},
            {621,"Phòng chuẩn bị chiến đấu Tông Sư"},
            {622,"Phòng chuẩn bị chiến đấu Danh Sĩ"},
            {623,"Phòng chuẩn bị chiến đấu Hào Hiệp"},
            {624,"Trong Nhạn Môn Quan"},
            {625,"Thiếu Lâm (Thâu Đêm)"},
            {626,"Diễn Võ Trường"},
            {627,"Diễn Võ Trường"},
            {628,"Đường Gia Bảo"},
            {629,"Vấn Đỉnh Thiên Hạ"},
            {630,"Mật Chiến Thất"},
            {631,"Hi Hoàng Thần Vực"},
            {632,"Oa Hoàng Thần Vực"},
            {633,"Nông Hoàng Thần Vực"},
            {634,"Sào Hoàng Thần Vực"},
            {635,"Nhân Hoàng Thần Vực"},
            {636,"Dung Hoàng Thần Vực"},
            {637,"Công Hoàng Thần Vực"},
            {638,"Thái Hoàng Thần Vực"},
            {639,"Quân Thiên Thành (Đêm)"},
            {640,"Thần Binh Các (Cấp 1)"},
            {641,"Thần Binh Đường (Cấp 2)"},
            {642,"Thần Binh Đàn (Cấp 3)"},
            {643,"Thần Binh Lâu (Cấp 4)"},
            {644,"Thần Binh Điện (Cấp 5)"},
            {645,"Huyết Chiến Nhạn Môn Quan"},
            {646,"Hàn Băng Hải Vực"},
            {647,"Diệt Thế Hỏa Quật"},
            {648,"Cổ Hoặc Linh Cốc"},
            {649,"Phòng dự bị chiến đấu "},
            {650,"Vô Nhai Cảnh"},
            {651,"Viêm Ma Sơn"},
            {652,"Tam Tài Hiệp Cốc (Đơn)"},
            {653,"Tam Tài Hiệp Cốc"},
            {654,"Quần Hùng Lôi"},
            {655,"Quần Hùng Lôi"},
            {656,"Quần Hùng Lôi"},
            {657,"Quần Hùng Lôi"},
            {658,"Tụ Nghĩa Tinh Đàn"},
            {659,"Đạo Trường Sư Môn"},
            {660,"Đào Viên Cảnh"},
            {661,"Giải Đấu Ngôi Sao"},
            {662,"Tam Thần Huyễn Cảnh"},
            {663,"Thiên Thu Điện"},
            {664,"Bất Quy Lâm"},
            {665,"Vô Nhai Hải"},
            {666,"Viêm La Thiên"},
            {667,"Ngọc Hoàng Sơn"},
            {668,"Điện Tiền Quảng Trường"},
            {669,"Tẩm Thủy Đan Lâm"},
            {670,"Vân Dao Thước Lĩnh"},
            {671,"Côn Ngô-Mạc Nam Thanh Nguyên"},
            {672,"Côn Ngô-Vong Xuyên Hoa Hải"},
            {673,"Côn Ngô-Thiên Kỳ Nam Hoài"},
        };

        public static List<string> TrangBiLaoHuu = new List<string>()
        {
            "Cửu Tiêu Lý", // giày 74
            "Toái Phách Ngoa", // giày 74
            "Ưng Dương", // phù 75
            "Phượng Vũ", // nhẫn 75
            "Túc Giao", // phù 75
            "Bách Xuyên", // nhẫn 75
            "Cửu Tiêu Hộ Thủ", // găng 76
            "Toái Phách Chưởng", // găng 76
            "Cửu Tiêu Cầu", // áo 78
            "Toái Phách Khải", // áo 78
            "Thôi Tuyết", // yêu đái 82
            "Túy Vũ", // phù 85     
            "Sương Hiểu", // nhẫn 85
            "Xuyên Mộ", // phù 85
            "Yêu Ngôn", // nhẫn 85
            "Liệt Nhật", // hạng liên 88
            "Toái Phách Khôi", // mão 72
            "Cửu Tiêu Mão", // mão 72            
        };

        public static List<string> ThanKhiDoTuong = new List<string>()
        {
            "Dung Kim Lạc Nhật Đồ",
            "Thu Thủy Vô Ngấn Đồ",
            "Bích Hải Ngân Đào Đồ",
            "Vạn Hách Tùng Phong Đồ",
            "Bích Hải Lăng Ba Đồ Tường",
            "Tử Dương Tuyệt Linh Đồ Tường",
            "Bách Đại Hồng Quang Đồ Tường"
        };

        public static HashSet<string> ThuCuoi40 = new HashSet<string>()
        {
            "Thú Cưỡi: Linh Hồ", // quỷ cốc            
            "Thú Cưỡi: Kim Tiền Báo", // đường môn
            "Thú Cưỡi: Linh Dương", // mộ dung
            "Thú Cưỡi: Mao Ngưu", // tinh túc
            "Thú Cưỡi: Lộc", // tiêu dao
            "Thú Cưỡi: Hổ", // thiếu lâm
            "Thú Cưỡi: Điêu", // thiên sơn
            "Thú Cưỡi: Hoàng Phiêu Mã", // thiên long
            "Thú Cưỡi: Thanh Phụng", // ngamy
            "Thú Cưỡi: Hạc", // võ đang
            "Thú Cưỡi: Sư Tử", // minh giáo
            "Thú Cưỡi: Khôi Lang", // cái bang
            "Thú Cưỡi: Mộc Diên", // Dao Hoa
        };

        public static HashSet<string> ThuCuoi60 = new HashSet<string>()
        {
            "Thú Cưỡi: Xích Vĩ Hồ", // quỷ cốc
            "Thú Cưỡi: Vân Tuyết Báo", // đường môn
            "Thú Cưỡi: Tuyết Linh Dương", // mộ dung
            "Thú Cưỡi: Bạch Mao Ngưu", // tinh túc
            "Thú Cưỡi: Bạch Lộc", // tiêu dao
            "Thú Cưỡi: Bạch Hổ", // thiếu lâm
            "Thú Cưỡi: Bạch Điêu", // thiên sơn
            "Thú Cưỡi: Thanh Bạch Tông Mã", // thiên long
            "Thú Cưỡi: Hồng Bạch Phụng", // ngamy
            "Thú Cưỡi: Kim Dực Hạc", // võ đang
            "Thú Cưỡi: Bạch Sư", // minh giáo
            "Thú Cưỡi: Bạch Lang", // cái bang
            "Thú Cưỡi: Tinh Phong Diên", // Dao Hoa
        };


        // tiêu viễn sơn #{SSS_ZJ_120109_13}
        public static Dictionary<string, string> GameHashStrign = new Dictionary<string, string>()
        {
            { "#{SJZYH_150824_2}|#{SJZ_100129_48}", "Vận chuyển đến Tây Vận Các" },
            { "#{SJZYH_150824_3}|#{SJZ_100129_62}", "Vận chuyển đến Bán Long Các" },
            { "#{SJZYH_150824_4}", "Vận chuyển đến Tinh La Đàn" },
            { "#{SJZ_100129_63}", "Vận chuyển đến Tứ Tuyệt Điện" },                        
        };

        public static HashSet<string> SimpleMission = new HashSet<string>()
        {
            "26|Thiên sư kỳ đãi (1)->Kỳ Vọng Của Thiên Sư (1)|Châu Thiên Sư[256,273-0]|#",
            "28|Thiên sư kỳ đãi (2)->Kỳ Vọng Của Thiên Sư (2)|Châu Thiên Sư[256,273-0]|#",
            "30|Thiên sư kỳ đãi (3)->Kỳ Vọng Của Thiên Sư (3)|Châu Thiên Sư[256,273-0]|#",
            "32|Thiên sư kỳ đãi (4)->Kỳ Vọng Của Thiên Sư (4)|Châu Thiên Sư[256,273-0]|#",
            "35|Thiên sư kỳ đãi (5)->Kỳ Vọng Của Thiên Sư (5)|Châu Thiên Sư[256,273-0]|#",
            "38|Thiên sư kỳ đãi (6)->Kỳ Vọng Của Thiên Sư (6)|Châu Thiên Sư[256,273-0]|#",
            "40|Thiên sư kỳ đãi (7)->Kỳ Vọng Của Thiên Sư (7)|Châu Thiên Sư[256,273-0]|#",
            "42|Thiên sư kỳ đãi (8)->Kỳ Vọng Của Thiên Sư (8)|Châu Thiên Sư[256,273-0]|#",
            "45|Thiên sư kỳ đãi (9)->Kỳ Vọng Của Thiên Sư (9)|Châu Thiên Sư[256,273-0]|#",
            "48|Thiên sư kỳ đãi (10)->Kỳ Vọng Của Thiên Sư (10)|Châu Thiên Sư[256,273-0]|#",
            "59|Thảo Nguyên kiếp phỉ|Tiêu Tường[163,159-20]|Kill{Loan Đao Mã Phỉ[275,150-20]}",
            "60|Hắc Phong thích|Nguyễn lão thái thái[90,200-20]|Kill{Mông Cổ Hắc Phong[255,285-20]}&Pick{8-Mông Cổ Hắc Phong Thích)}|2.62.00", //==
            "60|Đường lương thực yếu|Nguyễn Thực[198,203-20]|Kill{30-Thảo Nguyên Lang[177,250-20]}|3.37.00", //==
            "62|Mệnh lệnh của Quận Chủ|Tiêu Tường[163,159-20]|Kill{35-Thiểm Điện Mã Phỉ[83,233-20]}|3.10.00", //==
            "62|Bằng chứng anh hùng|Tiêu Tường[163,159-20]|Kill{35-Truy Phong Mã Phỉ[192,115-20]}|3.10.00", //==
            "65|Thiêu đốt|Nguyễn Thực[198,203-20]|Kill{40-Đả Thảo Cốc Tống Binh[55,50-20]}|4.18.00", //==
            "65|Nghe danh xuống ngựa|Nguyễn Thành[138,53-20]->Thẩm Vạn Tam[170,73-33]|#|2.53.00", //==
            "65|Cơ hội buôn bán vô hạn|Thẩm Vạn Tam[170,73-33]|Kill{Hồng Bào Tri Thù[43,248-20]}&Pick{10-Tơ Của Hồng Bào Tri Thù}|3.73.00", //==
            "65|Con đường buôn bán gập ghềnh|Thẩm Vạn Tam[170,73-33]|Kill{30-Tùng Lâm Dã Nhân[112,120-20]}|3.62.00", //==
            "65|Vô gian đạo|Thái Dương Hoa[196,49-33]->Cam Thảo[162,274-33]|#|4.1.00", //==
            "67|Kinh Vệ Phân Minh|Thái Dương Hoa[196,49-33]|Kill{30-Sơn Viện Hộ Pháp[231,47-20]}|4.1.00", //==            
            "67|Ăn miếng trả miếng|Cam Thảo[162,274-33]|Kill{30-Sơn Viện Tư Tế[121,74-20]}|3.83.00", //==
            "67|Làm khó|Cam Thảo[162,274-33]->Nguyệt Lý[193,71-33]|#|3.13.00", //==
            "70|Tuyệt thế hảo nam nhi|Đông Thích[163,284-33]->Thẩm Vạn Tam[170,73-33]|#|3.24.00", //==
            "70|Dừng xe Phong Lâm đêm|Thẩm Vạn Tam[170,73-33]->Sách Mẫu Lạp[50,51-27]|#|3.53.00", //==
            "70|La Bốc cần Long Huyết|La Bốc[46,52-27]|Kill{Long Huyết Thạch Nhân[85,266-27]}&Pick{10-Linh Kiện Long Huyết Thạch}|3.1.00", //==
            "70|La Bốc cần Vân Mẫu|La Bốc[46,52-27]|Kill{Vân Mẫu Thạch Nhân[150,103-27]}&Pick{10-Linh Kiện Của Vân Mẫu}|2.98.00", //==
            "70|La Bốc cần Nhện|La Bốc[46,52-27]|Kill{Kịch Độc Lang Thù[83,52-27]}&Pick{10-Chân Của Kịch Độc Tri Thù+10-Kịch Độc Tri Thù Ty}|2.90.00", //==
            "72|Nữ Oa Thạch Nhân|A Hắc[280,47-27]|Kill{35-Nữ Oa Thạch Nhân[154,252-27]}|3.20.00", //==
            "72|Đại Yển Sư|A Hắc[280,47-27]|Kill{35-Đại Yển Sư[268,72-27]}|3.20.00", //==
            "72|Yển Sư Hộ Pháp|A Hắc[280,47-27]|Kill{35-Yển Sư Hộ Pháp[210,208-27]}|3.20.00", //==
            "72|Ca ca ngốc|A Y Na[278,44-27]->La Bốc[46,52-27]|#|2.1.00", //==
            "72|Không hấp bánh bao|Cổ Lỗ Lạp[276,45-27]->Gia Luật Kim[170,207-21]|#|2.56.32", //==
            "75|Bạch Lang Bì|Ba Đồ[159,196-21]|Kill{Bạch Lang Vương[161,268-21]}&Pick{1-Bạch Lang Bì}|5.50.00", //==
            //"75|Đuổi Hắc Phong|Bá Nhan[164,200-21]|Kill{Bạch Lang Vương[161,268-21]}&Pick{Bạch Lang Bì}", Special
            "75|Hắc Phong Mật|Ba Đồ[159,196-21]|Kill{Đại Hắc Phong[84,222-21]}&Pick{1-Hắc Phong Mật}|5.50.00", //==
            "78|Hồng Y Mã Phỉ|Gia Luật Kim[170,207-21]|Kill{40-Hồng Y Mã Phỉ[212,58-21]}|9.65.00", //==
            "78|Hắc Y Mã Phỉ|Gia Luật Kim[170,207-21]|Kill{40-Hắc Y Mã Phỉ[94,98-21]}|9.65.00", //==
            "80|Bắt giặc bắt vua|Gia Luật Kim[170,207-21]|Kill{1-A Sử Na Mặc Cốc[106,60-21]}|5.45.00", //==
            "80|Mãng Cái Tam Kiệt|Gia Luật Kim[170,207-21]|Kill{1-Long Thắng[204,52-21]+1-Tiêu Doãn[222,270-21]+1-Lý Thông[217,47-21]}|5.80.00", //==
            "80|Nghiên cứu chữ Huyết|Diệp Lưu Phàm[115,59-34]|Kill{Nam Vực Ngạc Ngư[70,135-34]}&Pick{6-Nam Vực Ngạc Ngư Huyết Dịch}|5.00.00", //==
            "80|Nhân vật kịch tình|Diệp Lưu Phàm[115,59-34]->Đinh Liên Y[112,60-34]|#|1.80.00", //==
            "80|Năm hạt quýt|Diệp Lưu Phàm[115,59-34]|Kill{Thạch Lão Nhân[154,162-34]}&Pick{5-Quất Hạch}|5.20.0", //==
            "80|Đen tối|Đinh Liên Y[112,60-34]|Kill{35-Ngạc Ngư Bang Tặc Đồ}|5.0.00", //==
            "82|Phòng trống|Đinh Liên Y[112,60-34]->Diệp Lưu Phàm[115,59-34]|#|2.0.0", //==
        };


        public static HashSet<string> UuTien = new HashSet<string>()
        {
            "Cùng Kỳ-Huyễn",
            "Thị Ma Giả",
            "Băng Mặc Hổ",
            "Huyền Mặc Hổ",
            "Hỏa Mặc Hổ",
            "Độc Mặc Hổ",
            "Tinh La Võ Sĩ",
            "Thanh Ưng",
            "Tín đồ",
            "Băng Tằm",
            "Huyết chú vu cổ",
            "Phiên tăng chấp sự",
            "Thổ phồn ác tăng đầu lĩnh",
            "Hắc Sắc Tâm Ma",
            "Nhiếp Hồn",
            "Hoa Độc",
            "Tâm Ma",
            "Vô Tướng Tung Ảnh",
            "Đạo Thư Ác Tăng",
        };

        public static List<string> TrangTuHien = new List<string>()
        {
            "62,118",
            "64,112",
            "64,123",
            "72,112",
            "72,123",
            "74,118",
        };


        public static List<string> MoDungBac = new List<string>()
        {
            "118,40",
            "124,40",
            "126,45",
            "124,50",
            "118,50",
            "116,45",
        };

        public static HashSet<string> Trap = new HashSet<string>()
        {
            "Tướng Tiến Tửu 1",
            "Tướng Tiến Tửu 2",
            "Hồi Âm Phiên",
            "Bẫy hiệu ứng Khai Minh Nộ Hỏa Xung Thiên",
            "Nộ Hỏa Xung Thiên",
            "Nhiên Thiêu Nộ Hỏa",
            "Băng Đông Tam Xích",
            "Địa Sát Trận",
            "Nhân Vong Trận",
            "Thiên Canh Trận",
            "Băng tằm ty (băng)",
            "Băng tằm ty (hoả)",
            "Bẫy Tiêu Dao Nội Tức",
            "Thập Bộ Nhất Sát 1",
            "Thập Bộ Nhất Sát 2",
            "Thập Bộ Nhất Sát 3",
            "Thập Bộ Nhất Sát 4",
            "Thập Bộ Nhất Sát 5",
            "Thập Bộ Nhất Sát 6",
            "Thập Bộ Nhất Sát 7",
            "Thập Bộ Nhất Sát 8",
            "Thập Bộ Nhất Sát 9",
            "Thập Bộ Nhất Sát 10",
            "Băng Tuyết Thiên",
            "Liệt Diệm Phần Thân",
            "Thùy Nhập Địa Ngục",
            "Bách Độc Triền Linh",
            "Cao Bạo Đạn",
            "Sát thương Vi Đà Chưởng",
            "Sát thương La Hán Quyền",
            "Sát thương Lăng Ba Vi Bộ",
            "Bẫy Cờ",
            "NPC Hắc Thủy",
            "Thích Cốt Hàn Sương",
            "Cuồng Phong Vũ",
        };

        public static List<string> TamThanRound3 = new List<string>()
        {
            "132,162",
            "142,152",
            "122,152",
            "132,142",
            "132,152"
        };

        public static List<string> TamThanRound1 = new List<string>()
        {
            "52,122",
            "62,112",
            "52,102",
            "42,112",
            "52,112"
        };


        public static List<string> TamThanRound2 = new List<string>()
        {
            "126,74",
            "136,64",
            "126,54",
            "116,64",
            "126,64"
        };


        public static Dictionary<int, string> BlackListSkill = new Dictionary<int, string>()
        {
            { 0, "Tấn công" }, // TaskTools2_13
            { 1, "Thu phục" }, // PetSkill2_4
            { 21, "Triệu hồi" }, // RideHeader1_1
            { 22, "Về Đại Lý Thành" }, // TaskTools4_1
            { 28, "Tinh Túc Khinh Công" }, // Shoes2_5
            { 34, "Tân Thủ Khinh Công" }, // Shoes2_4
            { 35, "Ngồi thiền" }, // MenpaiLiveSkill2_7
            { 249, "Cạm Bẫy Thiêu Đốt" }, // FightSkillXinShou_7
            { 278, "Đổi Bảo Vật Tiện Lợi" }, // CircularTaskTool43_2
        };

        public static Dictionary<string, string> YuanBaoFood = new Dictionary<string, string>()
        {
            { "PetFood_12", "Trân Thú Hồi Thần Đan"},
            { "PetFood_11", "Trân Thú Vạn Bổ Đan"},
            { "PetFood_16", "Trân Thú Hồi Xuân Đan"},
            { "PetFood_15", "Trân Thú Tư Bổ Đan"},
            { "PetBauble_4", "Bóng Nhiều Màu" },
        };

        // #{MZPVE_150812_360} -- Vô sở bất tri thám kỳ văn
        // #{MZPVE_150812_971} -- Chúc tửu cộng tế kiến mộc đài
        // #{MZPVE_150812_1346} -- Phá Băng Ngư Đường Tập Ngư Thuật
        // #{MZPVE_150812_1720} -- Dung Thiết Luyện Lô Tu Chú Tạo
        // #{MZPVE_150812_2094} -- Bách Luyện Đoàn Đài Chú Kiếm Thành

        public static Dictionary<string, string> ChienMinhQuestHoTong = new Dictionary<string, string>()
        {
            { "Tây lương khoáng xa xuất Lôi Tuyệt", "#{MZPVE_150812_80}" },
            { "Thái cư phiêu nhiên huề thủ hành", "#{MZPVE_150812_831}" },
            { "Kính hải vô nhai hộ khoáng quy", "#{MZPVE_150812_1203}" },
            { "Võ Khố Manh Oa Bất Hư Hành", "#{MZPVE_150812_1577}" },
            { "Xe Khoáng hiểm xuất Ngọc Hoàng Sơn", "#{MZPVE_150812_1950}" },
        };

        public static Dictionary<string, string> ChienMinhQuestThuThap = new Dictionary<string, string>()
        {
            { "Diệu biện chân giả thủ sơn đồng", "#{MZPVE_150812_245}" },
            { "Hoang mộc thiên tải sinh quỳnh hoa", "#{MZPVE_150812_904}" },
            { "Chân giả xà lung biện hồng hoàn", "#{MZPVE_150812_1279}" },
            { "Phù Tang xán nhiên kim bạc sắc", "#{MZPVE_150812_1653}" },
            { "Linh tê cổ mộc biện chân ngụy", "#{MZPVE_150812_2026}" },
           
        };

        public static Dictionary<string, string> ChienMinhItemThuThap = new Dictionary<string, string>()
        {
            { "Nước đường ngon", "CircularTaskTool27_9" }, // Thiên Thu
            { "Mộc Linh Thần Nhãn", "taiwanzhuanyong3_16" },
            { "Huyễn Hải Trúc Địch", "Icons01_2" },
            { "Phù Tang La Bàn", "CommonLiveSkill2_6" },
            { "Thổ Linh Chú Phù", "OtherTools2_6" },
        };

        public static Dictionary<string, string> ChienMinhQuestDuocPho = new Dictionary<string, string>()
        {
            { "Thủ sơn bách niên hà thủ ô", "#{MZPVE_150812_360}" },
            { "Dược giả liên tâm tầm tiên thảo", "#{MZPVE_150812_1018}" },
            { "Khả thán băng liên a na ý", "#{MZPVE_150812_1394}" },
            { "Hỏa diệm chi địa tầm kỳ hoa", "#{MZPVE_150812_1768}" },
            { "Ngọc Hoàng tiên sơn dao thảo phương", "#{MZPVE_150812_2142}" },            
        };        

        public static Dictionary<string, string> ChienMinhQuestUseItem = new Dictionary<string, string>() // done
        {
            { "Vô sở bất tri thám kỳ văn", "#{MZPVE_150812_360}" },
            { "Chúc tửu cộng tế kiến mộc đài", "#{MZPVE_150812_971}" },
            { "Phá Băng Ngư Đường luyện ngư thuật", "#{MZPVE_150812_1346}" },
            { "Dung thiết luyện lô tu chú tạo", "#{MZPVE_150812_1720}" },
            { "Bách Luyện đoàn đài chú kiếm thành", "#{MZPVE_150812_2094}" },
            { "Dựng lò rèn đúc", "Dựng lò rèn đúc" },
            { "Cùng xây tế đài", "Cùng xây tế đài" },
            { "Nghe ngóng chuyện lạ", "Nghe ngóng chuyện lạ" },
            { "Câu cá trên băng", "Câu cá trên băng" },
            {"Đúc kiếm thành công","Đúc kiếm thành công" }

        };


        

        public static Dictionary<string,string> ChienMinhItemUse = new Dictionary<string, string>()
        {
            // Thiên Thu Điện
            { "Ma Lạt Huyền Hải Hà", "CircularTaskTool73_2" },
            { "Quân Thiên Dã Sơn Khuẩn", "CircularTaskTool73_3" },
            { "Tây Lương Bồ Đào", "CircularTaskTool71_7" },
            { "Kim Trản Mỹ Tửu", "CircularTaskTool71_11" },
            { "Gà Nướng", "Food_11" },
            // Bất Quy Lâm
            { "Liềm Cắt Cỏ", "CircularTaskTool73_5" },
            { "Chổi", "TaskTools5_4" },
            { "Bách Hợp Hoa", "Flower_2" },
            { "Điện Thờ", "playerhouse7_3" },
            { "Nến Tế Tự", "Merchandise1_11" },
            // Vô Nhai Hải
            { "Ngư Đường Dược Tễ", "CircularTaskTool4_8"},
            { "Tiểu Quỷ Ngư", "CircularTaskTool72_3"},
            { "Mồi Câu", "OtherTools2_11"},
            { "Tiểu Hồng Ngư", "CircularTaskTool72_2"},
            { "Tiểu Băng Ngư", "CircularTaskTool72_1"},
            // Viêm La Thiên
            { "Kim Nguyên Khoáng", "CircularTaskTool74_2" },
            { "Đồng Nguyên Khoáng", "CircularTaskTool73_16" },
            { "Tích Nguyên Khoáng", "CircularTaskTool74_3" },
            { "Thiết Nguyên Khoáng", "CircularTaskTool74_1" },
            { "Thái Nguyên Khoáng", "CircularTaskTool74_4" },
            // Hoàng Ngọc Sơn
            { "Hàn Thiết Kiếm Phôi", "CircularTaskTool72_7" },
            { "Thúy Ngọc Kiếm Phôi", "CircularTaskTool72_6" },
            { "Hồng Đồng Kiếm Phôi", "CircularTaskTool72_8" },
            { "Huyết Thiết Kiếm Phôi", "CircularTaskTool72_4" },
            { "Thanh Đồng Kiếm Phôi", "CircularTaskTool72_5" },
        };

        public static Dictionary<string, string> ChienMinhDuocPho = new Dictionary<string, string>()
        {
            { "Thiên Hoang Dược Phổ", "CircularTaskTool5_2" },
            { "Cuốc Ngũ Hành", "CircularTaskTool71_8" },
        };

        public static Dictionary<string, string> ChienMinhQuestMatTham = new Dictionary<string, string>()
        {
            { "Tìm mật thám Thiên Thu Kỳ Công", "#{MZPVE_150812_2344}" },
            { "Tìm mật thám Bất Quy Kỳ Công", "#{MZPVE_150812_2378}" },
            { "Tìm Mật Thám Vô Nhai Kỳ Công", "#{MZPVE_150812_2403}" },
            { "Tìm Mật Thám Viêm La Kỳ Công", "#{MZPVE_150812_2428}" },
            { "Tìm mật thám Ngọc Hoàng Kỳ Công", "#{MZPVE_150812_2453}" },
        };

        public static Dictionary<int, string> ThuCuoi80 = new Dictionary<int, string>()
        {
            { 10141153, "Thú Cưỡi: Lưu Ly Phụng"}, // ngamy
            { 10141157, "Thú Cưỡi: Ngân Nguyệt Lang"}, // caibang
            { 10141161, "Thú Cưỡi: Liệt Diệm Sư"}, // minhgiao
            { 10141165, "Thú Cưỡi: Như Ý Hổ"}, // thieulam
            { 10141169, "Thú Cưỡi: Long Huyết Mã"}, // thienlong
            { 10141173, "Thú Cưỡi: Vụ Ảnh Điêu"}, // thienson
            { 10141177, "Thú Cưỡi: Thuỵ Liên Hạc"}, // vodang
            { 10141181, "Thú Cưỡi: Thất Thái Lộc"}, // tieudao
            { 10141222, "Thú Cưỡi: U Quang Linh"}, // modung           
            { 10141185, "Thú Cưỡi: Thanh Mao Ngưu"}, // tinhtuc
            { 10141495, "Thú Cưỡi: U Linh Báo"}, // duongmon
        };



        public static HashSet<string> BoQua = new HashSet<string>()
        {
            "Bích Lân Cương Thi",
            "Thực Phẩm Hỏng",
            "Nguyệt Lý",
            "Yến Tử Ổ trang đinh",
            "Công Dã Càn",
            "Bao Bất Đồng",
            "Đặng Bách Xuyên",
            "Nhất Phẩm Đường Võ Sĩ",
            "Top Hall Samurais",
            "Swallow's Dock Servant",
            "Attendant Gung",
            "Attendant Bao",
            "Attendant Fong",
        };

        public static HashSet<string> YenTuOBoQua = new HashSet<string>()
        {
         
        };

      

        public static List<string> NgocThoiTrangCap1 = new List<string>()
        {
            "Băng Lam Lưu Vân-Cước (Cấp 1)",
            "Băng Lam Lưu Vân-Khiên (Cấp 1)",
            "Băng Lam Lưu Vân-Yêu (Cấp 1)",
            "Hoa Lạc Hồng Trần-Cước (Cấp 1)",
            "Hoa Lạc Hồng Trần-Khiên (Cấp 1)",
            "Hoa Lạc Hồng Trần-Yêu (Cấp 1)",
            "Thúy Ngọc Tinh Trần-Cước (Cấp 1)",
            "Thúy Ngọc Tinh Trần-Khiên (Cấp 1)",
            "Thúy Ngọc Tinh Trần-Yêu (Cấp 1)",
            "Tranh Ảnh Như Mộng-Cước (Cấp 1)",
            "Tranh Ảnh Như Mộng-Khiên (Cấp 1)",
            "Tranh Ảnh Như Mộng-Yêu (Cấp 1)",
            "Tử Vi Tinh Quang-Cước (Cấp 1)",
            "Tử Vi Tinh Quang-Khiên (Cấp 1)",
            "Tử Vi Tinh Quang-Yêu (Cấp 1)",
            "Toái Toàn Tinh Thần-Cước (Cấp 1)",
            "Toái Toàn Tinh Thần-Khiên (Cấp 1)",
            "Toái Toàn Tinh Thần-Yêu (Cấp 1)",            
            "Diệu Vũ Phương Lan-Cước (Cấp 1)",
            "Diệu Vũ Phương Lan-Khiên (Cấp 1)",
            "Diệu Vũ Phương Lan-Yêu (Cấp 1)",
            "Điệp Ảnh Tâm Hoa-Khiên (Cấp 1)",            
            "Điệp Ảnh Tâm Hoa-Yêu (Cấp 1)",
            "Điệp Ảnh Tâm Hoa-Cước (Cấp 1)",
            "Thước Vũ Hồng Liên-Cước (Cấp 1)",
            "Thước Vũ Hồng Liên-Khiên (Cấp 1)",
            "Thước Vũ Hồng Liên-Yêu (Cấp 1)",
        };

        public static HashSet<string> VuKhiDaTaoDo = new HashSet<string>()
        {
            "Đao Phủ",
            "Thương Bổng",
            "Đơn Đoản",
            "Song Đoản",
            "Tiêu Kiếm",
            "Phiến",
            "Hoàn",
            "Nỏ",                      
            "Trường Trượng",            
        };


        // "Điệp Ảnh Tâm Hoa-Cước (Cấp 1)",







        public static List<string> PhungMinhVuongLang = new List<string>()
        {
            //"39,39",
            //"57,40",
            //"56,57",
            //"40,56",
            //"49,49",
            "43,54",
            "53,54",
            "56,48",
            "53,42",
            "43,42",
            "40,48",
            //"44,40",
            //"52,40",
            //"54,48",
            //"52,54",
            //"44,54",
            //"42,48",
            //"47,48",
        };

        public static List<string> PhungMinhVuongLangEx = new List<string>()
        {
            //"39,39",
            //"57,40",
            //"56,57",
            //"40,56",
            //"49,49",
            "43,54",
            "53,54",
            "56,48",
            "53,42",
            "43,42",
            "40,48",
            //"44,40",
            //"52,40",
            //"54,48",
            //"52,54",
            //"44,54",
            //"42,48",
            //"47,48",
        };

   

        // 36,30-36,36-30,38-24,36-24,30-30,28
        public static List<string> LoiDaiSinhTu = new List<string>()
        {
            "36,29",
            "36,37",
            "30,39",
            "23,37",
            "23,29",
            "30,27",
        };

        public static List<string> TuTuyetTrang = new List<string>()
        {
            "25,95",
            "32,95",
            "34,100",
            "32,106",
            "25,106",
            "22,100"
        };

        public static Dictionary<string, int> ThucAnPet = new Dictionary<string, int>()
        {

        };

        // is huyet te delay = 353
    }
}
