//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag
using System.Drawing;

namespace Plukliste
{
    class PluklisteProgram
    {

        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.White;

            Directory.CreateDirectory("import");

            string path = @"C:\Users\kenn7\Desktop\export";
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Directory \"export\" not found");
                Console.ReadLine();
                return;
            }
            List<string> files;

            files = Directory.EnumerateFiles(path).ToList();
            if (files.Count == 0)
            {
                Console.WriteLine("No files found.");
                return;
            }

            var CurrentFileIndex = 0;
            char readKey = ' ';
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




                PrintOptions(CurrentFileIndex, files);

                readKey = Console.ReadKey().KeyChar;
                if (readKey >= 'a') readKey -= (char)('a' - 'A'); //HACK: To upper
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Red; //status in red
                switch (readKey)
                {
                    case 'G':
                        files = Directory.EnumerateFiles(path).ToList();
                        CurrentFileIndex = -1;
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
                        File.Move(files[CurrentFileIndex], string.Format(@"import\\{0}", filewithoutPath));
                        Console.WriteLine($"Plukseddel {files[CurrentFileIndex]} afsluttet.");
                        files.Remove(files[CurrentFileIndex]);
                        if (CurrentFileIndex == files.Count) CurrentFileIndex--;
                        break;
                }
                Console.ForegroundColor = ConsoleColor.White;
                //reset color

            }
        }
        public static Pluklist? readFile(string CurrentFile)
        {
            using (FileStream file = File.OpenRead(CurrentFile))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer =
                           new System.Xml.Serialization.XmlSerializer(typeof(Pluklist));
                var plukliste = (Pluklist?)xmlSerializer.Deserialize(file);
                return plukliste;
            }


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
            //TODO: Add adresse to screen print

            Console.WriteLine("\n{0,-7}{1,-9}{2,-20}{3}", "Antal", "Type", "Produktnr.", "Navn");
            foreach (var item in plukliste.Lines)
            {
                Console.WriteLine("{0,-7}{1,-9}{2,-20}{3}", item.Amount, item.Type, item.ProductID, item.Title);
            }
        }
        public static void PrintOptions(int CurrentFileIndex, List<string> files)
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
    }
}


