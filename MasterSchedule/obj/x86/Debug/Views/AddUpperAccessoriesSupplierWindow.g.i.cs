﻿#pragma checksum "..\..\..\..\Views\AddUpperAccessoriesSupplierWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9BA81B1CBB9F6D74E91833AC617443E933555B5B0B8EAFCCE348715D887FD722"
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
    /// AddUpperAccessoriesSupplierWindow
    /// </summary>
    public partial class AddUpperAccessoriesSupplierWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\..\Views\AddUpperAccessoriesSupplierWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border brAddNew;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Views\AddUpperAccessoriesSupplierWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSupplierName;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Views\AddUpperAccessoriesSupplierWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtAccessoriesName;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\Views\AddUpperAccessoriesSupplierWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAdd;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\Views\AddUpperAccessoriesSupplierWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgSuppliers;
        
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/addupperaccessoriessupplierwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\AddUpperAccessoriesSupplierWindow.xaml"
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
            
            #line 7 "..\..\..\..\Views\AddUpperAccessoriesSupplierWindow.xaml"
            ((MasterSchedule.Views.AddUpperAccessoriesSupplierWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.brAddNew = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.txtSupplierName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txtAccessoriesName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.btnAdd = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\..\Views\AddUpperAccessoriesSupplierWindow.xaml"
            this.btnAdd.Click += new System.Windows.RoutedEventHandler(this.btnAdd_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.dgSuppliers = ((System.Windows.Controls.DataGrid)(target));
            
            #line 51 "..\..\..\..\Views\AddUpperAccessoriesSupplierWindow.xaml"
            this.dgSuppliers.LoadingRow += new System.EventHandler<System.Windows.Controls.DataGridRowEventArgs>(this.dgSuppliers_LoadingRow);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

