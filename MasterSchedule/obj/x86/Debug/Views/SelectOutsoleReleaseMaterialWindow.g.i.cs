﻿#pragma checksum "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "40653E5E811ABA110DA10B663D5441CF1CBD554F2CAF7656228FB2BB42F223E2"
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
    /// SelectOutsoleReleaseMaterialWindow
    /// </summary>
    public partial class SelectOutsoleReleaseMaterialWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.AutoCompleteBox txtReportId;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearchExpand;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridSearch;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.AutoCompleteBox txtProductNo;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvReportId;
        
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/selectoutsolereleasematerialwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
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
            
            #line 6 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
            ((MasterSchedule.Views.SelectOutsoleReleaseMaterialWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtReportId = ((System.Windows.Controls.AutoCompleteBox)(target));
            return;
            case 3:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.btnOk_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnSearchExpand = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
            this.btnSearchExpand.Click += new System.Windows.RoutedEventHandler(this.btnSearchExpand_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.gridSearch = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.txtProductNo = ((System.Windows.Controls.AutoCompleteBox)(target));
            return;
            case 7:
            this.btnSearch = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
            this.btnSearch.Click += new System.Windows.RoutedEventHandler(this.btnSearch_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lvReportId = ((System.Windows.Controls.ListView)(target));
            
            #line 36 "..\..\..\..\Views\SelectOutsoleReleaseMaterialWindow.xaml"
            this.lvReportId.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.lvReportId_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

