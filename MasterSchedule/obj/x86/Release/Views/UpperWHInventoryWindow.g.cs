﻿#pragma checksum "..\..\..\..\Views\UpperWHInventoryWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B649D8F5A0574393F1892833AC826D5F"
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
    /// UpperWHInventoryWindow
    /// </summary>
    public partial class UpperWHInventoryWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Views\UpperWHInventoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgInventory;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\Views\UpperWHInventoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn Column1;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\Views\UpperWHInventoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn Column2;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Views\UpperWHInventoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn Column3;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\Views\UpperWHInventoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn Column4;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Views\UpperWHInventoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblSewingOutput;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Views\UpperWHInventoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblOutsoleOutput;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\Views\UpperWHInventoryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblMatching;
        
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/upperwhinventorywindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\UpperWHInventoryWindow.xaml"
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
            
            #line 4 "..\..\..\..\Views\UpperWHInventoryWindow.xaml"
            ((MasterSchedule.Views.UpperWHInventoryWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dgInventory = ((System.Windows.Controls.DataGrid)(target));
            
            #line 11 "..\..\..\..\Views\UpperWHInventoryWindow.xaml"
            this.dgInventory.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.dgInventory_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Column1 = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 4:
            this.Column2 = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 5:
            this.Column3 = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 6:
            this.Column4 = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 7:
            this.lblSewingOutput = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.lblOutsoleOutput = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.lblMatching = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

