//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag
using System.Drawing;
using System.IO;
using System.IO.Enumeration;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace Plukliste
{
    class PluklisteProgram
    {
        //app state
        public static char readKey = ' ';
        public static int CurrentFileIndex = 0;
        
        static void Main()
        {
            Xml xmlFiles = new("filesToImport", "export");
            xmlFiles.ImportFiles();
            while (readKey != 'Q')
            {
                xmlFiles.DisplayOneFile(CurrentFileIndex);
                PrintOperationOptions(CurrentFileIndex, xmlFiles.Files);
                PerformOperation(xmlFiles.Pluklists[CurrentFileIndex]);
            }
        }
        public static void PerformOperation(Pluklist pluklist)
        {
            readKey = Console.ReadKey().KeyChar;
            if (readKey >= 'a') readKey -= (char)('a' - 'A'); //HACK: To upper
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red; //status in red

            Xml xmlFiles = new("filesToImport", "export");
            xmlFiles.ImportFiles();
            
            switch (readKey)
            {
                case 'G':
                    CurrentFileIndex = 0;
                    Console.WriteLine("Pluklister genindlæst");
                    break;
                case 'F':
                    if (CurrentFileIndex > 0) CurrentFileIndex--;
                    break;
                case 'N':
                    if (CurrentFileIndex < xmlFiles.Files.Count - 1) CurrentFileIndex++;
                    break;
                case 'A':
                    //Move files to import directory
                    var filewithoutPath = xmlFiles.Files[CurrentFileIndex].Substring(xmlFiles.Files[CurrentFileIndex].LastIndexOf('\\'));
                    System.IO.File.Move(xmlFiles.Files[CurrentFileIndex], string.Format(@"filesToImport\\{0}", filewithoutPath));
                    Console.WriteLine($"Plukseddel {xmlFiles.Files[CurrentFileIndex]} afsluttet.");
                    xmlFiles.Files.Remove(xmlFiles.Files[CurrentFileIndex]);
                    if (CurrentFileIndex == xmlFiles.Files.Count) CurrentFileIndex--;
                    break;
                case 'P':
                    Html htmlFile = new("templates", "letters");
                    htmlFile.Pluklist = pluklist;
                    htmlFile.ImportFiles();
                    htmlFile.UpdateFiles();
                    htmlFile.ExportFiles(2);
                    break;
            }
        }
        public static void PrintOperationOptions(int CurrentFileIndex, List<string> files)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\nOptions:");
            PrintOption("Q", "uit");

            if (CurrentFileIndex >= 0)
            {
                PrintOption("A", "fslut plukseddel");
                PrintOption("P", "rint ordrebekræftelse");
            }
            if (CurrentFileIndex > 0)
            {
                PrintOption("F", "orrige plukseddel");
            }
            if (CurrentFileIndex < files.Count - 1)
            {
                PrintOption("N", "æste plukseddel");
            }
            PrintOption("g", "enindlæs pluksedler");
        }
        public static void PrintOption(string letter, string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(letter);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }
    }
}


