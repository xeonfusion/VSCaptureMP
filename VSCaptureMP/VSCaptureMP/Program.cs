/*
 * This file is part of VitalSignsCaptureMP v1.003.
 * Copyright (C) 2017-18 John George K., xeonfusion@users.sourceforge.net

    VitalSignsCaptureMP is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    VitalSignsCaptureMP is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with VitalSignsCaptureMP.  If not, see <http://www.gnu.org/licenses/>.*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.IO.Ports;

namespace VSCaptureMP
{
    class Program
    {
        static EventHandler dataEvent;

        static void Main(string[] args)
        {
            Console.WriteLine("VitalSignsCaptureMP MIB (C)2017-18 John George K.");

            Console.WriteLine();
            Console.WriteLine("1. Connect via MIB RS232 port");
            Console.WriteLine("2. Connect via LAN port");
            Console.WriteLine();
            Console.Write("Choose connection mode (1-2):");

            string sConnectset = Console.ReadLine();
            int nConnectset = 1;
            if (sConnectset != "") nConnectset = Convert.ToInt32(sConnectset);

            if (nConnectset == 1) ConnectviaMIB();
            else if (nConnectset == 2) ConnectviaLAN();
            
            //ConnectviaLAN();
            //ConnectviaMIB();

            ConsoleKeyInfo cki;

            do
            {
                cki = Console.ReadKey(true);
               
            }
            while (cki.Key != ConsoleKey.Escape);

        }

        public class UdpState
        {
            //Udp client
            public MPudpclient udpClient;
            //RemoteIP
            public IPEndPoint remoteIP;            
        }

        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        public static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                
                // Retrieve the state object and the client   
                // from the asynchronous state object.  
                UdpState state = (UdpState)ar.AsyncState;
                
                MPudpclient Client = state.udpClient;

                string path = Path.Combine(Directory.GetCurrentDirectory(), "MPrawoutput.txt");

                // Read data from the remote device.  
                byte[] readbuffer = Client.EndReceive(ar,ref state.remoteIP);
                int bytesRead = readbuffer.GetLength(0);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  

                    Client.ByteArrayToFile(path, readbuffer, bytesRead);

                    Client.ReadData(readbuffer);
                    
                    //  Get the rest of the data.  
                    Client.BeginReceive(new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                   
                
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
       
        public static void ReadData(object sender, byte[] readbuffer)
        {
            (sender as MPudpclient).ReadData(readbuffer);
        }

        public static void WaitForSeconds(int nsec)
        {
            DateTime dt = DateTime.Now;
            DateTime dt2 = dt.AddSeconds(nsec);
            do
            {
                dt = DateTime.Now;
            }
            while (dt2 > dt);

        }

        public static void ConnectviaLAN()
        {
            Console.WriteLine("You may connect an Ethernet cable to the Philips Intellivue monitor LAN port");
            Console.WriteLine("Note the IP address from the Network Status menu in the monitor");

            Console.WriteLine();
            Console.WriteLine("Numeric Data Transmission sets:");
            Console.WriteLine("1. 1 second (Real time)");
            Console.WriteLine("2. 12 second (Averaged)");
            Console.WriteLine("3. 1 minute (Averaged)");
            Console.WriteLine("4. 5 minute (Averaged)");
            Console.WriteLine("5. Single poll");
            Console.WriteLine();
            Console.Write("Choose Data Transmission interval (1-5):");

            string sIntervalset = Console.ReadLine();
            int[] setarray = { 1, 12, 60, 300, 0 };
            short nIntervalset = 2;
            int nInterval = 12;
            if (sIntervalset != "") nIntervalset = Convert.ToInt16(sIntervalset);
            if (nIntervalset > 0 && nIntervalset < 6) nInterval = setarray[nIntervalset - 1];

            Console.WriteLine();
            Console.WriteLine("CSV Data Export Options:");
            Console.WriteLine("1. Single value list");
            Console.WriteLine("2. Data packet list");
            Console.WriteLine("3. Consolidated data list");
            Console.WriteLine();
            Console.Write("Choose CSV export option (1-3):");

            string sCSVset = Console.ReadLine();
            int nCSVset = 3;
            if (sCSVset != "") nCSVset = Convert.ToInt32(sCSVset);

            Console.WriteLine();
            Console.WriteLine("Waveform data export options:");
            Console.WriteLine("0. None");
            //Console.WriteLine("1. All");
            Console.WriteLine("1. ECG I, II, III");
            Console.WriteLine("2. ECG II, ECG V5, RESP, PLETH, ART IBP, CVP, CO2, AWP, AWF");
            Console.WriteLine("3. ECG AVR, ECG AVL, ECG AVF");
            Console.WriteLine("4. ECG V1, ECG V2, ECG V3");
            Console.WriteLine("5. ECG V4, ECG V5, ECG V6");
            Console.WriteLine("6. EEG1, EEG2, EEG3, EEG4");
            Console.WriteLine("7. Compound ECG");
            Console.WriteLine("8. Compound ECG, PLETH, ART, CVP, CO2");
            Console.WriteLine("9. All");

            Console.WriteLine();
            Console.WriteLine("Selecting all waves can lead to data loss due to bandwidth issues");
            Console.Write("Choose Waveform data export priority option (0-9):");

            string sWaveformSet = Console.ReadLine();
            short nWaveformSet = 0;
            if (sWaveformSet != "") nWaveformSet = Convert.ToInt16(sWaveformSet);

            // Create a new UdpClient object with default settings.
            MPudpclient _MPudpclient = MPudpclient.getInstance;

            Console.WriteLine("Enter the target IP address of the monitor assigned by DHCP:");

            string IPAddressRemote = Console.ReadLine();

            Console.WriteLine("Connecting to {0}...", IPAddressRemote);
            Console.WriteLine();
            Console.WriteLine("Requesting Transmission set {0} from monitor", nIntervalset);
            Console.WriteLine();
            Console.WriteLine("Data will be written to CSV file MPDataExport.csv in same folder");
            Console.WriteLine();
            Console.WriteLine("Press Escape button to Stop");

            if (nCSVset > 0 && nCSVset < 4) _MPudpclient.m_csvexportset = nCSVset;

            if (IPAddressRemote != "")
            {
                //Default Philips monitor port is 24105
                _MPudpclient.m_remoteIPtarget = new IPEndPoint(IPAddress.Parse(IPAddressRemote), 24105);


                try
                {
                    _MPudpclient.Connect(_MPudpclient.m_remoteIPtarget);

                    UdpState state = new UdpState();
                    state.udpClient = _MPudpclient;
                    state.remoteIP = _MPudpclient.m_remoteIPtarget;

                    //Send AssociationRequest message
                    //_MPudpclient.SendAssociationRequest();
                    _MPudpclient.SendWaveAssociationRequest();

                    string path = Path.Combine(Directory.GetCurrentDirectory(), "MPrawoutput.txt");

                    //Receive AssociationResult message and MDSCreateEventReport message
                    byte[] readassocbuffer = _MPudpclient.Receive(ref _MPudpclient.m_remoteIPtarget);
                    _MPudpclient.ByteArrayToFile(path, readassocbuffer, readassocbuffer.GetLength(0));


                    byte[] readmdsconnectbuffer = _MPudpclient.Receive(ref _MPudpclient.m_remoteIPtarget);
                    _MPudpclient.ByteArrayToFile(path, readmdsconnectbuffer, readmdsconnectbuffer.GetLength(0));

                    //Send MDSCreateEventResult message
                    _MPudpclient.ProcessPacket(readmdsconnectbuffer);

                    //Send PollDataRequest message
                    //_MPudpclient.SendPollDataRequest();
                    // _MPudpclient.SendExtendedPollDataRequest();

                    //Send Extended PollData Requests cycled every second
                    Task.Run(() => _MPudpclient.SendCycledExtendedPollDataRequest(nInterval));

                    //WaitForSeconds(1);

                    if (nWaveformSet != 0)
                    {
                        _MPudpclient.GetRTSAPriorityListRequest();
                        //_MPudpclient.SetRTSAPriorityListRequest();
                        if (nWaveformSet != 9)
                        {
                            _MPudpclient.SetRTSAPriorityList(nWaveformSet);
                        }

                        Task.Run(() => _MPudpclient.SendCycledExtendedPollWaveDataRequest(nInterval));
                    }

                    //Keep Connection Alive
                    Task.Run(() => _MPudpclient.KeepConnectionAlive(nInterval));

                    //Receive PollDataResponse message
                    _MPudpclient.BeginReceive(new AsyncCallback(ReceiveCallback), state);

                    //Parse PollDataResponses

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error opening/writing to UDP port :: " + ex.Message, "Error!");
                }

            }
            else
            {
                Console.WriteLine("Invalid IP Address");
            }



        }
        public static void ConnectviaMIB()
        {
            // Create a new SerialPort object with default settings.
            MPSerialPort _serialPort = MPSerialPort.getInstance;

            Console.WriteLine("Select the Port to which Intellivue Monitor is to be connected, Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine(" {0}", s);
            }


            Console.Write("COM port({0}): ", _serialPort.PortName.ToString());
            string portName = Console.ReadLine();

            if (portName != "")
            {
                // Allow the user to set the appropriate properties.
                _serialPort.PortName = portName;
            }


            try
            {
                _serialPort.Open();

                //EventHandler dataEvent = new EventHandler((object sender, EventArgs e)=>ReadData(sender));


                if (_serialPort.OSIsUnix())
                {
                    dataEvent += new EventHandler((object sender, EventArgs e) => ReadData(sender));
                }

                if (!_serialPort.OSIsUnix())
                {
                    _serialPort.DataReceived += new SerialDataReceivedEventHandler(p_DataReceived);
                }

                Console.WriteLine("You may now connect the serial cable to the Intellivue Monitor MIB port");
                Console.WriteLine("Press any key to continue..");

                Console.ReadKey(true);

                    Console.WriteLine();
                    Console.WriteLine("Numeric Data Transmission sets:");
                    Console.WriteLine("1. 1 second (Real time)");
                    Console.WriteLine("2. 12 second (Averaged)");
                    Console.WriteLine("3. 1 minute (Averaged)");
                    Console.WriteLine("4. 5 minute (Averaged)");
                    Console.WriteLine("5. Single poll");
                    Console.WriteLine();
                    Console.Write("Choose Data Transmission interval (1-5):");

                    string sIntervalset = Console.ReadLine();
                    int[] setarray = { 1, 12, 60, 300, 0 };
                    short nIntervalset = 2;
                    int nInterval = 12;
                    if (sIntervalset != "") nIntervalset = Convert.ToInt16(sIntervalset);
                    if (nIntervalset > 0 && nIntervalset < 6) nInterval = setarray[nIntervalset - 1];

                    Console.WriteLine();
                    Console.WriteLine("CSV Data Export Options:");
                    Console.WriteLine("1. Single value list");
                    Console.WriteLine("2. Data packet list");
                    Console.WriteLine("3. Consolidated data list");
                    Console.WriteLine();
                    Console.Write("Choose CSV export option (1-3):");

                    string sCSVset = Console.ReadLine();
                    int nCSVset = 3;
                    if (sCSVset != "") nCSVset = Convert.ToInt32(sCSVset);

                    Console.WriteLine();
                    Console.WriteLine("Waveform data export priority options:");
                    Console.WriteLine("0. None");
                    //Selecting all waves can lead to data loss due to bandwidth issues
                    //Console.WriteLine("1. All");
                    Console.WriteLine("1. ECG I, II, III");
                    Console.WriteLine("2. ECG II, ECG V5, RESP, PLETH, ART IBP, CVP, CO2, AWP, AWF");
                    Console.WriteLine("3. ECG AVR, ECG AVL, ECG AVF");
                    Console.WriteLine("4. ECG V1, ECG V2, ECG V3");
                    Console.WriteLine("5. ECG V4, ECG V5, ECG V6");
                    Console.WriteLine("6. EEG1, EEG2, EEG3, EEG4");
                    Console.WriteLine("7. Compound ECG");
                    Console.WriteLine("8. Compound ECG, PLETH, ART, CVP, CO2");
                    Console.WriteLine("9. All");

                    Console.WriteLine();
                    Console.WriteLine("Selecting all waves can lead to data loss due to bandwidth issues");
                    Console.Write("Choose Waveform data export priority option (0-9):");
                

                string sWaveformSet = Console.ReadLine();
                    short nWaveformSet = 0;
                    if (sWaveformSet != "") nWaveformSet = Convert.ToInt16(sWaveformSet);

                    Console.WriteLine("Requesting Transmission set {0} from monitor", nIntervalset);
                    Console.WriteLine();
                    Console.WriteLine("Data will be written to CSV file MPDataExport.csv in same folder");
                    Console.WriteLine();

                
                Console.WriteLine("Press Escape button to Stop");

                if (nCSVset > 0 && nCSVset < 4) _serialPort.m_csvexportset = nCSVset;
                
                _serialPort.SendWaveAssociationRequest();
                
                //Send Extended PollData Requests cycled every second
                Task.Run(() => _serialPort.SendCycledExtendedPollDataRequest(nInterval));

                WaitForMilliSeconds(500);

                if (nWaveformSet != 0)
                {
                    _serialPort.GetRTSAPriorityListRequest();
                    //_serialPort.SetRTSAPriorityListRequest();
                    if(nWaveformSet != 9)
                    {
                        _serialPort.SetRTSAPriorityList(nWaveformSet);
                    }

                    Task.Run(() => _serialPort.SendCycledExtendedPollWaveDataRequest(nInterval));
                }

                //Keep Connection Alive
                Task.Run(() => _serialPort.KeepConnectionAlive(nInterval));

                //Receive PollDataResponse message
                
                if (_serialPort.OSIsUnix())
                {
                    do
                    {
                        if (_serialPort.BytesToRead != 0)
                        {
                            //dataEvent.Invoke(new object(), new EventArgs());
                            dataEvent.Invoke(_serialPort, new EventArgs());
                        }

                        if (Console.KeyAvailable == true)
                        {
                            if (Console.ReadKey(true).Key == ConsoleKey.Escape) break;
                        }
                    }
                    while (Console.KeyAvailable == false);

                }

                if (!_serialPort.OSIsUnix())
                {
                    ConsoleKeyInfo cki;

                    do
                    {
                        cki = Console.ReadKey(true);
                    }
                    while (cki.Key != ConsoleKey.Escape);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening/writing to serial port :: " + ex.Message, "Error!");
            }
            finally
            {
                _serialPort.StopTransfer();

                _serialPort.Close();

            }


        }

        static void p_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            ReadData(sender);

        }

        public static void ReadData(object sender)
        {
            try
            {
                (sender as MPSerialPort).ReadBuffer();

            }
            catch (TimeoutException) { }
        }

        public static void WaitForMilliSeconds(int nmillisec)
        {
            DateTime dt = DateTime.Now;
            DateTime dt2 = dt.AddMilliseconds(nmillisec);
            do
            {
                dt = DateTime.Now;
            }
            while (dt2 > dt);

        }

    }

}
