//*****************************************************************************
// BTAccess .Net Class Library
// for use with BTAccess Mobile on PocketPC
//
// Copyright (c) 2004-2006 High Point Software, Inc.
//
// For technical support email support@high-point.com
// or visit us at www.high-point.com
//*****************************************************************************
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.WindowsCE.Forms; 

//Note: to open or close outline regions, position cursor and then type:
//   CTL-M, CTL-L to open all regions
//   CTL-M, CTL-O to close all regions
//   CTL-M, CTL-M to open one region

#region BTAccess Overview
//--------------------------------------------------------------------------------
// BTAccess .Net v2.0   November 2004
// Copyright (c) 2004 High Point Sofware (www.high-point.com)
// Please note this interface requires v2.0 of the BTAccess.dll on PocketPC.
//
// The BTAccess namespace contains the BtStack and BtDevice C# classes, similar to 
// the C++ BTAccess classes.  It also contains a set of public enums and 
// structure definitions required for both internal usage and user-application 
// usage.
//
// The BtStack class also implements a MessageWindow override which receives 
// messages from BTAccess.dll.  It then turns them into standard .Net events 
// using delegates. See the StackDemoNet C# sample code for an example of how 
// to tie into these events.
//
// Note that since there is nothing equivalent to a ".lib" file in .Net there
// is no way to provide the compiled code to a developer unless he's willing
// to use a separate dll (see option 1 below).  Therefore we offer 3 ways to 
// use the .Net interface to BTAccess:
//
//    1. Use the BTAccess assembly:
//       We have built the BTAccess .Net classes into a separate assembly 
//       (BTAccessNet.dll) so you can just add a Reference to it in your .Net 
//       project.  The drawback with this approach is that yet another DLL for 
//       BTAccess must be copied to your PocketPC device, along with the standard
//       BTAccess.dll.
//
//    2. Use C# source code:
//       Just add this source file to your project,add a "using BTAccess;" 
//       statement, and rebuild your program.  You should not have to make
//       any changes to this source file to use it.
//
//    3. Derive other source code:
//       For users of other .Net languages you can use a disassembly tool like
//       .Net Reflector to view our BTAccessNet.dll assembly (IL code) in any language 
//       you like, and then copy/paste this new source code directly into your project 
//       as in #2.
//-------------------------------------------------------------------------------------
#endregion

namespace SensorShare.Compact.Bluetooth
{
	#region BTAccess Definitions
	//BTAccess General Error Codes
	public enum eBTRC
	{
		BT_OK,
		BT_FAIL,
		BT_DEVICE_NOTAVAIL,
		BT_SVC_NOT_SUPPORTED,
		BT_SVC_NOT_UNIQUE,
		BT_FTP_OPEN_FAILED,
		BT_FTP_CLOSE_FAILED,
		BT_FTP_SEND_ERROR,
		BT_UNSUPPORTED
	}

	//BT Device Classes
	public enum	eBTDEVCLASS
	{
		BTDEVCLASS_UNSPECIFIED,
		BTDEVCLASS_COMPUTER,
		BTDEVCLASS_PHONE,
		BTDEVCLASS_LAN_ACCESS,
		BTDEVCLASS_AUDIO,
		BTDEVCLASS_PERIPHERAL,
		BTDEVCLASS_IMAGING
	}

	//BT Device Types
	public enum eBTDEVTYPE
	{
		BTDEVTYPE_UNCLASSIFIED,
		BTDEVTYPE_COMP_WORKSTATION,
		BTDEVTYPE_COMP_SERVER,
		BTDEVTYPE_COMP_LAPTOP,
		BTDEVTYPE_COMP_HANDHELD,
		BTDEVTYPE_COMP_PALM,
		BTDEVTYPE_PHONE_CELLULAR,
		BTDEVTYPE_PHONE_CORDLESS,
		BTDEVTYPE_PHONE_SMART,
		BTDEVTYPE_PHONE_MODEM,
		BTDEVTYPE_LAN_ACCESS,
		BTDEVTYPE_AUDIO_HEADSET,
		BTDEVTYPE_IMAGING_DISPLAY,
		BTDEVTYPE_IMAGING_CAMERA,
		BTDEVTYPE_IMAGING_SCANNER,
		BTDEVTYPE_IMAGING_PRINTER,
	}

