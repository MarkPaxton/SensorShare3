using System;
using System.Collections;
using System.Collections.Generic;

namespace SensorShare.Compact.Bluetooth
{
    public class BTDeviceHashtable : Hashtable
    {
        public BtDevice this[string key]
        {
            get { return (BtDevice)base[key]; }
            set { base[key] = value; }
        }
    }

    /// <summary>
    /// Bluetooth Device Types associated with fields in the BTDevices.DeviceType database
    /// </summary>
    public enum BTDeviceTypes
    {
        Client = 1,
        Server = 2,
        PC = 3,
        Phone = 4,
        Other = 0,
    }



    public static class BTHelper
    {
        public static readonly List<string> KnownDevices =
            new List<string>(new string[] {                
                "FA:73:5E:37:12:00",
                "FA:73:5E:37:00:08",
                "72:FF:8C:37:12:00",
                "72:FF:8C:37:00:08",
                "41:2F:8D:37:12:00",
                "41:2F:8D:37:00:08",
                "C6:01:8D:37:12:00",
                "C6:01:8D:37:00:08",
                "8D:2F:41:37:12:00",
                "8D:2F:41:37:00:08",
                "D6:0C:12:37:12:00",
                "D6:0C:12:37:00:08",
                "8F:73:03:37:12:00",
                "8F:73:03:37:00:08"
                 });

        //BTHelper()
        //{
        //    //KnownDevices.Add("FA:73:5E:37:12:00"); // "00:12:37:5E:73:FA"
        //    //KnownDevices.Add("72:FF:8C:37:12:00"); // "00:12:37:8C:FF:72" PDA36
        //    //KnownDevices.Add("41:2F:8D:37:12:00"); // "00:12:37:8D:2F:41"
        //    //KnownDevices.Add("C6:01:8D:37:12:00"); // "00:12:37:8D:01:C6"
        //    //KnownDevices.Add("8D:2F:41:37:12:00"); // "00:12:37:41:2F:8D"
        //    //KnownDevices.Add("C6:01:8D:37:00:08"); // PDA38
        //}

        public static string StringFcnResult(eBTRC rc)
        {
            string s;
            s = String.Format("{0}",
                rc == eBTRC.BT_OK ? "BT_OK" :
                rc == eBTRC.BT_FAIL ? "BT_FAIL" :
                rc == eBTRC.BT_DEVICE_NOTAVAIL ? "BT_DEVICE_NOTAVAIL" :
                rc == eBTRC.BT_SVC_NOT_SUPPORTED ? "BT_SVC_NOT_SUPPORTED" :
                rc == eBTRC.BT_SVC_NOT_UNIQUE ? "BT_SVC_NOT_UNIQUE" :
                                                 "unknown rc");
            return s;
        }

        public static string PANConnectResultToString(eBTRC result)
        {
            string errorMessage;
            switch (result)
            {
                case eBTRC.BT_FAIL:
                    errorMessage = "Connection attempt has failed";
                    break;
                case eBTRC.BT_SVC_NOT_SUPPORTED:
                    errorMessage = "The device does not support the PAN service";
                    break;
                case eBTRC.BT_DEVICE_NOTAVAIL:
                    errorMessage = "The device is not responding to Bluetooth communications";
                    break;
                case eBTRC.BT_UNSUPPORTED:
                    errorMessage = "This is the return code if the method is called from the standard BTaccess library.";
                    break;
                default:
                    errorMessage = "Unknown result returned.";
                    break;
            }
            return errorMessage;
        }

        private static string serviceTypeToString(eBTSVC svc)
        {
            string serviceType;
            switch (svc)
            {
                case eBTSVC.BTSVC_SENDFILE:
                    serviceType = "FileTransfer";
                    break;
                case eBTSVC.BTSVC_GETBUSINESSCARD:
                    serviceType = "BusCard";
                    break;
                case eBTSVC.BTSVC_EXCHANGECARDS:
                    serviceType = "BusCard";
                    break;
                case eBTSVC.BTSVC_SENDBUSINESSCARD:
                    serviceType = "BusCard";
                    break;
                case eBTSVC.BTSVC_SERIAL:
                    serviceType = "SPP";
                    break;
                case eBTSVC.BTSVC_LAP:
                    serviceType = "LAP";
                    break;
                case eBTSVC.BTSVC_DIALUP:
                    serviceType = "DUN";
                    break;
                case eBTSVC.BTSVC_PANHOST:
                    serviceType = "PAN Host";
                    break;
                case eBTSVC.BTSVC_PANUSER:
                    serviceType = "PAN User";
                    break;
                default:
                    serviceType = "Unknown";
                    break;
            }
            return serviceType;
        }
    }
}


