using System;
using System.Text;
using System.Diagnostics;

namespace MLib.Diagnostics
{
    public class CPU
    {
        public static int ProcessorCount
        {
            get
            {
                return Environment.ProcessorCount;
            }
        }



        /// <summary>
        /// Returns the current processor usage
        /// </summary>
        public double CurrentProcessorUsage
        {
            get
            {
                return cpu.NextValue();
            }
        }


        PerformanceCounter cpu;
        /// <summary>
        /// Provides methods for measuring cpu usage
        /// </summary>
        /// <param name="CoreNumber">Set -1 for total</param>
        public CPU(int CoreNumber)
        {
            cpu = new PerformanceCounter();
            cpu.CategoryName = "Processor";
            cpu.CounterName = "% Processor Time";
            if (CoreNumber == -1)
                cpu.InstanceName = "_Total";
            else
                cpu.InstanceName = CoreNumber.ToString();

            
        }


    }
}
