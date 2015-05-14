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

        public FileParser(string file, type typ)
        {
            getDataTable(file, typ);
        }

        public static enum type { Study, Team, Status, Event }

        private void getDataTable(string file, type typ)
        {
            data = new DataTable();

            switch (typ)
            {
                case type.Study:
                    break;
                case type.Team:
                    break;
                case type.Status:
                    break;
                case type.Event:
                    break;
            }

            foreach (DataRow dr in data.Rows)
            {
                foreach (DataColumn dc in data.Columns)
                {
                    dr[dc.ColumnName] = "";
                }
            }

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
