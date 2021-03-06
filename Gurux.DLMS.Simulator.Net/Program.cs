﻿//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
//
// Copyright (c) Gurux Ltd
//
//---------------------------------------------------------------------------
//
//  DESCRIPTION
//
// This file is a part of Gurux Device Framework.
//
// Gurux Device Framework is Open Source software; you can redistribute it
// and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; version 2 of the License.
// Gurux Device Framework is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// More information of Gurux products: http://www.gurux.org
//
// This code is licensed under the GNU General Public License v2.
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects;
using Gurux.Net;
using Gurux.Serial;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Gurux.DLMS.Simulator.Net
{
    class Program
    {
        /// <summary>
        /// Read simulated values from the meter.
        /// </summary>
        static void ReadSimulatedValues(Settings settings)
        {
            Reader.GXDLMSReader reader = null;
            try
            {
                ////////////////////////////////////////
                //Initialise connection settings.
                if (settings.media is GXSerial)
                {
                    GXSerial serial = settings.media as GXSerial;
                    if (settings.iec)
                    {
                        serial.BaudRate = 300;
                        serial.DataBits = 7;
                        serial.Parity = System.IO.Ports.Parity.Even;
                        serial.StopBits = System.IO.Ports.StopBits.One;
                    }
                    else
                    {
                        serial.BaudRate = 9600;
                        serial.DataBits = 8;
                        serial.Parity = System.IO.Ports.Parity.None;
                        serial.StopBits = System.IO.Ports.StopBits.One;
                    }
                }
                else if (settings.media is GXNet)
                {
                }
                else
                {
                    throw new Exception("Unknown media type.");
                }
                ////////////////////////////////////////
                reader = new Reader.GXDLMSReader(settings.client, settings.media, settings.trace, settings.invocationCounter, settings.iec);
                settings.media.Open();
                //Some meters need a break here.
                Thread.Sleep(1000);
                reader.ReadAll(settings.outputFile);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Start simulator.
        /// </summary>
        static void StartSimulator(Settings settings)
        {
            if (settings.media is GXSerial)
            {
                GXDLMSMeter server = new GXDLMSMeter(settings.client.UseLogicalNameReferencing, Enums.InterfaceType.HDLC);
                if (settings.client.UseLogicalNameReferencing)
                {
                    Console.WriteLine("Logical Name DLMS Server in serial port {0}.", settings.media);
                }
                else
                {
                    Console.WriteLine("Short Name DLMS Server in serial port {0}.", settings.media);
                }
                server.Initialize(settings.media, settings.trace, settings.inputFile, 1, false);
                Console.WriteLine("----------------------------------------------------------");
                ConsoleKey k;
                while ((k = Console.ReadKey().Key) != ConsoleKey.Escape)
                {
                    if (k == ConsoleKey.Delete)
                    {
                        Console.Clear();
                    }
                    Console.WriteLine("Press Esc to close application or delete clear the console.");
                }
                //Close servers.
                server.Close();
            }
            else
            {
                //Create Network media component and start listen events.
                //4059 is Official DLMS port.
                ///////////////////////////////////////////////////////////////////////
                //Create Gurux DLMS server component for Short Name and start listen events.
                List<GXDLMSMeter> servers = new List<GXDLMSMeter>();
                string str;
                if (settings.client.InterfaceType == Enums.InterfaceType.HDLC)
                {
                    str = "DLMS HDLC";
                }
                else
                {
                    str = "DLMS WRAPPER";
                }
                if (settings.client.UseLogicalNameReferencing)
                {
                    str += " Logical Name ";
                }
                else
                {
                    str += " Short Name ";
                }
                GXNet net = (GXNet)settings.media;
                net.Server = true;
                if (settings.exclusive)
                {
                    Console.WriteLine(str + "simulator start in port {0} implementing {1} meters.", net.Port, settings.serverCount);
                }
                else
                {
                    Console.WriteLine(str + "simulator start in ports {0}-{1}.", net.Port, net.Port + settings.serverCount - 1);
                }
                for (int pos = 0; pos != settings.serverCount; ++pos)
                {
                    GXDLMSMeter server = new GXDLMSMeter(settings.client.UseLogicalNameReferencing, settings.client.InterfaceType);
                    server.Conformance = Conformance.None;
                    server.MaxReceivePDUSize = 0;
                    servers.Add(server);
                    if (settings.exclusive)
                    {
                        server.Initialize(net, settings.trace, settings.inputFile, (UInt32) pos + 1, settings.exclusive);
                    }
                    else
                    {
                        try
                        {
                            server.Initialize(new GXNet(net.Protocol, net.Port + pos), settings.trace, settings.inputFile, (UInt32)pos + 1, settings.exclusive);
                        }
                        catch (System.Net.Sockets.SocketException ex)
                        {
                            Console.WriteLine(string.Format("Port {0} already in use.", net.Port + pos));
                        }
                    }
                    if (pos == 0 && settings.client.UseLogicalNameReferencing)
                    {
                        Console.WriteLine("Associations:");
                        foreach (GXDLMSAssociationLogicalName it in server.Items.GetObjects(ObjectType.AssociationLogicalName))
                        {
                            if (it.AuthenticationMechanismName.MechanismId == Authentication.None)
                            {
                                Console.WriteLine("Without authentication.");
                            }
                            else
                            {
                                Console.WriteLine("{0} authentication, password {1}",
                                    it.AuthenticationMechanismName.MechanismId,
                                    ASCIIEncoding.ASCII.GetString(it.Secret));
                            }
                        }
                    }

                }
                ConsoleKey k;
                while ((k = Console.ReadKey().Key) != ConsoleKey.Escape)
                {
                    if (k == ConsoleKey.Delete)
                    {
                        Console.Clear();
                    }
                    Console.WriteLine("Press Esc to close application or delete clear the console.");
                }
                Console.WriteLine("Closing servers.");
                //Close servers.
                foreach (GXDLMSMeter server in servers)
                {
                    server.Close();
                }
                Console.WriteLine("Servers closed.");
            }
        }
        static int Main(string[] args)
        {
            try
            {
                Settings settings = new Settings();
                int ret = Settings.GetParameters(args, settings);
                if (ret != 0)
                {
                    return ret;
                }
                if (!string.IsNullOrEmpty(settings.outputFile))
                {
                    ReadSimulatedValues(settings);
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("Simulator template is created: " + settings.outputFile);
                }
                else if (!string.IsNullOrEmpty(settings.inputFile))
                {
                    StartSimulator(settings);
                }
                else
                {
                    Console.WriteLine("Device values file is not given.");
                }
            }
            catch (System.IO.IOException ex)
            {
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Available ports:");
                Console.WriteLine(string.Join(" ", Gurux.Serial.GXSerial.GetPortNames()));
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
            return 0;
        }
    }
}
