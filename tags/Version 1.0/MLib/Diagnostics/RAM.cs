using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Collections.Generic;
using System.Management.Instrumentation;

namespace MLib.Diagnostics
{
    public class RAM
    {
        /// <summary>
        /// Returns the current  ram usage
        /// </summary>
        public double CurrentRamUsage
        {
            get
            {
                return ram.NextValue();
            }
        }

        public double MaxiumumRam
        {
            get
            {
                ManagementObjectSearcher Search = new ManagementObjectSearcher("Select * From Win32_ComputerSystem");
                double Ram_Bytes = 0;

                foreach (ManagementObject Mobject in Search.Get())
                {
                    Ram_Bytes = (Convert.ToDouble(Mobject["TotalPhysicalMemory"])) / 1048576;
                    break;
                }
                
                
                return Ram_Bytes;
            }
        }



        /// <summary>
        /// Slower method for getting ram usage. The faster one is not static.
        /// </summary>
        public static double RamUsage
        {
            get
            {
                PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                return ramCounter.NextValue();
            }
        }


        PerformanceCounter ram;
        /// <summary>
        /// Provides methods for measuring ram usage
        /// </summary>
        public RAM()
        {
            ram = new PerformanceCounter("Memory", "Available MBytes");
        }

    }
}
