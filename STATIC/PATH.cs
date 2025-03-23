using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class PATH
    {
        public static string Game
        {
            get
            {
                return _i.Game.IniParser.Read("Config", "GamePath");
            }
            set
            {
                _i.Game.IniParser.Write("Config", "GamePath", value);
            }
        }
    }
}
