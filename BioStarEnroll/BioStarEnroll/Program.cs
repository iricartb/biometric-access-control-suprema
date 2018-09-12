using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;

namespace BioStarEnroll {

   class Program {
      private const int TEMPLATE_SIZE = 384;

      private static string sIPAddress;
      private static string sCard;
      private static string sCardCustom;
      private static string sFingerprint;
      private static string sFingerprint2;
      private static int nPort;
      private static uint nIdUser;

      private static int oHandle = 0;
      private static uint nIdDevice = 0;
      private static int nTypeDevice = 0;
      private static byte[] oTemplateFingerprint = new byte[Program.TEMPLATE_SIZE * 2 * 2];

      static void Main(string[] args) {
         try {
            if (args.Count() == 7) {
               Program.sIPAddress = args[0];
               Program.nPort = Int32.Parse(args[1]);
               Program.nIdUser = UInt32.Parse(args[2]);
               Program.sCard = args[3];
               Program.sCardCustom = args[4];
               Program.sFingerprint = args[5];
               Program.sFingerprint2 = args[6];

               if (Program.isValidArguments(Program.sIPAddress, Program.nPort, Program.sCard, Program.sCardCustom, Program.sFingerprint, Program.sFingerprint2)) {
                  bool bInitialize = Program.initialize();
                  if (bInitialize) {
                     int nConnectDeviceStatus = Program.connectDevice(Program.sIPAddress, Program.nPort);

                     if (nConnectDeviceStatus == 1) {
                        int nFingerChecksum = 0;
                        int nFingerChecksum2 = 0;
                        BSSDK.BEUserHdr oUserHdr = new BSSDK.BEUserHdr();

                        oUserHdr.version = 0x01;
                        oUserHdr.userID = nIdUser;
                        oUserHdr.startTime = 0;
                        oUserHdr.expiryTime = 0;

                        if ((Program.sCard != "") && (Program.sCardCustom != "")) {
                           oUserHdr.cardID = UInt32.Parse(Program.sCard);
                           oUserHdr.cardCustomID = (byte) Int32.Parse(Program.sCardCustom);
                        }

                        oUserHdr.cardFlag = (byte) 1;
                        oUserHdr.cardVersion = BSSDK.BE_CARD_VERSION_1;

                        oUserHdr.adminLevel = (ushort) BSSDK.BS_USER_NORMAL;
                        oUserHdr.securityLevel = (ushort) BSSDK.BS_USER_SECURITY_DEFAULT;
                        oUserHdr.accessGroupMask = 0xffffffff;

                        if (Program.sFingerprint.ToString().Length == (Program.TEMPLATE_SIZE * 4)) {
                           oUserHdr.numOfFinger = 1;

                           byte[] sDecodeFingerprint = Program.decodeHexString(Program.sFingerprint);

                           Buffer.BlockCopy(sDecodeFingerprint, 0, Program.oTemplateFingerprint, 0, (Program.TEMPLATE_SIZE * 2));
                           
                           for (int i = 0; i < (Program.TEMPLATE_SIZE * 2); i++) {
                              nFingerChecksum += Program.oTemplateFingerprint[i];
                           }

                           if (Program.sFingerprint2.ToString().Length == (Program.TEMPLATE_SIZE * 4)) {
                              oUserHdr.numOfFinger++;

                              byte[] sDecodeFingerprint2 = Program.decodeHexString(Program.sFingerprint2);

                              Buffer.BlockCopy(sDecodeFingerprint2, 0, Program.oTemplateFingerprint, (Program.TEMPLATE_SIZE * 2), (Program.TEMPLATE_SIZE * 2));

                              for (int i = 0; i < (Program.TEMPLATE_SIZE * 2); i++) {
                                 nFingerChecksum2 += Program.oTemplateFingerprint[(Program.TEMPLATE_SIZE * 2) + i];
                              }
                           }
                        }
                        else {
                           oUserHdr.numOfFinger = 0;
                        }

                        oUserHdr.fingerChecksum = new ushort[2] { (ushort)nFingerChecksum, (ushort) nFingerChecksum2 };
                        oUserHdr.isDuress = new byte[2] { 0, 0 };
                        oUserHdr.disabled = 0;
                        oUserHdr.opMode = BSSDK.BS_AUTH_MODE_DISABLED;
                        oUserHdr.dualMode = 0;

                        IntPtr oUserInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BEUserHdr)));
                        Marshal.StructureToPtr(oUserHdr, oUserInfo, true);
                        
                        int nResult = BSSDK.BS_EnrollUserBEPlus(Program.oHandle, oUserInfo, Program.oTemplateFingerprint);
                        
                        if (nResult == BSSDK.BS_SUCCESS) Console.WriteLine("[ SUCCESS ] El usuario con ID: " + Program.nIdUser.ToString() + " ha sido registrado correctamente en el dispositivo con IP:" + Program.sIPAddress + " y puerto:" + Program.nPort.ToString() + ".");
                        else Console.WriteLine("[ ERROR ] No se ha podido registrar el usuario con ID: " + Program.nIdUser.ToString() + " en el dispositivo con IP:" + Program.sIPAddress + " y puerto:" + Program.nPort.ToString() + ".\r\nCodigo de Error: " + nResult); 

                        Marshal.FreeHGlobal(oUserInfo);

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
               else Console.WriteLine("[ ERROR ] El formato de los argumentos IP_Address/Port es incorrecto o bien los codigos de biometria estan vacios o el codigo de fingerprint no mide " + (Program.TEMPLATE_SIZE * 4).ToString() + "bytes.");  
            }
            else Console.WriteLine("[ ERROR ] El numero de argumentos es incorrecto.\r\n\n\t* Argumentos: IP_Address Port UserID CardID CardCustomID FingerprintTemplate FingerprintTemplate2");
         } catch(Exception oException) {
            Console.WriteLine("[ ERROR ] " + oException.Message.ToString() );    
         }
      }

      private static bool isValidArguments(string sIPAddress, int nPort, string sCard, string sCardCustom, string sFingerprint, string sFingerprint2) {
         IPAddress oIPAddress = null;
 
         bool bValidIPAddress = IPAddress.TryParse(sIPAddress, out oIPAddress);
         bool bValidPort = ((nPort > 0) && (nPort <= 65535));
         bool bValidBiometricParameters = (((sCard != "") && (sCardCustom != "")) || (sFingerprint != ""));
         bool bValidFingerprint = ((sFingerprint == "") || ((sFingerprint != "") && (sFingerprint.ToString().Length == (Program.TEMPLATE_SIZE * 4))));
         bool bValidFingerprint2 = ((sFingerprint2 == "") || ((sFingerprint2 != "") && (sFingerprint2.ToString().Length == (Program.TEMPLATE_SIZE * 4))));

         return ((bValidIPAddress) && (bValidPort) && (bValidBiometricParameters) && (bValidFingerprint) && (bValidFingerprint2));
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

      private static byte[] decodeHexString(string sSentence) {
         byte[] sOutput = new byte[Program.TEMPLATE_SIZE * 2];
         string sCodeHex;

         for(int i = 0; i < (sSentence.Length / 2); i++) {
            sCodeHex = sSentence.Substring((i * 2), 2); 
            sOutput[i] = Convert.ToByte(int.Parse(sCodeHex, System.Globalization.NumberStyles.AllowHexSpecifier));  
         }

         return sOutput;
      }
   }
}
