/*
 * This file is part of VitalSignsCaptureMP v1.001.
 * Copyright (C) 2017 John George K., xeonfusion@users.sourceforge.net

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

namespace VSCaptureMP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("VitalSignsCaptureMP (C)2017 John George K.");

            Console.WriteLine();

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
            if(nIntervalset >0 && nIntervalset<6) nInterval = setarray[nIntervalset-1];

            Console.WriteLine();
            Console.WriteLine("CSV Data Export Options:");
            Console.WriteLine("1. Single value list");
            Console.WriteLine("2. Data packet list");
            Console.WriteLine("3. Consolidated data list");
            Console.WriteLine();
            Console.Write("Choose CSV export option (1-3):");

            string sCSVset = Console.ReadLine();
            int nCSVset = 1;
            if (sCSVset != "") nCSVset = Convert.ToInt32(sCSVset);
            
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
                    _MPudpclient.SendAssociationRequest();

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
                    Task.Run (()=>_MPudpclient.SendCycledExtendedPollDataRequest(nInterval));
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

        

    }

        
    
}
