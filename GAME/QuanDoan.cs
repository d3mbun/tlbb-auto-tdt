using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class QuanDoan
    {
        public IEnumerable<Game> Party1 { get; set; } = Enumerable.Empty<Game>();
        public IEnumerable<Game> Party2 { get; set; } = Enumerable.Empty<Game>();
    }
}
