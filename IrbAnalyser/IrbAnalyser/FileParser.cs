﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace IrbAnalyser
{
    class FileParser
    {
        public static string filename;
        public DataTable data;

        public FileParser(string file)
        {
            filename = file;
        }


        public void getDataTable()
        {
            data = new DataTable();
            var lines = File.ReadAllLines(filename).ToList();
            if (lines.Count > 0)
            {
                var column = lines[0].Split((char)9);
                foreach (var title in column)
                {
                    data.Columns.Add(title.ToString());
                }
                lines.RemoveAt(0);
                lines.ForEach(line => data.Rows.Add(line.Split((char)9)));
            }

        }


    }
}