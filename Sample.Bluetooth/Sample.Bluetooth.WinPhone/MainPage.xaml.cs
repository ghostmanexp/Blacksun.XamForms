﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Acr.XamForms.UserDialogs.WindowsPhone;
using Blacksun.Bluetooth.Winphone;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Xamarin.Forms;


namespace Sample.Bluetooth.WinPhone
{
    public partial class MainPage 
    {
        public MainPage()
        {
            InitializeComponent();

            Forms.Init();
            new WP8BluetoothClient();
            new UserDialogService();
            LoadApplication(new Bluetooth.App());
        }
    }
}
