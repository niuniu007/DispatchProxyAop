using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class TestHelo : ITestHelo
    {
        public string Helo(string name)
        {
            return name;
        }
    }
}
