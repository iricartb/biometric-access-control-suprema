using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;

namespace BioStarTime {

   class Program {
      private static string sIPAddress;
      private static int nPort;
      private static string sDate;

      private static int oHandle = 0;
      private static uint nIdDevice = 0;
      private static int nTypeDevice = 0;

      static void Main(string[] args) {
         try {
            if (args.Count() == 3) {
               Program.sIPAddress = args[0];
               Program.nPort = Int32.Parse(args[1]);
               Program.sDate = args[2];

               if (Program.isValidArguments(Program.sIPAddress, Program.nPort, Program.sDate)) {
                  bool bInitialize = Program.initialize();
                  if (bInitialize) {
                     int nConnectDeviceStatus = Program.connectDevice(Program.sIPAddress, Program.nPort);

                     if (nConnectDeviceStatus == 1) {
                        int nResult = BSSDK.BS_SetTime(Program.oHandle, Program.convertDateToUnixTimestamp(Program.sDate));

                        if (nResult == BSSDK.BS_SUCCESS) Console.WriteLine("[ SUCCESS ] Se ha establecido la hora en el dispositivo con IP:" + Program.sIPAddress + " y puerto:" + Program.nPort.ToString() + ".");
                        else Console.WriteLine("[ ERROR ] No se ha podido establecer la hora en el dispositivo con IP:" + Program.sIPAddress + " y puerto:" + Program.nPort.ToString() + ".\r\nCodigo de Error: " + nResult); 

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

