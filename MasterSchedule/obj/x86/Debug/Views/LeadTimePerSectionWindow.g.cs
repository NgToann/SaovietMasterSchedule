﻿#pragma checksum "..\..\..\..\Views\LeadTimePerSectionWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "79EA748F1B63E484698037B7D23D4006A16C8987DE9D909B9A2102A729A0C42B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    /// LeadTimePerSectionWindow
    /// </summary>
    public partial class LeadTimePerSectionWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpDateFrom;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpDateTo;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboSection;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem S;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem A;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem O;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem SL;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCreateChart;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/leadtimepersectionwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
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
            
            #line 5 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
            ((MasterSchedule.Views.LeadTimePerSectionWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dpDateFrom = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 3:
            this.dpDateTo = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 4:
            this.cboSection = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.S = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 6:
            this.A = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 7:
            this.O = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 8:
            this.SL = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 9:
            this.btnCreateChart = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\..\..\Views\LeadTimePerSectionWindow.xaml"
            this.btnCreateChart.Click += new System.Windows.RoutedEventHandler(this.btnCreateChart_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.reportViewer = ((Microsoft.Reporting.WinForms.ReportViewer)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

