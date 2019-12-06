/*
 * This file is part of VitalSignsCaptureMP v1.007.
 * Copyright (C) 2017-19 John George K., xeonfusion@users.sourceforge.net

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
using System.IO.Ports;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Runtime.Serialization.Json;
using System.Net;


namespace VSCaptureMP
{
    public class CRCmethods
    {
        //FCS calculation given in Network Working Group Request for Comment: 1171 (PPP protocol).
        //The one’s complement of the CRC is transmitted, rather than the CRC itself

          public static ushort[] FCSTable = {
          0x0000, 0x1189, 0x2312, 0x329b, 0x4624, 0x57ad, 0x6536, 0x74bf,
          0x8c48, 0x9dc1, 0xaf5a, 0xbed3, 0xca6c, 0xdbe5, 0xe97e, 0xf8f7,
          0x1081, 0x0108, 0x3393, 0x221a, 0x56a5, 0x472c, 0x75b7, 0x643e,
          0x9cc9, 0x8d40, 0xbfdb, 0xae52, 0xdaed, 0xcb64, 0xf9ff, 0xe876,
          0x2102, 0x308b, 0x0210, 0x1399, 0x6726, 0x76af, 0x4434, 0x55bd,
          0xad4a, 0xbcc3, 0x8e58, 0x9fd1, 0xeb6e, 0xfae7, 0xc87c, 0xd9f5,
          0x3183, 0x200a, 0x1291, 0x0318, 0x77a7, 0x662e, 0x54b5, 0x453c,
          0xbdcb, 0xac42, 0x9ed9, 0x8f50, 0xfbef, 0xea66, 0xd8fd, 0xc974,
          0x4204, 0x538d, 0x6116, 0x709f, 0x0420, 0x15a9, 0x2732, 0x36bb,
          0xce4c, 0xdfc5, 0xed5e, 0xfcd7, 0x8868, 0x99e1, 0xab7a, 0xbaf3,
          0x5285, 0x430c, 0x7197, 0x601e, 0x14a1, 0x0528, 0x37b3, 0x263a,
          0xdecd, 0xcf44, 0xfddf, 0xec56, 0x98e9, 0x8960, 0xbbfb, 0xaa72,
          0x6306, 0x728f, 0x4014, 0x519d, 0x2522, 0x34ab, 0x0630, 0x17b9,
          0xef4e, 0xfec7, 0xcc5c, 0xddd5, 0xa96a, 0xb8e3, 0x8a78, 0x9bf1,
          0x7387, 0x620e, 0x5095, 0x411c, 0x35a3, 0x242a, 0x16b1, 0x0738,
          0xffcf, 0xee46, 0xdcdd, 0xcd54, 0xb9eb, 0xa862, 0x9af9, 0x8b70,
          0x8408, 0x9581, 0xa71a, 0xb693, 0xc22c, 0xd3a5, 0xe13e, 0xf0b7,
          0x0840, 0x19c9, 0x2b52, 0x3adb, 0x4e64, 0x5fed, 0x6d76, 0x7cff,
          0x9489, 0x8500, 0xb79b, 0xa612, 0xd2ad, 0xc324, 0xf1bf, 0xe036,
          0x18c1, 0x0948, 0x3bd3, 0x2a5a, 0x5ee5, 0x4f6c, 0x7df7, 0x6c7e,
          0xa50a, 0xb483, 0x8618, 0x9791, 0xe32e, 0xf2a7, 0xc03c, 0xd1b5,
          0x2942, 0x38cb, 0x0a50, 0x1bd9, 0x6f66, 0x7eef, 0x4c74, 0x5dfd,
          0xb58b, 0xa402, 0x9699, 0x8710, 0xf3af, 0xe226, 0xd0bd, 0xc134,
          0x39c3, 0x284a, 0x1ad1, 0x0b58, 0x7fe7, 0x6e6e, 0x5cf5, 0x4d7c,
          0xc60c, 0xd785, 0xe51e, 0xf497, 0x8028, 0x91a1, 0xa33a, 0xb2b3,
          0x4a44, 0x5bcd, 0x6956, 0x78df, 0x0c60, 0x1de9, 0x2f72, 0x3efb,
          0xd68d, 0xc704, 0xf59f, 0xe416, 0x90a9, 0x8120, 0xb3bb, 0xa232,
          0x5ac5, 0x4b4c, 0x79d7, 0x685e, 0x1ce1, 0x0d68, 0x3ff3, 0x2e7a,
          0xe70e, 0xf687, 0xc41c, 0xd595, 0xa12a, 0xb0a3, 0x8238, 0x93b1,
          0x6b46, 0x7acf, 0x4854, 0x59dd, 0x2d62, 0x3ceb, 0x0e70, 0x1ff9,
          0xf78f, 0xe606, 0xd49d, 0xc514, 0xb1ab, 0xa022, 0x92b9, 0x8330,
          0x7bc7, 0x6a4e, 0x58d5, 0x495c, 0x3de3, 0x2c6a, 0x1ef1, 0x0f78
        };

        
            
        public int GetFCS(byte[] Buffer)
        {
            int FCS = 0;
            const int InitialFCS = 0xFFFF;
            FCS = InitialFCS;

            for (int i = 0; i <= Buffer.Length - 1; i++)
            {
                FCS = (FCS >> 8) ^ FCSTable[(FCS ^ Buffer[i]) & 0xFF];
            }
            return FCS;
        }

        public byte[] OnesComplement(int NumberToComplement)
        {
            byte[] ComplementVal = BitConverter.GetBytes(NumberToComplement);

            for (int i = 0; i <= 1; i++)
            {
                ComplementVal[i] = (byte)(ComplementVal[i] ^ 0xff);
            }

            Array.Resize(ref ComplementVal, 2);
            return ComplementVal;
        }

    }

    

    public sealed class MPSerialPort:SerialPort
    {
        private int MPPortBufSize;
        public byte[] MPPort_rxbuf;
        private bool m_fstart = true;
        private bool m_storestart = false;
        private bool m_storeend = false;
        private bool m_bitshiftnext = false;
        public List<byte[]> FrameList = new List<byte[]>();
        private List<byte> m_bList = new List<byte>();


        public List<NumericValResult> m_NumericValList = new List<NumericValResult>();
        public List<string> m_NumValHeaders = new List<string>();
        public StringBuilder m_strbuildvalues = new StringBuilder();
        public StringBuilder m_strbuildheaders = new StringBuilder();
        public List<WaveValResult> m_WaveValResultList = new List<WaveValResult>();
        public StringBuilder m_strbuildwavevalues = new StringBuilder();
        public bool m_transmissionstart = true;
        public string m_strTimestamp;
        public ushort m_actiontype;
        public int m_elementcount = 0;
        public int m_headerelementcount = 0;
        public int m_csvexportset = 1;
        public List<SaSpec> m_SaSpecList = new List<SaSpec>();
        public List<SaCalibData16> m_SaCalibDataSpecList = new List<SaCalibData16>();
        public bool m_calibratewavevalues = false;

        public ushort m_obpollhandle = 0;
        public uint m_idlabelhandle = 0;
        public DateTime m_baseDateTime = new DateTime();
        public uint m_baseRelativeTime = 0;
        public string m_DeviceID;
        public string m_jsonposturl;
        public int m_dataexportset = 1;


        public class NumericValResult
        {
            public string Timestamp;
            public string Relativetimestamp;
            public string PhysioID;
            public string Value;
            public string DeviceID;
        }

        public class WaveValResult
        {
            public string Timestamp;
            public string Relativetimestamp;
            public string PhysioID;
            public byte[] Value;
            public string DeviceID;
            public ushort obpoll_handle;
            public SaSpec saSpecData = new SaSpec();
            public SaCalibData16 saCalibData = new SaCalibData16();
        }

        //Create a singleton serialport subclass
        private static volatile MPSerialPort MPPort = null;

		public static MPSerialPort getInstance
		{

			get
			{
				if (MPPort == null)
				{
					lock (typeof(MPSerialPort))
						if (MPPort == null)
						{
							MPPort = new MPSerialPort();
						}

				} return MPPort;
			}

		}

        public MPSerialPort()
        {
            MPPort = this;

            MPPortBufSize = 4096;
            MPPort_rxbuf = new byte[MPPortBufSize];

            if (OSIsUnix())
                MPPort.PortName = "/dev/ttyUSB0"; //default Unix port
            else MPPort.PortName = "COM1"; //default Windows port

            MPPort.BaudRate = 115200;
            MPPort.Parity = Parity.None;
            MPPort.DataBits = 8;
            MPPort.StopBits = StopBits.One;

            MPPort.Handshake = Handshake.None;
            MPPort.RtsEnable = true;
            MPPort.DtrEnable = true;

            // Set the read/write timeouts
            MPPort.ReadTimeout = 600000;
            MPPort.WriteTimeout = 600000;

            //ASCII Encoding in C# is only 7bit so
            //MPPort.Encoding = Encoding.GetEncoding("ISO-8859-1");
            MPPort.Encoding = Encoding.Unicode;

        }

        public void ClearReadBuffer()
        {
            //Clear the buffer
            for (int i = 0; i < MPPortBufSize; i++)
            {
                MPPort_rxbuf[i] = 0;
            }
        }

        public int ReadBuffer()
        {
            int bytesreadtotal = 0;

            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "MPRawoutput1.raw");

                int lenread = 0;

                do
                {
                    ClearReadBuffer();
                    lenread = MPPort.Read(MPPort_rxbuf, 0, MPPortBufSize);
                    
                    byte[] copyarray = new byte[lenread];

                    for (int i = 0; i < lenread; i++)
                    {
                        copyarray[i] = MPPort_rxbuf[i];
                        CreateFrameListFromByte(copyarray[i]);
                    }

                    ByteArrayToFile(path, copyarray, copyarray.GetLength(0));
                    bytesreadtotal += lenread;
                    /*if (FrameList.Count > 0)
                    {
                        ReadPacketFromFrame();

                        FrameList.RemoveRange(0, FrameList.Count);
                        
                    }*/
                   
                }
                while (MPPort.BytesToRead != 0);

                if(MPPort.BytesToRead == 0)
                {
                    if (FrameList.Count > 0)
                    {
                        ReadPacketFromFrame();
                        
                        FrameList.RemoveRange(0, FrameList.Count);

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening/writing to serial port :: " + ex.Message, "Error!");
            }


            return bytesreadtotal;
        }

        public void ReadPacketFromFrame()
        {
            if (FrameList.Count > 0)
            {
                foreach (byte[] fArray in FrameList)
                {
                    //ProcessSerialFrame(fArray);
                    ProcessPacket(fArray);
                }

            }
        }

        
        public void CreateFrameListFromByte(byte b)
        {

            switch(b)
            {
                case DataConstants.BOFCHAR:
                    m_storestart = true;
                    m_storeend = false;
                    break;
                case DataConstants.EOFCHAR:
                    m_storestart = false;
                    m_storeend = true;
                    break;
                case DataConstants.ESCAPECHAR:
                    m_bitshiftnext = true;
                    break;
                default:
                    if(m_bitshiftnext == true)
                    {
                        b ^= (DataConstants.BIT5COMPL);
                        m_bitshiftnext = false;
                        m_bList.Add(b);
                    }
                    else if(m_storestart == true && m_storeend == false) m_bList.Add(b);
                    break;
            }


            if (m_storestart == false && m_storeend == true)
            {
                int framelen = m_bList.Count();
                if (framelen != 0)
                {
                    byte[] bArray = new byte[framelen];
                    bArray = m_bList.ToArray();

                    //serial header is 4 bytes and checksum 2 bytes
                    int serialheaderwithuserdatalen = (framelen - 2);
                    int serialuserdataframelen = (framelen - 6);
                    byte[] dataArray = new byte[serialheaderwithuserdatalen];
                    byte[] userdataArray = new byte[serialuserdataframelen];

                    //Get header and user data without checksum
                    Array.Copy(bArray, 0, dataArray, 0, serialheaderwithuserdatalen);
                    //Remove serial header and checksum to get actual packet
                    Array.Copy(bArray, 4, userdataArray, 0, serialuserdataframelen);
                    
                    //Read checksum
                    byte[] checksumbytes = new byte[2];
                    Array.Copy(bArray, framelen - 2, checksumbytes, 0, 2);

                    ushort checksum = BitConverter.ToUInt16(checksumbytes,0);

                    //Calculate checksum
                    CRCmethods crccheck = new CRCmethods();
                    int checksumcalc = crccheck.GetFCS(dataArray);
                    byte[] checksumbytevalue = crccheck.OnesComplement(checksumcalc);

                    ushort checksumcomputed = BitConverter.ToUInt16(checksumbytevalue,0);

                    if (checksumcomputed == checksum)
                    {
                        FrameList.Add(userdataArray);
                    }
                    else Console.WriteLine("Checksum Error");

                    //m_bList.Clear();
                    m_bList.RemoveRange(0, m_bList.Count);
                    m_storeend = false;
                    
                }
               
            }

        }

        public void WriteBuffer(byte[] txbuf)
        {
            byte[] bofframebyte = { DataConstants.ESCAPECHAR, (DataConstants.BOFCHAR ^ DataConstants.BIT5COMPL), 0 };
            byte[] eofframebyte = { DataConstants.ESCAPECHAR, (DataConstants.EOFCHAR ^ DataConstants.BIT5COMPL), 0 };
            byte[] ctrlbyte = { DataConstants.ESCAPECHAR, (DataConstants.ESCAPECHAR ^ DataConstants.BIT5COMPL), 0 };

            List<byte> temptxbufflist = new List<byte>();
            
            foreach (byte b in txbuf)
            {
                switch (b)
                {
                    case DataConstants.BOFCHAR:
                        temptxbufflist.Add(bofframebyte[0]);
                        temptxbufflist.Add(bofframebyte[1]);
                        break;
                    case DataConstants.EOFCHAR:
                        temptxbufflist.Add(eofframebyte[0]);
                        temptxbufflist.Add(eofframebyte[1]);
                        break;
                    case DataConstants.ESCAPECHAR:
                        temptxbufflist.Add(ctrlbyte[0]);
                        temptxbufflist.Add(ctrlbyte[1]);
                        break;
                    default:
                        temptxbufflist.Add(b);
                        break;
                };
            }

            //Add 4 byte serial header
            byte[] serialframeheader = { 0x11, 0x01, 0x00, 0x00 };
            ushort serialuserdatalength = (ushort)txbuf.Length;
            byte[] seriallength = BitConverter.GetBytes(serialuserdatalength);
            if (BitConverter.IsLittleEndian) Array.Reverse(seriallength);

            //Add serial header to input buffer transformed with transparency bytes
            Array.Copy(seriallength, 0, serialframeheader, 2, 2);
            List<byte> seriallist = serialframeheader.ToList();
            seriallist.AddRange(temptxbufflist);

            byte[] inputbuff = new byte[(txbuf.Length + 4)];
            Array.Copy(serialframeheader, 0, inputbuff, 0, 4);
            Array.Copy(txbuf, 0, inputbuff, 4, txbuf.Length);

            CRCmethods crccheck = new CRCmethods();
            
            //Calculate checksum from serial header with input buffer, not transformed with transparency bytes
            int checksumcalc = crccheck.GetFCS(inputbuff);
            byte[] checksumbytes = crccheck.OnesComplement(checksumcalc);

            
            for (int i = 0; i < checksumbytes.Length; i++)
            {
                seriallist.Add(checksumbytes[i]);
            }

            seriallist.Add(DataConstants.EOFCHAR);
            seriallist.Insert(0, DataConstants.BOFCHAR);
            
            byte[] finaltxbuff = seriallist.ToArray();

            try
            {
                MPPort.Write(finaltxbuff, 0, finaltxbuff.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening/writing to serial port :: " + ex.Message, "Error!");
            }

        }

        public void ProcessSerialFrame(byte[] framebuffer)
        {
            MemoryStream memstream = new MemoryStream(framebuffer);
            BinaryReader binreader = new BinaryReader(memstream);

            byte[] serialframeheader = binreader.ReadBytes(4);
            byte[] serialuserdata = binreader.ReadBytes(framebuffer.Length-serialframeheader.Length);
            ProcessPacket(serialuserdata);

        }

        public void SendWaveAssociationRequest()
        {
            WriteBuffer(DataConstants.aarq_msg_wave_ext_poll2);
        }

        public void SendWaveAssociationRequest2()
        {
            byte[] associate = {
                0x0D, 0xEC, 0x05, 0x08, 0x13, 0x01, 0x00, 0x16, 0x01, 0x02, 0x80, 0x00, 0x14, 0x02, 0x00, 0x02, 0xC1, 0xDC, 0x31, 0x80, 0xA0, 0x80, 0x80, 0x01, 0x01, 0x00, 0x00, 0xA2, 0x80, 0xA0, 0x03, 0x00, 0x00, 0x01, 0xA4, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0x06, 0x04, 0x52, 0x01, 0x00, 0x01, 0x30, 0x80, 0x06, 0x02, 0x51, 0x01, 0x00, 0x00, 0x00, 0x00, 0x30, 0x80, 0x02, 0x01, 0x02, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01, 0x01, 0x30, 0x80, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0xA0, 0x80, 0x60, 0x80, 0xA1, 0x80, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x00, 0xBE, 0x80, 0x28, 0x80, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01, 0x01, 0x02, 0x01, 0x02, 0x81, 0x48, 0x80, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x2C, 0x00, 0x01, 0x00, 0x28, 0x80, 0x00, 0x00, 0x00, 0x00, 0x0F, 0xDE, 0x80, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00, 0x30, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x60, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x0C, 0xF0, 0x01, 0x00, 0x08, 0x88, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };
            WriteBuffer(associate);
        }
        
        public void SendMDSCreateEventResult()
        {
            WriteBuffer(DataConstants.mds_create_resp_msg);
        }

        public void SendExtendedPollDataRequest()
        {
            WriteBuffer(DataConstants.ext_poll_request_msg3);
        }

        public void SendExtendedPollWaveDataRequest()
        {
            WriteBuffer(DataConstants.ext_poll_request_wave_msg);
        }

        public void GetRTSAPriorityListRequest()
        {
            WriteBuffer(DataConstants.get_rtsa_prio_msg);
        }

        public void SetRTSAPriorityList(int nWaveSetType)
        {
            List<byte> WaveTrType = new List<byte>();
            CreateWaveformSet(nWaveSetType, WaveTrType);
            SendRTSAPriorityMessage(WaveTrType.ToArray());
        }

        public static void CreateWaveformSet(int nWaveSetType, List<byte> WaveTrtype)
        {
            //Upto 3 ECG and/or 8 non-ECG waveforms can be polled by selecting the appropriate labels
            //in the Wave object priority list

            switch (nWaveSetType)
            {
                case 0:
                    break;
                case 1:
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x03))); //count
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x0C))); //length
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_II")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_I")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_III")))));
                    break;
                case 2:
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x06))); //count
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x18))); //length
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_II")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_PRESS_BLD_ART_ABP")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_PRESS_BLD_ART")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_PULS_OXIM_PLETH")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_PRESS_BLD_VEN_CENT")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_RESP")))));
                    break;
                case 3:
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x03))); //count
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x0C))); //length
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_AVR")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_AVL")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_AVF")))));
                    break;
                case 4:
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x03))); //count
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x0C))); //length
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_V1")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_V2")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_V3")))));
                    break;
                case 5:
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x03))); //count
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x0C))); //length
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_V4")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_V5")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL_V6")))));
                    break;
                case 6:
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x04))); //count
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x10))); //length
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_EEG_NAMES_EEG_CHAN1_LBL")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_EEG_NAMES_EEG_CHAN2_LBL")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_EEG_NAMES_EEG_CHAN3_LBL")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_EEG_NAMES_EEG_CHAN4_LBL")))));
                    break;
                case 7:
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x02))); //count
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x08))); //length
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_PRESS_BLD_ART_ABP")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_PRESS_BLD_ART")))));
                    break;
                case 8:
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x06))); //count
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianshortus(0x18))); //length
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_ECG_ELEC_POTL")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_PULS_OXIM_PLETH")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_PRESS_BLD_ART_ABP")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_PRESS_BLD_ART")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_PRESS_BLD_VEN_CENT")))));
                    WaveTrtype.AddRange(BitConverter.GetBytes(correctendianuint((uint)(Enum.Parse(typeof(DataConstants.WavesIDLabels), "NLS_NOM_AWAY_CO2")))));
                    break;
            }
        }

        public void SendRTSAPriorityMessage(byte[] WaveTrType)
        {
            List<byte> tempbufflist = new List<byte>();

            //Assemble request in reverse order first to calculate lengths
            //Insert TextIdList
            tempbufflist.InsertRange(0, WaveTrType);
            
            Ava avatype = new Ava();
            avatype.attribute_id = (ushort)IntelliVue.AttributeIDs.NOM_ATTR_POLL_RTSA_PRIO_LIST;
            avatype.length = (ushort)WaveTrType.Length;
            //avatype.length = (ushort)tempbufflist.Count;
            tempbufflist.InsertRange(0,BitConverter.GetBytes(correctendianshortus(avatype.length)));
            tempbufflist.InsertRange(0, BitConverter.GetBytes(correctendianshortus(avatype.attribute_id)));
            
            byte[] AttributeModEntry = { 0x00, 0x00 };
            tempbufflist.InsertRange(0, AttributeModEntry);
            
            byte[] ModListlength = BitConverter.GetBytes(correctendianshortus((ushort)tempbufflist.Count));
            byte[] ModListCount = { 0x00, 0x01 };
            tempbufflist.InsertRange(0, ModListlength);
            tempbufflist.InsertRange(0, ModListCount);
            
            byte[] ManagedObjectID = {0x00, 0x21, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            tempbufflist.InsertRange(0, ManagedObjectID);
            
            ROIVapdu rovi = new ROIVapdu();
            rovi.length = (ushort)tempbufflist.Count;
            rovi.command_type = (ushort)IntelliVue.Commands.CMD_CONFIRMED_SET;
            rovi.inovke_id = 0x0000;
            tempbufflist.InsertRange(0, BitConverter.GetBytes(correctendianshortus(rovi.length)));
            tempbufflist.InsertRange(0, BitConverter.GetBytes(correctendianshortus(rovi.command_type)));
            tempbufflist.InsertRange(0, BitConverter.GetBytes(correctendianshortus(rovi.inovke_id)));
            
            ROapdus roap = new ROapdus();
            roap.length = (ushort)tempbufflist.Count;
            roap.ro_type = (ushort)IntelliVue.RemoteOperationHeader.ROIV_APDU;
            tempbufflist.InsertRange(0, BitConverter.GetBytes(correctendianshortus(roap.length)));
            tempbufflist.InsertRange(0, BitConverter.GetBytes(correctendianshortus(roap.ro_type)));
            
            byte[] Spdu = { 0xE1, 0x00, 0x00, 0x02 };
            tempbufflist.InsertRange(0, Spdu);
            
            byte[] finaltxbuff = tempbufflist.ToArray();
            WriteBuffer(finaltxbuff);
        }

        public async Task SendCycledExtendedPollWaveDataRequest(int nInterval)
        {
            int nmillisecond = nInterval * 1000;
            if (nmillisecond != 0)
            {
                do
                {
                    WriteBuffer(DataConstants.ext_poll_request_wave_msg);
                    await Task.Delay(nmillisecond);

                }
                while (true);
            }
            else WriteBuffer(DataConstants.ext_poll_request_wave_msg);
        }


        public async Task SendCycledExtendedPollDataRequest(int nInterval)
        {
            int nmillisecond = nInterval;
            if (nmillisecond != 0)
            {
                do
                {
                    WriteBuffer(DataConstants.ext_poll_request_msg5);
                    //WriteBuffer(DataConstants.poll_request_msg);
                    await Task.Delay(nmillisecond);

                }
                while (true);
            }
            WriteBuffer(DataConstants.ext_poll_request_msg5);
            //WriteBuffer(DataConstants.poll_request_msg);
        }

        public async Task KeepConnectionAlive(int nInterval)
        {
            int nmillisecond = 6 * 1000;
            if (nmillisecond != 0 && nInterval > 1000)
            {
                do
                {
                    SendMDSCreateEventResult();
                    await Task.Delay(nmillisecond);
                }
                while (true);
            }

        }

        public void ParseMDSCreateEventReport(byte[] readmdsconnectbuffer)
        {
            MemoryStream memstream = new MemoryStream(readmdsconnectbuffer);
            BinaryReader binreader = new BinaryReader(memstream);

            byte[] header = binreader.ReadBytes(34);
            ushort attriblist_count = correctendianshortus(binreader.ReadUInt16());
            ushort attriblist_length = correctendianshortus(binreader.ReadUInt16());
            int avaobjectscount = Convert.ToInt32(attriblist_count);

            if (avaobjectscount > 0)
            {
                byte[] attriblistobjects = binreader.ReadBytes(attriblist_length);

                MemoryStream memstream2 = new MemoryStream(attriblistobjects);
                BinaryReader binreader2 = new BinaryReader(memstream2);


                for (int i = 0; i < avaobjectscount; i++)
                {

                    Ava avaobjects = new Ava();
                    DecodeMDSAttribObjects(ref avaobjects, ref binreader2);
                }
            }

        }

        public void DecodeMDSAttribObjects(ref Ava avaobject, ref BinaryReader binreader)
        {
            avaobject.attribute_id = correctendianshortus(binreader.ReadUInt16());
            avaobject.length = correctendianshortus(binreader.ReadUInt16());
            //avaobject.attribute_val = correctendianshortus(binreader4.ReadUInt16());
            if (avaobject.length > 0)
            {
                byte[] avaattribobjects = binreader.ReadBytes(avaobject.length);


                switch (avaobject.attribute_id)
                {
                    //Get Date and Time
                    case DataConstants.NOM_ATTR_TIME_ABS:
                        GetAbsoluteTimeFromBCDFormat(avaattribobjects);
                        break;
                    //Get Relative Time attribute
                    case DataConstants.NOM_ATTR_TIME_REL:
                        GetBaselineRelativeTimestamp(avaattribobjects);
                        break;
                    //Get Patient demographics
                    case DataConstants.NOM_ATTR_PT_ID:
                        break;
                    case DataConstants.NOM_ATTR_PT_NAME_GIVEN:
                        break;
                    case DataConstants.NOM_ATTR_PT_NAME_FAMILY:
                        break;
                    case DataConstants.NOM_ATTR_PT_DOB:
                        break;
                }
            }


        }

        private static int BinaryCodedDecimalToInteger(int value)
        {
            if (value != 0xFF)
            {
                int lowerNibble = value & 0x0F;
                int upperNibble = value >> 4;

                int multipleOfOne = lowerNibble;
                int multipleOfTen = upperNibble * 10;

                return (multipleOfOne + multipleOfTen);
            }
            else return 0;
        }

        public void GetAbsoluteTimeFromBCDFormat(byte[] bcdtimebuffer)
        {
            int century = BinaryCodedDecimalToInteger(bcdtimebuffer[0]);
            int year = BinaryCodedDecimalToInteger(bcdtimebuffer[1]);
            int month = BinaryCodedDecimalToInteger(bcdtimebuffer[2]);
            int day = BinaryCodedDecimalToInteger(bcdtimebuffer[3]);
            int hour = BinaryCodedDecimalToInteger(bcdtimebuffer[4]);
            int minute = BinaryCodedDecimalToInteger(bcdtimebuffer[5]);
            int second = BinaryCodedDecimalToInteger(bcdtimebuffer[6]);
            int fraction = BinaryCodedDecimalToInteger(bcdtimebuffer[7]);

            int formattedyear = (century * 100) + year;

            DateTime dateTime = new DateTime(formattedyear, month, day, hour, minute, second, fraction);

            m_baseDateTime = dateTime;

        }

        public void GetBaselineRelativeTimestamp(byte[] timebuffer)
        {
            m_baseRelativeTime = correctendianuint(BitConverter.ToUInt32(timebuffer, 0));
        }


        public void ProcessPacket(byte[] packetbuffer)
        {
            MemoryStream memstream = new MemoryStream(packetbuffer);
            BinaryReader binreader = new BinaryReader(memstream);

            byte[] sessionheader = binreader.ReadBytes(4);
            ushort ROapdu_type = correctendianshortus(binreader.ReadUInt16());

            switch (ROapdu_type)
            {
                case DataConstants.ROIV_APDU:
                    // This is an MDS create event, answer with create response
                    ParseMDSCreateEventReport(packetbuffer);
                    SendMDSCreateEventResult();
                    break;
                case DataConstants.RORS_APDU:
                    CheckPollPacketActionType(packetbuffer);
                    break;
                case DataConstants.RORLS_APDU:
                    CheckLinkedPollPacketActionType(packetbuffer);
                    break;
                case DataConstants.ROER_APDU:
                    break;
                default:
                    break;
            }

        }

        public void CheckPollPacketActionType(byte[] packetbuffer)
        {
            MemoryStream memstream = new MemoryStream(packetbuffer);
            BinaryReader binreader = new BinaryReader(memstream);

            byte[] header = binreader.ReadBytes(20);
            ushort action_type = correctendianshortus(binreader.ReadUInt16());
            m_actiontype = action_type;

            switch (action_type)
            {
                case DataConstants.NOM_ACT_POLL_MDIB_DATA:
                    PollPacketDecoder(packetbuffer, 44);
                    break;
                case DataConstants.NOM_ACT_POLL_MDIB_DATA_EXT:
                    PollPacketDecoder(packetbuffer, 46);
                    break;
                default:
                    break;
            }

        }

        public void CheckLinkedPollPacketActionType(byte[] packetbuffer)
        {
            MemoryStream memstream = new MemoryStream(packetbuffer);
            BinaryReader binreader = new BinaryReader(memstream);

            byte[] header = binreader.ReadBytes(22);
            ushort action_type = correctendianshortus(binreader.ReadUInt16());
            m_actiontype = action_type;

            switch (action_type)
            {
                case DataConstants.NOM_ACT_POLL_MDIB_DATA:
                    PollPacketDecoder(packetbuffer, 46);
                    break;
                case DataConstants.NOM_ACT_POLL_MDIB_DATA_EXT:
                    PollPacketDecoder(packetbuffer, 48);
                    break;
                default:
                    break;
            }

        }

        public void PollPacketDecoder(byte[] packetbuffer, int headersize)
        {
            int packetsize = packetbuffer.GetLength(0);

            MemoryStream memstream = new MemoryStream(packetbuffer);
            BinaryReader binreader = new BinaryReader(memstream);

            byte[] header = binreader.ReadBytes(headersize);
            byte[] packetdata = new byte[packetsize - header.Length];
            Array.Copy(packetbuffer, header.Length, packetdata, 0, packetdata.Length);

            m_strTimestamp = GetPacketTimestamp(header);
            uint currentRelativeTime = UInt32.Parse(m_strTimestamp);
            //DateTime dtDateTime = DateTime.Now;
            double ElapsedTimeMilliseonds = (currentRelativeTime - m_baseRelativeTime)*125/1000;
            DateTime dtDateTime = m_baseDateTime.AddMilliseconds(ElapsedTimeMilliseonds);

            string strDateTime = dtDateTime.ToString("dd-MM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Console.WriteLine("Time:{0}", strDateTime);
            Console.WriteLine("Time:{0}", m_strTimestamp);


            //ParsePacketType

            PollInfoList pollobjects = new PollInfoList();

            int scpollobjectscount = DecodePollObjects(ref pollobjects, packetdata);

            if (scpollobjectscount > 0)
            {
                MemoryStream memstream2 = new MemoryStream(pollobjects.scpollarray);
                BinaryReader binreader2 = new BinaryReader(memstream2);


                for (int i = 0; i < scpollobjectscount; i++)
                {

                    SingleContextPoll scpoll = new SingleContextPoll();
                    int obpollobjectscount = DecodeSingleContextPollObjects(ref scpoll, ref binreader2);

                    if (obpollobjectscount > 0)
                    {
                        MemoryStream memstream3 = new MemoryStream(scpoll.obpollobjectsarray);
                        BinaryReader binreader3 = new BinaryReader(memstream3);

                        for (int j = 0; j < obpollobjectscount; j++)
                        {

                            ObservationPoll obpollobject = new ObservationPoll();
                            int avaobjectscount = DecodeObservationPollObjects(ref obpollobject, ref binreader3);

                            if (avaobjectscount > 0)
                            {
                                MemoryStream memstream4 = new MemoryStream(obpollobject.avaobjectsarray);
                                BinaryReader binreader4 = new BinaryReader(memstream4);

                                for (int k = 0; k < avaobjectscount; k++)
                                {
                                    Ava avaobject = new Ava();
                                    DecodeAvaObjects(ref avaobject, ref binreader4);
                                }

                            }
                        }
                    }
                }

                if (m_dataexportset == 2) ExportNumValListToJSON("Numeric");
                ExportDataToCSV();
                ExportWaveToCSV();
            }

        }

        public int DecodePollObjects(ref PollInfoList pollobjects, byte[] packetbuffer)
        {

            MemoryStream memstream = new MemoryStream(packetbuffer);
            BinaryReader binreader = new BinaryReader(memstream);

            pollobjects.count = correctendianshortus(binreader.ReadUInt16());
            if (pollobjects.count > 0) pollobjects.length = correctendianshortus(binreader.ReadUInt16());

            int scpollobjectscount = Convert.ToInt32(pollobjects.count);
            if (pollobjects.length > 0) pollobjects.scpollarray = binreader.ReadBytes(pollobjects.length);

            return scpollobjectscount;
        }

        public int DecodeSingleContextPollObjects(ref SingleContextPoll scpoll, ref BinaryReader binreader2)
        {
            scpoll.context_id = correctendianshortus(binreader2.ReadUInt16());
            scpoll.count = correctendianshortus(binreader2.ReadUInt16());
            //There can be empty singlecontextpollobjects
            //if(scpoll.count>0) scpoll.length = correctendianshortus(binreader2.ReadUInt16());
            scpoll.length = correctendianshortus(binreader2.ReadUInt16());

            int obpollobjectscount = Convert.ToInt32(scpoll.count);
            if (scpoll.length > 0) scpoll.obpollobjectsarray = binreader2.ReadBytes(scpoll.length);

            return obpollobjectscount;
        }

        public int DecodeObservationPollObjects(ref ObservationPoll obpollobject, ref BinaryReader binreader3)
        {
            obpollobject.obj_handle = correctendianshortus(binreader3.ReadUInt16());

            m_obpollhandle = obpollobject.obj_handle;

            AttributeList attributeliststruct = new AttributeList();

            attributeliststruct.count = correctendianshortus(binreader3.ReadUInt16());
            if (attributeliststruct.count > 0) attributeliststruct.length = correctendianshortus(binreader3.ReadUInt16());

            int avaobjectscount = Convert.ToInt32(attributeliststruct.count);
            if (attributeliststruct.length > 0) obpollobject.avaobjectsarray = binreader3.ReadBytes(attributeliststruct.length);

            return avaobjectscount;
        }

        public void DecodeAvaObjects(ref Ava avaobject, ref BinaryReader binreader4)
        {
            avaobject.attribute_id = correctendianshortus(binreader4.ReadUInt16());
            avaobject.length = correctendianshortus(binreader4.ReadUInt16());
            //avaobject.attribute_val = correctendianshortus(binreader4.ReadUInt16());
            if (avaobject.length > 0)
            {
                byte[] avaattribobjects = binreader4.ReadBytes(avaobject.length);


                switch (avaobject.attribute_id)
                {
                    case DataConstants.NOM_ATTR_ID_HANDLE:
                        //ReadIDHandle(avaattribobjects);
                        break;
                    case DataConstants.NOM_ATTR_ID_LABEL:
                        ReadIDLabel(avaattribobjects);
                        break;
                    case DataConstants.NOM_ATTR_NU_VAL_OBS:
                        ReadNumericObservationValue(avaattribobjects);
                        break;
                    case DataConstants.NOM_ATTR_NU_CMPD_VAL_OBS:
                        ReadCompoundNumericObsValue(avaattribobjects);
                        break;
                    case DataConstants.NOM_ATTR_METRIC_SPECN:
                        break;
                    case DataConstants.NOM_ATTR_ID_LABEL_STRING:
                        ReadIDLabelString(avaattribobjects);
                        break;
                    case DataConstants.NOM_ATTR_SA_VAL_OBS:
                        ReadWaveSaObservationValueObject(avaattribobjects);
                        break;
                    case DataConstants.NOM_ATTR_SA_CMPD_VAL_OBS:
                        ReadCompoundWaveSaObservationValue(avaattribobjects);
                        break;
                    case DataConstants.NOM_ATTR_SA_SPECN:
                        ReadSaSpecifications(avaattribobjects);
                        break;
                    case DataConstants.NOM_ATTR_SCALE_SPECN_I16:
                        //ReadSaScaleSpecifications(avaattribobjects);
                        break;
                    case DataConstants.NOM_ATTR_SA_CALIB_I16:
                        ReadSaCalibrationSpecifications(avaattribobjects);
                        break;
                    default:
                        // unknown attribute -> do nothing
                        break;

                }
            }

        }

        public string GetPacketTimestamp(byte[] header)
        {
            MemoryStream memstream = new MemoryStream(header);
            BinaryReader binreader = new BinaryReader(memstream);

            int pollmdibdatareplysize = 20;
            if (m_actiontype == DataConstants.NOM_ACT_POLL_MDIB_DATA) pollmdibdatareplysize = 20;
            else if (m_actiontype == DataConstants.NOM_ACT_POLL_MDIB_DATA_EXT) pollmdibdatareplysize = 22;

            int firstpartheaderlength = (header.Length - pollmdibdatareplysize);
            byte[] firstpartheader = binreader.ReadBytes(firstpartheaderlength);
            byte[] pollmdibdatareplyarray = binreader.ReadBytes(pollmdibdatareplysize);

            double relativetime = 0;
            if (m_actiontype == DataConstants.NOM_ACT_POLL_MDIB_DATA)
            {
                PollMdibDataReply pollmdibdatareply = new PollMdibDataReply();

                MemoryStream memstream2 = new MemoryStream(pollmdibdatareplyarray);
                BinaryReader binreader2 = new BinaryReader(memstream2);

                pollmdibdatareply.poll_number = correctendianshortus(binreader2.ReadUInt16());
                pollmdibdatareply.rel_time_stamp = correctendianuint(binreader2.ReadUInt32());

                relativetime = pollmdibdatareply.rel_time_stamp;
            }
            else if (m_actiontype == DataConstants.NOM_ACT_POLL_MDIB_DATA_EXT)
            {
                PollMdibDataReplyExt pollmdibdatareplyext = new PollMdibDataReplyExt();

                MemoryStream memstream2 = new MemoryStream(pollmdibdatareplyarray);
                BinaryReader binreader2 = new BinaryReader(memstream2);

                pollmdibdatareplyext.poll_number = correctendianshortus(binreader2.ReadUInt16());
                pollmdibdatareplyext.sequence_no = correctendianshortus(binreader2.ReadUInt16());
                pollmdibdatareplyext.rel_time_stamp = correctendianuint(binreader2.ReadUInt32());

                relativetime = pollmdibdatareplyext.rel_time_stamp;
            }

            string strRelativeTime = relativetime.ToString();

            //AbsoluteTime is not supported by several monitors
            /*AbsoluteTime absolutetime = new AbsoluteTime();

            absolutetime.century = binreader2.ReadByte();
            absolutetime.year = binreader2.ReadByte();
            absolutetime.month = binreader2.ReadByte();
            absolutetime.day = binreader2.ReadByte();
            absolutetime.hour = binreader2.ReadByte();
            absolutetime.minute = binreader2.ReadByte();
            absolutetime.second = binreader2.ReadByte();
            absolutetime.fraction = binreader2.ReadByte();*/

            return strRelativeTime;

        }

        public void ReadIDHandle(byte[] avaattribobjects)
        {
            MemoryStream memstream5 = new MemoryStream(avaattribobjects);
            BinaryReader binreader5 = new BinaryReader(memstream5);

            ushort IDhandle = correctendianshortus(binreader5.ReadUInt16());

        }

        public void ReadIDLabel(byte[] avaattribobjects)
        {
            MemoryStream memstream5 = new MemoryStream(avaattribobjects);
            BinaryReader binreader5 = new BinaryReader(memstream5);

            uint IDlabel = correctendianuint(binreader5.ReadUInt32());

            m_idlabelhandle = IDlabel;
        }

        public void ReadIDLabelString(byte[] avaattribobjects)
        {

            MemoryStream memstream5 = new MemoryStream(avaattribobjects);
            BinaryReader binreader5 = new BinaryReader(memstream5);

            StringMP strmp = new StringMP();

            strmp.length = correctendianshortus(binreader5.ReadUInt16());
            //strmp.value1 = correctendianshortus(binreader5.ReadUInt16());
            byte[] stringval = binreader5.ReadBytes(strmp.length);

            string label = Encoding.UTF8.GetString(stringval);
            Console.WriteLine("Label String: {0}", label);
        }

        public void ReadNumericObservationValue(byte[] avaattribobjects)
        {
            MemoryStream memstream5 = new MemoryStream(avaattribobjects);
            BinaryReader binreader5 = new BinaryReader(memstream5);

            NuObsValue NumObjectValue = new NuObsValue();
            NumObjectValue.physio_id = correctendianshortus(binreader5.ReadUInt16());
            NumObjectValue.state = correctendianshortus(binreader5.ReadUInt16());
            NumObjectValue.unit_code = correctendianshortus(binreader5.ReadUInt16());
            NumObjectValue.value = correctendianuint(binreader5.ReadUInt32());

            double value = FloattypeToValue(NumObjectValue.value);

            //string physio_id = NumObjectValue.physio_id.ToString();
            string physio_id = Enum.GetName(typeof(IntelliVue.AlertSource), NumObjectValue.physio_id);

            string state = NumObjectValue.state.ToString();
            string unit_code = NumObjectValue.unit_code.ToString();

            string valuestr;
            if (value != DataConstants.FLOATTYPE_NAN)
            {
                valuestr = value.ToString();
            }
            else valuestr = "-";

            NumericValResult NumVal = new NumericValResult();
            NumVal.Relativetimestamp = m_strTimestamp;

            uint currentRelativeTime = UInt32.Parse(m_strTimestamp);
            double ElapsedTimeMilliseonds = (currentRelativeTime - m_baseRelativeTime) * 125 / 1000;
            DateTime dtDateTime = m_baseDateTime.AddMilliseconds(ElapsedTimeMilliseonds);
            //NumVal.Timestamp = dtDateTime.ToString();

            //string strDateTime = dtDateTime.ToString("dd-MM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            string strDateTime = dtDateTime.ToString("G", DateTimeFormatInfo.InvariantInfo);
            NumVal.Timestamp = strDateTime;
            //NumVal.Timestamp = DateTime.Now.ToString();

            NumVal.PhysioID = physio_id;
            NumVal.Value = valuestr;
            NumVal.DeviceID = m_DeviceID;

            m_NumericValList.Add(NumVal);
            m_NumValHeaders.Add(NumVal.PhysioID);

            Console.WriteLine("Physiological ID: {0}", physio_id);
            //Console.WriteLine("State: {0}", state);
            //Console.WriteLine("Unit code: {0}", unit_code);
            Console.WriteLine("Value: {0}", valuestr);
            Console.WriteLine();
        }

        public void ReadCompoundNumericObsValue(byte[] avaattribobjects)
        {
            MemoryStream memstream6 = new MemoryStream(avaattribobjects);
            BinaryReader binreader6 = new BinaryReader(memstream6);

            NuObsValueCmp NumObjectValueCmp = new NuObsValueCmp();
            NumObjectValueCmp.count = correctendianshortus(binreader6.ReadUInt16());
            NumObjectValueCmp.length = correctendianshortus(binreader6.ReadUInt16());

            int cmpnumericobjectscount = Convert.ToInt32(NumObjectValueCmp.count);

            if (cmpnumericobjectscount > 0)
            {
                for (int j = 0; j < cmpnumericobjectscount; j++)
                {
                    byte[] cmpnumericarrayobject = binreader6.ReadBytes(10);

                    ReadNumericObservationValue(cmpnumericarrayobject);
                }
            }

        }

        public void ReadWaveSaObservationValueObject(byte[] avaattribobjects)
        {
            MemoryStream memstream7 = new MemoryStream(avaattribobjects);
            BinaryReader binreader7 = new BinaryReader(memstream7);

            ReadWaveSaObservationValue(ref binreader7);

        }

        public void ReadSaSpecifications(byte[] avaattribobjects)
        {
            MemoryStream memstream7 = new MemoryStream(avaattribobjects);
            BinaryReader binreader7 = new BinaryReader(memstream7);

            SaSpec Saspecobj = new SaSpec();
            Saspecobj.array_size = correctendianshortus(binreader7.ReadUInt16());
            Saspecobj.sample_size = binreader7.ReadByte();
            Saspecobj.significant_bits = binreader7.ReadByte();
            Saspecobj.SaFlags = correctendianshortus(binreader7.ReadUInt16());

            Saspecobj.obpoll_handle = m_obpollhandle;
            
            //Add to a list of Sample array specification definitions if it's not already present
            
            int salistindex = m_SaSpecList.FindIndex(x => x.obpoll_handle == Saspecobj.obpoll_handle);
            if (salistindex == -1)
            {
                m_SaSpecList.Add(Saspecobj);
            }
            else
            {
                m_SaSpecList.RemoveAt(salistindex);
                m_SaSpecList.Add(Saspecobj);
            }
        }

        public void ReadWaveSaObservationValue(ref BinaryReader binreader7)
        {
            SaObsValue WaveSaObjectValue = new SaObsValue();
            WaveSaObjectValue.physio_id = correctendianshortus(binreader7.ReadUInt16());
            WaveSaObjectValue.state = correctendianshortus(binreader7.ReadUInt16());
            WaveSaObjectValue.length = correctendianshortus(binreader7.ReadUInt16());

            int wavevalobjectslength = Convert.ToInt32(WaveSaObjectValue.length);
            byte[] WaveValObjects = binreader7.ReadBytes(wavevalobjectslength);
            
            string physio_id = Enum.GetName(typeof(IntelliVue.AlertSource), WaveSaObjectValue.physio_id);

            WaveValResult WaveVal = new WaveValResult();
            WaveVal.Relativetimestamp = m_strTimestamp;

            //DateTime dtDateTime = DateTime.Now;
            uint currentRelativeTime = UInt32.Parse(m_strTimestamp);
            double ElapsedTimeMilliseonds = (currentRelativeTime - m_baseRelativeTime) * 125 / 1000;
            DateTime dtDateTime = m_baseDateTime.AddMilliseconds(ElapsedTimeMilliseonds);
            
            string strDateTime = dtDateTime.ToString("dd-MM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //WaveVal.Timestamp = DateTime.Now.ToString();
            
            WaveVal.Timestamp = strDateTime;
            WaveVal.PhysioID = physio_id;
            WaveVal.DeviceID = m_DeviceID;

            WaveVal.obpoll_handle = m_obpollhandle;
            ushort physio_id_handle = WaveSaObjectValue.physio_id;

            WaveVal.saCalibData = m_SaCalibDataSpecList.Find(x => x.physio_id == physio_id_handle);
            if (WaveVal.saCalibData == null)
            {
                WaveVal.saCalibData = new SaCalibData16();
                if (physio_id_handle == 0x107)
                {
                    //use default values for ecg II
                    WaveVal.saCalibData.lower_absolute_value = 0;
                    WaveVal.saCalibData.upper_absolute_value = 1;
                    WaveVal.saCalibData.lower_scaled_value = 0x1fe7;
                    WaveVal.saCalibData.upper_scaled_value = 0x20af;
                }
                else if (physio_id_handle == 0x102)
                {
                    //use default values for ecg V5
                    WaveVal.saCalibData.lower_absolute_value = 0;
                    WaveVal.saCalibData.upper_absolute_value = 1;
                    WaveVal.saCalibData.lower_scaled_value = 0x1fd4;
                    WaveVal.saCalibData.upper_scaled_value = 0x209c;
                }
                else if (physio_id_handle == 0x4A10)
                {
                    //use default values for art ibp
                    WaveVal.saCalibData.lower_absolute_value = 0;
                    WaveVal.saCalibData.upper_absolute_value = 150;
                    WaveVal.saCalibData.lower_scaled_value = 0x0320;
                    WaveVal.saCalibData.upper_scaled_value = 0x0c80;
                }
                else if (physio_id_handle == 0x5000)
                {
                    //use default values for resp
                    WaveVal.saCalibData.lower_absolute_value = 0;
                    WaveVal.saCalibData.upper_absolute_value = 1;
                    WaveVal.saCalibData.lower_scaled_value = 0x04ce;
                    WaveVal.saCalibData.upper_scaled_value = 0x0b33;
                }
                else WaveVal.saCalibData = null;
                
            }

            WaveVal.Value = new byte[wavevalobjectslength];
            Array.Copy(WaveValObjects, WaveVal.Value, wavevalobjectslength);

            //Find the Sample array specification definition that matches the observation sample array size

            WaveVal.saSpecData = m_SaSpecList.Find(x => x.obpoll_handle == WaveVal.obpoll_handle);
            if (WaveVal.saSpecData == null)
            {
                WaveVal.saSpecData = new SaSpec();
                if (wavevalobjectslength % 128 == 0)
                {
                    //use default values for ecg
                    WaveVal.saSpecData.significant_bits = 0x0E;
                    WaveVal.saSpecData.SaFlags = 0x3000;
                    WaveVal.saSpecData.sample_size = 0x10;
                    WaveVal.saSpecData.array_size = 0x80;
                }
                else if (wavevalobjectslength % 64 == 0)
                {
                    //use default values for art ibp
                    WaveVal.saSpecData.significant_bits = 0x0E;
                    WaveVal.saSpecData.SaFlags = 0x3000;
                    WaveVal.saSpecData.sample_size = 0x10;
                    WaveVal.saSpecData.array_size = 0x40;
                  
                }
                else if (wavevalobjectslength % 32 == 0)
                {
                    //use default values for resp
                    WaveVal.saSpecData.significant_bits = 0x0C;
                    WaveVal.saSpecData.SaFlags = 0x8000;
                    WaveVal.saSpecData.sample_size = 0x10;
                    WaveVal.saSpecData.array_size = 0x20;
                }
                else if (wavevalobjectslength % 16 == 0)
                {
                    //use default values for pleth
                    WaveVal.saSpecData.significant_bits = 0x0C;
                    WaveVal.saSpecData.SaFlags = 0x8000;
                    WaveVal.saSpecData.sample_size = 0x10;
                    WaveVal.saSpecData.array_size = 0x10;
                }
                
            }
    
            m_WaveValResultList.Add(WaveVal);

        }

        public void ReadCompoundWaveSaObservationValue(byte[] avaattribobjects)
        {
            MemoryStream memstream8 = new MemoryStream(avaattribobjects);
            BinaryReader binreader8 = new BinaryReader(memstream8);

            SaObsValueCmp WaveSaObjectValueCmp = new SaObsValueCmp();
            WaveSaObjectValueCmp.count = correctendianshortus(binreader8.ReadUInt16());
            WaveSaObjectValueCmp.length = correctendianshortus(binreader8.ReadUInt16());

            int cmpwaveobjectscount = Convert.ToInt32(WaveSaObjectValueCmp.count);
            int cmpwaveobjectslength = Convert.ToInt32(WaveSaObjectValueCmp.length);

            byte[] cmpwavearrayobject = binreader8.ReadBytes(cmpwaveobjectslength);

            if (cmpwaveobjectscount > 0)
            {
                MemoryStream memstream9 = new MemoryStream(cmpwavearrayobject);
                BinaryReader binreader9 = new BinaryReader(memstream9);

                for (int k = 0; k < cmpwaveobjectscount; k++)
                {
                    ReadWaveSaObservationValue(ref binreader9);
                }
            }
        }

        public void ReadSaScaleSpecifications(byte[] avaattribobjects)
        {
            MemoryStream memstream9 = new MemoryStream(avaattribobjects);
            BinaryReader binreader9 = new BinaryReader(memstream9);

            ScaleRangeSpec16 ScaleSpec = new ScaleRangeSpec16();

            ScaleSpec.lower_absolute_value = FloattypeToValue(correctendianuint(binreader9.ReadUInt32()));
            ScaleSpec.upper_absolute_value = FloattypeToValue(correctendianuint(binreader9.ReadUInt32()));
            ScaleSpec.lower_scaled_value = correctendianshortus(binreader9.ReadUInt16());
            ScaleSpec.upper_scaled_value = correctendianshortus(binreader9.ReadUInt16());

            ScaleSpec.obpoll_handle = m_obpollhandle;
            ScaleSpec.physio_id = Get16bitLSBfromUInt(m_idlabelhandle);
        }

        public void ReadSaCalibrationSpecifications(byte[] avaattribobjects)
        {
            MemoryStream memstream10 = new MemoryStream(avaattribobjects);
            BinaryReader binreader10 = new BinaryReader(memstream10);

            SaCalibData16 SaCalibData = new SaCalibData16();

            SaCalibData.lower_absolute_value = FloattypeToValue(correctendianuint(binreader10.ReadUInt32()));
            SaCalibData.upper_absolute_value = FloattypeToValue(correctendianuint(binreader10.ReadUInt32()));
            SaCalibData.lower_scaled_value = correctendianshortus(binreader10.ReadUInt16());
            SaCalibData.upper_scaled_value = correctendianshortus(binreader10.ReadUInt16());
            SaCalibData.increment = correctendianshortus(binreader10.ReadUInt16());
            SaCalibData.cal_type = correctendianshortus(binreader10.ReadUInt16());

            SaCalibData.obpoll_handle = m_obpollhandle;

            //Get 16 bit physiological id from 32 bit wave id label
            SaCalibData.physio_id = Get16bitLSBfromUInt(m_idlabelhandle);

            //Add to a list of Sample array calibration specification definitions if it's not already present
            int salistindex = m_SaCalibDataSpecList.FindIndex(x => x.physio_id == SaCalibData.physio_id);

            if (salistindex == -1)
            {
                m_SaCalibDataSpecList.Add(SaCalibData);
            }
            else
            {
                m_SaCalibDataSpecList.RemoveAt(salistindex);
                m_SaCalibDataSpecList.Add(SaCalibData);
            }
        }



        public static double FloattypeToValue(uint fvalue)
        {
            double value = 0;
            if (fvalue != DataConstants.FLOATTYPE_NAN)
            {
                int exponentbits = (int)(fvalue >> 24);
                int mantissabits = (int)(fvalue << 8);
                mantissabits = (mantissabits >> 8);

                sbyte signedexponentbits = (sbyte)exponentbits; // Get Two's complement signed byte
                decimal exponent = Convert.ToDecimal(signedexponentbits);

                double mantissa = mantissabits;
                value = mantissa * Math.Pow((double)10, (double)exponent);

                return value;
            }
            else return (double)fvalue;
        }

        public static ushort Get16bitLSBfromUInt(uint sourcevalue)
        {
            uint lsb = (sourcevalue & 0xFFFF);
            
            return (ushort)lsb;
        }

        public static int correctendianshort(ushort sValue)
        {
            byte[] bArray = BitConverter.GetBytes(sValue);
            if (BitConverter.IsLittleEndian) Array.Reverse(bArray);

            int nresult = BitConverter.ToInt16(bArray, 0);
            return nresult;
        }

        public static ushort correctendianshortus(ushort sValue)
        {
            byte[] bArray = BitConverter.GetBytes(sValue);
            if (BitConverter.IsLittleEndian) Array.Reverse(bArray);

            ushort result = BitConverter.ToUInt16(bArray, 0);
            return result;
        }

        public static uint correctendianuint(uint sValue)
        {
            byte[] bArray = BitConverter.GetBytes(sValue);
            if (BitConverter.IsLittleEndian) Array.Reverse(bArray);

            uint result = BitConverter.ToUInt32(bArray, 0);
            return result;
        }

        public static short correctendianshorts(short sValue)
        {
            byte[] bArray = BitConverter.GetBytes(sValue);
            if (BitConverter.IsLittleEndian) Array.Reverse(bArray);

            short result = BitConverter.ToInt16(bArray, 0);
            return result;
        }

        public void ExportDataToCSV()
        {
            switch (m_csvexportset)
            {
                case 1:
                    SaveNumericValueList();
                    break;
                case 2:
                    SaveNumericValueListRows();
                    break;
                case 3:
                    SaveNumericValueListConsolidatedCSV();
                    break;
                default:
                    break;
            }
        }

        public void WriteNumericHeadersList()
        {
            if (m_NumericValList.Count != 0)
            {
                string pathcsv = Path.Combine(Directory.GetCurrentDirectory(), "MPDataExport.csv");

                m_strbuildheaders.Append("Time");
                m_strbuildheaders.Append(',');
                m_strbuildheaders.Append("RelativeTime");
                m_strbuildheaders.Append(',');


                foreach (NumericValResult NumValResult in m_NumericValList)
                {
                    m_strbuildheaders.Append(NumValResult.PhysioID);
                    m_strbuildheaders.Append(',');

                }

                m_strbuildheaders.Remove(m_strbuildheaders.Length - 1, 1);
                m_strbuildheaders.Replace(",,", ",");
                m_strbuildheaders.AppendLine();
                ExportNumValListToCSVFile(pathcsv, m_strbuildheaders);

                m_strbuildheaders.Clear();
                m_NumValHeaders.RemoveRange(0, m_NumValHeaders.Count);


            }
        }

        public void SaveNumericValueList()
        {
            if (m_NumericValList.Count != 0)
            {
                string pathcsv = Path.Combine(Directory.GetCurrentDirectory(), "MPDataExport.csv");

                foreach (NumericValResult NumValResult in m_NumericValList)
                {
                    m_strbuildvalues.Append(NumValResult.Timestamp);
                    m_strbuildvalues.Append(',');
                    m_strbuildvalues.Append(NumValResult.Relativetimestamp);
                    m_strbuildvalues.Append(',');
                    m_strbuildvalues.Append(NumValResult.PhysioID);
                    m_strbuildvalues.Append(',');
                    m_strbuildvalues.Append(NumValResult.Value);
                    m_strbuildvalues.AppendLine();
                }

                ExportNumValListToCSVFile(pathcsv, m_strbuildvalues);
                m_strbuildvalues.Clear();
                m_NumericValList.RemoveRange(0, m_NumericValList.Count);
            }
        }

        public void SaveNumericValueListRows()
        {
            if (m_NumericValList.Count != 0)
            {
                WriteNumericHeadersList();
                string pathcsv = Path.Combine(Directory.GetCurrentDirectory(), "MPDataExport.csv");

                m_strbuildvalues.Append(m_NumericValList.ElementAt(0).Timestamp);
                m_strbuildvalues.Append(',');
                m_strbuildvalues.Append(m_NumericValList.ElementAt(0).Relativetimestamp);
                m_strbuildvalues.Append(',');


                foreach (NumericValResult NumValResult in m_NumericValList)
                {
                    m_strbuildvalues.Append(NumValResult.Value);
                    m_strbuildvalues.Append(',');

                }

                m_strbuildvalues.Remove(m_strbuildvalues.Length - 1, 1);
                m_strbuildvalues.Replace(",,", ",");
                m_strbuildvalues.AppendLine();

                ExportNumValListToCSVFile(pathcsv, m_strbuildvalues);
                m_strbuildvalues.Clear();
                m_NumericValList.RemoveRange(0, m_NumericValList.Count);
            }
        }

        public void SaveNumericValueListConsolidatedCSV()
        {
            //This method saves all numeric data with the same relative time attribute to a list in memory till
            //the next data with a different realtive time attribute arrives, only then the first set gets exported

            if (m_NumericValList.Count != 0)
            {
                WriteNumericHeadersListConsolidatedCSV();
                string pathcsv = Path.Combine(Directory.GetCurrentDirectory(), "MPDataExport.csv");

                uint firstelementreltimestamp = Convert.ToUInt32(m_NumericValList.ElementAt(0).Relativetimestamp);
                int listcount = m_NumericValList.Count;

                for (int i = m_elementcount; i < listcount; i++)
                {
                    uint elementreltime = Convert.ToUInt32(m_NumericValList.ElementAt(i).Relativetimestamp);
                    if (elementreltime == firstelementreltimestamp)
                    {
                        m_strbuildvalues.Append(m_NumericValList.ElementAt(i).Value);
                        m_strbuildvalues.Append(',');
                        m_elementcount++;
                    }
                    else
                    {
                        m_strbuildvalues.Insert(0, ',');
                        m_strbuildvalues.Insert(0, m_NumericValList.ElementAt(0).Relativetimestamp);
                        m_strbuildvalues.Insert(0, ',');
                        m_strbuildvalues.Insert(0, m_NumericValList.ElementAt(0).Timestamp);


                        m_strbuildvalues.Remove(m_strbuildvalues.Length - 1, 1);
                        m_strbuildvalues.Replace(",,", ",");
                        m_strbuildvalues.AppendLine();

                        ExportNumValListToCSVFile(pathcsv, m_strbuildvalues);
                        m_strbuildvalues.Clear();
                        m_NumericValList.RemoveRange(0, m_elementcount);
                        m_elementcount = 0;
                        listcount = m_NumericValList.Count;
                    }
                }



            }
        }

        public void WriteNumericHeadersListConsolidatedCSV()
        {
            if (m_NumericValList.Count != 0 && m_transmissionstart)
            {
                string pathcsv = Path.Combine(Directory.GetCurrentDirectory(), "MPDataExport.csv");

                uint firstelementreltimestamp = Convert.ToUInt32(m_NumericValList.ElementAt(0).Relativetimestamp);
                int listcount = m_NumValHeaders.Count;

                for (int i = m_headerelementcount; i < listcount; i++)
                {
                    uint elementreltime = Convert.ToUInt32(m_NumericValList.ElementAt(i).Relativetimestamp);
                    if (elementreltime == firstelementreltimestamp)
                    {
                        m_strbuildheaders.Append(m_NumValHeaders.ElementAt(i));
                        m_strbuildheaders.Append(',');
                        m_headerelementcount++;
                    }
                    else
                    {
                        m_strbuildheaders.Insert(0, ',');
                        m_strbuildheaders.Insert(0, "RelativeTime");
                        m_strbuildheaders.Insert(0, ',');
                        m_strbuildheaders.Insert(0, "Time");


                        m_strbuildheaders.Remove(m_strbuildheaders.Length - 1, 1);
                        m_strbuildheaders.Replace(",,", ",");
                        m_strbuildheaders.AppendLine();
                        ExportNumValListToCSVFile(pathcsv, m_strbuildheaders);

                        m_strbuildheaders.Clear();
                        m_NumValHeaders.RemoveRange(0, m_headerelementcount);
                        m_headerelementcount = 0;
                        listcount = m_NumValHeaders.Count;
                        m_transmissionstart = false;
                    }

                }


            }
        }

        public void ExportWaveToCSV()
        {
            int wavevallistcount = m_WaveValResultList.Count;

            if (wavevallistcount != 0)
            {

                foreach (WaveValResult WavValResult in m_WaveValResultList)
                {
                    string WavValID = string.Format("{0}WaveExport.csv", WavValResult.PhysioID);

                    string pathcsv = Path.Combine(Directory.GetCurrentDirectory(), WavValID);

                    int wavvalarraylength = WavValResult.Value.GetLength(0);

                    for (int index = 0; index < wavvalarraylength; index++)
                    {
                        //Data sample size is 16 bits, but the significant bits represent actual sample value

                        //Read every 2 bytes
                        byte msb = WavValResult.Value.ElementAt(index);
                        byte lsb = WavValResult.Value.ElementAt(index + 1);

                        int msbval = msb;
                        //mask depends on no. of significant bits
                        //int mask = 0x3FFF; //mask for 14 bits
                        int mask = CreateMask(WavValResult.saSpecData.significant_bits);

                        //int shift = (m_sample_size-8);
                        int msbshift = (msb << 8);

                        if (WavValResult.saSpecData.SaFlags < 0x4000)
                        {
                            msbval = (msbshift & mask);
                            msbval = (msbval >> 8);
                        }
                        else msbval = msb;
                        msb = Convert.ToByte(msbval);

                        byte[] data = { msb, lsb };
                        if (BitConverter.IsLittleEndian) Array.Reverse(data);

                        double Waveval = BitConverter.ToInt16(data, 0);

                        if (WavValResult.saSpecData.SaFlags != 0x2000 && m_calibratewavevalues == true)
                        {
                            Waveval = CalibrateSaValue(Waveval, WavValResult.saCalibData);
                        }

                        index = index + 1;

                        {
                            m_strbuildwavevalues.Append(WavValResult.Timestamp);
                            m_strbuildwavevalues.Append(',');
                            m_strbuildwavevalues.Append(WavValResult.Relativetimestamp);
                            m_strbuildwavevalues.Append(',');
                            m_strbuildwavevalues.Append(Waveval.ToString());
                            m_strbuildwavevalues.Append(',');
                            m_strbuildwavevalues.AppendLine();
                        }
                    }

                    ExportNumValListToCSVFile(pathcsv, m_strbuildwavevalues);

                    m_strbuildwavevalues.Clear();
                }

                m_WaveValResultList.RemoveRange(0, wavevallistcount);
            }
        }

        public static int CreateMask(int significantbits)
        {
            int mask = 0;

            for (int i = 0; i < significantbits; i++)
            {
                mask |= (1 << i);
            }
            return mask;
        }
        
        public double CalibrateSaValue(double Waveval, SaCalibData16 sacalibdata)
        {
            if (!double.IsNaN(Waveval))
            {
                if (sacalibdata != null)
                {
                    double prop = 0;
                    double value = 0;
                    double Wavevalue = Waveval;
                    
                    //Check if value is out of range
                    if (Waveval > sacalibdata.upper_scaled_value) Waveval = sacalibdata.upper_scaled_value;
                    if (Waveval < sacalibdata.lower_scaled_value) Waveval = sacalibdata.lower_scaled_value;

                    //Get proportion from scaled values
                    if (sacalibdata.upper_scaled_value != sacalibdata.lower_scaled_value)
                    {
                        prop = (Waveval - sacalibdata.lower_scaled_value) / (sacalibdata.upper_scaled_value - sacalibdata.lower_scaled_value);
                    }
                    if (sacalibdata.upper_absolute_value != sacalibdata.lower_absolute_value)
                    {
                        value = sacalibdata.lower_absolute_value + (prop * (sacalibdata.upper_absolute_value - sacalibdata.lower_absolute_value));
                        value = Math.Round(value, 2);
                    }

                    Wavevalue = value;
                    return Wavevalue;
                }
                else return Waveval;
                
            }
            else return Waveval;
        }

        public void ExportNumValListToCSVFile(string _FileName, StringBuilder strbuildNumVal)
        {
            try
            {
                // Open file for reading. 
                using (StreamWriter wrStream = new StreamWriter(_FileName, true, Encoding.UTF8))
                {
                    wrStream.Write(strbuildNumVal);
                    strbuildNumVal.Clear();

                    // close file stream. 
                    wrStream.Close();
                }

            }

            catch (Exception _Exception)
            {
                // Error. 
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

        }

        public bool ByteArrayToFile(string _FileName, byte[] _ByteArray, int nWriteLength)
        {
            try
            {
                // Open file for reading. 
                using (FileStream _FileStream = new FileStream(_FileName, FileMode.Append, FileAccess.Write))
                {
                    // Writes a block of bytes to this stream using data from a byte array
                    _FileStream.Write(_ByteArray, 0, nWriteLength);

                    // close file stream. 
                    _FileStream.Close();
                }
    
                return true;
            }

            catch (Exception _Exception)
            {
                // Error. 
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }
            // error occured, return false. 
            return false;
        }

        public void ExportNumValListToJSON(string datatype)
        {
            //string filename = String.Format("MP{0}DataExport.json", datatype);

            //string pathjson = Path.Combine(Directory.GetCurrentDirectory(), filename);

            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<NumericValResult>));

            MemoryStream memstream = new MemoryStream();
            jsonSerializer.WriteObject(memstream, m_NumericValList);

            string serializedJSON = Encoding.UTF8.GetString(memstream.ToArray());
            memstream.Close();

            try
            {
                // Open file for reading. 
                //using (StreamWriter wrStream = new StreamWriter(pathjson, true, Encoding.UTF8))
                //{
                //  wrStream.Write(serializedJSON);
                //  wrStream.Close();
                //}

                PostJSONDataToServer(serializedJSON);

            }

            catch (Exception _Exception)
            {
                // Error. 
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }
        }

        public void PostJSONDataToServer(string postData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_jsonposturl);
            request.Method = "POST";

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            // Get the response.  
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(response.StatusDescription);

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
                reader.Close();
                dataStream.Close();
            }

            response.Close();
        }

        public void StopTransfer()
        {
            WriteBuffer(DataConstants.assoc_abort_resp_msg);
        }


        public bool OSIsUnix()
        {
            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128)) return true;
            else return false;

        }
    }
}
