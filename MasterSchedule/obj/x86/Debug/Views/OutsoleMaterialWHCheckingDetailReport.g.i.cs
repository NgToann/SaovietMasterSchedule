﻿#pragma checksum "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F1EE8590A41F066A5059B73E4AE6D54432BF185B7DF04DBB8006EC53899F2FE9"
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
using Microsoft.Reporting.WinForms;
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
    /// OutsoleMaterialWHCheckingReportWindow
    /// </summary>
    public partial class OutsoleMaterialWHCheckingReportWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radPO;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPoSearch;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radDateTime;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpFrom;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpTo;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Reporting.WinForms.ReportViewer reportViewer;
        
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/outsolematerialwhcheckingdetailreport.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
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
            
            #line 9 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
            ((MasterSchedule.Views.OutsoleMaterialWHCheckingReportWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.radPO = ((System.Windows.Controls.RadioButton)(target));
            
            #line 34 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
            this.radPO.Checked += new System.Windows.RoutedEventHandler(this.radPO_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtPoSearch = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.radDateTime = ((System.Windows.Controls.RadioButton)(target));
            
            #line 36 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
            this.radDateTime.Checked += new System.Windows.RoutedEventHandler(this.radDateTime_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.dpFrom = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 6:
            this.dpTo = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 7:
            this.btnSearch = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\..\..\Views\OutsoleMaterialWHCheckingDetailReport.xaml"
            this.btnSearch.Click += new System.Windows.RoutedEventHandler(this.btnSearch_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.reportViewer = ((Microsoft.Reporting.WinForms.ReportViewer)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

