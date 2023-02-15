using Microsoft.VisualBasic;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plukliste
{
    internal class FileHandler
    {
        public FileHandler(string importPath, string exportPath)
        {
            Files = new List<string>();
            ImportPath = importPath;
            ExportPath = exportPath;
        }


        public string? ImportPath { get; set; }
        public string? ExportPath { get; set; }
        public List<string> Files { get; set; }
        public Pluklist Pluklist { get; set; }
        

        //validate
        public bool Validate()
        {
            //if (Files.Count == 0)
            //{
            //    Console.WriteLine("No filesNames found");
            //    return false;
            //}
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
            if (Files.Count == 0)
            {
                Console.WriteLine("No filesNames found");
                return false;
            }

            return true;
        }
        public void ImportFiles()
        {
            try
            {
                List<string> filesNames = Directory.EnumerateFiles(ImportPath).ToList();
                if (filesNames.Count == 0)
                {
                    Console.WriteLine("No filesNames found.");
                    return;
                }
                if (ImportPath == null)
                {
                    return;
                }
                foreach (var fileName in filesNames)
                {
                    var file = File.ReadAllText(fileName);
                    Files.Add(file);
                }
            } catch(Exception error) {
                Console.WriteLine(error.ToString());
            }

        }
        public void UpdateFiles()
        {
            if (this.Validate())
            {
                List<string> UpdateFiles = new List<string>();
                foreach(string file in Files)
                {
                    string updatedFile = file.Replace("[Adresse]", Pluklist.Adresse);
                    updatedFile = updatedFile.Replace("[Name]", Pluklist.Name);
                    updatedFile = updatedFile.Replace("<head>", "<head> <style>table, th, td{border: solid black 1px;}</style>");


                    string pluklist = "<table style=\"width:100%;\"> <tr> <th>Title</th> <th>Amount</th>";
                    foreach(var line in Pluklist.Lines)
                    {
                        pluklist += line.ToString() + "\n";
                    }
                    pluklist += "</table>";
                    updatedFile = updatedFile.Replace("[Plukliste]", pluklist);

                    UpdateFiles.Add(updatedFile);
                }
                Files = UpdateFiles;
            }
        }
        public void ExportFiles(int type )
        {
            string path = Path.Combine("letter", $"{Pluklist.Name}.html");
            string html = Files[type];
            File.WriteAllText(path, html);
            Console.WriteLine("File was added to {0}", Path.Combine(Environment.CurrentDirectory, path));

        }
    }
}
