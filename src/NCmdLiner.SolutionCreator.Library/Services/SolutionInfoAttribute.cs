namespace NCmdLiner.SolutionCreator.Library.Services
{
    public class SolutionInfoAttribute
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((SolutionInfoAttribute)obj).Name == Name;
        }
    }
}