	//BT Service Types
	public enum eBTSVC
	{
		BTSVC_UNKNOWN,
		BTSVC_SENDFILE,
		BTSVC_SENDBUSINESSCARD,
		BTSVC_EXCHANGECARDS,
		BTSVC_GETBUSINESSCARD,
		BTSVC_SERIAL,
		BTSVC_DIALUP,
		BTSVC_LAP,
		BTSVC_PANHOST,
		BTSVC_PANUSER,
		BTSVC_SENDPIM_OBJECT,
		BTSVC_CONNECTION_SERVICES,	//Only matches on SPP, LAP, DUN, PAN for device.GetServiceList
		BTSVC_ALL_SERVICES			//Matches all services for device.GetServiceList
	}

	//BT Transmit Power Levels
	public enum	eBTPOWER_LEVEL
	{
		BTXMIT_POWER_LOW,
		BTXMIT_POWER_MED,
		BTXMIT_POWER_HIGH
	}

	//BT Incoming Signal Strength Levels
	public enum	eBTRSSI_LEVEL
	{
		BTRSSI_LOW,		//Low or no signal at all
		BTRSSI_GOOD,
		BTRSSI_HIGH
	}

	//BT Connection Status Values
	public enum eBTCONN_STATUS
	{
		BT_CONNECTION_COMPLETE,
		BT_CONNECTION_FAILED,
		BT_CONNECTION_LOST
	}

	//BT PAN Node Types
	public enum ePAN_NODETYPE
	{
		BT_PAN_NODETYPE_HOST,
		BT_PAN_NODETYPE_USER
	}


	//---------------------------------------------------------------------------------
	//Common structures used to communicate between .Net and legacy BTAccess.dll
	//---------------------------------------------------------------------------------
	public struct ConnStatus
	{
		public string			DeviceAddr;
		public string			DeviceName;
		public string			COMPort;
		public eBTSVC			SvcType;
		public eBTCONN_STATUS	Status;
	}

	public struct GenSecOptions
	{
		public string LocalDeviceName;
		public string FTPRootDir;
		public bool	  IsDiscoverable;
		public bool	  IsConnectable;
	}

	public struct SvcSecOptions
	{
		public bool IsEnabled;
		public bool IsAuthRequired;
		public bool IsPINRequired;
		public bool IsEncryptRequired;
	}

	public struct SvcListItem
	{
		public eBTSVC eSvc;
		public string SvcName;
	}

	public struct SessionListItem
	{
		public string	DeviceAddr;
		public string	DeviceName;
		public string   SvcName;
		public eBTSVC	SvcType;
		public bool		InitiatedLocally;
	}

	public struct RecvFileInfo
	{
		public string	DeviceAddr;
		public string	DeviceName;
		public string   FileName;
	}
	#endregion

	#region BtStack Class
	public class BtStack
	{
		#region Stack Events
		//Define C# events to correspond to BTAccess messages
		public delegate void BtDevFoundHandler(BtDevice device);
		public delegate void BtConnStatusHandler(ConnStatus connStatus);
		public delegate void BtSendFileCompleteHandler(eBTRC rc);
		public delegate void BtSendFileProgressHandler(Int32 CurrentBytes, Int32 TotalBytes);
		public delegate void BtRecvFileCompleteHandler(RecvFileInfo info);
		public delegate void BtBcCompleteHandler(eBTRC rc);
		public delegate void BtSearchCompleteHandler();

		public static event BtDevFoundHandler	       BtDevFound;
		public static event BtConnStatusHandler        BtConnStatus;
		public static event BtSendFileCompleteHandler  BtSendFileComplete;
		public static event BtSendFileProgressHandler  BtSendFileProgress;
		public static event BtRecvFileCompleteHandler  BtRecvFileComplete;
		public static event BtBcCompleteHandler		   BtBcComplete;
		public static event BtSearchCompleteHandler    BtSearchComplete;
		#endregion

