namespace DcPwr
{
    public class Dcpwr
    {
        public string Name { get; set; } = string.Empty;

        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public bool IsNameSet()
        {
            return !string.IsNullOrEmpty(Name);
        }

        public string GetGreeting()
        {
            return IsNameSet() ? $"Hello, {Name}!" : "Hello!";
        }
    }
}
