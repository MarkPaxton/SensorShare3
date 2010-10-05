using System;
//using OpenNETCF.Net;
using System.Net;
using System.Windows.Forms;
using OpenNETCF.Net.NetworkInformation;

namespace SensorShare.Compact
{
   public partial class SensorShareServer
   {
      public void InitialiseWiFi()
      {
         NetworkInterface[] adapters = null;

         adapters = (NetworkInterface[]) WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces();
         EAPParameters eapParam = new EAPParameters();
         eapParam.Enable8021x = false;
         foreach (NetworkInterface adapterToCast in adapters)
         {
            if (adapterToCast.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
            {
               WirelessZeroConfigNetworkInterface adapter = (WirelessZeroConfigNetworkInterface)adapterToCast;
               if (adapter.AssociatedAccessPoint != SensorShareConfig.WiFiName)
               {
                  //log.Append("InitialiseWiFi", "Setting Wireless Settings");
                  try
                  {
                     if (adapter.AssociatedAccessPoint != SensorShareConfig.WiFiName)
                     {
                        adapter.AddPreferredNetwork(SensorShareConfig.WiFiName, false, SensorShareConfig.WiFiKey, 1,
                            AuthenticationMode.WPAAdHoc,
                            WEPStatus.WEPEnabled, eapParam);
                        adapter.ConnectToPreferredNetwork(SensorShareConfig.WiFiName);
                     }
                  }
                  catch (Exception ex)
                  { }
               }
            }
         }
      }
      //public void InitialiseWiFi()
      //{
      //   AdapterCollection adapters = null;

      //   adapters = OpenNETCF.Net.Networking.GetAdapters();
      //   OpenNETCF.Net.EAPParameters eapParam = new OpenNETCF.Net.EAPParameters();
      //   OpenNETCF.Net.AccessPointCollection preferredAPs;
      //   OpenNETCF.Net.AccessPoint sensorShareAP;
      //   eapParam.Enable8021x = false;
      //   foreach (Adapter adapter in adapters)
      //   {
      //      if (adapter.IsWireless)
      //      {
      //         if (adapter.AssociatedAccessPoint != SensorShareConfig.WiFiName)
      //         {
      //            log.Append("InitialiseWiFi", "Setting Wireless Settings");
      //            try
      //            {
      //               preferredAPs = adapter.PreferredAccessPoints;
      //               sensorShareAP = preferredAPs.FindBySSID(SensorShareConfig.WiFiName);
      //               if (sensorShareAP == null)
      //               {
      //                  adapter.SetWirelessSettingsAddEx(SensorShareConfig.WiFiName, false, SensorShareConfig.WiFiKey, 1,
      //                      AuthenticationMode.Ndis802_11AuthModeShared,
      //                      WEPStatus.Ndis802_11WEPEnabled,
      //                      eapParam);
      //               }
      //               else
      //               {
      //                  adapter.SetWirelessSettings(SensorShareConfig.WiFiName);
      //               }
      //            }
      //            catch (Exception ex)
      //            { }
      //         }
      //      }
      //   }
      //}

      // Event handler for network connection events
      private void DeviceManagement_NetworkConnected()
      {
         string logMessage = "Network connection detected";
         log.Append("DeviceManagement_NetworkConnected", logMessage);

         // List adaptors and check for valid IP addresses
         NetworkInterface[] adapters = (NetworkInterface[]) NetworkInterface.GetAllNetworkInterfaces();
         foreach (NetworkInterface adapter in adapters)
         {
            if (!adapter.CurrentIpAddress.Equals(IPAddress.Any))
            {
               logMessage = String.Format("{0}: {1}", adapter.Description, adapter.CurrentIpAddress);
               //MessageBox.Show(logMessage);
               log.Append("DeviceManagement_NetworkConnected", logMessage);
            }
         }
      }
   }
}