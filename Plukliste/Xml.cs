using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace Plukliste
{
    public class Xml : FileHandler
    {
        //constructor
        public Xml(string importPath, string exportPath) : base(importPath, exportPath)
        {
            Directory.CreateDirectory("export");
        }
        
        //fields
        public List<Pluklist> Pluklists = new List<Pluklist>();

        //methods
        //Extends the base class method and convert the files to objects
        public override void ImportFiles()
        {
            if (this.IsValid)
            {
                List<string> filesNames = Directory.EnumerateFiles(ImportPath).ToList();
                if (filesNames.Count == 0)
                {
                    Console.WriteLine("Folder is empty.");
                    return;
                }
                foreach (var fileName in filesNames)
                {
                    Files.Add(fileName);
                    using (FileStream fileStream = File.OpenRead(fileName))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Pluklist));
                        
                        var pluklist = (Pluklist?)serializer.Deserialize(fileStream);
                        Pluklists.Add(pluklist);
                    } 
                }
            }
        }
        public bool validate()
        {
            if (!Directory.Exists("filesToImport"))
            {
                Console.WriteLine("Directory \"filesToImport\" not found");
                Console.ReadLine();
                return false;
            }
            if (Files.Count == 0)
            {
                Console.WriteLine("Folder is empty.");
                return false;
            }
            if (Pluklists.Count == 0)
            {
                Console.WriteLine("Folder is empty.");
                return false;
            }
            return true;
        }
        public void DisplayOneFile(int index)
        {
            
            if (!this.IsValid)
            {
                return;
            }
            if (index > Files.Count - 1 || index < 0)
            {
                Console.WriteLine("Index out of range.");
                return;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Plukliste {index + 1} af {Files.Count}");
            Console.WriteLine($"\nfile: {Files[index]}");
            Console.WriteLine("\n{0, -13}{1}", "Name:", Pluklists[index].Name);
            Console.WriteLine("{0, -13}{1}", "Forsendelse:", Pluklists[index].Forsendelse);
            Console.WriteLine("{0, -13}{1}", "Adresse:", Pluklists[index].Adresse);

            //TODO: Add adresse to screen print

            Console.WriteLine("\n{0,-7}{1,-9}{2,-20}{3}", "Antal", "Type", "Produktnr.", "Navn");
            foreach (var item in Pluklists[index].Lines)
            {
                Console.WriteLine("{0,-7}{1,-9}{2,-20}{3}", item.Amount, item.Type, item.ProductID, item.Title);
            }
        }


    }
}
