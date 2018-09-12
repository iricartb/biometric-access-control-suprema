using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace EWSUsers {
   class Program {
      private static string sIPAddress;
      private static int nPort;
      private static int nId1;
      private static int nId2;
      private static int nId3;
      private static int nUsers;

      static void Main(string[] args) {
         try {
            if (args.Count() == 6) {
               Program.sIPAddress = args[0];
               Program.nPort = Int32.Parse(args[1]);
               Program.nId1 = Int32.Parse(args[2]);
               Program.nId2 = Int32.Parse(args[3]);
               Program.nId3 = Int32.Parse(args[4]);
               Program.nUsers = Int32.Parse(args[5]);

               if (Program.isValidArguments(Program.sIPAddress, Program.nPort, Program.nId1, Program.nId2, Program.nId3, Program.nUsers)) {
                  int nIdCurrentUser;

                  TcpClient oConnection = new TcpClient();
                  
                  oConnection.Connect(Program.sIPAddress, Program.nPort);
                  Console.WriteLine("\r\n[ OK ] Conexion establecida con la controladora EWS.");

                  Stream oStream = oConnection.GetStream();
                  
                  for (nIdCurrentUser = 1; nIdCurrentUser < (Program.nUsers + 1); nIdCurrentUser++) { 
                     byte[] oData = new Byte[35];

                     // Start Byte (1 Byte)
                     oData[0] = 33;

                     // Address (3 Bytes)
                     oData[1] = (byte) Program.nId1;
                     oData[2] = (byte) Program.nId2;
                     oData[3] = (byte) Program.nId3;

                     // Size (2 Bytes)
                     oData[4] = 26;
                     oData[5] = 0;
                  
                     // Size (1 Byte)
                     oData[6] = 70;
                     
                     byte[] oIdCurrentUser = new byte[] { (byte)(nIdCurrentUser >> 8), (byte)(nIdCurrentUser & 0x00ff) };
   
                     // Data (25 Bytes)
                     // User_ID (4 Bytes - Lo, Hi, H-er, H-st)
                     Console.WriteLine("----------------------------");
                     Console.WriteLine("USER ID LOW:" + oIdCurrentUser[1]);
                     Console.WriteLine("USER ID HIGH:" + oIdCurrentUser[0]);
                     Console.WriteLine("----------------------------");

                     oData[7] = oIdCurrentUser[1];
                     oData[8] = oIdCurrentUser[0];
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

                     Console.WriteLine("[ OK ] Comando ADD USER enviado correctamente a la controladora EWS.");

                     byte[] oDataReceived = new byte[100];
                     int k = oStream.Read(oDataReceived, 0, 100);

                     Console.Write("[ OK ] Respuesta de la controladora: ");
                     for (int i = 0; i < k; i++) Console.Write(oDataReceived[i]);

                     if ((k == 6) && (((int) oDataReceived[4]) == 255)) {
                        Console.WriteLine("\r\n[ SUCCESS ] Usuario con identificador " + nIdCurrentUser + " registrado correctamente a la controladora EWS.");          
                     }
                     else Console.WriteLine("\r\n[ ERROR ] Usuario con identificador " + nIdCurrentUser + " no se ha registrado correctamente a la controladora EWS."); 
                  }

                  oConnection.Close();
               }
               else Console.WriteLine("\r\n[ ERROR ] El formato de los argumentos IP_Address/Port es incorrecto."); 
            }
            else Console.WriteLine("\r\n[ ERROR ] El numero de argumentos es incorrecto.\r\n\n\t* Argumentos: IP_Address Port id1 id2 id3 num_users_to_create (1..65535)");
         } catch(Exception oException) {
            Console.WriteLine("\r\n[ ERROR ] " + oException.Message.ToString());    
         }
      }

      private static bool isValidArguments(string sIPAddress, int nPort, int nId1, int nId2, int nId3, int nUsers) {
         IPAddress oIPAddress = null;

         bool bValidIPAddress = IPAddress.TryParse(sIPAddress, out oIPAddress);
         bool bValidPort = ((nPort > 0) && (nPort <= 65535));
         bool bValidId1 = ((nId1 >= 0) && (nId1 <= 255));
         bool bValidId2 = ((nId2 >= 0) && (nId2 <= 255));
         bool bValidId3 = ((nId3 >= 0) && (nId3 <= 255));
         bool bValidUsers = ((nUsers > 0) && (nUsers <= 65535));
          
         return ((bValidIPAddress) && (bValidPort) && (bValidId1) && (bValidId2) && (bValidId3) && (bValidUsers));
      }
   }
}
