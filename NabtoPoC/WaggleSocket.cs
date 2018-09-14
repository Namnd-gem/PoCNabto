using Nabto.Client;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NabtoPoC
{
    class WaggleSocket
    {
        private static string TAG = "WaggleSocket";
        private Socket socket = null;
        private readonly string ip = "127.0.0.1";
        private readonly int _port = 80;
        private int _devicePort = 8080;
        private byte printStatus = WaggleConst.PRT_STATUS_NOTCONNECTED; 
        public int percent = 0;       
        Thread thread;
        public bool bThreadRunnig = true;
        public bool isWorking = false;
        byte cmdPrintStatus = WaggleConst.E_PRT_NONE;
        string secret_code = "924230dc241cd2ce2d5285b400f5d98c";
        string _deviceId = "frhppr.2.us.ytong.rakwireless.com";
        public void initSocket()
        {
            Log.d(TAG, "initSocket()");

            CloseSocket();

            isWorking = false;
            
            NabtoClient nabto = new NabtoClient(); // Get a reference to the Nabto client runtime and start it up.
            var email = "guest";
            var password = "123456";
            
            var threadStart = false;
            try
            {
                    using (Session session = nabto.CreateSession(email, password)) // Create a session using the specified user credentials.
                    {
                        Console.WriteLine("Creating tunnel...");
                        using (Nabto.Client.Tunneling.Tunnel tunnel = session.CreateTunnel(_deviceId, 8080, "127.0.0.1", 80))
                        {
                            //var device = session.CreateDeviceConnection(deviceId);

                            //device.
                            while (Console.KeyAvailable == false)
                            {
                            Console.Write("Tunnel created - current state: {0}\r", tunnel.State);
                            var remoteRelayMicroState = Nabto.Client.Tunneling.TunnelState.RemoteRelayMicro;
                            var RemotePeerToPeerState = Nabto.Client.Tunneling.TunnelState.RemotePeerToPeer;
                            if (threadStart == false && (tunnel.State == remoteRelayMicroState || tunnel.State == RemotePeerToPeerState))
                            {
                                SetSocket(ip, _devicePort);
                                DoCommandStart();
                                DoCommandSecretCode();
                                var socketPrinterRunnable = new SocketPrinterRunnable(bThreadRunnig, socket);
                                thread = new Thread(socketPrinterRunnable.run);
                                thread.Start();
                                threadStart = true;
                                int ccc = 1;
                                while (ccc>0)
                                {
                                    int action = 0;
                                    Console.WriteLine("action:");
                                    action = int.Parse(Console.ReadLine());
                                    if (action == 1)
                                    {
                                        DoCommandTemp();
                                    } 
                                    else if (action==2) {                                  
                                        string attachment_file_name = "20180912072713";
                                        byte[] array = Encoding.UTF8.GetBytes(attachment_file_name);
                                        int c = array.Length + 3;
                                        byte[] cmdSend = new byte[c];
                                        cmdSend[0] = (byte)0xA5;
                                        cmdSend[1] = 0x06;
                                        for (int i = 0; i < array.Length; i++)
                                        {
                                            cmdSend[i + 2] = array[i];
                                        }
                                        cmdSend[c - 1] = (byte)0xFF;
                                        Send(cmdSend);
                                    }
                                    else
                                    {
                                        DoSendPrint();
                                    }
                                    Console.WriteLine("continue:");
                                    ccc = int.Parse(Console.ReadLine());
                                }
                               
                            }                            
                        }
                            Console.WriteLine();
                            Console.WriteLine("Shutting down...");
                            Console.ReadKey(true);
                        }
                    }
              
            }
            catch (Exception e)
            {
                Log.d(TAG, e.Message);
            }
            //}
        }
        
        //TODO Update
        public void onResult(string result)
        {
            // TODO Auto-generated method stub
            if (result.Equals("CONNECT_TIMEOUT") ||
                    result.Equals("NTCS_CLOSED") ||
                    result.Equals("NTCS_UNKNOWN") ||
                    result.Equals("FAILED"))
            {
                //TODO update
                //if (_remoteTunnel1 != null)
                //{


                //    Log.d(TAG, "_remoteTunnel1 : closeTunnels ");


                //    _remoteTunnel1.closeTunnels();
                //    _remoteTunnel1 = null;

                //    mHandler.removeMessages(400);
                //    mHandler.sendEmptyMessageDelayed(400, 10);
                //}
            }
            else
            {
                _devicePort = 3333;


                try
                {
                    SetSocket(ip, _devicePort);

                   var socketPrinterRunnable = new SocketPrinterRunnable(bThreadRunnig, socket);
                    thread = new Thread(socketPrinterRunnable.run);
                    thread.Start();

                   
                    DoCommandStart();

                    Log.d(TAG, "doCommandStart =================== ");

                   
                    DoCommandSecretCode();

                    Log.d(TAG, "doCommandSecretCode =================== ");


                }
                catch (Exception e1)
                {
                    Log.d(TAG, e1.Message);
                }
            }
        }
       
        public void CloseSocket()
        {

            Log.d(TAG, "closeSocket");

            try
            {
                isWorking = false;

                if (socket != null)
                {
                    socket.Close();
                }
                
                bThreadRunnig = false;
                //			if (socketPrinterRunnable.getState() != Thread.State.NEW)

                if (thread != null)
                    thread.Abort();                
            }
            catch (Exception e)
            {
                Log.d(TAG, e.Message);
            }
        }

        public void SetSocket(string ip, int port)
        {

            //	closeSocket();

            bThreadRunnig = true;

            try {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ip, port);
                //networkWriter = new DataOutputStream(new BufferedOutputStream(socket.getOutputStream()));
                //networkReader = new DataInputStream(new BufferedInputStream(socket.getInputStream()));
            } catch (Exception e) {
                Log.d(TAG, e.Message);
            }
        }

        public void DoCommandStart()
        {

            try
            {
                Send(WaggleConst.CMD_START);
                Log.d(TAG, "doCommandStart =================== ");
            }
            catch (Exception e)
            {
                Log.d("doCommandStart =======", e.Message);
            }
        }

        public void DoCommandSecretCode()
        {
            
            byte[] array = Encoding.UTF8.GetBytes(secret_code);
            string byteToString = Encoding.ASCII.GetString(array, 0, array.Length);
            Log.d(TAG, "doCommandSecretCode =================== : " + byteToString);

            int c = array.Length + 3;
            byte [] cmdSend= new byte[c];
            cmdSend[0] = (byte)0xA5;
            cmdSend[1] = 0x15;
            for (int i = 0; i < array.Length; i++)
            {
                cmdSend[i + 2] = array[i];
            }
            cmdSend[c - 1] = (byte)0xFF;
            try
            {
                Send(cmdSend);
            }
            catch (Exception e)
            {
                Log.d("doCommandSecretCode =======", e.Message);
            }
        }

        public void DoCommandStatus()
        {

            Log.d(TAG, "doCommandStatus =================== : ");

            try
            {
               Send(WaggleConst.CMD_STATUS);
            }
            catch (Exception e)
            {
                Log.d("doCommandStatus =======", e.Message);
            }
        }
        
        public void DoCommandTemp()
        {

            Log.d(TAG, "doCommandTemp =================== : ");
            try
            {
                Send(WaggleConst.CMD_TEMP);
            }
            catch (Exception e)
            {
                Log.d("doCommandTemp =======", e.Message);
            }
        }
        
        public void SetCommandLedOnOff(bool bOnOff)
        {

            Log.d(TAG, "setCommandLedOnOff =================== bOnOff : " + bOnOff);
            try
            {
                var cmd = bOnOff ? WaggleConst.CMD_SET_LED_ON : WaggleConst.CMD_SET_LED_OFF;
                Send(cmd);
            }
            catch (Exception e)
            {
                Log.d("setCommandLedOnOff =======", e.Message);
            }
        }

        public void getCommandLedOnOff()
        {

            Log.d(TAG, "getCommandLedOnOff =================== : ");
            try
            {
                Send(WaggleConst.CMD_GET_LED_ON);
            }
            catch (Exception e)
            {
                Log.d("getCommandLedOnOff =======", e.Message);
            }
        }

        public void PrinterStart()
        {
           
            printStatus = WaggleConst.PRT_STATUS_PRITING;
            cmdPrintStatus = WaggleConst.E_PRT_START;
            try
            {
               Send(WaggleConst.CMD_PRT_START);
            }
            catch (Exception e)
            {
                Log.d("printStart =======", e.Message);
            }
        }

        public void PrinterStop()
        {
           
            cmdPrintStatus = WaggleConst.E_PRT_STOP;
            printStatus = WaggleConst.PRT_STATUS_IDLE;

            Log.d(TAG, "printerStop =================== : ");
            try
            {
               Send(WaggleConst.CMD_PRT_STOP);
            }
            catch (Exception e)
            {
                Log.d("printerStop =======", e.Message);
            }
        }
             
        public void PrinterPause()
        {
           
            cmdPrintStatus = WaggleConst.E_PRT_PAUSE;
            printStatus = WaggleConst.PRT_STATUS_PAUSE;

            Log.d(TAG, "printerPause =================== : ");
            try
            {
                Send(WaggleConst.CMD_PRT_PAUSE);
            }
            catch (Exception e)
            {
                Log.d("printerPause =======", e.Message);
            }
        }

        public void DoSendPrint()
        {

            //mHandler.removeMessages(700);
            //mHandler.sendEmptyMessageDelayed(700, 10);
            /////////////////////////////////////////////////
            ////
            string attachment_file_name = "20180912072713";
            byte[] array = Encoding.UTF8.GetBytes(attachment_file_name);
            int c = array.Length + 3;
            byte[] cmdSend = new byte[c];
            cmdSend[0] = (byte)0xA5;
            cmdSend[1] = WaggleConst.E_SET_FILENAME;
            for (int i = 0; i < array.Length; i++)
            {
                cmdSend[i + 2] = array[i];
            }
            cmdSend[c - 1] = (byte)0xFF;
            try
            {
                Send(cmdSend);
            }
            catch (Exception e)
            {
                Log.d("send file name =======", e.Message);
            }

            ///////////////////////////////////////////////
            //
            string fileSize = string.Format("{0:D}", 1781616);
            array = Encoding.UTF8.GetBytes(fileSize);
            c = array.Length + 3;
            cmdSend = new byte[c];
            cmdSend[0] = (byte)0xA5;
            cmdSend[1] = WaggleConst.E_SET_FILESIZE;
            for (int i = 0; i < array.Length; i++)
            {
                cmdSend[i + 2] = array[i];
            }
            cmdSend[c - 1] = (byte)0xFF;
            try
            {
                Send(cmdSend);
            }
            catch (Exception e)
            {
                Log.d("send file size =======", e.Message);
            }

            ///////////////////////////////////////////////
            //
            string fileUlr = "http://waggle-images.s3.amazonaws.com/drive_files/attachments/000/002/387/original/e5b80ccf917f2f6d8fdc9ae99c9a9edd.gcode";
            array = Encoding.UTF8.GetBytes(fileUlr);
            c = array.Length + 3;
            cmdSend = new byte[c];
            cmdSend[0] = (byte)0xA5;
            cmdSend[1] = WaggleConst.E_SET_FILEDOWNLOAD;
            for (int i = 0; i < array.Length; i++)
            {
                cmdSend[i + 2] = array[i];
            }
            cmdSend[c - 1] = (byte)0xFF;
            try
            {
                Send(cmdSend);
            }
            catch (Exception e)
            {
                Log.d("send file fileUlr =======", e.Message);
            }
            SetNozzleTemp(215);
            PrinterStart();
        }

        public void GetFirmwareVersion()
        {

            Log.d(TAG, "getFirmwareVersion =================== : ");

            try
            {
                Send(WaggleConst.CMD_GET_FWVER);
            }
            catch (Exception e)
            {

            }
        }

        public string GetPrinterStatusString()
        {

            string strPrintStatus = "";
            switch (printStatus)
            {
                case WaggleConst.PRT_STATUS_IDLE:
                    strPrintStatus = "Idle";
                    break;
                case WaggleConst.PRT_STATUS_PRITING:
                    strPrintStatus = "Printing";
                    break;
                case WaggleConst.PRT_STATUS_PAUSE:
                    strPrintStatus = "Paused";
                    break;
                case WaggleConst.PRT_STATUS_NOTCONNECTED:
                    strPrintStatus = "Disconnected";
                    break;

                case WaggleConst.PRT_STATUS_FAILED:
                    strPrintStatus = "Failed";
                    break;
            }

            return strPrintStatus;
        }
        
        public void SetPrinterStatus(byte printStatus)
        {

            this.printStatus = printStatus;

            //if (fragControl != null)
            //    fragControl.setPrinterStatus(printStatus);

            //if (videoPlay != null)
            //    videoPlay.setPrinterStatus(printStatus);
        }

        public void SetNozzleTemp(int temp)
        {
            ///////////////////////////////////////////////
            //
            var strTemp = string.Format("{0:F2}", (float)temp);
            byte[] array = Encoding.UTF8.GetBytes(strTemp);
            int c = array.Length + 3;
            byte[] cmdSend = new byte[c];
            cmdSend[0] = (byte)0xA5;
            cmdSend[1] = WaggleConst.E_SET_NOZZLE;
            for (int i = 0; i < array.Length; i++)
            {
                cmdSend[i + 2] = array[i];
            }
            cmdSend[c - 1] = (byte)0xFF;
            try
            {
                Send(cmdSend);
            }
            catch (Exception e)
            {
            }
        }

        public void SetBedTemp(int temp)
        {
            
            var strTemp = string.Format("{0:F2}", (float)temp);
            byte[] array = Encoding.UTF8.GetBytes(strTemp); 
            int c = array.Length + 3;
            byte[] cmdSend = new byte[c];
            cmdSend[0] = (byte)0xA5;
            cmdSend[1] = WaggleConst.E_SET_BED;
            for (int i = 0; i < array.Length; i++)
            {
                cmdSend[i + 2] = array[i];
            }
            cmdSend[c - 1] = (byte)0xFF;
            try
            {
                Send(cmdSend);
            }
            catch (Exception e)
            {
            }
        }

        private void Send(byte[] data) {
            socket.Send(data);
        }
    }
}
