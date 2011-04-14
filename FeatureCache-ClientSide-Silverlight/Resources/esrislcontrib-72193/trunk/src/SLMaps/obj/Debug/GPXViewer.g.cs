﻿#pragma checksum "C:\Gabriel\Imagem\Projetos\FeatureCache\Resources\esrislcontrib-72193\trunk\src\SLMaps\GPXViewer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "27F54ED190D15FE25D85258B45C49A06"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Toolkit;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SLMaps {
    
    
    public partial class GPXViewer : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtGPXurl;
        
        internal System.Windows.Controls.Button btnLoadGPX;
        
        internal ESRI.ArcGIS.Client.Map MyMap;
        
        internal ESRI.ArcGIS.Client.Toolkit.MapTip MyMapTip;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/SLMaps;component/GPXViewer.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtGPXurl = ((System.Windows.Controls.TextBox)(this.FindName("txtGPXurl")));
            this.btnLoadGPX = ((System.Windows.Controls.Button)(this.FindName("btnLoadGPX")));
            this.MyMap = ((ESRI.ArcGIS.Client.Map)(this.FindName("MyMap")));
            this.MyMapTip = ((ESRI.ArcGIS.Client.Toolkit.MapTip)(this.FindName("MyMapTip")));
        }
    }
}
