﻿using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Blacksun.Bluetooth.Android;
using BlacksunBluetoothAndroid;
using SampleBlacksunBluetooth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace SampleBlacksunBluetoothAndroid
{
    [Activity(Label = "SampleBlacksunBluetoothDroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FormsApplicationActivity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            UserDialogs.Init(() => (Activity)Forms.Context);
            new AndroidBluetoothClient();
            Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

