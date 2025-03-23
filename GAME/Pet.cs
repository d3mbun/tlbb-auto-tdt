using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class Pet
    {
        // 6c 70
 
        
        public uint NgoaiCong { get; set; }
        public uint NoiCong { get; set; }
        public uint PetAddress { get; set; }
        public uint PetDome { get; set; }
        public uint Lvl { get; set; }
        public string PetId { get; set; } = string.Empty;
        public uint TuChatNgoaiCong { get; set; }
        public uint TuChatNoiCong { get; set; }

 

        public uint TuChatTheLuc { get; set; }
        public uint TuChatThanPhap { get; set; }
        public uint TuChatTriLuc { get; set; }
        public string PetName { get; set; } = string.Empty;
        public bool IsFight { get; set; }

        public uint idx { get; set; }

        public bool IsNoiCong
        {
            get
            {
                if (TuChatNoiCong > TuChatNgoaiCong && TuChatNoiCong > TuChatTheLuc && TuChatNoiCong > TuChatTriLuc && TuChatNoiCong > TuChatThanPhap)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsNgoaiCong
        {
            get
            {
                if (TuChatNgoaiCong > TuChatNoiCong && TuChatNgoaiCong > TuChatTheLuc && TuChatNgoaiCong > TuChatTriLuc && TuChatNgoaiCong > TuChatThanPhap)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsTheLuc
        {
            get
            {
                if (TuChatTheLuc > TuChatNgoaiCong && TuChatTheLuc > TuChatNoiCong && TuChatTheLuc > TuChatTriLuc && TuChatTheLuc > TuChatThanPhap)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsThanPhap
        {
            get
            {
                if (TuChatThanPhap > TuChatNgoaiCong && TuChatThanPhap > TuChatNoiCong && TuChatThanPhap > TuChatTheLuc && TuChatThanPhap > TuChatTriLuc)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
