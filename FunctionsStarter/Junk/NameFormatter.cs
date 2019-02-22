namespace FunctionsStarter.Junk
{
    public class NameFormatter
    {
        public FormattedName Format(string name)
        {
            return new FormattedName()
            {
                First = name.Split(' ')[0],
                Last = name.Split(' ')[1]
            };
        }
    }

    public class FormattedName
    {
        public string First { get; set; }
        public string Last { get; set; }
    }
}