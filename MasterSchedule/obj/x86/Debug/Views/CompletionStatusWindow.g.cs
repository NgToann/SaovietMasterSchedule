﻿#pragma checksum "..\..\..\..\Views\CompletionStatusWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8F2A9E64285749EE920A65940C414A9415CDDD5EB6FAA400B47FB84C8D3477D1"
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
    /// CompletionStatusWindow
    /// </summary>
    public partial class CompletionStatusWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\..\Views\CompletionStatusWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpETDStart;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Views\CompletionStatusWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpETDEnd;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Views\CompletionStatusWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnLoad;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\Views\CompletionStatusWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtArticleNo;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\Views\CompletionStatusWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblStyle;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\..\Views\CompletionStatusWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtShoeName;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\Views\CompletionStatusWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblIsFinished;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\..\Views\CompletionStatusWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chboFinished;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\Views\CompletionStatusWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chboUnfinished;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\Views\CompletionStatusWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnView;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\Views\CompletionStatusWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgMain;
        
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/completionstatuswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\CompletionStatusWindow.xaml"
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
            
            #line 4 "..\..\..\..\Views\CompletionStatusWindow.xaml"
            ((MasterSchedule.Views.CompletionStatusWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dpETDStart = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 3:
            this.dpETDEnd = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 4:
            this.btnLoad = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\..\Views\CompletionStatusWindow.xaml"
            this.btnLoad.Click += new System.Windows.RoutedEventHandler(this.btnLoad_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.txtArticleNo = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.lblStyle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.txtShoeName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.lblIsFinished = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.chboFinished = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 10:
            this.chboUnfinished = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 11:
            this.btnView = ((System.Windows.Controls.Button)(target));
            
            #line 69 "..\..\..\..\Views\CompletionStatusWindow.xaml"
            this.btnView.Click += new System.Windows.RoutedEventHandler(this.btnView_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.dgMain = ((System.Windows.Controls.DataGrid)(target));
            
            #line 80 "..\..\..\..\Views\CompletionStatusWindow.xaml"
            this.dgMain.LoadingRow += new System.EventHandler<System.Windows.Controls.DataGridRowEventArgs>(this.dgMain_LoadingRow);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

