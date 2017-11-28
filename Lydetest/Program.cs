using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LydSemesterModtagBroadcast;

namespace Lydetest
{
    class Program
    {
        static void Main(string[] args)
        {
            Service1 service = new Service1();

            service.GetAllLyd();
        }
    }
}
