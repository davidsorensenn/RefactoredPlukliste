using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plukliste
{
    internal class Printer
    {
        public string Path { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public List<Pluklist> pluklists { get; set; }
        public Printer(string path, string address, string name)
        {
            Path = path;
            Address = address;
            Name = name;
            pluklists = new List<Pluklist>();
        }
        public void AddPluklist(Pluklist pluklist)
        {
            pluklists.Add(pluklist);
        }
        public void Print()
        {
           //output html file
        }
    }
}
