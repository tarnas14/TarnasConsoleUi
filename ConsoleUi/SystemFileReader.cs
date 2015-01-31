namespace Tarnas.ConsoleUi
{
    using System.Collections.Generic;
    using System.IO;

    public class SystemFileReader : FileReader
    {
        public IEnumerable<string> GetLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
    }
}