		#region MessageWindow Override
		//MessageWindow class to handle Win msgs coming from BTAccess dll
		internal class BTMsgWin : MessageWindow
		{
			//Windows Messages from legacy BTAccess.dll
			public const int WM_USER                 = 1024;
			public const int BTMSG_DEVICEFOUND		 = WM_USER + 501;
			public const int BTMSG_BC_COMPLETE		 = WM_USER + 502;
			public const int BTMSG_SENDFILE_COMPLETE = WM_USER + 503;
			public const int BTMSG_SENDFILE_PROGRESS = WM_USER + 504;
			public const int BTMSG_RECVFILE_COMPLETE = WM_USER + 505;
			public const int BTMSG_SEARCH_COMPLETE   = WM_USER + 506;
			public const int BTMSG_CONNECTION_STATUS = WM_USER + 601;

			protected override void WndProc(ref Message msg)
			{
				//Note: all parameters must be manually marshalled!

				#region BTMSG_DEVICEFOUND
				//--------------------------------
				// BTMSG_DEVICEFOUND
				//--------------------------------
				if (msg.Msg == BTMSG_DEVICEFOUND)
				{
					//WPARAM = 0
					//LPARAM = DevFound struct: devclass, devtype, isconnected, devaddr, devname
					IntPtr p = msg.LParam;

					//Fill in C# device object with found-device fields
					BtDevice device = new BtDevice();

					device.DeviceClass = (eBTDEVCLASS)Marshal.ReadInt32(p);
					p = (IntPtr) ((int)p + 4);

					device.DeviceType = (eBTDEVTYPE) Marshal.ReadInt32(p);
					p = (IntPtr) ((int)p + 4);

					device.IsConnected = Marshal.ReadInt32(p)== 0 ? false : true;
					p = (IntPtr) ((int)p + 4);

					device.DeviceAddr = Marshal.PtrToStringUni(p);
					p = (IntPtr) ((int)p + 48);  //24 TCHARs

					device.DeviceName = Marshal.PtrToStringUni(p);

					//Fire BtDevFound event to any listeners
					if (BtStack.BtDevFound != null)
						BtStack.BtDevFound(device);

					//Have to get DLL to do its own memory delete
					BT_DeletePointer(msg.LParam);
				}
				#endregion

				#region BTMSG_CONNECTION_STATUS
				//--------------------------------
				// BTMSG_CONNECTION_STATUS
				//--------------------------------
				else
				if (msg.Msg == BTMSG_CONNECTION_STATUS)
				{
					//WPARAM = 0
					//LPARAM = ConnStatus struct: svctype, status, comport, devaddr, devname
					IntPtr p = msg.LParam;

					//Fill in C# ConnStatus object with status info
					ConnStatus connStatus = new ConnStatus();

					connStatus.SvcType = (eBTSVC)Marshal.ReadInt32(p);
					p = (IntPtr) ((int)p + 4);

					connStatus.Status = (eBTCONN_STATUS) Marshal.ReadInt32(p);
					p = (IntPtr) ((int)p + 4);

					connStatus.COMPort = Marshal.PtrToStringUni(p);
					p = (IntPtr) ((int)p + 16);  //8 TCHARs

					connStatus.DeviceAddr = Marshal.PtrToStringUni(p);
					p = (IntPtr) ((int)p + 48);  //24 TCHARs

					connStatus.DeviceName = Marshal.PtrToStringUni(p);

					//Fire BtConnStatus event to any listeners
					if (BtStack.BtConnStatus != null)
						BtStack.BtConnStatus(connStatus);

					//Have to get DLL to do its own memory delete
					BT_DeletePointer(msg.LParam);
				}
				#endregion

				#region BTMSG_SENDFILE_PROGRESS
				//--------------------------------
				// BTMSG_SENDFILE_PROGRESS
				//--------------------------------
				else
				if (msg.Msg == BTMSG_SENDFILE_PROGRESS)
				{
					//WPARAM = completion code
					//LPARAM = 0

					//Fire BtSendFileProgress event to any listeners
					if (BtStack.BtSendFileProgress != null)
						BtStack.BtSendFileProgress( (Int32) msg.WParam, (Int32) msg.LParam);
				}
				#endregion

				#region BTMSG_SENDFILE_COMPLETE
				//--------------------------------
				// BTMSG_SENDFILE_COMPLETE
				//--------------------------------
				else
				if (msg.Msg == BTMSG_SENDFILE_COMPLETE)
				{
					//WPARAM = completion code
					//LPARAM = 0

					//Fire BtSendFileComplete event to any listeners
					if (BtStack.BtSendFileComplete != null)
						BtStack.BtSendFileComplete( (eBTRC) ( (int) msg.WParam));
				}
				#endregion
			
				#region BTMSG_BC_COMPLETE
				//--------------------------------
				// BTMSG_BC_COMPLETE
				//--------------------------------
				else
				if (msg.Msg == BTMSG_BC_COMPLETE)
				{
					//WPARAM = completion code
					//LPARAM = 0

					//Fire BtBcComplete event to any listeners
					if (BtStack.BtBcComplete != null)
						BtStack.BtBcComplete( (eBTRC) ( (int) msg.WParam));
				}
				#endregion

				#region BTMSG_SEARCH_COMPLETE
				//--------------------------------
				// BTMSG_SEARCH_COMPLETE
				//--------------------------------
				else
				if (msg.Msg == BTMSG_SEARCH_COMPLETE)
				{
					//Fire BtSearchComplete event to any listeners
					if (BtStack.BtSearchComplete != null)
						BtStack.BtSearchComplete();
				}
				#endregion
					
				#region BTMSG_RECVFILE_COMPLETE
				//--------------------------------
				// BTMSG_RECVFILE_COMPLETE
				//--------------------------------
				else
				if (msg.Msg == BTMSG_RECVFILE_COMPLETE)
				{
					//WPARAM = 0
					//LPARAM = RecvFile struct: devaddr, devname, filename
					IntPtr p = msg.LParam;

					//Fill in C# device object with found-device fields
					RecvFileInfo info = new RecvFileInfo();

					info.DeviceAddr = Marshal.PtrToStringUni(p);
					p = (IntPtr) ((int)p + 48);  //24 TCHARs

					info.DeviceName = Marshal.PtrToStringUni(p);
					p = (IntPtr) ((int)p + 512);  //256 TCHARs

					info.FileName = Marshal.PtrToStringUni(p);

					//Fire BtRecvFileComplete event to any listeners
					if (BtStack.BtRecvFileComplete != null)
						BtStack.BtRecvFileComplete(info);

					//Have to get DLL to do its own memory delete
					BT_DeletePointer(msg.LParam);
				}
				#endregion

				base.WndProc(ref msg);
			}
		}
		#endregion

