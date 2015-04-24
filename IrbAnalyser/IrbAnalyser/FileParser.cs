using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace IrbAnalyser
{
    class FileParser
    {
        public DataTable data;

        public FileParser(string file)
        {
            getDataTable(file);
        }


        private void getDataTable(string file)
        {
            data = new DataTable();
            var lines = File.ReadAllLines(file).ToList();
            if (lines.Count > 0)
            {
                var column = Tools.removeQuote(lines[0].Split((char)9));
                foreach (var title in column)
                {
                    data.Columns.Add(title.ToString());
                }
                lines.RemoveAt(0);
                lines.ForEach(line => data.Rows.Add(Tools.removeQuote(line.Split((char)9))));
            }

        }


    }
}
