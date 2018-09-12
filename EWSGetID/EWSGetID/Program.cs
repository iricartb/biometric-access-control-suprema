using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace EWSGetID {
   class Program {
      private static string sIPAddress;
      private static int nPort;

      static void Main(string[] args) {
         try {
            if (args.Count() == 2) {
               Program.sIPAddress = args[0];
               Program.nPort = Int32.Parse(args[1]);

               if (Program.isValidArguments(Program.sIPAddress, Program.nPort)) {
                  TcpClient oConnection = new TcpClient();
                  
                  oConnection.Connect(Program.sIPAddress, Program.nPort);
                  Console.WriteLine("\r\n[ OK ] Conexion establecida con la controladora EWS.");

                  Stream oStream = oConnection.GetStream();

                  byte[] oData = new Byte[35];

                  // Start Byte (1 Byte)
                  oData[0] = 33;

                  // Address (3 Bytes)
                  oData[1] = 21;
                  oData[2] = 74;
                  oData[3] = 100;

                  // Size (2 Bytes)
                  oData[4] = 26;
                  oData[5] = 0;

                  // Size (1 Byte)
                  oData[6] = 70;

                  // Data (25 Bytes)
                  // User_ID (4 Bytes - Lo, Hi, H-er, H-st)
                  oData[7] = 0;
                  oData[8] = 0;
                  oData[9] = 0;
                  oData[10] = 0;

                  // Timezone Rdr1 (3 Bytes)
                  oData[11] = 255;
                  oData[12] = 255;
                  oData[13] = 255;

                  // Timezone Rdr2 (3 Bytes)
                  oData[14] = 255;
                  oData[15] = 255;
                  oData[16] = 255;

                  // User Type (1 Bytes)
                  oData[17] = 174;

                  // Keycode (4 Bytes)
                  oData[18] = 1;
                  oData[19] = 2;
                  oData[20] = 3;
                  oData[21] = 4;

                  // Card Valid From (3 Bytes)
                  oData[22] = 1;
                  oData[23] = 1;
                  oData[24] = 10;

                  // Card Valid Until (3 Bytes)
                  oData[25] = 31;
                  oData[26] = 12;
                  oData[27] = 99;

                  // Flags (4 Bytes - frout rdr1, frout rdr2, rdraccass, apben/dis)
                  oData[28] = 3;
                  oData[29] = 3;
                  oData[30] = 1;
                  oData[31] = 0;

                  // CRC (2 Bytes)
                  CRC16calc oChecksum = new CRC16calc(InitialCrcValue.NonZero2);
                  byte[] oChecksumBytes = oChecksum.ComputeChecksumBytes(new Byte[] { oData[1], oData[2], oData[3], oData[4], oData[5], oData[6], oData[7], oData[8], oData[9], oData[10], oData[11], oData[12], oData[13], oData[14], oData[15], oData[16], oData[17], oData[18], oData[19], oData[20], oData[21], oData[22], oData[23], oData[24], oData[25], oData[26], oData[27], oData[28], oData[29], oData[30], oData[31] });

                  oData[32] = oChecksumBytes[1];
                  oData[33] = oChecksumBytes[0];

                  // End Byte (1 Byte)
                  oData[34] = 10;

                  oStream.Write(oData, 0, oData.Length);

                  Console.Write("[ OK ] Leyendo de la controladora ... ");
                  byte[] oDataReceived = new byte[1000];
                  int k = oStream.Read(oDataReceived, 0, 1000);

                  for (int i = 0; i < k; i++) Console.Write(((int) oDataReceived[i]) + " ");

                  oConnection.Close();
               }
               else Console.WriteLine("\r\n[ ERROR ] El formato de los argumentos IP_Address/Port es incorrecto."); 
            }
            else Console.WriteLine("\r\n[ ERROR ] El numero de argumentos es incorrecto.\r\n\n\t* Argumentos: IP_Address Port");
         } catch(Exception oException) {
            Console.WriteLine("\r\n[ ERROR ] " + oException.Message.ToString());    
         }
      }

      private static bool isValidArguments(string sIPAddress, int nPort) {
         IPAddress oIPAddress = null;

         bool bValidIPAddress = IPAddress.TryParse(sIPAddress, out oIPAddress);
         bool bValidPort = ((nPort > 0) && (nPort <= 65535));
          
         return ((bValidIPAddress) && (bValidPort));
      }
   }
}
