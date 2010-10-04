using System;

namespace ScienceScope
{
    public partial class Logbook
    {
        public class Command
        {
            public static readonly Command ReadASCII = new Command(new byte[] { Convert.ToByte(0x20) }, "Read data ASCII", 33, 7000);
            public static readonly Command GetVersion = new Command(new byte[] { Convert.ToByte(0x30) }, "Get Logbook version", 3, 600);
            public static readonly Command BatteryTest = new Command(new byte[] { Convert.ToByte(0x72) }, "Check battery status", 2, 600);
            public static readonly Command KeepAwake = new Command(new byte[] { Convert.ToByte(0x70) }, "Keep awake", 1, 500);
            public static readonly Command PowerDownAndStop = new Command(new byte[] { Convert.ToByte(0x74) }, "Power down and stop", 1, 500);
            public static readonly Command ConfirmExtendedProtocol = new Command(new byte[] { Convert.ToByte(0x52) }, "Confirm extended protocol", 1, 500);
            public static readonly Command IdentifySensors = new Command(new byte[] { Convert.ToByte(0x54) }, "Identify sensors", 5, 500);
            public static readonly Command GetTime = new Command(new byte[] { Convert.ToByte(0x84) }, "Read time and date", 5, 500);
            public static readonly Command GetBatteryLevel = new Command(new byte[] { Convert.ToByte(0xAA) }, "Get battery level", 2, 500);
            public static readonly Command GetSensorRanges = new Command(new byte[] { Convert.ToByte(0x8A) }, "Get sensor ranges", 5, 500);
            //            public static readonly Command GetInputIDs = new Command(new byte[] { Convert.ToByte(0xE8) }, "Get input IDs", 17, 500);
            public static readonly Command InputPowerOn = new Command(new byte[] { Convert.ToByte(0x36) }, "Turn input power on", 1, 500);

            public byte[] command;
            public string name;
            public int responseLength;
            public int timeout;
            public byte commandConfirm;

            public Command(byte[] command, string name, int responseLength, int timeout, byte commandConfirm)
            {
                this.command = command;
                this.name = name;
                this.responseLength = responseLength;
                this.timeout = timeout;
                this.commandConfirm = commandConfirm;
            }

            public Command(byte[] command, int responseLength, int timeout)
            {
                this.command = command;
                this.name = "";
                this.responseLength = responseLength;
                this.timeout = timeout;
                this.commandConfirm = Convert.ToByte(Convert.ToInt16(command[0]) - 1);
            }

            public Command(byte[] command, string name, int responseLength, int timeout)
            {
                this.command = command;
                this.name = name;
                this.responseLength = responseLength;
                this.timeout = timeout;
                this.commandConfirm = Convert.ToByte(Convert.ToInt16(command[0]) - 1);
            }
        }

    }
}