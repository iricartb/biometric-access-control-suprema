using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BioStarDisenroll
{
    class BSSDK
    {
        //
        // API Declarations
        //
        [DllImport("BS_SDK.dll",
             CharSet = CharSet.Ansi,
             EntryPoint = "BS_InitSDK")]
        public static extern int BS_InitSDK();

        [DllImport("BS_SDK.dll",
             CharSet = CharSet.Ansi,
             EntryPoint = "BS_OpenInternalUDP")]
        public static extern int BS_OpenInternalUDP( ref int handle );

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_SearchDeviceInLAN")]
        public static extern int BS_SearchDeviceInLAN(int handle, ref int numOfDevice, uint[] deviceIDs, int[] deviceTypes, uint[] readerAddrs );

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_SetDeviceID")]
        public static extern int BS_SetDeviceID(int handle, uint deviceID, int deviceType );

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_GetDeviceID")]
        public static extern int BS_GetDeviceID(int handle, ref uint deviceID, ref int deviceType);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_ReadConfig")]
        public static extern int BS_ReadConfig(int handle, int configType, ref int configSize, IntPtr data);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_WriteConfig")]
        public static extern int BS_WriteConfig(int handle, int configType, int configSize, IntPtr data);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_ReadConfigUDP")]
        public static extern int BS_ReadConfigUDP(int handle, uint targetAddr, uint targetID, int configType, ref int configSize, IntPtr data);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_WriteConfigUDP")]
        public static extern int BS_WriteConfigUDP(int handle, uint targetAddr, uint targetID, int configType, int configSize, IntPtr data);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_OpenSocket")]
        public static extern int BS_OpenSocket(string addr, int port, ref int handle);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_OpenSocketEx")]
        public static extern int BS_OpenSocket(string addr, int port, string hostaddr, ref int handle);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_CloseSocket")]
        public static extern int BS_CloseSocket(int handle);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_GetTime")]
        public static extern int BS_GetTime(int handle, ref int timeVal);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_SetTime")]
        public static extern int BS_SetTime(int handle, int timeVal);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_GetUserDBInfo")]
        public static extern int BS_GetUserDBInfo(int handle, ref int numOfUser, ref int numOfTemplate);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_GetAllUserInfoBEPlus")]
        public static extern int BS_GetAllUserInfoBEPlus(int handle, IntPtr userHdr, ref int numOfUser);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_GetAllUserInfoEx")]
        public static extern int BS_GetAllUserInfoEx(int handle, IntPtr userHdr, ref int numOfUser);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_GetUserInfoBEPlus")]
        public static extern int BS_GetUserInfoBEPlus(int handle, uint userID, IntPtr userHdr );

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_DeleteUser")]
        public static extern int BS_DeleteUser(int handle, uint userID);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_DeleteAllUser")]
        public static extern int BS_DeleteAllUser(int handle);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_GetAllAccessGroupEx")]
        public static extern int BS_GetAllAccessGroupEx(int handle, ref int numOfGroup, IntPtr accessGroup);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_GetUserBEPlus")]
        public static extern int BS_GetUserBEPlus(int handle, uint userID, IntPtr userHdr, byte[] templateData);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_GetUserEx")]
        public static extern int BS_GetUserEx(int handle, uint userID, IntPtr userHdr, byte[] templateData);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_EnrollUserBEPlus")]
        public static extern int BS_EnrollUserBEPlus(int handle, IntPtr userHdr, byte[] templateData);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_EnrollUserEx")]
        public static extern int BS_EnrollUserEx(int handle, IntPtr userHdr, byte[] templateData);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_GetLogCount")]
        public static extern int BS_GetLogCount(int handle, ref int logCount);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_ReadLog")]
        public static extern int BS_ReadLog(int handle, int startTime, int endTime, ref int logCount, IntPtr logRecord);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_ReadNextLog")]
        public static extern int BS_ReadNextLog(int handle, int startTime, int endTime, ref int logCount, IntPtr logRecord);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_DeleteLog")]
        public static extern int BS_DeleteLog(int handle, int logCount, ref int deletedCount);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_ReadCardIDEx")]
        public static extern int BS_ReadCardIDEx(int handle, ref uint cardID, ref int customID);

        [DllImport("BS_SDK.dll",
            CharSet = CharSet.Ansi,
            EntryPoint = "BS_ScanTemplate")]
        public static extern int BS_ScanTemplate(int handle, byte[] templateData);

        //
        // Structure Declarations
        //
        [ StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		public struct BESysInfoData
		{
            public uint magicNo;
            public int version;
            public uint timestamp;
            public uint checksum;
            [ MarshalAs( UnmanagedType.ByValArray, SizeConst=4 )] 
            public int[] headerReserved;            

            public uint ID;
            [ MarshalAs( UnmanagedType.ByValArray, SizeConst=8 )] 
            public byte[] macAddr;
            [ MarshalAs( UnmanagedType.ByValArray, SizeConst=16 )] 
            public byte[] boardVer;
            [ MarshalAs( UnmanagedType.ByValArray, SizeConst=16 )] 
            public byte[] firmwareVer;
            [ MarshalAs( UnmanagedType.ByValArray, SizeConst=40 )] 
            public int[] reserved;
		};

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct BESysInfoDataBLN
        {
            public uint magicNo;
            public int version;
            public uint timestamp;
            public uint checksum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] headerReserved;

            public uint ID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] macAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] boardVer;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] firmwareVer;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] productName;
            public int language;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
            public int[] reserved;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct BSSysInfoConfig
        {
            public uint ID;
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] macAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] productName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] boardVer;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] firmwareVer;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] blackfinVer;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] kernelVer;

            public int language;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] reserved;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct BEConfigData
        {
            // header
            public uint magicNo;
            public int version;
            public uint timestamp;
            public uint checksum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        	public int[] headerReserved;

	        // operation mode
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public int[] opMode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public int[] opModeSchedule;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        public byte[] opDoubleMode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
	        public int[] opReserved;

            // ip
            [MarshalAs(UnmanagedType.I1)]
            public bool useDHCP;
            public uint ipAddr;
            public uint gateway;
            public uint subnetMask;
            public uint serverIpAddr;
            public int port;
            [MarshalAs(UnmanagedType.I1)]
            public bool useServer;
            [MarshalAs(UnmanagedType.I1)]
            public bool synchTime;            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] ipReserved;

           	// fingerprint
	        public int securityLevel;
            public int fastMode;
            public int fingerReserved1;
            public int timeout; // 1 ~ 20 sec
            public int matchTimeout; // Infinite(0) ~ 10 sec
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
	        public int[] fingerReserved2;

            // padding
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3016)]
            public int[] padding;
        };


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct BEConfigDataBLN
        {
            // header
            public uint magicNo;
            public int version;
            public uint timestamp;
            public uint checksum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] headerReserved;

            // operation mode
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] opMode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] opModeSchedule;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] opDualMode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] opReserved1;
            public int opModePerUser;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public int[] identificationMode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public int[] identificationModeSchedule;
            public int[] opReserved2;

            // ip
            [MarshalAs(UnmanagedType.I1)]
            public bool useDHCP;
            public uint ipAddr;
            public uint gateway;
            public uint subnetMask;
            public uint serverIpAddr;
            public int port;
            [MarshalAs(UnmanagedType.I1)]
            public bool useServer;
            [MarshalAs(UnmanagedType.I1)]
            public bool synchTime;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public int[] ipReserved;

            // fingerprint
            public int imageQuality;
            public int securityLevel;
            public int fastMode;
            public int fingerReserved1;
            public int timeout; // 1 ~ 20 sec
            public int matchTimeout; // Infinite(0) ~ 10 sec
            public int templateType;
            public int fakeDetection;
            [MarshalAs(UnmanagedType.I1)]
            public bool useServerMatching;
            [MarshalAs(UnmanagedType.I1)]
            public bool useCheckDuplicate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public int[] fingerReserved2;

            // padding
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3930)]
            public int[] padding;
        };


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct BSIPConfig
        {
            public int lanType;
            public bool useDHCP;
            public uint port;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	        public byte[] ipAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	        public byte[] gateway;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	        public byte[] subnetMask;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	        public byte[] serverIP;

            public int maxConnection;
            public bool useServer;
            public uint serverPort;
            public bool syncTimeWithServer;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
	        public byte[] reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct BEUserHdr
        {
            public int version;
            public uint userID;

            public int startTime;
            public int expiryTime;

            public uint cardID;
            public byte cardCustomID;
            public byte commandCardFlag;
            public byte cardFlag;
            public byte cardVersion;

            public ushort adminLevel;
            public ushort securityLevel;

            public uint accessGroupMask;

            public ushort numOfFinger;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public ushort[] fingerChecksum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] isDuress;

            public int disabled;
            public int opMode;
            public int dualMode;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] password;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public int[] reserved2;
        };


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct BSUserHdrEx
        {
            public uint ID;

            public ushort headerVersion;
	        public ushort adminLevel;
	        public ushort securityLevel;
	        public ushort statusMask; 
	        public uint accessGroupMask;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
            public byte[] name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
            public byte[] department;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public byte[] password;

	        public ushort numOfFinger;
	        public ushort duressMask;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public ushort[] checksum; // index 0, 1

	        public ushort authMode; 
	        public ushort authLimitCount; // 0 for no limit 
	        public ushort reserved; 
	        public ushort timedAntiPassback; // in minutes. 0 for no limit 
	        public uint cardID; // 0 for not used
	        public byte	bypassCard;
	        public byte	disabled;
	        public uint expireDateTime;
	        public int customID; //card Custom ID
	        public int version; // card Info Version
            public uint startDateTime; 
        };


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct BSAccessGroupEx
        {
            public int groupID;
            
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String name;
            public int numOfReader;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public uint[] readerID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] scheduleID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] reserved;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct BSLogRecord
        {
            public byte eventType;
            public byte subEvent;
            public ushort tnaEvent;
            public int eventTime;
            public uint userID;
            public uint reserved;
        };

        // 
        // Constants
        //
        public const int BS_SUCCESS = 0;
        public const int BS_ERR_NOT_FOUND = -306;

        public const int BS_DEVICE_BIOSTATION = 0;
        public const int BS_DEVICE_BEPLUS = 1;
        public const int BS_DEVICE_BIOLITE = 2;
		  public const int BS_DEVICE_XPASS = 3;

        public const int BEPLUS_CONFIG = 0x50;
        public const int BEPLUS_CONFIG_SYS_INFO = 0x51;

        public const int BLN_CONFIG = 0x70;
        public const int BLN_CONFIG_SYS_INFO = 0x71;

        public const int BS_CONFIG_SYS_INFO = 0x41;
        public const int BS_CONFIG_TCPIP = 0x10;

        public const int NO_ACCESS_GROUP = 0xFD;
        public const int FULL_ACCESS_GROUP = 0xFE;

        public const int BS_AUTH_MODE_DISABLED      = 0;
        public const int BS_AUTH_FINGER_ONLY		= 1020;
        public const int BS_AUTH_FINGER_N_PASSWORD	= 1021;
        public const int BS_AUTH_FINGER_OR_PASSWORD	= 1022;
        public const int BS_AUTH_PASS_ONLY			= 1023;
        public const int BS_AUTH_CARD_ONLY          = 1024;

        public const int BE_CARD_VERSION_1 = 0x13;

        // user levels for BioStation
        public const int BS_USER_ADMIN = 240;
        public const int BS_USER_NORMAL = 241;

        // security levels for BioStation
        public const int BS_USER_SECURITY_DEFAULT   = 260;
        public const int BS_USER_SECURITY_LOWER     = 261;
        public const int BS_USER_SECURITY_LOW       = 262;
        public const int BS_USER_SECURITY_NORMAL    = 263;
        public const int BS_USER_SECURITY_HIGH      = 264;
        public const int BS_USER_SECURITY_HIGHER    = 265;

        // log events
        public const int BE_EVENT_SCAN_SUCCESS 	= 0x58;
        public const int BE_EVENT_ENROLL_BAD_FINGER = 0x16;
        public const int BE_EVENT_ENROLL_SUCCESS = 0x17;
        public const int BE_EVENT_ENROLL_FAIL = 0x18;
        public const int BE_EVENT_ENROLL_CANCELED = 0x19;

        public const int BE_EVENT_VERIFY_BAD_FINGER = 0x26;
        public const int BE_EVENT_VERIFY_SUCCESS = 0x27;
        public const int BE_EVENT_VERIFY_FAIL = 0x28;
        public const int BE_EVENT_VERIFY_CANCELED = 0x29;
        public const int BE_EVENT_VERIFY_NO_FINGER = 0x2a;

        public const int BE_EVENT_IDENTIFY_BAD_FINGER = 0x36;
        public const int BE_EVENT_IDENTIFY_SUCCESS = 0x37;
        public const int BE_EVENT_IDENTIFY_FAIL = 0x38;
        public const int BE_EVENT_IDENTIFY_CANCELED = 0x39;

        public const int BE_EVENT_DELETE_BAD_FINGER = 0x46;
        public const int BE_EVENT_DELETE_SUCCESS = 0x47;
        public const int BE_EVENT_DELETE_FAIL = 0x48;
        public const int BE_EVENT_DELETE_ALL_SUCCESS = 0x49;

        public const int BE_EVENT_VERIFY_DURESS = 0x62;
        public const int BE_EVENT_IDENTIFY_DURESS = 0x63;

        public const int BE_EVENT_TAMPER_SWITCH_ON = 0x64;
        public const int BE_EVENT_TAMPER_SWITCH_OFF = 0x65;

        public const int BE_EVENT_SYS_STARTED = 0x6a;
        public const int BE_EVENT_IDENTIFY_NOT_GRANTED = 0x6d;
        public const int BE_EVENT_VERIFY_NOT_GRANTED = 0x6e;

        public const int BE_EVENT_IDENTIFY_LIMIT_COUNT = 0x6f;
        public const int BE_EVENT_IDENTIFY_LIMIT_TIME = 0x70;

        public const int BE_EVENT_IDENTIFY_DISABLED = 0x71;
        public const int BE_EVENT_IDENTIFY_EXPIRED = 0x72;

        public const int BE_EVENT_APB_FAIL = 0x73;
        public const int BE_EVENT_COUNT_LIMIT = 0x74;
        public const int BE_EVENT_TIME_INTERVAL_LIMIT 	= 0x75;
        public const int BE_EVENT_INVALID_AUTH_MODE		= 0x76;
        public const int BE_EVENT_EXPIRED_USER			= 0x77;
        public const int BE_EVENT_NOT_GRANTED			= 0x78;

        public const int BE_EVENT_DETECT_INPUT0	= 0x54;
        public const int BE_EVENT_DETECT_INPUT1 = 0x55;

        public const int BE_EVENT_TIMEOUT = 0x90;

        public const int BS_EVENT_RELAY_ON	= 0x80;
        public const int BS_EVENT_RELAY_OFF	= 0x81;

        public const int BE_EVENT_DOOR0_OPEN 	= 0x82;
        public const int BE_EVENT_DOOR1_OPEN		= 0x83;
        public const int BE_EVENT_DOOR0_CLOSED 	= 0x84;
        public const int BE_EVENT_DOOR1_CLOSED	= 0x85;

        public const int BE_EVENT_DOOR0_FORCED_OPEN	= 0x86;
        public const int BE_EVENT_DOOR1_FORCED_OPEN	= 0x87;

        public const int BE_EVENT_DOOR0_HELD_OPEN	= 0x88;
        public const int BE_EVENT_DOOR1_HELD_OPEN	= 0x89;

        public const int BE_EVENT_DOOR0_RELAY_ON		= 0x8A;
        public const int BE_EVENT_DOOR1_RELAY_ON		= 0x8B;

        public const int BE_EVENT_INTERNAL_INPUT0	= 0xA0;
        public const int BE_EVENT_INTERNAL_INPUT1	= 0xA1;
        public const int BE_EVENT_SECONDARY_INPUT0	= 0xA2;
        public const int BE_EVENT_SECONDARY_INPUT1	= 0xA3;

        public const int BE_EVENT_SIO0_INPUT0		= 0xB0;
        public const int BE_EVENT_SIO0_INPUT1		= 0xB1;
        public const int BE_EVENT_SIO0_INPUT2		= 0xB2;
        public const int BE_EVENT_SIO0_INPUT3		= 0xB3;

        public const int BE_EVENT_SIO1_INPUT0		= 0xB4;
        public const int BE_EVENT_SIO1_INPUT1		= 0xB5;
        public const int BE_EVENT_SIO1_INPUT2		= 0xB6;
        public const int BE_EVENT_SIO1_INPUT3		= 0xB7;

        public const int BE_EVENT_SIO2_INPUT0		= 0xB8;
        public const int BE_EVENT_SIO2_INPUT1		= 0xB9;
        public const int BE_EVENT_SIO2_INPUT2		= 0xBA;
        public const int BE_EVENT_SIO2_INPUT3		= 0xBB;

        public const int BE_EVENT_SIO3_INPUT0		= 0xBC;
        public const int BE_EVENT_SIO3_INPUT1		= 0xBD;
        public const int BE_EVENT_SIO3_INPUT2		= 0xBE;
        public const int BE_EVENT_SIO3_INPUT3		= 0xBF;

        public const int BE_EVENT_LOCKED			= 0xC0;
        public const int BE_EVENT_UNLOCKED			= 0xC1;

        public const int BE_EVENT_TIME_SET			= 0xD2;
        public const int BE_EVENT_SOCK_CONN        = 0xD3;
        public const int BE_EVENT_SOCK_DISCONN     = 0xD4;
        public const int BE_EVENT_SERVER_SOCK_CONN = 0xD5;
        public const int BE_EVENT_SERVER_SOCK_DISCONN = 0xD6;
        public const int BE_EVENT_LINK_CONN        = 0xD7;
        public const int BE_EVENT_LINK_DISCONN     = 0xD8;
        public const int BE_EVENT_INIT_IP          = 0xD9;
        public const int BE_EVENT_INIT_DHCP        = 0xDA;
        public const int BE_EVENT_DHCP_SUCCESS     = 0xDB;
    }
}
