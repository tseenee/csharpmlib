using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace MLib.Diagnostics
{
    public class MemoryEditor
    {
        /// <summary>
        /// Converts string to Uint
        /// </summary>
        /// <param name="value">String to be parsed</param>
        /// <returns></returns>
        public static uint StringToUint(string Value)
        {
            return Convert.ToUInt32(Value, 16);
        }

        /// <summary>
        /// Converts Uint to string
        /// </summary>
        /// <param name="value">Uint to be parsed</param>
        /// <returns></returns>
        public static string UintToString(uint Value)
        {
            StringBuilder builder = new StringBuilder("0x");
            builder.Append(Convert.ToString(Value, 16).PadLeft(8, '0'));
            return builder.ToString();
        }

        /// <summary>
        /// Convert array of bytes to integer
        /// </summary>
        /// <param name="ByteArray">Array to be converted</param>
        /// <returns></returns>
        public static int ByteToInt(byte[] ByteArray)
        {
            return BitConverter.ToInt32(ByteArray, 0);
        }

        /// <summary>
        /// Convert array of bytes to double
        /// </summary>
        /// <param name="ByteArray">Array to be converted</param>
        /// <returns></returns>
        public static double ByteToDoublet(byte[] ByteArray)
        {
            return BitConverter.ToDouble(ByteArray, 0);
        }

        /// <summary>
        /// Convert array of bytes to float
        /// </summary>
        /// <param name="ByteArray">Array to be converted</param>
        /// <returns></returns>
        public static float ByteToFloat(byte[] ByteArray)
        {
            return BitConverter.ToSingle(ByteArray, 0);
        }



        private const uint PROCESS_VM_READ = (0x0010);
        private const uint PROCESS_VM_WRITE = (0x0020);
        private const uint PROCESS_VM_OPERATION = (0x0008);
        

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
		public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,[In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);
	

        private IntPtr m_hProcess = IntPtr.Zero;

        private Process gProcess;

        /// <summary>
        /// Prepares Memory Editor to use
        /// </summary>
        /// <param name="process">The process that will be opened</param>
        public MemoryEditor(Process process)
        {
            gProcess = process;
            m_hProcess = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION, 1, (uint)gProcess.Id);
        }

        public void Close()
        {
            CloseHandle(m_hProcess);
        }


        

        private byte[] ReadProcessMemory(IntPtr MemoryAddress, uint bytesToRead, out int bytesReaded)
        {
            byte[] buffer = new byte[bytesToRead];

            IntPtr ptrBytesReaded;
            ReadProcessMemory(m_hProcess, MemoryAddress, buffer, bytesToRead, out ptrBytesReaded);
            bytesReaded = ptrBytesReaded.ToInt32();
            return buffer;
        }

        private void WriteProcessMemory(IntPtr MemoryAddress, byte[] Buffer , out int BytesWritten)
        {
                IntPtr ptrBytesReaded;
                WriteProcessMemory(m_hProcess, MemoryAddress, Buffer, (uint)Buffer.Length, out ptrBytesReaded);
                BytesWritten = ptrBytesReaded.ToInt32();
        }




        /// <summary>
        /// Reads a part of the memory
        /// </summary>
        /// <param name="Address">Address to read from</param>
        /// <returns>Bytes read</returns>
        public byte[] ReadMemory(uint Address)
        {
            return ReadMemory(Address, 0);
        }


        /// <summary>
        /// Reads a part of the memory
        /// </summary>
        /// <param name="Address">Address to read from</param>
        /// <param name="Offset">Offset of the address</param>
        /// <returns>Bytes read</returns>
        public byte[] ReadMemory(uint Address, uint Offset)
        {
            int lol;
            byte[] buffer = ReadProcessMemory((IntPtr)Address, 8, out lol);

            //System.Windows.Forms.MessageBox.Show(lol.ToString());

            if (Offset > 0)
            {
                uint butts = BitConverter.ToUInt32(buffer, 0);
                butts += Offset;
                buffer = ReadProcessMemory((IntPtr)butts, 8, out lol);
            }

            return buffer;
        }


        /// <summary>
        /// Writes a part of the memory
        /// </summary>
        /// <param name="Address">Address to write in</param>
        /// <param name="Array">Array of bytes to write</param>
        public void WriteMemory(uint Address, byte[] Array)
        {
            WriteMemory(Address, 0, Array);
        }

        /// <summary>
        /// Writes a part of the memory
        /// </summary>
        /// <param name="Address">Address to write in</param>
        /// <param name="Offset">Offset of the address</param>
        /// <param name="Array">Array of bytes to write</param>
        public void WriteMemory(uint Address, uint Offset, byte[] Array)
        {
            int lol;

            /*if ((Array.Length % 8  > 0) || (Array.Length < 8))
            {
                byte[] Temp = Array;

                int size = Temp.Length + (8 % Array.Length);

                if (Array.Length < 8)
                    size = Temp.Length + (8 - Array.Length);

                Array = new byte[size];

                for (int i = 0; i < Temp.Length; i++)
                    Array[i] = Temp[i];


                for (int i = Temp.Length; i < size; i++)
                    Array[i] = 0;
            }*/

            WriteProcessMemory((IntPtr)Address, Array, out lol);
        }
    }
}
