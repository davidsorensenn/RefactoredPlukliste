using System.Xml.Serialization;

namespace Plukliste
{
    class ImportXML : IImportFile
    {
        public Pluklist Read(string path)
        {
            //code that reads and return public Pluklist Read(string path)
            

            using (FileStream fileStream = File.OpenRead(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Pluklist));
                var pluklist = (Pluklist?)serializer.Deserialize(fileStream);
                return pluklist;
            }

        }
    }
}
