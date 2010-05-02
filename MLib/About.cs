using System;
using System.Collections.Generic;
using System.Text;

namespace MLib
{
    public static class About
    {
        private const string dev = "Dejan Pelzel";
        private const string com = "M-Dev";
        private const string ver = "MLib - 1.0.0";
        private const string copy = "Nothing yet";

        /// <summary>
        /// Contains information about the developer of MLib
        /// </summary>
        public static string Developer
        {
            get
            {
                return dev;
            }
        }

        /// <summary>
        /// Contains information about the company developing MLib
        /// </summary>
        public static string Company
        {
            get
            {
                return com;
            }
        }

        /// <summary>
        /// Contains information about the version of MLib
        /// </summary>
        public static string Version
        {
            get
            {
                return com;
            }
        }

        /// <summary>
        /// Contains Copyright information
        /// </summary>
        public static string Copyright
        {
            get
            {
                return copy;
            }
        }
    }
}
