using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace EWSLockDoors {
   class Program {
      private static string sIPAddress;
      private static int nPort;
      private static int nId1;
      private static int nId2;
      private static int nId3;

      static void Main(string[] args) {
         try {
            if (args.Count() == 5) {
               Program.sIPAddress = args[0];
               Program.nPort = Int32.Parse(args[1]);
               Program.nId1 = Int32.Parse(args[2]);
               Program.nId2 = Int32.Parse(args[3]);
               Program.nId3 = Int32.Parse(args[4]);

               if (Program.isValidArguments(Program.sIPAddress, Program.nPort, Program.nId1, Program.nId2, Program.nId3)) {
                  bool bError = true;
                  TcpClient oConnection = new TcpClient();
                  
                  oConnection.Connect(Program.sIPAddress, Program.nPort);

                  Stream oStream = oConnection.GetStream();
                  
                  for (int nDoor = 1; nDoor <= 2; nDoor++) { 
                     byte[] oData = new Byte[11];

                     // Start Byte (1 Byte)
                     oData[0] = 33;

                     // Address (3 Bytes)
                     oData[1] = (byte) Program.nId1;
                     oData[2] = (byte) Program.nId2;
                     oData[3] = (byte) Program.nId3;

                     // Size (2 Bytes)
                     oData[4] = 2;
                     oData[5] = 0;
                  
                     // Command (1 Byte)
                     oData[6] = 51;
                     
                     // Data (1 Byte)
                     // Door_Number
                     oData[7] = (byte) nDoor;

                     // CRC (2 Bytes)
                     CRC16calc oChecksum = new CRC16calc(InitialCrcValue.NonZero2);
                     byte[] oChecksumBytes = oChecksum.ComputeChecksumBytes(new Byte[] { oData[1], oData[2], oData[3], oData[4], oData[5], oData[6], oData[7] });

                     oData[8] = oChecksumBytes[1];
                     oData[9] = oChecksumBytes[0];

                     // End Byte (1 Byte)
                     oData[10] = 10;

                     oStream.Write(oData, 0, oData.Length);

                     byte[] oDataReceived = new byte[100];
                     int k = oStream.Read(oDataReceived, 0, 100);

                     if ((k == 6) && ((((int) oDataReceived[4]) == 255) || (((int) oDataReceived[4]) == 249))) {
                        if (nDoor == 1) bError = false;
                        else bError = bError && false;        
                     }
                     else bError = true;
                  }
                  
                  if (!bError) {
                     Console.WriteLine("[ SUCCESS ] Comando de bloqueo efectuado correctamente a la controladora EWS.");
                  }
                  else Console.WriteLine("[ ERROR ] Comando de bloqueo no se ha efectuado correctamente a la controladora EWS."); 

                  oConnection.Close();
               }
               else Console.WriteLine("[ ERROR ] El formato de los argumentos IP_Address/Port es incorrecto."); 
            }
            else Console.WriteLine("[ ERROR ] El numero de argumentos es incorrecto.\r\n\n\t* Argumentos: IP_Address Port id1 id2 id3");
         } catch(Exception oException) {
            Console.WriteLine("[ ERROR ] " + oException.Message.ToString());    
         }
      }

      private static bool isValidArguments(string sIPAddress, int nPort, int nId1, int nId2, int nId3) {
         IPAddress oIPAddress = null;

         bool bValidIPAddress = IPAddress.TryParse(sIPAddress, out oIPAddress);
         bool bValidPort = ((nPort > 0) && (nPort <= 65535));
         bool bValidId1 = ((nId1 >= 0) && (nId1 <= 255));
         bool bValidId2 = ((nId2 >= 0) && (nId2 <= 255));
         bool bValidId3 = ((nId3 >= 0) && (nId3 <= 255));
          
         return ((bValidIPAddress) && (bValidPort) && (bValidId1) && (bValidId2) && (bValidId3));
      }
   }
}
