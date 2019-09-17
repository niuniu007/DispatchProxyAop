using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Mail:IMail
    {
        public string Send(string name)
        {
            Console.WriteLine(name);
            return $"你的名字是{name}";
           
        }
    }
}
