﻿#pragma checksum "..\..\..\..\Views\ImportUpperMaterialScheduleWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5B2C6FB17129510C59586188F7ECCEB639C6D7EB736A25F9F0FC1ABED7854D41"
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
    /// ImportUpperMaterialScheduleWindow
    /// </summary>
    public partial class ImportUpperMaterialScheduleWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\Views\ImportUpperMaterialScheduleWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgUpperMaterialSchedule;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\Views\ImportUpperMaterialScheduleWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar progressBar;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\..\Views\ImportUpperMaterialScheduleWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblStatus;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\Views\ImportUpperMaterialScheduleWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnImport;
        
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/importuppermaterialschedulewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\ImportUpperMaterialScheduleWindow.xaml"
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
            
            #line 4 "..\..\..\..\Views\ImportUpperMaterialScheduleWindow.xaml"
            ((MasterSchedule.Views.ImportUpperMaterialScheduleWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dgUpperMaterialSchedule = ((System.Windows.Controls.DataGrid)(target));
            
            #line 13 "..\..\..\..\Views\ImportUpperMaterialScheduleWindow.xaml"
            this.dgUpperMaterialSchedule.LoadingRow += new System.EventHandler<System.Windows.Controls.DataGridRowEventArgs>(this.dgUpperMaterialSchedule_LoadingRow);
            
            #line default
            #line hidden
            return;
            case 3:
            this.progressBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 4:
            this.lblStatus = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.btnImport = ((System.Windows.Controls.Button)(target));
            
            #line 80 "..\..\..\..\Views\ImportUpperMaterialScheduleWindow.xaml"
            this.btnImport.Click += new System.Windows.RoutedEventHandler(this.btnImport_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