		#region P/Invoke Definitions
		//---------------------------------------------------------------------------------
		//P/Invoke function prototypes from BTAccess flat api extensions (in BTAccess.dll)
		//---------------------------------------------------------------------------------
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_ConnectToStack(IntPtr hWnd);
		[DllImport("BTAccess.dll")]
		static extern void			  BT_DisconnectFromStack();
		[DllImport("BTAccess.dll")]
		static extern bool			  BT_IsRadioOn();
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_RadioOn();
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_RadioOff();
		[DllImport("BTAccess.dll")]
		static extern bool			  BT_GetBtAddress([Out] byte[] DeviceAddr);
		[DllImport("BTAccess.dll")]
		static extern void			  BT_GetVersion([Out] StringBuilder sVersion);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_StartDeviceSearch();
		[DllImport("BTAccess.dll")]
		static extern void 			  BT_StopDeviceSearch();
		[DllImport("BTAccess.dll")]
		static extern void 			  BT_GetGeneralSecurityOptions([Out] StringBuilder sDevName,
			[Out] StringBuilder sFTPRootDir,
			out   Int32 IsDiscoverable,
			out   Int32 IsConnectable);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_SetGeneralSecurityOptions(string sDevName,
			string sFTPRootDir,
			Int32  IsDiscoverable,
			Int32  IsConnectable);
		[DllImport("BTAccess.dll")]
		static extern void 			  BT_GetSvcSecurityOptions    (eBTSVC eSvc,
			out Int32 IsEnabled,
			out Int32 IsAuthRequired,
			out Int32 IsPINRequired,
			out Int32 IsEncryptRequired);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_SetSvcSecurityOptions   (eBTSVC eSvc,
			Int32 IsEnabled,
			Int32 IsAuthRequired,
			Int32 IsPINRequired,
			Int32 IsEncryptRequired);
		[DllImport("BTAccess.dll")]
		static extern IntPtr		  BT_GetActiveConnections(ref int Cnt);
		[DllImport("BTAccess.dll")]
		static extern void			  BT_DeleteActiveConnections(IntPtr pList);
		[DllImport("BTAccess.dll")]
		static extern void			  BT_DeletePointer(IntPtr LParam);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_RegisterForRecvFileEvent();
		[DllImport("BTAccess.dll")]
		static extern void			  BT_UnregisterForRecvFileEvent();
		#endregion

