﻿#pragma checksum "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "37A6FB0505310F2740F8AB7B53B638F235C2B855239BC0F62853761EA181FCCF"
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
    /// RawMaterialSearchBoxWindow
    /// </summary>
    public partial class RawMaterialSearchBoxWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtFindWhat;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cboIsMatch;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spShowHide;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbShow;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFindAll;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClose;
        
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/rawmaterialsearchboxwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
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
            
            #line 5 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
            ((MasterSchedule.Views.RawMaterialSearchBoxWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtFindWhat = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.cboIsMatch = ((System.Windows.Controls.CheckBox)(target));
            
            #line 25 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
            this.cboIsMatch.Checked += new System.Windows.RoutedEventHandler(this.cboIsMatch_Checked);
            
            #line default
            #line hidden
            
            #line 25 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
            this.cboIsMatch.Unchecked += new System.Windows.RoutedEventHandler(this.cboIsMatch_Unchecked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.spShowHide = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            this.rbShow = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 6:
            this.btnFindAll = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
            this.btnFindAll.Click += new System.Windows.RoutedEventHandler(this.btnFindAll_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnClose = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\..\Views\RawMaterialSearchBoxWindow.xaml"
            this.btnClose.Click += new System.Windows.RoutedEventHandler(this.btnClose_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

