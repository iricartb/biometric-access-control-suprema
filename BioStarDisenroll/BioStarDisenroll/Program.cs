using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;

namespace BioStarDisenroll {

   class Program {
      private const int TEMPLATE_SIZE = 384;

      private static string sIPAddress;
      private static int nPort;
      private static uint nIdUser;

      private static int oHandle = 0;
      private static uint nIdDevice = 0;
      private static int nTypeDevice = 0;
      private static byte[] oTemplateFingerprint = new byte[Program.TEMPLATE_SIZE * 2 * 2];

      static void Main(string[] args) {
         try {
            if (args.Count() == 3) {
               Program.sIPAddress = args[0];
               Program.nPort = Int32.Parse(args[1]);
               Program.nIdUser = UInt32.Parse(args[2]);

               if (Program.isValidArguments(Program.sIPAddress, Program.nPort)) {
                  bool bInitialize = Program.initialize();
                  if (bInitialize) {
                     int nConnectDeviceStatus = Program.connectDevice(Program.sIPAddress, Program.nPort);

                     if (nConnectDeviceStatus == 1) {
                        int nResult = BSSDK.BS_DeleteUser(Program.oHandle, Program.nIdUser);

                        IntPtr oUserInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BEUserHdr)));

                        nResult = BSSDK.BS_GetUserBEPlus(Program.oHandle, Program.nIdUser, oUserInfo, Program.oTemplateFingerprint);    
                        
                        Marshal.FreeHGlobal(oUserInfo);

                        if (nResult != BSSDK.BS_SUCCESS) Console.WriteLine("[ SUCCESS ] El usuario con ID: " + Program.nIdUser.ToString() + " ha sido eliminado correctamente del dispositivo con IP:" + Program.sIPAddress + " y puerto:" + Program.nPort.ToString() + ".");
                        else Console.WriteLine("[ ERROR ] No se ha podido eliminar el usuario con ID: " + Program.nIdUser.ToString() + " del dispositivo con IP:" + Program.sIPAddress + " y puerto:" + Program.nPort.ToString() + ".\r\nCodigo de Error: " + nResult); 

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
               else Console.WriteLine("[ ERROR ] El formato de los argumentos IP_Address/Port es incorrecto.");  
            }
            else Console.WriteLine("[ ERROR ] El numero de argumentos es incorrecto.\r\n\n\t* Argumentos: IP_Address Port UserID");
         } catch(Exception oException) {
            Console.WriteLine("[ ERROR ] " + oException.Message.ToString() );    
         }
      }

      private static bool isValidArguments(string sIPAddress, int nPort) {
         IPAddress oIPAddress = null;
 
         bool bValidIPAddress = IPAddress.TryParse(sIPAddress, out oIPAddress);
         bool bValidPort = ((nPort > 0) && (nPort <= 65535));

         return ((bValidIPAddress) && (bValidPort));
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
   }
}
