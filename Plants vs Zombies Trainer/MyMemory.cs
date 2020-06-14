using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MemoryHack
{
    class MyMemory
    {
        private IntPtr processHandle;
        private Process process;
        private const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        //const int PROCESS_WM_READ = 0x0010;
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        private static byte[] ReadBytes(IntPtr Handle, int Address, int size)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[size];
            ReadProcessMemory(Handle, Address, buffer, size, ref bytesRead);
            return buffer;
        }
        /// <summary>
        /// Lấy Base Address của chương trình
        /// </summary>
        /// <returns>Base address</returns>
        public int GetBaseAddress()
        {
            return process.MainModule.BaseAddress.ToInt32();
        }

        private static bool WriteBytes(IntPtr Handle, int Address,int value, int size)
        {
            int BytesWrite = 0;
            byte[] buffer = BitConverter.GetBytes(value);
            return WriteProcessMemory(Handle, Address, buffer, size, ref BytesWrite);
        }
        public MyMemory(string processName)
        {
            try
            {
                process = Process.GetProcessesByName(processName)[0];
                
                processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);
            }
            catch
            {
                process = null;
            }
            
        }
        /// <summary>
        /// Đọc địa chỉ 4 bytes kiểu int có giá trị từ -2,147,483,648 tới 2,147,483,647
        /// </summary>
        /// <param name="Address">Địa chỉ của ô nhớ</param>
        /// <returns>Giá trị của ô nhớ</returns>
        public int ReadInt(int Address)	
        {
            return BitConverter.ToInt32(ReadBytes(processHandle, Address, 4), 0);
        }
        /// <summary>
        /// Đọc địa chỉ 4 bytes kiểu uint có giá trị từ 0 đến 4,294,967,295
        /// </summary>
        /// <param name="Address">Địa chỉ của ô nhớ</param>
        /// <returns>Giá trị của ô nhớ</returns>
        public uint ReadUInt(int Address)
        {
            return BitConverter.ToUInt32(ReadBytes(processHandle, Address, 4), 0);
        }
        /// <summary>
        /// Đọc địa chỉ 2 bytes kiểu short có giá trị từ -32,768 đến 32,767
        /// </summary>
        /// <param name="Address">Địa chỉ của ô nhớ</param>
        /// <returns>Giá trị của ô nhớ</returns>
        public short ReadShort(int Address)
        {
            return BitConverter.ToInt16(ReadBytes(processHandle, Address, 2), 0);
        }
        /// <summary>
        /// Đọc địa chỉ 2 bytes kiểu ushort có giá trị từ 0 đến 65,535
        /// </summary>
        /// <param name="Address">Địa chỉ của ô nhớ</param>
        /// <returns>Giá trị của ô nhớ</returns>
        public ushort ReadUShort(int Address)
        {
            return BitConverter.ToUInt16(ReadBytes(processHandle, Address, 2), 0);
        }
        /// <summary>
        /// Đọc địa chỉ 1 byte có giá trị từ 0 đến 255
        /// </summary>
        /// <param name="Address">Địa chỉ của ô nhớ</param>
        /// <returns>Giá trị của ô nhớ</returns>
        public byte ReadByte(int Address)
        {
            return ReadBytes(processHandle, Address, 1)[0];
        }
        /// <summary>
        /// Đọc địa chỉ 8 bytes kiểu double có giá trị từ 1.7E – 308 đến 1.7E + 308, với 15, 16 chữ số có nghĩa
        /// </summary>
        /// <param name="Address">Địa chỉ của ô nhớ</param>
        /// <returns>Giá trị của ô nhớ</returns>
        public double ReadDouble(int Address)
        {
            return BitConverter.ToDouble(ReadBytes(processHandle, Address, 8), 0);
        }
        /// <summary>
        /// Đọc địa chỉ 4 bytes kiểu float có giá trị từ 3.4E – 38 đến 3.4E + 38  với 7 chữ số có nghĩa
        /// </summary>
        /// <param name="Address">Địa chỉ của ô nhớ</param>
        /// <returns>Giá trị của ô nhớ</returns>
        public float ReadFloat(int Address)
        {
            return BitConverter.ToSingle(ReadBytes(processHandle, Address, 4), 0);
        }
        /// <summary>
        /// Ghi giá trị vào địa chỉ ô nhớ
        /// </summary>
        /// <param name="Address">Địa chỉ của ô nhớ</param>
        /// <param name="value">Giá trị</param>
        /// <param name="length">Số bytes muốn ghi</param>
        public void WriteNumber(int Address,int value, int length = 4)
        {
           
            WriteBytes(processHandle, Address, value, length);
        }
        public string ReadString(int Address, int length = 32)
        {
            string temp3 = Encoding.Unicode.GetString(ReadBytes(processHandle, Address, length));

            string[] temp3str = temp3.Split('\0');
            return temp3str[0];
        }
        public void WriteString(int Address,string value, uint length = 32)
        {
            int bytesWritten = 0;
            byte[] buffer = Encoding.Unicode.GetBytes(value+"\0"); // '\0' marks the end of string

            // replace 0x0046A3B8 with your address
            WriteProcessMemory(processHandle, Address, buffer, buffer.Length, ref bytesWritten);
        }
        /// <summary>
        /// Kiểm tra game đã được mở hay còn chạy không
        /// </summary>
        /// <returns>true hoặc false</returns>
        public bool isOK()
        {
            return process != null && !process.HasExited;
        }
    }
}
