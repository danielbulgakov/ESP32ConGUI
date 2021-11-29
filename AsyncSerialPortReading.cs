using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionProtocol
{
    namespace ExtensionMethods.SerialPort
    {
        public static class SerialPortExtensions
        {
            public async static Task ReadAsync(this System.IO.Ports.SerialPort serialPort , byte[] buffer, int offset, int count)
            {
                var bytesToRead = count;
                var temp = new byte[count];

                while (bytesToRead > 0)
                {
                    var readBytes = await serialPort.BaseStream.ReadAsync(temp, 0, bytesToRead);
                    Array.Copy(temp, 0, buffer, offset + count - bytesToRead, readBytes);
                    bytesToRead -= readBytes;
                }
            }

            public async static Task<byte[]> ReadAsync(this System.IO.Ports.SerialPort serialPort , int count)
            {
                var buffer = new byte[count];
                await serialPort.ReadAsync(buffer, 0, count);
                return buffer;
            }
        }
    }
}
