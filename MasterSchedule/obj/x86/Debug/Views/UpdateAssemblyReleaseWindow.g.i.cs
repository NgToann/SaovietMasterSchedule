﻿#pragma checksum "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EEBA9F37497484FC2C2384587DBD6271BC78E9DFC24655286A8234900C783DAE"
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
    /// UpdateAssemblyReleaseWindow
    /// </summary>
    public partial class UpdateAssemblyReleaseWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblReportId;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer svMain;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spMain;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddMore;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExport;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRelease;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Popup popupAddMore;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtProductNo;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddMoreOk;
        
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/updateassemblyreleasewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
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
            
            #line 5 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
            ((MasterSchedule.Views.UpdateAssemblyReleaseWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lblReportId = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.svMain = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 4:
            this.spMain = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            this.btnAddMore = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
            this.btnAddMore.Click += new System.Windows.RoutedEventHandler(this.btnAddMore_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnExport = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
            this.btnExport.Click += new System.Windows.RoutedEventHandler(this.btnExport_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnRelease = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
            this.btnRelease.Click += new System.Windows.RoutedEventHandler(this.btnRelease_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.popupAddMore = ((System.Windows.Controls.Primitives.Popup)(target));
            return;
            case 9:
            this.txtProductNo = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.btnAddMoreOk = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\..\Views\UpdateAssemblyReleaseWindow.xaml"
            this.btnAddMoreOk.Click += new System.Windows.RoutedEventHandler(this.btnAddMoreOk_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

