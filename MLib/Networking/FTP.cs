using System;
using System.Net;
using System.IO;

namespace BasicFTPClientNamespace
{
    class BasicFTPClient
    {
        public string Username;
        public string Password;
        public string Host;
        public int Port;

        public BasicFTPClient()
        {
            Username = "anonymous";
            Password = "anonymous@internet.com";
            Port = 21;
            Host = "";
        }

        public BasicFTPClient(string theUser, string thePassword, string theHost)
        {
            Username = theUser;
            Password = thePassword;
            Host = theHost;
            Port = 21;
        }

        private Uri BuildServerUri(string Path)
        {
            return new Uri(String.Format("ftp://{0}:{1}/{2}", Host, Port, Path));
        }

        /// <summary>
        /// This method downloads the given file name from the FTP server
        /// and returns a byte array containing its contents.
        /// Throws a WebException on encountering a network error.
        /// </summary>

        public byte[] DownloadData(string path)
        {
            // Get the object used to communicate with the server.
            WebClient request = new WebClient();

            // Logon to the server using username + password
            request.Credentials = new NetworkCredential(Username, Password);
            return request.DownloadData(BuildServerUri(path));
        }

        /// <summary>
        /// This method downloads the FTP file specified by "ftppath" and saves
        /// it to "destfile".
        /// Throws a WebException on encountering a network error.
        /// </summary>
        public void DownloadFile(string ftppath, string destfile)
        {
            // Download the data
            byte[] Data = DownloadData(ftppath);

            // Save the data to disk
            FileStream fs = new FileStream(destfile, FileMode.Create);
            fs.Write(Data, 0, Data.Length);
            fs.Close();
        }

        /// <summary>
        /// Upload a byte[] to the FTP server
        /// </summary>
        /// <param name="path">Path on the FTP server (upload/myfile.txt)</param>
        /// <param name="Data">A byte[] containing the data to upload</param>
        /// <returns>The server response in a byte[]</returns>

        public byte[] UploadData(string path, byte[] Data)
        {
            // Get the object used to communicate with the server.
            WebClient request = new WebClient();

            // Logon to the server using username + password
            request.Credentials = new NetworkCredential(Username, Password);
            return request.UploadData(BuildServerUri(path), Data);
        }

        /// <summary>
        /// Load a file from disk and upload it to the FTP server
        /// </summary>
        /// <param name="ftppath">Path on the FTP server (/upload/myfile.txt)</param>
        /// <param name="srcfile">File on the local harddisk to upload</param>
        /// <returns>The server response in a byte[]</returns>
        public byte[] UploadFile(string ftppath, string srcfile)
        {
            // Read the data from disk
            FileStream fs = new FileStream(srcfile, FileMode.Open);
            byte[] FileData = new byte[fs.Length];

            int numBytesToRead = (int)fs.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = fs.Read(FileData, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0) break;

                numBytesRead += n;
                numBytesToRead -= n;
            }
            numBytesToRead = FileData.Length;
            fs.Close();

            // Upload the data from the buffer
            return UploadData(ftppath, FileData);
        }

    }
}
