﻿#pragma checksum "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6886851B2F71502B3712EA4A0CB1066B9B2A8EBC18F4C63DDCE5D274FD734473"
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
    /// OutsoleWHDeliveryDetailWindow
    /// </summary>
    public partial class OutsoleWHDeliveryDetailWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgInventory;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridTotal;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnShowBalanceOnly;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExcelFile;
        
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/outsolewhdeliverydetailwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow.xaml"
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
            
            #line 4 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow.xaml"
            ((MasterSchedule.Views.OutsoleWHDeliveryDetailWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dgInventory = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.gridTotal = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.btnShowBalanceOnly = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow.xaml"
            this.btnShowBalanceOnly.Click += new System.Windows.RoutedEventHandler(this.btnShowBalanceOnly_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnExcelFile = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\..\..\Views\OutsoleWHDeliveryDetailWindow.xaml"
            this.btnExcelFile.Click += new System.Windows.RoutedEventHandler(this.btnExcelFile_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