		#region BtStack Wrapper Functions
		//---------------------------------------------------------------------------------
		// Private member data
		//---------------------------------------------------------------------------------
		internal BTMsgWin btWin = new BTMsgWin();   //Our MessageWindow handler

		//---------------------------------------------------
		// BtStack.Connect
		//---------------------------------------------------
		public eBTRC Connect()
		{
			return BT_ConnectToStack(btWin.Hwnd);
		}

		//---------------------------------------------------
		// BtStack.Disconnect
		//---------------------------------------------------
		public void Disconnect()
		{
			BT_DisconnectFromStack();
		}

		//---------------------------------------------------
		// BtStack.IsRadioOn
		//---------------------------------------------------
		public bool IsRadioOn()
		{
			return BT_IsRadioOn();
		}

		//---------------------------------------------------
		// BtStack.RadioOn
		//---------------------------------------------------
		public eBTRC RadioOn()
		{
			return BT_RadioOn();
		}

		//---------------------------------------------------
		// BtStack.RadioOff
		//---------------------------------------------------
		public eBTRC RadioOff()
		{
			return BT_RadioOff();
		}

		//---------------------------------------------------
		// BtStack.GetVersion
		//---------------------------------------------------
		public string GetVersion()
		{
			StringBuilder s = new StringBuilder(80);
			BT_GetVersion(s);
			string sVersion=s.ToString();
			return sVersion;
		}
	
		//---------------------------------------------------
		// BtStack.GetGeneralSecurityOptions
		//---------------------------------------------------
		public GenSecOptions GetGeneralSecurityOptions()
		{
			GenSecOptions Opts = new GenSecOptions();

			StringBuilder sName = new StringBuilder(120);
			StringBuilder sFTP  = new StringBuilder(256);
			Int32 iDisc;
			Int32 iConn;
			BT_GetGeneralSecurityOptions(sName, sFTP, out iDisc, out iConn);

			Opts.LocalDeviceName = sName.ToString();
			Opts.FTPRootDir      = sFTP.ToString();
			Opts.IsDiscoverable  = iDisc==0 ? false : true;
			Opts.IsConnectable   = iConn==0 ? false : true;

			return Opts;
		}

		//---------------------------------------------------
		// BtStack.SetGeneralSecurityOptions
		//---------------------------------------------------
		public eBTRC SetGeneralSecurityOptions(GenSecOptions Opts)
		{
			eBTRC rc = BT_SetGeneralSecurityOptions(Opts.LocalDeviceName, 
				Opts.FTPRootDir,
				Opts.IsDiscoverable ? 1 : 0,
				Opts.IsConnectable  ? 1 : 0);
			return rc;
		}

		//---------------------------------------------------
		// BtStack.GetSvcSecurityOptions
		//---------------------------------------------------
		public SvcSecOptions GetSvcSecurityOptions(eBTSVC eSvc)
		{
			SvcSecOptions Opts = new SvcSecOptions();

			Int32 ia, ib, ic, id;
			BT_GetSvcSecurityOptions(eSvc, out ia, out ib, out ic, out id);

			Opts.IsEnabled			= ia==0 ? false : true;
			Opts.IsAuthRequired		= ib==0 ? false : true;
			Opts.IsPINRequired		= ic==0 ? false : true;
			Opts.IsEncryptRequired	= id==0 ? false : true;

			return Opts;
		}

		//---------------------------------------------------
		// BtStack.SetSvcSecurityOptions
		//---------------------------------------------------
		public eBTRC SetSvcSecurityOptions(eBTSVC eSvc, SvcSecOptions Opts)
		{
			eBTRC rc = BT_SetSvcSecurityOptions(eSvc,
				Opts.IsEnabled			? 1 : 0,
				Opts.IsAuthRequired		? 1 : 0,
				Opts.IsPINRequired		? 1 : 0,
				Opts.IsEncryptRequired	? 1 : 0);

			return rc;
		}

		//---------------------------------------------------
		// BtStack.GetBtAddress
		//---------------------------------------------------
		public byte[] GetBtAddress()
		{
			byte[] btAddr = new byte[6];
			BT_GetBtAddress(btAddr);
			return btAddr;
		}

