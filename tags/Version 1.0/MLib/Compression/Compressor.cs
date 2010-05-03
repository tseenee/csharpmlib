using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;

namespace MLib.Compression
{
    /// <summary>
    /// Class with compressing functions
    /// </summary>
    public class Compressor
    {
        public static CompressedFileInfo Info(string FilePath)
        {
            CompressedFileInfo Inf = new CompressedFileInfo(FilePath);

            return Inf;
        }

        List<String> files;
        public List<String> Files
        {
            get
            {
                return files;
            }
        }



        /// <summary>
        /// Compressor for more files into a single file
        /// </summary>
        public Compressor()
        {
            files = new List<string>();
        }

        #region Add/Remove
        /// <summary>
        /// Adds a file to the file list
        /// </summary>
        /// <param name="FilePath">Path to the file</param>
        public void AddFile(string FilePath)
        {
            if (!files.Contains(Path.GetFileName(FilePath)))
            {
                if(File.Exists(FilePath))
                    files.Add(FilePath);
                else
                    throw new FileNotFoundException("Given file does not exist.");
            }
            else
                throw new Exception("The file with the same path already exists.");
        }

        /// <summary>
        /// Removes a file from the file list
        /// </summary>
        /// <param name="FilePath">Path to the file</param>
        public void Remove(string FilePath)
        {
            files.Remove(FilePath);
        }
        #endregion

        #region Compress
        /// <summary>
        /// Compresses the files into one file on the given path.
        /// </summary>
        /// <param name="FilePath">Path of the compressed file. The old file will be overwritten.</param>
        public void Compress(string FilePath)
        {
            if ((files.Count < 1) || (FilePath == null))
            {
                if (files.Count < 1)
                    throw new Exception("There are no files to compress.");
                if (FilePath == null)
                    throw new ArgumentNullException("FilePath can not be null.");
            }
            else
            {
                try
                {
                    FileStream fs = new FileStream(FilePath, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);

                    bw.Write(files.Count);
                    //Shran vse podatke od vseh fajlov
                    for (int i = 0; i < files.Count; i++)
                    {
                        String f = files[i];
                        FileInfo fInfo = new FileInfo(f);

                        bw.Write(Path.GetFileName(f));
                        bw.Write(Convert.ToInt32(fInfo.Length));
                    }
                    //Shran vse fajle
                    for (int i = 0; i < files.Count; i++)
                    {
                        String f = files[i];
                        FileInfo fInfo = new FileInfo(f);
                        bw.Write(File.ReadAllBytes(f), 0, (int)fInfo.Length);
                    }
                    fs.Close();
                    bw.Close();

                    //CompressGZIP(FilePath);
                }
                catch { throw new Exception("Error while compressing the files."); }

            }
        }
        #endregion




        #region Extract
        /// <summary>
        /// Decompresses a compressed file
        /// </summary>
        /// <param name="FilePath">Path of the compressed file</param>
        public static void Extract(string FilePath)
        {
            Extract(FilePath, null, true);
        }

        /// <summary>
        /// Decompresses a compressed file
        /// </summary>
        /// <param name="FilePath">Path of the compressed file</param>
        /// <param name="Overwrite">Overwrite existing files</param>
        public static void Extract(string FilePath, bool Overwrite)
        {
            Extract(FilePath, null, Overwrite);
        }


        /// <summary>
        /// Decompresses a compressed file
        /// </summary>
        /// <param name="FilePath">Path of the compressed file</param>
        /// <param name="ExtractPath">Path of the extraction</param>
        /// <param name="Overwrite">Overwrite existing files</param>
        public static void Extract(string FilePath, string ExtractPath, bool Overwrite)
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    FileStream fr = new FileStream(FilePath, FileMode.Open);
                    //GZipStream gz = new GZipStream(fr, CompressionMode.Decompress);
                    //BinaryReader br = new BinaryReader(gz);
                    BinaryReader br = new BinaryReader(fr);


                    int fNumber = br.ReadInt32();
                    List<int> eSizes = new List<int>();
                    List<String> eFiles = new List<String>();

                    //Prebere vse fajle
                    for (int i = 0; i < fNumber; i++)
                    {
                        eFiles.Add(br.ReadString());
                        eSizes.Add(br.ReadInt32());
                    }
                    
                    string Dir = Path.GetFileNameWithoutExtension(FilePath);
                    string exPath = Path.GetDirectoryName(FilePath) + @"\" + Dir + @"\";


                    //Pogleda ce je podana pot
                    try
                    {
                        string d2 = ExtractPath;
                        if (d2[d2.Length - 1].ToString() != @"\")
                            d2 += @"\";

                        d2 = Path.GetDirectoryName(ExtractPath);
                        if (d2 != @"\")
                        {


                            exPath = d2;
                        }
                    }
                    catch { }



                    if (!Directory.Exists(exPath))
                        Directory.CreateDirectory(exPath);

                    //Prebere vse fajle glede na to kaj je prebral prej
                    for (int i = 0; i < fNumber; i++)
                    {
                        if (((!File.Exists(exPath + eFiles[i])) || (Overwrite)))
                        {
                            byte[] buffer = br.ReadBytes(eSizes[i]);
                            File.WriteAllBytes(exPath + eFiles[i], buffer);
                        }
                    }
                    //fr.Close();
                    br.Close();
                }
                catch { throw new Exception("Could not finish decompressing the file. The file may be corrupted"); }
            }
            else
                throw new FileNotFoundException("Given file does not exist.");
        }
        #endregion

        #region CompressGZIP
        /// <summary>
        /// Compresses a file with GZIP
        /// </summary>
        /// <param name="FilePath">Path to the file</param>
        public static void CompressGZIP(string FilePath)
        {
            CompressGZIP(FilePath, FilePath);
        }

        /// <summary>
        /// Compresses a file with GZIP to a given location
        /// </summary>
        /// <param name="FilePath">Path to the file</param>
        /// <param name="CompressedPath">Path to the compressed fiile</param>
        public static void CompressGZIP(string FilePath, string CompressedPath)
        {
            try
            {
                byte[] buffer = File.ReadAllBytes(FilePath);

                FileStream fs = new FileStream(CompressedPath, FileMode.Create);
                GZipStream gz = new GZipStream(fs, CompressionMode.Compress);

                gz.Write(buffer, 0, buffer.Length);

                fs.Close();
            }
            catch {throw new Exception("Error while compressing the file."); }
        }
        #endregion

        #region DecompressGZIP
        /// <summary>
        /// Decompresses a GZIP compressed file
        /// </summary>
        /// <param name="FilePath">Path to the file</param>
        public static void DecompressGZIP(string FilePath)
        {
            DecompressGZIP(FilePath, FilePath);
        }

        /// <summary>
        /// Decompresses a GZIP compressed file to a given location
        /// </summary>
        /// <param name="FilePath">Path to the file</param>
        /// <param name="DecompressedPath">Path to the decompressed file</param>
        public static void DecompressGZIP(string FilePath, string DecompressedPath)
        {
            try
            {
                
                FileStream fr = new FileStream(FilePath, FileMode.Open);
                GZipStream gz = new GZipStream(fr, CompressionMode.Decompress);

                FileInfo faI = new FileInfo(FilePath);
                byte[] array = new byte[faI.Length];
                gz.Read(array, 0, array.Length);
                gz.Close();

                FileStream fs = new FileStream(DecompressedPath, FileMode.Create);
                fs.Write(array, 0, array.Length);
                fs.Close();
            }
            catch { throw new Exception("Could not finish decompressing the file. The file may be corrupted"); }
        }
        #endregion
    }
}
