using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP05HomeworkServer
{
    static class Counter
    {
       static public int IdCounter { get; set; } = 1;
    }
    internal class Car
    {
        public int Id { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        public Car( )
        {
            Id = Counter.IdCounter++;
        }
    }
}
