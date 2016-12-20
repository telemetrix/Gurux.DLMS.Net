//
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

using System;
using System.Collections.Generic;
using System.Text;
using Gurux.DLMS;
using System.ComponentModel;
using System.Xml.Serialization;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Internal;
using Gurux.DLMS.Enums;

namespace Gurux.DLMS.Objects
{
    public class GXDLMSMBusClient : GXDLMSObject, IGXDLMSBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSMBusClient()
        : base(ObjectType.MBusClient)
        {
            CaptureDefinition = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ln">Logical Name of the object.</param>
        public GXDLMSMBusClient(string ln)
        : base(ObjectType.MBusClient, ln, 0)
        {
            CaptureDefinition = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ln">Logical Name of the object.</param>
        /// <param name="sn">Short Name of the object.</param>
        public GXDLMSMBusClient(string ln, ushort sn)
        : base(ObjectType.MBusClient, ln, sn)
        {
            CaptureDefinition = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Provides reference to an �M-Bus master port setup� object, used to configure
        /// an M-Bus port, each interface allowing to exchange data with one or more
        /// M-Bus slave devices
        /// </summary>
        [XmlIgnore()]
        public string MBusPortReference
        {
            get;
            set;
        }

        [XmlIgnore()]
        public List<KeyValuePair<string, string>> CaptureDefinition
        {
            get;
            set;
        }

        [XmlIgnore()]
        public UInt32 CapturePeriod
        {
            get;
            set;
        }

        [XmlIgnore()]
        public int PrimaryAddress
        {
            get;
            set;
        }

        [XmlIgnore()]
        public UInt32 IdentificationNumber
        {
            get;
            set;
        }

        [XmlIgnore()]
        public UInt16 ManufacturerID
        {
            get;
            set;
        }

        /// <summary>
        /// Carries the Version element of the data header as specified in
        /// EN 13757-3 sub-clause 5.6.
        /// </summary>
        [XmlIgnore()]
        public int DataHeaderVersion
        {
            get;
            set;
        }

        [XmlIgnore()]
        public int DeviceType
        {
            get;
            set;
        }

        [XmlIgnore()]
        public int AccessNumber
        {
            get;
            set;
        }

        [XmlIgnore()]
        public int Status
        {
            get;
            set;
        }

        [XmlIgnore()]
        public int Alarm
        {
            get;
            set;
        }

        [XmlIgnore()]
        public UInt16 Configuration
        {
            get;
            set;
        }

        [XmlIgnore()]
        public int EncryptionKeyStatus
        {
            get;
            set;
        }

        /// <inheritdoc cref="GXDLMSObject.GetValues"/>
        public override object[] GetValues()
        {
            return new object[] { LogicalName, MBusPortReference, CaptureDefinition, CapturePeriod,
                              PrimaryAddress, IdentificationNumber, ManufacturerID, DataHeaderVersion, DeviceType, AccessNumber,
                              Status, Alarm
                            };
        }

        #region IGXDLMSBase Members

        /// <summary>
        /// Data interface do not have any methods.
        /// </summary>
        /// <param name="index"></param>
        byte[] IGXDLMSBase.Invoke(GXDLMSSettings settings, ValueEventArgs e)
        {
            e.Error = ErrorCode.ReadWriteDenied;
            return null;
        }

        int[] IGXDLMSBase.GetAttributeIndexToRead()
        {
            List<int> attributes = new List<int>();
            //LN is static and read only once.
            if (string.IsNullOrEmpty(LogicalName))
            {
                attributes.Add(1);
            }
            //MBusPortReference
            if (CanRead(2))
            {
                attributes.Add(2);
            }
            //CaptureDefinition
            if (CanRead(3))
            {
                attributes.Add(3);
            }
            //CapturePeriod
            if (CanRead(4))
            {
                attributes.Add(4);
            }
            //PrimaryAddress
            if (CanRead(5))
            {
                attributes.Add(5);
            }
            //IdentificationNumber
            if (CanRead(6))
            {
                attributes.Add(6);
            }
            //ManufacturerID
            if (CanRead(7))
            {
                attributes.Add(7);
            }
            //Version
            if (CanRead(8))
            {
                attributes.Add(8);
            }
            //DeviceType
            if (CanRead(9))
            {
                attributes.Add(9);
            }
            //AccessNumber
            if (CanRead(10))
            {
                attributes.Add(10);
            }
            //Status
            if (CanRead(11))
            {
                attributes.Add(11);
            }
            //Alarm
            if (CanRead(12))
            {
                attributes.Add(12);
            }
            if (Version > 0)
            {
                //Configuration
                if (CanRead(12))
                {
                    attributes.Add(12);
                }
                //EncryptionKeyStatus
                if (CanRead(12))
                {
                    attributes.Add(12);
                }
            }
            return attributes.ToArray();
        }

        /// <inheritdoc cref="IGXDLMSBase.GetNames"/>
        string[] IGXDLMSBase.GetNames()
        {
            if (Version == 0)
            {
                return new string[] {Gurux.DLMS.Properties.Resources.LogicalNameTxt, "MBus Port Reference",
                                 "Capture Definition", "Capture Period", "Primary Address", "Identification Number",
                                 "Manufacturer ID", "Version", "Device Type", "Access Number", "Status", "Alarm"
                                };
            }
            return new string[] {Gurux.DLMS.Properties.Resources.LogicalNameTxt, "MBus Port Reference",
                             "Capture Definition", "Capture Period", "Primary Address", "Identification Number",
                             "Manufacturer ID", "Version", "Device Type", "Access Number", "Status", "Alarm",
                             "Configuration", "Encryption Key Status"
                            };
        }

        int IGXDLMSBase.GetAttributeCount()
        {
            if (Version == 0)
            {
                return 12;
            }
            return 14;

        }

        int IGXDLMSBase.GetMethodCount()
        {
            return 8;
        }

        public override DataType GetDataType(int index)
        {
            if (index == 1)
            {
                return DataType.OctetString;
            }
            if (index == 2)
            {
                return DataType.OctetString;
            }
            if (index == 3)
            {
                return DataType.Array;
            }
            if (index == 4)
            {
                return DataType.UInt32;
            }
            if (index == 5)
            {
                return DataType.UInt8;
            }
            if (index == 6)
            {
                return DataType.UInt32;
            }
            if (index == 7)
            {
                return DataType.UInt16;
            }
            if (index == 8)
            {
                return DataType.UInt8;
            }
            if (index == 9)
            {
                return DataType.UInt8;
            }
            if (index == 10)
            {
                return DataType.UInt8;
            }
            if (index == 11)
            {
                return DataType.UInt8;
            }
            if (index == 12)
            {
                return DataType.UInt8;
            }
            if (Version > 0)
            {
                if (index == 13)
                {
                    return DataType.UInt16;
                }
                if (index == 14)
                {
                    return DataType.Enum;
                }
            }
            throw new ArgumentException("GetDataType failed. Invalid attribute index.");
        }

        object IGXDLMSBase.GetValue(GXDLMSSettings settings, ValueEventArgs e)
        {
            if (e.Index == 1)
            {
                return this.LogicalName;
            }
            if (e.Index == 2)
            {
                return MBusPortReference;
            }
            if (e.Index == 3)
            {
                GXByteBuffer buff = new GXByteBuffer();
                buff.Add((byte)DataType.Array);
                GXCommon.SetObjectCount(CaptureDefinition.Count, buff);
                foreach (KeyValuePair<string, string> it in CaptureDefinition)
                {
                    buff.Add((byte)DataType.Structure);
                    buff.Add(2);
                    GXCommon.SetData(settings, buff, DataType.UInt8, it.Key);
                    GXCommon.SetData(settings, buff, DataType.OctetString, ASCIIEncoding.ASCII.GetBytes(it.Value));
                }
                return buff.Array();
            }
            if (e.Index == 4)
            {
                return CapturePeriod;
            }
            if (e.Index == 5)
            {
                return PrimaryAddress;
            }
            if (e.Index == 6)
            {
                return IdentificationNumber;
            }
            if (e.Index == 7)
            {
                return ManufacturerID;
            }
            if (e.Index == 8)
            {
                return DataHeaderVersion;
            }
            if (e.Index == 9)
            {
                return DeviceType;
            }
            if (e.Index == 10)
            {
                return AccessNumber;
            }
            if (e.Index == 11)
            {
                return Status;
            }
            if (e.Index == 12)
            {
                return Alarm;
            }
            if (Version > 0)
            {
                if (e.Index == 13)
                {
                    return Configuration;
                }
                if (e.Index == 14)
                {
                    return EncryptionKeyStatus;
                }
            }
            e.Error = ErrorCode.ReadWriteDenied;
            return null;
        }

        void IGXDLMSBase.SetValue(GXDLMSSettings settings, ValueEventArgs e)
        {
            if (e.Index == 1)
            {
                if (e.Value is string)
                {
                    LogicalName = e.Value.ToString();
                }
                else
                {
                    LogicalName = GXDLMSClient.ChangeType((byte[])e.Value, DataType.OctetString).ToString();
                }
            }
            else if (e.Index == 2)
            {
                if (e.Value is string)
                {
                    MBusPortReference = e.Value.ToString();
                }
                else
                {
                    MBusPortReference = GXDLMSClient.ChangeType((byte[])e.Value, DataType.OctetString).ToString();
                }
            }
            else if (e.Index == 3)
            {
                CaptureDefinition.Clear();
                if (e.Value != null)
                {
                    foreach (object[] it in (object[])e.Value)
                    {
                        CaptureDefinition.Add(new KeyValuePair<string, string>(GXDLMSClient.ChangeType((byte[])it[0], DataType.OctetString).ToString(),
                                              GXDLMSClient.ChangeType((byte[])it[1], DataType.OctetString).ToString()));
                    }
                }
            }
            else if (e.Index == 4)
            {
                CapturePeriod = Convert.ToUInt32(e.Value);
            }
            else if (e.Index == 5)
            {
                PrimaryAddress = Convert.ToByte(e.Value);
            }
            else if (e.Index == 6)
            {
                IdentificationNumber = Convert.ToUInt32(e.Value);
            }
            else if (e.Index == 7)
            {
                ManufacturerID = Convert.ToUInt16(e.Value);
            }
            else if (e.Index == 8)
            {
                DataHeaderVersion = Convert.ToByte(e.Value);
            }
            else if (e.Index == 9)
            {
                DeviceType = Convert.ToByte(e.Value);
            }
            else if (e.Index == 10)
            {
                AccessNumber = Convert.ToByte(e.Value);
            }
            else if (e.Index == 11)
            {
                Status = Convert.ToByte(e.Value);
            }
            else if (e.Index == 12)
            {
                Alarm = Convert.ToByte(e.Value);
            }
            else if (Version > 0)
            {
                if (e.Index == 13)
                {
                    Configuration = Convert.ToUInt16(e.Value);
                }
                else if (e.Index == 14)
                {
                    EncryptionKeyStatus = Convert.ToInt32(e.Value);
                }
                else
                {
                    e.Error = ErrorCode.ReadWriteDenied;
                }
            }
            else
            {
                e.Error = ErrorCode.ReadWriteDenied;
            }
        }
        #endregion
    }
}
