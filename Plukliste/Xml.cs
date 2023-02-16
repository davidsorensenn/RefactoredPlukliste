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
            Directory.CreateDirectory(exportPath);

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
        public void ExportFiles(int index)
        {


            var fileName = Files[index];
            fileName.Substring(1);

            string exportPath = Path.Combine(Environment.CurrentDirectory, ExportPath, fileName.Substring(fileName.LastIndexOf("\\") + 1));



            File.Move(fileName, exportPath);
            Console.WriteLine("File moved to {0}", Path.Combine(Environment.CurrentDirectory, ExportPath, $"{Pluklists[index].Name}.xml"));
            Pluklists.RemoveAt(index);
            Files.RemoveAt(index);
            //TODO: this donsn't delete the file. only the variables.
            //the files should be deleated from Import 
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

    interface IImportFile
    {
        List<Pluklist> Read(string path);
    }

    class ImportXML : IImportFile
    {
        public List<Pluklist> Read(string path)
        {
            //code that reads and return public Pluklist Read(string path)
            List<Pluklist> list = new List<Pluklist>();

            List<string> filesNames = Directory.EnumerateFiles(path).ToList();
            if (filesNames.Count == 0)
            {
                Console.WriteLine("Folder is empty.");
                return null;
            }
            foreach (var fileName in filesNames)
            {

                using (FileStream fileStream = File.OpenRead(fileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Pluklist));
                    var pluklist = (Pluklist?)serializer.Deserialize(fileStream);
                    list.Add(pluklist);
                }
            }
            return list;
        }
    }

    class ImportCSV : IImportFile
    {
        public List<Pluklist> Read(string path)
        {
            throw new NotImplementedException();
        }
    }
}
