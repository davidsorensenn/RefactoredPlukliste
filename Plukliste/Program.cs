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
        public static List<string> files;
        static void Main()
        {
            ColorToStandard();
            Directory.CreateDirectory("export");
            Directory.CreateDirectory("letter");


            if (!Directory.Exists("filesToImport"))
            {
                Console.WriteLine("Directory \"export\" not found");
                Console.ReadLine();
                return;
            }

            files = ReadFiles();


            while (readKey != 'Q')
            {



                Console.WriteLine($"Plukliste {CurrentFileIndex + 1} af {files.Count}");
                Console.WriteLine($"\nfile: {files[CurrentFileIndex]}");

                //read file
                var plukliste = readFile(files[CurrentFileIndex]);


                if (ValidatePlukliste(plukliste))
                {
                    PrintPluklistDetails(plukliste);
                }
                PrintOperationOptions(CurrentFileIndex, files);
                PerformOperation(plukliste);
            }
        }
        public static void PerformOperation(Pluklist pluklist)
        {
            readKey = Console.ReadKey().KeyChar;
            if (readKey >= 'a') readKey -= (char)('a' - 'A'); //HACK: To upper
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red; //status in red
            switch (readKey)
            {
                case 'G':
                    files = ReadFiles();
                    CurrentFileIndex = 0;
                    Console.WriteLine("Pluklister genindlæst");
                    break;
                case 'F':
                    if (CurrentFileIndex > 0) CurrentFileIndex--;
                    break;
                case 'N':
                    if (CurrentFileIndex < files.Count - 1) CurrentFileIndex++;
                    break;
                case 'A':
                    //Move files to import directory
                    var filewithoutPath = files[CurrentFileIndex].Substring(files[CurrentFileIndex].LastIndexOf('\\'));
                    System.IO.File.Move(files[CurrentFileIndex], string.Format(@"filesToImport\\{0}", filewithoutPath));
                    Console.WriteLine($"Plukseddel {files[CurrentFileIndex]} afsluttet.");
                    files.Remove(files[CurrentFileIndex]);
                    if (CurrentFileIndex == files.Count) CurrentFileIndex--;
                    break;
                case 'R':
                    //Move files to import directory
                    FileHandler fileHandler = new FileHandler("templates", "letters");

                    PrintLetter(pluklist, fileHandler, 2);
                    
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
            //reset color
        }
        public static Pluklist? readFile(string CurrentFile)
        {
            using (FileStream file = System.IO.File.OpenRead(CurrentFile))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer =
                           new System.Xml.Serialization.XmlSerializer(typeof(Pluklist));
                var plukliste = (Pluklist?)xmlSerializer.Deserialize(file);
                return plukliste;
            }


        }
        public static List<string>? ReadFiles()
        {
            files = Directory.EnumerateFiles("filesToImport").ToList();
            if (files.Count == 0)
            {
                Console.WriteLine("No files found.");
                return null;
            }
            else return files;
        }
        public static bool ValidatePlukliste(Pluklist? plukliste)
        {
            bool valid = false;
            if (plukliste != null)
            {
                if (plukliste.IsValid) valid = true;
            }
            return valid;
        }
        public static void PrintPluklistDetails(Pluklist plukliste)
        {
            Console.WriteLine("\n{0, -13}{1}", "Name:", plukliste.Name);
            Console.WriteLine("{0, -13}{1}", "Forsendelse:", plukliste.Forsendelse);
            Console.WriteLine("{0, -13}{1}", "Adresse:", plukliste.Adresse);

            //TODO: Add adresse to screen print

            Console.WriteLine("\n{0,-7}{1,-9}{2,-20}{3}", "Antal", "Type", "Produktnr.", "Navn");
            foreach (var item in plukliste.Lines)
            {
                Console.WriteLine("{0,-7}{1,-9}{2,-20}{3}", item.Amount, item.Type, item.ProductID, item.Title);
            }
        }
        public static void PrintOperationOptions(int CurrentFileIndex, List<string> files)
        {
            //Print options
            Console.WriteLine("\n\nOptions:");
            PrintOption("Q", "uit");

            if (CurrentFileIndex >= 0)
            {
                PrintOption("A", "fslut plukseddel");
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
        public static void ColorToGreen()
        {
            Console.ForegroundColor = ConsoleColor.Green;

        }
        public static void ColorToStandard()
        {
            Console.ForegroundColor = ConsoleColor.White;

        }
        public static void PrintOption(string letter, string message)
        {
            ColorToGreen();
            Console.Write(letter);
            ColorToStandard();
            Console.WriteLine(message);
        }
        public static void PrintLetter(Pluklist pluklist, FileHandler fileHandler, int type)
        {
            //Arange
            fileHandler.Pluklist = pluklist;
            
           
            //act
            fileHandler.ImportFiles();
            fileHandler.UpdateFiles();
            fileHandler.ExportFiles(2);
            Console.WriteLine("File was added to path \\letter");
        }

    }
}


