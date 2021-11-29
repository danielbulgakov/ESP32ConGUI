using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using ConnectionProtocol.ExtensionMethods.SerialPort;
using System.Diagnostics;

namespace ConnectionProtocol
{
    enum KeyWords { 
        DataSplit = 255,
    }

    class ESP32Connect
    {
        private SerialPort serialPort;
        private byte[] DATA;
        int BytesPerPackage = 8;

        public ESP32Connect(UInt16 PortID, int BaudRate)
        {
            serialPort = new SerialPort();
            serialPort.PortName = "COM" + PortID;
            serialPort.BaudRate = BaudRate;
        }

        public void EstablishConnection()
        {
            DATA = new byte[BytesPerPackage];
            var isValid = SerialPort.GetPortNames().Any(x => string.Compare(x, serialPort.PortName, true) == 0);
            if (!isValid)
                throw new System.IO.IOException(string.Format("{0} port was not found", serialPort.PortName));

            try
            {
                serialPort.Open();
            }
            catch(Exception ex)
            {
                throw new System.IO.IOException(string.Format("Cant connect to Port {0}   ",  serialPort.PortName));
            }


        }

        private async void RequestData()
        {
            byte[] buffer = new byte[BytesPerPackage];
            try
            {
                buffer = await serialPort.ReadAsync(BytesPerPackage);
                if(buffer[0] == 255)
                {
                    DATA = buffer;
                }
            }
            catch (Exception excepcion)
            {
                Trace.WriteLine(excepcion.Message);
            }
        }

        public int[] Get3Digits()
        {
            int[] Digits = new int[3];
            
            RequestData();
            Digits[0] = BitConverter.ToInt16(Get2ByteWord(2), 0);
            Digits[1] = BitConverter.ToInt16(Get2ByteWord(4), 0);
            Digits[2] = BitConverter.ToInt16(Get2ByteWord(6), 0);

            return Digits;
        }

        private byte[] Get2ByteWord(int StartIndex)
        {
            byte[] buffer = new byte[2];
            buffer[0] = DATA[StartIndex];
            buffer[1] = DATA[StartIndex + 1];
            return buffer;
        }

    }
}
