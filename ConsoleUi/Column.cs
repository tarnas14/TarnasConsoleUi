namespace Tarnas.ConsoleUi
{
    using System.Collections.Generic;
    using System.Linq;

    public class Column
    {
        public Column()
        {
            Data = new List<string>();
            Header = string.Empty;
        }

        public string Header { get; set; }
        public IList<string> Data { get; set; }

        public int Width
        {
            get { return Data.Select(d => d.Length).Aggregate(Header.Length, (l1, l2) => l1 > l2 ? l1 : l2); }
        }

        public int Height
        {
            get { return Data.Count; }
        }

        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public bool AlignRight { get; set; }

        public IEnumerable<string> GetDataRows()
        {
            for (int i = 0; i < Height; ++i)
            {
                yield return GetDataRow(i);
            }
        }

        public string GetDataRow(int rowId)
        {
            if (rowId < 0 || rowId >= Height)
            {
                return Format(string.Empty);
            }

            return Format(Data[rowId]);
        }

        private string Format(string data)
        {
            return string.Format("{0}{1," + ((!AlignRight) ? "-" : string.Empty) + Width + "}{2}", Prefix, data, Suffix);
        }

        public string GetHeader()
        {
            return Format(Header);
        }
    }
}