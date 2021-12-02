using System;
using System.Text;
using System.Collections.Generic;

namespace ChatCoreTest
{
    internal class Program
    {

        private static byte[] m_PacketData;
        private static uint m_Pos;
        static byte[] myData = new byte[1060];

        public static void Main(string[] args)
        {
            m_PacketData = new byte[1024];
            m_Pos = 0;

            Write(109);
            Write(109.99f);
            Write("Hello!");

            Console.Write($"Output Byte array(length:{m_Pos}): ");
            for (var i = 0; i < m_Pos; i++)
            {
                Console.Write(m_PacketData[i] + ", ");
            }
            PackegeData();
            Read();
            Console.ReadLine();
        }

        // write an integer into a byte array
        private static bool Write(int i)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(i);
            _Write(bytes);
            return true;
        }

        // write a float into a byte array
        private static bool Write(float f)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(f);
            _Write(bytes);
            return true;
        }

        // write a string into a byte array
        private static bool Write(string s)
        {
            // convert string to byte array
            var bytes = Encoding.Unicode.GetBytes(s);

            // write byte array length to packet's byte array
            if (Write(bytes.Length) == false)
            {
                return false;
            }

            _Write(bytes);
            return true;
        }

        // write a byte array into packet's byte array
        private static void _Write(byte[] byteData)
        {
            // converter little-endian to network's big-endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteData);
            }

            byteData.CopyTo(m_PacketData, m_Pos);
            m_Pos += (uint)byteData.Length;
        }


        public static void PackegeData()
        {
            var pos = BitConverter.GetBytes(m_Pos);
            if (BitConverter.IsLittleEndian) Array.Reverse(pos);
            pos.CopyTo(myData, 0);
            m_PacketData.CopyTo(myData, sizeof(uint));
        }
        public static void Read()
        {

            byte[] uiData = new byte[4];
            byte[] iData = new byte[4];
            byte[] fData = new byte[4];
            byte[] sDataLength = new byte[4];
            byte[] sData = new byte[12];

            //unpakage data
            for (int i = 0; i < sizeof(uint); i++)
            {
                uiData[i] = myData[i];
            }
            Array.Reverse(uiData);
            for (int i = 0; i < sizeof(int); i++)
            {
                iData[i] = myData[i + sizeof(uint)];
            }
            Array.Reverse(iData);
            for (int i = 0; i < sizeof(float); i++)
            {
                fData[i] = myData[i + sizeof(uint) + sizeof(int)];
            }
            Array.Reverse(fData);
            for(int i = 0; i < 12; i++)
            {
                sData[i] = myData[i + sizeof(int) * 2 + sizeof(float) + sizeof(uint)];
            }
            Array.Reverse(sData);

            var uirt = BitConverter.ToUInt32(uiData,0);
            var irt = BitConverter.ToInt32(iData, 0);
            var frt = BitConverter.ToSingle(fData, 0);
            var srt = Encoding.Unicode.GetString(sData);

            Console.WriteLine($"\nThe package length is {uirt}\nfirst data:{irt}\nsecond data:{frt}\nthird data:{srt}");
        }

    }
}
