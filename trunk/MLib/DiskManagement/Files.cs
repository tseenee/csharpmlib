using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MLib.DiskManagement
{
    class Files
    {
        public static List<String> TextFileToLineArray(string Path)
        {
            List<String> Array = new List<String>();

            try
            {
                StreamReader sr = new StreamReader(Path);
                while(sr.Peek() != -1)
                    Array.Add(sr.ReadLine());
                sr.Close();
            }
            catch { }


            return Array;
        }
    }
}
