using Microsoft.VisualBasic;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plukliste
{
    public abstract class FileHandler
    {
        //constructor
        public FileHandler(string importPath, string exportPath)
        {
            Files = new List<string>();
            ImportPath = importPath;
            ExportPath = exportPath;
        }

        //fields
        public string? ImportPath { get; set; }
        public string? ExportPath { get; set; }
        public List<string> Files { get; set; }

        public bool IsValid => Validate();

        //methods
        public bool Validate()
        {

            if (string.IsNullOrEmpty(ImportPath))
            {
                Console.WriteLine("Import path not set");
                return false;
            }
            if (string.IsNullOrEmpty(ExportPath))
            {
                Console.WriteLine("Export path not set");
                return false;
            }
            return true;
        }
        public virtual void ImportFiles()
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
                    var file = File.ReadAllText(fileName);

                    Files.Add(file);
                }
            }
        }
        public void ImportFactory()
        {
            if (!this.IsValid)
            {
                return;
            }
            foreach (var file in Files)
            {
                if (file.Substring(file.LastIndexOf(".")) == ".html")
                {

                }
                if (file.Substring(file.LastIndexOf(".")) == ".xml")
                {

                }
                if (file.Substring(file.LastIndexOf(".")) == ".csv")
                {

                }
            }

        }
    }
}
