using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Content.PM;
using Android.Provider;
using BlacksunBluetooth;
using BlacksunBluetooth.Exceptions;
using BlacksunBluetoothAndroid;
using Xamarin.Forms;
using BluetoothDeviceType = Android.Bluetooth.BluetoothDeviceType;

[assembly: Dependency(typeof(AndroidBluetoothClient))]
namespace BlacksunBluetoothAndroid
{
    public class AndroidBluetoothClient : IBluetoothClient
    {

        private BluetoothAdapter btAdapter;

        public AndroidBluetoothClient()
        {
            btAdapter = BluetoothAdapter.DefaultAdapter;
        }

        public async Task<bool> IsBluetoothOn()
        {

            //CheckPermissions();

            var bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (bluetoothAdapter == null)
            {
                // Device does not support Bluetooth
                throw new Exception("Device does not support Bluetooth");
            }
            else
            {
                return bluetoothAdapter.IsEnabled;
            }
        }

        public async Task<List<IBluetoothDevice>> GetPairedDevices()
        {

           // CheckPermissions();

            Debug.WriteLine("Bluetooth Android: Getting bluetooth devices");

            var devices = new List<IBluetoothDevice>();

            try
            {
                // Get a set of currently paired devices 
                var pairedDevices = btAdapter.BondedDevices;

                if (btAdapter.BondedDevices == null)
                {
                    Debug.WriteLine("Bluetooth Android: Bonded devices are null");
                }
                else
                {
                    // If there are paired devices, add each one to the ArrayAdapter 
                    if (pairedDevices.Count > 0)
                    {

                        Debug.WriteLine("Bluetooth Devices found");

                        foreach (var paireddevice in pairedDevices)
                        {
                            Debug.WriteLine("-"+paireddevice.Name+ " "+paireddevice.Address );
                            var device = new AndroidBluetoothDevice() { Name = paireddevice.Name, Address = paireddevice.Address };
                            device.BluetoothDevice = paireddevice;
                            try
                            {
                                switch (paireddevice.Type)
                                {
                                    case global::Android.Bluetooth.BluetoothDeviceType.Classic:
                                        device.Type = BlacksunBluetooth.BluetoothDeviceType.Classic;
                                        break;
                                    case global::Android.Bluetooth.BluetoothDeviceType.Dual:
                                        device.Type = BlacksunBluetooth.BluetoothDeviceType.Dual;
                                        break;
                                    case global::Android.Bluetooth.BluetoothDeviceType.Le:
                                        device.Type = BlacksunBluetooth.BluetoothDeviceType.Le;
                                        break;
                                    case global::Android.Bluetooth.BluetoothDeviceType.Unknown:
                                        device.Type = BlacksunBluetooth.BluetoothDeviceType.Unknown;
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            try
                            {
                                Debug.WriteLine("Bluetooth getting Uuids with SDP");
                                device.BluetoothDevice.FetchUuidsWithSdp();

                                Debug.WriteLine("Bluetooth Android: Getting UUIDs");
                                var array = paireddevice.GetUuids();
                                if (array != null)
                                {
                                    var uuids = array.ToList();

                                    Debug.WriteLine("Bluetooth Android: " + uuids.Count + " found");

                                    foreach (var uuid in uuids)
                                    {
                                        Debug.WriteLine(device.Name + " Adding uuid " + uuid.Uuid.ToString());
                                        var stringUUID = uuid.ToString();
                                        device.UniqueIdentifiers.Add(Guid.Parse(stringUUID));
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine("Bluetooth Android: No Paired devices");
                                }
                                

                            }
                            catch (Java.Lang.Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                            devices.Add(device);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Bluetooth Android: No devices found");
                    }
                }

                
            }
            catch (Java.Lang.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }

            



            return devices;
        }

        public async Task<IBluetoothDevice> FindDeviceByIdentifier(string identifier)
        {

            var bluetoothClient = new AndroidBluetoothClient();
            var devices = await bluetoothClient.GetPairedDevices();

            foreach (AndroidBluetoothDevice device in devices)
            {
                if (device.ContainsUniqueIdentifier(identifier))
                {
                    device.SetUniqueIdentifier(identifier);
                    return device;
                }

            }

            return null;
        }
        /*
        private void CheckPermissions()
        {
            var context = Android.App.Application.Context;

            PackageManager pm = context.PackageManager;
            var hasPerm = pm.CheckPermission(Android.Manifest.Permission.Bluetooth,context.PackageName);
            if (hasPerm == Permission.Denied)
            {
                throw new BluetoothPermissionException("Bluetooth permision not added");
            }

            hasPerm = pm.CheckPermission(Android.Manifest.Permission.BluetoothAdmin, context.PackageName);
            if (hasPerm == Permission.Denied)
            {
                throw new BluetoothPermissionException("BluetoothAdmin permision not added");
            }


        }
        */
        
    }
}