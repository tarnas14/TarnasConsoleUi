namespace Tarnas.ConsoleUi
{
    using System.Collections.Generic;

    public class UserCommand
    {
        public UserCommand()
        {
            Name = string.Empty;
            Params = new List<string>();
            Flags = new List<char>();
            NamedParams = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public IList<string> Params { get; set; }
        public IList<char> Flags { get; set; }
        public IDictionary<string, string> NamedParams { get; set; }

        public bool TryGetParam(string paramName, out string paramOutput)
        {
            return NamedParams.TryGetValue(paramName, out paramOutput);
        }

        public bool HasFlag(char flagName)
        {
            return Flags.Contains(flagName);
        }
    }
}