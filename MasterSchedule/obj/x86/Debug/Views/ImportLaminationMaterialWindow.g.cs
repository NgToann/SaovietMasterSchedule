﻿#pragma checksum "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "845B91CF2D874CD3FFDE9D2C5F86B6C602E8CB687EB153CED12F8887CE195E1A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MasterSchedule.Views;
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
    /// ImportLaminationMaterialWindow
    /// </summary>
    public partial class ImportLaminationMaterialWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOpenExcel;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtOrderNo;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFilter;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgLaminationMaterial;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar prgStatus;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtStatus;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/importlaminationmaterialwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
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
            this.btnOpenExcel = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
            this.btnOpenExcel.Click += new System.Windows.RoutedEventHandler(this.btnOpenExcel_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtOrderNo = ((System.Windows.Controls.TextBox)(target));
            
            #line 29 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
            this.txtOrderNo.PreviewKeyUp += new System.Windows.Input.KeyEventHandler(this.txtOrderNo_PreviewKeyUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnFilter = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
            this.btnFilter.Click += new System.Windows.RoutedEventHandler(this.btnFilter_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.dgLaminationMaterial = ((System.Windows.Controls.DataGrid)(target));
            
            #line 38 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
            this.dgLaminationMaterial.LoadingRow += new System.EventHandler<System.Windows.Controls.DataGridRowEventArgs>(this.dgLaminationMaterial_LoadingRow);
            
            #line default
            #line hidden
            return;
            case 5:
            this.prgStatus = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 6:
            this.txtStatus = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.btnImport = ((System.Windows.Controls.Button)(target));
            
            #line 89 "..\..\..\..\Views\ImportLaminationMaterialWindow.xaml"
            this.btnImport.Click += new System.Windows.RoutedEventHandler(this.btnImport_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

