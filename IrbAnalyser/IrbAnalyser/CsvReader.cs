using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;

namespace IrbAnalyser
{
    class CsvReader
    {
        public static string filename;
        public DataTable data;

        public CsvReader(string file)
        {
            filename = file;
        }

        public void getDataTable()
        {
            if (filename == null) throw new Exception("Cannot read from empty filename");
            try
            {
                data = new DataTable();
                string file = Path.GetFileName(filename);
                string sql = @"SELECT * FROM [" + file + "]";
                string pathOnly = Path.GetDirectoryName(filename);
                string connectionstr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                          ";Extended Properties=\"Text;HDR=Yes;FMT=TabDelimited\"";

                using (OleDbConnection conn = new OleDbConnection(connectionstr))
                {
                    using (OleDbDataAdapter adap = new OleDbDataAdapter(sql, conn))
                    {
                        conn.Open();
                        using (DataSet ds = new DataSet())
                        {
                            adap.Fill(data);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("The specified file could not be read", e);
            }
        }

        public string printDT()
        {
            string ret = "";
            foreach (DataColumn dc in data.Columns)
            {
                ret += dc.ColumnName + " ::: ";
            }
            foreach (DataRow dataRow in data.Rows)
            {
                ret += "\r\n";
                foreach (var item in dataRow.ItemArray)
                {
                    ret += " ::: " + item;
                }
            }
            return ret;
        }
    }
}
