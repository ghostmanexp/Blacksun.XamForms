﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacksunBluetooth
{
    public interface IBluetoothDevice
    {

        List<Guid> UniqueIdentifiers { get; }

        string Name { get; set; }

        string Address { get; set; }

        bool IsConnected { get; set; }

        BluetoothDeviceType Type { get; set; }

        bool ContainsUniqueIdentifier(string uniqueIdentifier);

        void SetUniqueIdentifier(string uniqueIdentifier);

        Task Connect();

        Task Connect(int port);

        Task Disconnect();

        Task Write(string message);

        Task Write(byte[] bytes);

    }
}
