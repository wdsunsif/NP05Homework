using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    enum HttpCommand
    {
        GET = 1,
        POST = 2,
        PUT = 3,
        DELETE = 4,
    }
    internal class Command
    {
        public HttpCommand HttpCommand { get; set; }
        public Car Value { get; set; }
        public int Index { get; set; }
    }
}
