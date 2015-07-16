using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace IrbAnalyser
{
    public static class Csv
    {
        public static void saveCsv(DataTable dt, string separator, string file, string extension = "")
        {
            if (dt.Rows.Count > 0)
            {
                /*if (dt.Columns.Contains("Organization") && dt.Columns.Contains("IRB Agency name") && dt.Columns.Contains("IRB Study ID"))
                {
                    dt = dt.AsEnumerable().OrderBy(x => x.Field<string>("IRB Agency name"))
                        .ThenBy(x => x.Field<string>("IRB Study ID"))
                        .ThenBy(x => x.Field<string>("Organization"))
                        .CopyToDataTable();
                }
                else if (dt.Columns.Contains("IRB Agency name") && dt.Columns.Contains("IRB Study ID"))
                {
                    dt = dt.AsEnumerable().OrderBy(x => x.Field<string>("IRB Agency name"))
                        .ThenBy(x => x.Field<string>("IRB Study ID"))
                        .CopyToDataTable();
                }
                else
                {
                    dt = dt.AsEnumerable().OrderBy(x => x.Field<string>("Study_number"))
                        .CopyToDataTable();
                }*/
                if (dt.Columns.Contains("Study_number"))
                {
                    dt = dt.AsEnumerable().OrderBy(x => x.Field<string>("Study_number"))
                        .CopyToDataTable();
                }

            }

            file = extension == "" ? file + ".txt" : file + extension;

            StringBuilder sb = new StringBuilder();

            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();

            sb.AppendLine(string.Join(separator, columnNames));

            foreach (DataRow dr in dt.Rows)
            {
                string[] fields = dr.ItemArray.Select(field => field.ToString()).ToArray();

                sb.AppendLine(string.Join(separator, fields));
            }

            File.WriteAllText(file, sb.ToString());


            /*foreach (DataColumn dc in dt.Columns)
            {
                sb.Append(dc.ColumnName + separator);
            }
            sb.Remove(sb.Length - separator.Length, separator.Length);
            sb.Append(Environment.NewLine);
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(dr[i].ToString() + separator;
                }
                sb.Remove(sb.Length - separator.Length, separator.Length);
                sb.Append(Environment.NewLine);
            }*/

        }

    }
}
