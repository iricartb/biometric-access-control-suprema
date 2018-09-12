using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;

namespace BioStarQuery {

   class Program {
      const int MAX_LOG = 50000;

      private static string sIPAddress;
      private static int nPort;
      private static string sStartDate;

      private static int oHandle = 0;
      private static uint nIdDevice = 0;
      private static int nTypeDevice = 0;

      static void Main(string[] args) {
         try {
            if (args.Count() == 3) {
               Program.sIPAddress = args[0];
               Program.nPort = Int32.Parse(args[1]);
               Program.sStartDate = args[2];

               if (Program.isValidArguments(Program.sIPAddress, Program.nPort, Program.sStartDate)) {
                  bool bInitialize = Program.initialize();
                  if (bInitialize) {
                     int nConnectDeviceStatus = Program.connectDevice(Program.sIPAddress, Program.nPort);

                     if (nConnectDeviceStatus == 1) {
                        IntPtr oLogRecord = Marshal.AllocHGlobal(Program.MAX_LOG * Marshal.SizeOf(typeof(BSSDK.BSLogRecord)));
                         
                        int nLogTotalCount = 0;
                        int nLogCount = 0;
                        int nResult;

                        do {
                           nResult = 0;
                           IntPtr oBuffer = new IntPtr(oLogRecord.ToInt32() + nLogTotalCount * Marshal.SizeOf(typeof(BSSDK.BSLogRecord)));

                           if (Program.sStartDate != "") {
                              if (nLogTotalCount == 0) nResult = BSSDK.BS_ReadLog(Program.oHandle, Program.convertDateToUnixTimestamp(Program.sStartDate), 0, ref nLogCount, oBuffer);
                              else nResult = BSSDK.BS_ReadNextLog(Program.oHandle, Program.convertDateToUnixTimestamp(Program.sStartDate), 0, ref nLogCount, oBuffer); 
                           }
                           else {
                              if (nLogTotalCount == 0) nResult = BSSDK.BS_ReadLog(Program.oHandle, 0, 0, ref nLogCount, oBuffer);
                              else nResult = BSSDK.BS_ReadNextLog(Program.oHandle, 0, 0, ref nLogCount, oBuffer); 
                           }

                           nLogTotalCount += nLogCount;
                        } while ((nLogCount == 8192) && (nResult == BSSDK.BS_SUCCESS));

                        if (nResult == BSSDK.BS_SUCCESS) {
                           for (int i = 0; i < nLogTotalCount; i++) {
                              BSSDK.BSLogRecord oRecord = (BSSDK.BSLogRecord) Marshal.PtrToStructure(new IntPtr(oLogRecord.ToInt32() + i * Marshal.SizeOf(typeof(BSSDK.BSLogRecord))), typeof(BSSDK.BSLogRecord));

                              if ((oRecord.eventType == BSSDK.BE_EVENT_VERIFY_SUCCESS) || (oRecord.eventType == BSSDK.BE_EVENT_IDENTIFY_SUCCESS)) {
                                 DateTime oEventTime = new DateTime(1970, 1, 1).AddSeconds(oRecord.eventTime);

                                 string sTypeAccessCode = "0";
          
                                 if (oRecord.eventType == BSSDK.BE_EVENT_VERIFY_SUCCESS) sTypeAccessCode = "0";
                                 else sTypeAccessCode = "1";

                                 Console.Write(oEventTime.ToString("yyyy-MM-dd HH:mm:ss") + " " + oRecord.userID.ToString() + " " + sTypeAccessCode + " " + oRecord.tnaEvent + "@");
                              }
                           }   
                        }
                        else {
                           Console.WriteLine("[ ERROR ] No se ha podido obtener el log del dispositivo con el host: " + Program.sIPAddress + " y puerto: " + Program.nPort.ToString() + ".");
                        }

                        Marshal.FreeHGlobal(oLogRecord);

                        Program.disconnectDevice();
                     }
                     else {
                        if (nConnectDeviceStatus == -1) Console.WriteLine("[ ERROR ] No se ha podido establecer conexion con el host: " + Program.sIPAddress + " y puerto: " + Program.nPort.ToString() + ". La funcion BS_OpenSocket ha fallado.");
                        else if (nConnectDeviceStatus == -2) Console.WriteLine("[ ERROR ] No se ha podido establecer conexion con el host: " + Program.sIPAddress + " y puerto: " + Program.nPort.ToString() + ". La funcion BS_GetDeviceID ha fallado.");
                        else if (nConnectDeviceStatus == -3) Console.WriteLine("[ ERROR ] No se ha podido establecer conexion con el host: " + Program.sIPAddress + " y puerto: " + Program.nPort.ToString() + ". El dispositivo no es un Biostar Entry Plus.");
                        else Console.WriteLine("[ ERROR ] No se ha podido establecer conexion con el host: " + Program.sIPAddress + " y puerto: " + Program.nPort.ToString() + ". La funcion BS_SetDeviceID ha fallado.");    
                     }
                  }
                  else Console.WriteLine("[ ERROR ] No se ha podido inicializar el Biostar.");
               }
               else Console.WriteLine("[ ERROR ] El formato de los argumentos IP_Address/Port es incorrecto o bien la fecha tiene un formato incorrecto.");  
            }
            else Console.WriteLine("[ ERROR ] El numero de argumentos es incorrecto.\r\n\n\t* Argumentos: IP_Address Port StartDate");
         } catch(Exception oException) {
            Console.WriteLine("[ ERROR ] " + oException.Message.ToString() );    
         }
      }

      private static bool isValidArguments(string sIPAddress, int nPort, string sStartDate) {
         IPAddress oIPAddress = null;
         DateTime oDateTime;

         bool bValidIPAddress = IPAddress.TryParse(sIPAddress, out oIPAddress);
         bool bValidPort = ((nPort > 0) && (nPort <= 65535));
         bool bValidStartDate = true;

         if (sStartDate != "") bValidStartDate = DateTime.TryParse(sStartDate, out oDateTime);
         
         return ((bValidIPAddress) && (bValidPort) && (bValidStartDate));
      }

      private static bool initialize() {
         int nResult = BSSDK.BS_InitSDK();

         if (nResult == BSSDK.BS_SUCCESS) return true;
         else return false;
      }

      private static int connectDevice(string sIPAddress, int nPort) {
         int nResult = BSSDK.BS_OpenSocket(sIPAddress, nPort, ref Program.oHandle);
         
         if (nResult == BSSDK.BS_SUCCESS) {
            nResult = BSSDK.BS_GetDeviceID(Program.oHandle, ref Program.nIdDevice, ref Program.nTypeDevice);
            if (nResult == BSSDK.BS_SUCCESS) {
               if ((Program.nTypeDevice == BSSDK.BS_DEVICE_BEPLUS) || (Program.nTypeDevice == BSSDK.BS_DEVICE_BIOLITE)) {
                  nResult = BSSDK.BS_SetDeviceID(Program.oHandle, Program.nIdDevice, Program.nTypeDevice);     
                  if (nResult == BSSDK.BS_SUCCESS) return 1;
                  else return -4;
               }
               else return -3;
            }
            else return -2; 
         }
         else return -1;
      }

      private static void disconnectDevice() {
         BSSDK.BS_CloseSocket(Program.oHandle);
      }

      public static int convertDateToUnixTimestamp(string sDate) {
         DateTime oDate = Convert.ToDateTime(sDate);
         DateTime oDateOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

         TimeSpan oDifference = oDate - oDateOrigin;

         return ((int) Math.Floor(oDifference.TotalSeconds));
      }
   }
}
