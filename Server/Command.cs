using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    enum HttpCommand
    {
        GET=1,
        POST=2,
        PUT=3,
        DELETE=4,
    }
    internal class Command
    {
        public HttpCommand HttpCommand { get; set; }
        public Car Value { get; set; }
        public int Index { get; set; }
    }
}
