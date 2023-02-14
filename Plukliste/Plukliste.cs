namespace Plukliste
{
    public class Pluklist
    {
        public string? Name;
        public string? Forsendelse;
        public string? Adresse;
        public bool IsValid => Validate();
        public List<Item> Lines = new List<Item>();
        public void AddItem(Item item) { Lines.Add(item); }
        public bool Validate()
        {
            bool valid = true;

            if (this.Lines == null || this.Lines.Count == 0)
            {
                valid = false;
            }
            return valid;
        }
    }

}







