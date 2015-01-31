namespace Tarnas.ConsoleUi
{
    using System.Collections.Generic;

    public class UserCommand
    {
        public UserCommand()
        {
            Name = string.Empty;
            Params = new List<string>();
            Flags = new List<string>();
        }

        public string Name { get; set; }
        public IList<string> Params { get; set; }
        public IList<string> Flags { get; set; }
    }
}