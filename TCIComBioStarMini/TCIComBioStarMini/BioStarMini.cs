using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Suprema;

namespace TCIComBioStarMini {

   [Guid("56B09AB9-499B-4ABF-B711-CA6EAE6F9ABC")]
   [ClassInterface(ClassInterfaceType.None)]
   [ComSourceInterfaces(typeof(IBioStarMiniEvents))]
   [ProgId("TCI.BioStarMini")]
   public class BioStarMini : IBioStarMini {
      const int MAX_TEMPLATE_SIZE = 384;
      
      private UFScannerManager oScannerManager;
      private UFS_STATUS oUFS;
      private byte[][] oTemplates;
      private string sOutputTemplates;

      public string getFingerprintTemplates() {
         initializeBioStarMini();
          
         int nNumScanners = initializeScanner();
         
         if (nNumScanners > 0) {
            UFScanner oScanner;
            int[] nTemplateSize = new int[2];
            int nEnrollQuality;

            oScanner = oScannerManager.Scanners[0];
            oScanner.Timeout = 5000;
            oScanner.TemplateSize = MAX_TEMPLATE_SIZE;
            oScanner.DetectCore = false;

            for(int i = 0; i < 2; i++) {
               oScanner.ClearCaptureImageBuffer();

               oUFS = oScanner.CaptureSingleImage();
               if (oUFS == UFS_STATUS.OK) {
                  oScanner.Extract(oTemplates[i], out nTemplateSize[i], out nEnrollQuality);

                  Thread.Sleep(1000);        
               }
               else {
                  i--;
               }
            }

            for(int i = 0; i < 2; i++) {
               sOutputTemplates += encodeBytesToHex(oTemplates[i]);
            }

            uninitializeScanner();
         }

         return sOutputTemplates;
      }
      
      private void initializeBioStarMini() {
         oTemplates = new byte[2][];
         oTemplates[0] = new byte[MAX_TEMPLATE_SIZE];
         oTemplates[1] = new byte[MAX_TEMPLATE_SIZE];
         sOutputTemplates = "";
      }

      private int initializeScanner() {
         oScannerManager = new UFScannerManager(null);
         oUFS = oScannerManager.Init();
         
         if (oUFS == UFS_STATUS.OK) {
            return oScannerManager.Scanners.Count;     
         }
         else {
            return 0;
         }
      }

      private void uninitializeScanner() {
         if (oScannerManager != null) {
            oScannerManager.Uninit();     
         }       
      }

      private string encodeBytesToHex(byte[] oSentence) {
         return BitConverter.ToString(oSentence).Replace("-", "");  
      }
   }

   [Guid("AD510D14-FA6C-44BE-8590-92B57D372406")]
   public interface IBioStarMini {
      [DispId(1)]
      string getFingerprintTemplates();   
   }

   [Guid("ECA5DD1D-096E-440c-BA6A-0118D351650B")]
   [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
   public interface IBioStarMiniEvents {
   }
}
