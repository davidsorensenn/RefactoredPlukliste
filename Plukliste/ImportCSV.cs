using System.Xml.Serialization;


namespace Plukliste
{
    class ImportCSV : IImportFile
    {
        public Pluklist Read(string path)
        {
        Pluklist pluklist = new();
            pluklist.Adresse = "Pickup";
           
           string name = path.Substring(path.IndexOf("_") + 1);
           name = name.Substring(0, name.IndexOf("."));
           name = name.Replace("_", " ");
            pluklist.Name = name;
           pluklist.Forsendelse = "Pickup";    

            List<string> data = File.ReadAllLines(path).ToList();
            data.RemoveAt(0);
            foreach(var item in data)
            {
                Item line = new Item();
                var values = item.Split(';');
                line.ProductID = values[0];
                line.Type = (ItemType)Enum.Parse(typeof(ItemType), values[1]);
                line.Title = values[2];
                line.Amount = int.Parse(values[3]);
                pluklist.AddItem(line);
            }
            return pluklist;
        }
    }
}
