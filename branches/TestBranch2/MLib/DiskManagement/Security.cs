using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.AccessControl;

namespace MLib.DiskManagement
{
    public static class Security
    {
        public static void RemoveFileSecurity(string fileName, string WindowsAccount, FileSystemRights rights, AccessControlType accessControlType)
        {
            FileSecurity fSecurity = File.GetAccessControl(fileName);
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(WindowsAccount, rights, accessControlType));
            File.SetAccessControl(fileName, fSecurity);
        }

        public static void AddFileSecurity(string fileName, string WindowsAccount, FileSystemRights rights, AccessControlType accessControlType)
        {
            FileSecurity fSecurity = File.GetAccessControl(fileName);
            fSecurity.AddAccessRule(new FileSystemAccessRule(WindowsAccount, rights, accessControlType));
            File.SetAccessControl(fileName, fSecurity);

        }
    }
}
