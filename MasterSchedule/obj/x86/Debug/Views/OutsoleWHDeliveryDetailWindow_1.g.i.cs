﻿#pragma checksum "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow_1.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "40EADBB8B50250A999412B4C8548D0451D3DB4666311CDAF68B2D3F475DF0F03"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MasterSchedule.Views {
    
    
    /// <summary>
    /// OutsoleWHDeliveryDetailWindow_1
    /// </summary>
    public partial class OutsoleWHDeliveryDetailWindow_1 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow_1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtTitle;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow_1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgWHDeliveryDetail;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow_1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar progressBar;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow_1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblStatus;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow_1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExcelExport;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/outsolewhdeliverydetailwindow_1.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow_1.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 5 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow_1.xaml"
            ((MasterSchedule.Views.OutsoleWHDeliveryDetailWindow_1)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.dgWHDeliveryDetail = ((System.Windows.Controls.DataGrid)(target));
            
            #line 15 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow_1.xaml"
            this.dgWHDeliveryDetail.LoadingRow += new System.EventHandler<System.Windows.Controls.DataGridRowEventArgs>(this.dgWHDeliveryDetail_LoadingRow);
            
            #line default
            #line hidden
            return;
            case 4:
            this.progressBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 5:
            this.lblStatus = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.btnExcelExport = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow_1.xaml"
            this.btnExcelExport.Click += new System.Windows.RoutedEventHandler(this.btnExcelExport_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

