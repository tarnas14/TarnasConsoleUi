namespace Tarnas.ConsoleUi
{
    using System.Collections.Generic;

    public interface FileReader
    {
        IEnumerable<string> GetLines(string filePath);
    }
}