		//---------------------------------------------------
		// BtStack.StartDeviceSearch
		//---------------------------------------------------
		public eBTRC StartDeviceSearch()
		{
			return BT_StartDeviceSearch();
		}

		//---------------------------------------------------
		// BtStack.StopDeviceSearch
		//---------------------------------------------------
		public void StopDeviceSearch()
		{
			BT_StopDeviceSearch();
		}
		
		//---------------------------------------------------
		// BtStack.GetActiveConnections
		//---------------------------------------------------
		public ArrayList GetActiveConnections()
		{
			int Cnt=0;
			ArrayList SessionList = new ArrayList();

			//Get list of connnections
			IntPtr pList = BT_GetActiveConnections(ref Cnt);
			IntPtr ptr = pList;

			//Create ArrayList of SessionListItems to return to caller
			for (int i=0; i<Cnt; i++)
			{
				SessionListItem SessionItem = new SessionListItem();

				SessionItem.SvcType = (eBTSVC) Marshal.ReadInt32(ptr);
				ptr = (IntPtr) ((int)ptr + 4);

				SessionItem.InitiatedLocally = Marshal.ReadInt32(ptr)==0 ? true : false;
				ptr = (IntPtr) ((int)ptr + 4);

				SessionItem.DeviceAddr  = Marshal.PtrToStringUni(ptr);
				ptr = (IntPtr) ((int)ptr + 48);

				SessionItem.DeviceName  = Marshal.PtrToStringUni(ptr);
				ptr = (IntPtr) ((int)ptr + 512);

				SessionItem.SvcName  = Marshal.PtrToStringUni(ptr);
				ptr = (IntPtr) ((int)ptr + 160);

				SessionList.Add(SessionItem);
			}

			//Have to get DLL to do its own memory delete
			BT_DeleteActiveConnections(pList);

			return SessionList;
		}	

		//---------------------------------------------------
		// BtStack.RegisterForRecvFileEvent
		//---------------------------------------------------
		public eBTRC RegisterForRecvFileEvent()
		{
			return BT_RegisterForRecvFileEvent();
		}

		//---------------------------------------------------
		// BtStack.UnregisterForRecvFileEvent
		//---------------------------------------------------
		public void UnregisterForRecvFileEvent()
		{
			BT_UnregisterForRecvFileEvent();
		}
		#endregion
	}
	#endregion

	#region BtDevice Class
	public class BtDevice
	{
		#region P/Invoke Definitions
		//---------------------------------------------------------------------------------
		//P/Invoke function prototypes from new BTAccess flat api (within BTAccess.dll)
		//---------------------------------------------------------------------------------
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_SPPConnect(string DeviceAddr, string SvcName);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_SPPDisconnect(string DeviceAddr);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_DUNConnect(string DeviceAddr, string ConnectionName);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_DUNDisconnect(string DeviceAddr);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_LAPConnect(string DeviceAddr);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_LAPDisconnect(string DeviceAddr);
		[DllImport("BTAccess.dll")]
		public static extern eBTRC			  BT_PANConnect(string DeviceAddr, ePAN_NODETYPE ePanType);
		[DllImport("BTAccess.dll")]
		public static extern eBTRC			  BT_PANDisconnect(string DeviceAddr, ePAN_NODETYPE ePanType);

		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_SendFile(string DeviceAddr, string FileName);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_CancelSendFile(string DeviceAddr);

		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_GetBusinessCard(string DeviceAddr);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_SendBusinessCard(string DeviceAddr);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_ExchangeBusinessCards(string DeviceAddr);

		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_CreateBond(string DeviceAddr, string PinCode);
		[DllImport("BTAccess.dll")]
		static extern eBTRC			  BT_RemoveBond(string DeviceAddr);
		[DllImport("BTAccess.dll")]
		static extern int 			  BT_IsBonded(string DeviceAddr);

		[DllImport("BTAccess.dll")]
		static extern IntPtr		  BT_GetServiceList(string DeviceAddr, eBTSVC eSvc, ref int Cnt);
		[DllImport("BTAccess.dll")]
		static extern void			  BT_DeleteServiceList(IntPtr pList);
		[DllImport("BTAccess.dll")]
		static extern eBTRSSI_LEVEL	  BT_GetSignalStrength(string DeviceAddr);
		#endregion

