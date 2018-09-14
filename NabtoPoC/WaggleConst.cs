namespace NabtoPoC
{
    static class WaggleConst
    {
        private static string TAG = "WaggleConst";

	    public const byte E_GCODE_REPLY = 0x02;
               
        public const byte E_PRT_NONE = 0x00;
        public const byte E_PRT_START = 0x03;
        public const byte E_PRT_PAUSE = 0x04;
        public const byte E_PRT_STOP = 0x05;
               
        public const byte E_NOZZLE = 0x08;
        public const byte E_PERCENT = 0x09;
        public const byte E_USB_RECON = 0x0A;
        public const byte E_LED_STATUS = 0x0B;
               
        public const byte E_FWUPDATE = 0x0F;
        public const byte E_PLUGINUPDATE = 0x2F;
               
        public const byte E_STATUS = 0x11;
        public const byte E_SSID = 0x12;
               
        public const byte E_SET_NOZZLE = 0x13;
        public const byte E_SET_BED = 0x14;
        public const byte E_SET_SCRT = 0x15;
               
        public const byte E_BED = 0x18;
               
        public const byte E_SET_LED = 0x1B;
               
        public const byte E_SET_FILENAME = 0x1D;
        public const byte E_SET_FILESIZE = 0x1E;
        public const byte E_SET_FILEDOWNLOAD = 0x07;
               
        public const byte E_GET_FWVER = 0x1F;

        //////////////////////////////////////////////////////////////////
        //
        public const byte PRT_STATUS_IDLE = 0x03;
        public const byte PRT_STATUS_PRITING = 0x04;
        public const byte PRT_STATUS_PAUSE = 0x05;
        public const byte PRT_STATUS_NOTCONNECTED = (byte)0xF3;

        public const byte PRT_STATUS_INIT = 0x0;
        public const byte PRT_STATUS_FAILED = (byte)0xFE;

        public const byte LED_OFF = 0x0;
        public const byte LED_ON = 0x1;

        public static byte[] CMD_START = { 0x01, 0x55, (byte)0x00 };
        public static byte[] CMD_STATUS = { (byte)0xA5, 0x11, (byte)0xFF };
        public static byte[] CMD_TEMP = { (byte)0xA5, 0x08, (byte)0xFF };

        public static byte[] CMD_PRT_START = { (byte)0xA5, (byte)E_PRT_START, (byte)0xFF };
        public static byte[] CMD_PRT_PAUSE = { (byte)0xA5, (byte)E_PRT_PAUSE, (byte)0xFF };
        public static byte[] CMD_PRT_STOP = { (byte)0xA5, (byte)E_PRT_STOP, (byte)0xFF };

        public static byte[] CMD_SET_LED_ON = { (byte)0xA5, (byte)0x1B, (byte)0x0B, (byte)0xFF };
        public static byte[] CMD_SET_LED_OFF = { (byte)0xA5, (byte)0x1B, (byte)0x0C, (byte)0xFF };
        public static byte[] CMD_GET_LED_ON = { (byte)0xA5, (byte)0x0B, (byte)0xFF };

        public static byte[] CMD_GET_FWVER = { (byte)0xA5, (byte)E_GET_FWVER, (byte)0xFF };

    }
}
