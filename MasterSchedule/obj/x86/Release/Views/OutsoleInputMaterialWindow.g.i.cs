﻿#pragma checksum "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0D953899E7A1B29415A0FE1D08CFDF01"
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
    /// OutsoleInputMaterialWindow
    /// </summary>
    public partial class OutsoleInputMaterialWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 25 "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgOutsoleMaterial;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridComboBoxColumn colSuppliers;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn colETD;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn colActualDate;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colCompleted;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSave;
        
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
            System.Uri resourceLocater = new System.Uri("/MasterSchedule;component/views/outsoleinputmaterialwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml"
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
            
            #line 5 "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml"
            ((MasterSchedule.Views.OutsoleInputMaterialWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dgOutsoleMaterial = ((System.Windows.Controls.DataGrid)(target));
            
            #line 28 "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml"
            this.dgOutsoleMaterial.CellEditEnding += new System.EventHandler<System.Windows.Controls.DataGridCellEditEndingEventArgs>(this.dgOutsoleMaterial_CellEditEnding);
            
            #line default
            #line hidden
            return;
            case 3:
            this.colSuppliers = ((System.Windows.Controls.DataGridComboBoxColumn)(target));
            return;
            case 4:
            this.colETD = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 5:
            this.colActualDate = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 6:
            this.colCompleted = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 8:
            this.btnSave = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml"
            this.btnSave.Click += new System.Windows.RoutedEventHandler(this.btnSave_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 7:
            
            #line 43 "..\..\..\..\Views\OutsoleInputMaterialWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnCompleted_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

