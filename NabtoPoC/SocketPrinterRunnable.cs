using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NabtoPoC
{
    class SocketPrinterRunnable 
    {
        private bool bThreadRunnig = true;
        //Thread thread = null;
        private Socket socket = null;
        bool isWorking = true;
        byte printStatus;
        string TAG = "WaggleSocket";
        int percent = 0;
        byte ledStatus;
        public SocketPrinterRunnable(bool bThreadRunnig, Socket socket)
        {
            this.bThreadRunnig = bThreadRunnig;
            this.socket = socket;
            //this.thread = thread;
        }
        public void run()
        {
            while (bThreadRunnig)
            {

                try
                {
                    Thread.Sleep(1000);
                    int count = socket.Available;
                    if (count > 0)
                    {
                        byte[] buffer;
                        buffer = new byte[count];
                        socket.Receive(buffer);
                      
                        Log.d(TAG, "socketPrinterRunnable : " + "======================");

                        var o = SocketResData(buffer);

                        for (int i = 0; i < o.Count; i++)
                        {
                            byte[] buf = o[i];
                            int length = buf.Length;
                            byte cmdType = (byte)buf[1];
                            byte[] bufferTemp1 = new byte[length - 3];
                            Array.Copy(buf, 2, bufferTemp1, 0, length - 3);
                            Log.d(TAG, "socketPrinterRunnable : buf[1] : " + Encoding.ASCII.GetString(bufferTemp1, 0, bufferTemp1.Length));
                            switch (cmdType)
                            {
                                case WaggleConst.E_SET_SCRT:
                                    {
                                        isWorking = true;
                                       // Log.d(TAG, "socketPrinterRunnable : E_SET_SCRT ");
                                    }
                                    break;

                                case WaggleConst.E_STATUS:
                                    {
                                        printStatus = buf[2]; //PRT_STATUS_NOTCONNECTED;//

                                        Log.d(TAG, "socketPrinterRunnable : E_STATUS " + printStatus);
                                    }
                                    break;

                                case WaggleConst.E_NOZZLE:
                                    {

                                        byte[] bufferTemp = new byte[length - 3];                                        
                                        Array.Copy(buf, 2, bufferTemp, 0, length - 3);                                        
                                        string strNozzle = Encoding.ASCII.GetString(bufferTemp, 0, bufferTemp.Length);
                                        string []va = strNozzle.Split("/");

                                        Log.d(TAG, "socketPrinterRunnable : E_NOZZLE : " + strNozzle);                                        
                                    }
                                    break;

                                case WaggleConst.E_BED:
                                    {

                                        byte[] bufferTemp = new byte[length - 3];
                                        Array.Copy(buf, 2, bufferTemp, 0, length - 3);                                        
                                        string strNozzle = System.Text.Encoding.ASCII.GetString(bufferTemp, 0, bufferTemp.Length);
                                        string []va = strNozzle.Split("/");

                                        Log.d(TAG, "socketPrinterRunnable : E_BED : " + strNozzle);
                                  
                                    }
                                    break;

                                case WaggleConst.E_LED_STATUS:
                                    {
                                        ledStatus = buf[2]; //PRT_STATUS_NOTCONNECTED;//
                                      
                                        Log.d(TAG, "socketPrinterRunnable : E_LED_STATUS ");
                                    }
                                    break;

                                case WaggleConst.E_PERCENT:
                                    {
                                        percent = buf[2];
                                        Log.d(TAG, "socketPrinterRunnable : E_PERCENT ...  " + percent);
                                      
                                    }
                                    break;

                                case WaggleConst.E_GET_FWVER:
                                    {                                        
                                        byte[] bufferTemp = new byte[length - 3];                                       
                                    }
                                    break;

                                case WaggleConst.E_SET_FILEDOWNLOAD:
                                    {
                                        byte bResult = buf[2];                                      

                                        if (bResult == (byte)(0xD0))
                                            Log.d(TAG, string.Format("socketPrinterRunnable : E_SET_FILEDOWNLOAD {0} : e_dl_finish", bResult));
                                        else if (bResult == (byte)(0xD1))
                                            Log.d(TAG, string.Format("socketPrinterRunnable : E_SET_FILEDOWNLOAD  {0} : e_dl_start", bResult));

                                    }
                                    break;

                                case WaggleConst.E_PRT_START:
                                    {
                                        Log.d(TAG, "socketPrinterRunnable : E_PRT_START ...  ");
                               
                                    }
                                    break;

                                case WaggleConst.E_PRT_PAUSE:
                                    {
                                        Log.d(TAG, "socketPrinterRunnable : E_PRT_PAUSE ...  ");                                       
                                    }
                                    break;

                                case WaggleConst.E_PRT_STOP:
                                    {
                                        Log.d(TAG, "socketPrinterRunnable : E_PRT_STOP ...  ");                                       
                                    }
                                    break;

                                default:
                                    byte[] bbb = new byte[length - 3];
                                    Array.Copy(buf, 2, bbb, 0, length - 3);
                                    string ccc = Encoding.ASCII.GetString(bbb, 0, bbb.Length);
                                    Log.d(TAG, "socketPrinterRunnable : Except ...  "+ ccc);
                                    break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {

                }               
            }

        }

        private List<byte[]> SocketResData(byte[] buffer)
        {
            int start = 0, end = 0;
            var myList = new List<byte[]>();
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0xaf)
                {
                    start = i;
                }

                if (buffer[i] == 0xff)
                {
                    end = i;
                    byte[] o = new byte[end - start + 1];
                    int c = 0;
                    for (int j = start; j <= end; j++)
                        o[c++] = buffer[j];

                    myList.Add(o);
                }
            }
            return myList;
        }
    }

    static class Log
    {
        public static void d(string tag, string message)
        {
            Console.WriteLine("{0}: {1}",tag, message);
        }
    }
}