		#region BtDevice Wrapper Functions
		//Override ToString to show device name for convenience
		public override string ToString()
		{
			return DeviceName;
		}
		
		//---------------------------------------------------------------------------------
		// Private member data
		//---------------------------------------------------------------------------------
		public string			DeviceAddr;
		public string			DeviceName;
		public eBTDEVCLASS		DeviceClass;
		public eBTDEVTYPE		DeviceType;
		public bool				IsConnected;

		//-----------------------------------------------------------
		// Connection methods
		//-----------------------------------------------------------
		public eBTRC SPPConnect()
		{
			return BT_SPPConnect(DeviceAddr, "");
		}
		public eBTRC SPPConnect(string SvcName)
		{
			return BT_SPPConnect(DeviceAddr, SvcName);
		}
		public eBTRC SPPDisconnect()
		{
			return BT_SPPDisconnect(DeviceAddr);
		}
		public eBTRC DUNConnect(string ConnectionName)
		{
			return BT_DUNConnect(DeviceAddr, ConnectionName);
		}
		public eBTRC DUNDisconnect()
		{
			return BT_DUNDisconnect(DeviceAddr);
		}
		public eBTRC LAPConnect()
		{
			return BT_LAPConnect(DeviceAddr);
		}
		public eBTRC LAPDisconnect()
		{
			return BT_LAPDisconnect(DeviceAddr);
		}
		public eBTRC PANConnect(ePAN_NODETYPE ePanType)
		{
			return BT_PANConnect(DeviceAddr, ePanType);
		}
		public eBTRC PANDisconnect(ePAN_NODETYPE ePanType)
		{
			return BT_PANDisconnect(DeviceAddr, ePanType);
		}

		//-----------------------------------------------------------
		// File Transfer methods
		//-----------------------------------------------------------
		public eBTRC SendFile(string FileName)
		{
			return BT_SendFile(DeviceAddr, FileName);
		}
		public eBTRC CancelSendFile()
		{
			return BT_CancelSendFile(DeviceAddr);
		}

		//-----------------------------------------------------------
		// Business Card methods
		//-----------------------------------------------------------
		public eBTRC GetBusinessCard()
		{
			return BT_GetBusinessCard(DeviceAddr);
		}
		public eBTRC SendBusinessCard()
		{
			return BT_SendBusinessCard(DeviceAddr);
		}
		public eBTRC ExchangeBusinessCards()
		{
			return BT_ExchangeBusinessCards(DeviceAddr);
		}

		//-----------------------------------------------------------
		// Bonding methods
		//-----------------------------------------------------------
		public eBTRC CreateBond(string PinCode)
		{
			return BT_CreateBond(DeviceAddr, PinCode);
		}
		public eBTRC RemoveBond()
		{
			return BT_RemoveBond(DeviceAddr);
		}
		public bool IsBonded()
		{
			return BT_IsBonded(DeviceAddr) == 1 ? true : false;
		}

		//-----------------------------------------------------------
		// Misc. methods
		//-----------------------------------------------------------
		public ArrayList GetServiceList(eBTSVC eSvc)
		{
			int Cnt=0;
			ArrayList SvcList = new ArrayList();

			//Get list of svcs returned as 4-byte SvcID and 80-byte SvcName string
			IntPtr pList = BT_GetServiceList(DeviceAddr, eSvc, ref Cnt);
			IntPtr ptr = pList;

			//Create ArrayList of SvcListItems to return to callerfl
			for (int i=0; i<Cnt; i++)
			{
				SvcListItem SvcItem = new SvcListItem();

				SvcItem.eSvc = (eBTSVC) Marshal.ReadInt32(ptr);
				ptr = (IntPtr) ((int)ptr + 4);

				SvcItem.SvcName  = Marshal.PtrToStringUni(ptr);
				ptr = (IntPtr) ((int)ptr + 160);

				SvcList.Add(SvcItem);
			}

			//Have to get DLL to do its own memory delete
			BT_DeleteServiceList(pList);

			return SvcList;
		}

		public eBTRSSI_LEVEL GetSignalStrength()
		{
			eBTRSSI_LEVEL eLevel = BT_GetSignalStrength(DeviceAddr);
			return eLevel;
		}

		#endregion
	}
	#endregion 
}