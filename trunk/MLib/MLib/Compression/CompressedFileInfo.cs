using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace MLib.Compression
{
    /// <summary>
    /// Holds information about a MZIP compressed file
    /// </summary>
    public class CompressedFileInfo
    {
        public CompressedFileInfo(string FilePath)
        {
            path = FilePath;
            fi = new FileInfo(path);
            
            FileStream fr = new FileStream(FilePath, FileMode.Open);
            //GZipStream gz = new GZipStream(fr, CompressionMode.Decompress);

            BinaryReader br = new BinaryReader(fr);
            int num = br.ReadInt32();

            for (int i = 0; i < num; i++)
            {
                files.Add(br.ReadString());
                sizes.Add(br.ReadInt32());
            }

            br.Close();

        }
        FileInfo fi;
        string path = "";
        List<String> files = new List<String>();
        List<int> sizes = new List<int>();

        public List<int> FileSizes
        {
            get
            {
                return sizes;
            }
        }

        public List<String> Files
        {
            get
            {
                return files;
            }
        }

        public double FileSize
        {
            get
            {
                return fi.Length;
            }
        }

        public string FileName
        {
            get
            {
                return fi.Name;
            }
        }
    }
}
