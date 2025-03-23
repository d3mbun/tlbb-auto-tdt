using System;
using System.Collections.Generic;
using System.Text;

namespace _i
{
    class URL
    {
        public static string HomePage
        {
            get
            {
                return "https://micro.tieudattai.org/";
            }
        }

        public static string Prefix
        {
            get
            {
                return "microauto";
            }
        }

        public static string LOGIN
        {
            get
            {
                return HomePage + Prefix + "/user.php";
            }
        }

        public static string REGISTER
        {
            get
            {
                return HomePage + Prefix + "/register.php";
            }
        }
    }
}
