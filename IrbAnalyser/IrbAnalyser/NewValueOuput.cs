using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IrbAnalyser
{
    public static class NewValueOuput
    {
        //static private string output;
        static private StringBuilder sb = new StringBuilder();

        public static void appendString(string msg, string key)
        { 
           //File.AppendText(Tools.filename + 'newValue.txt',);
            if (!sb.ToString().Contains(msg + ',' + key))
            {
                sb.AppendLine(msg + ',' + key);
            }
            //output += input + "/r/n";
           //File.AppendAllText(Tools.filename + "newValue.csv", input + "/r/n");
        }

        public static void saveFile(string filepath)
        {
            File.AppendAllText(filepath + "_triggers.csv", sb.ToString());
        }
    }
}
