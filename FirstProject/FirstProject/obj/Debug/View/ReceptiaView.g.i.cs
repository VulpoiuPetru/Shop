﻿#pragma checksum "..\..\..\View\ReceptiaView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "53D14B8B331A9B2D847057B6B156A1BF0864676BF633A182779CFBA6EFA6A152"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FirstProject.View;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace FirstProject.View {
    
    
    /// <summary>
    /// ReceptiaView
    /// </summary>
    public partial class ReceptiaView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\View\ReceptiaView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ButonRebut;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\View\ReceptiaView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ObservatiiProdus;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\View\ReceptiaView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ConfirmareButon;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\View\ReceptiaView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ProdusSelect;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\View\ReceptiaView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PersoanaSelect;
        
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
            System.Uri resourceLocater = new System.Uri("/FirstProject;component/view/receptiaview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\ReceptiaView.xaml"
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
            this.ButonRebut = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 2:
            this.ObservatiiProdus = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.ConfirmareButon = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\View\ReceptiaView.xaml"
            this.ConfirmareButon.Click += new System.Windows.RoutedEventHandler(this.Confirmare_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ProdusSelect = ((System.Windows.Controls.TextBox)(target));
            
            #line 18 "..\..\..\View\ReceptiaView.xaml"
            this.ProdusSelect.KeyDown += new System.Windows.Input.KeyEventHandler(this.ProdusSelect_KeyDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.PersoanaSelect = ((System.Windows.Controls.TextBox)(target));
            
            #line 19 "..\..\..\View\ReceptiaView.xaml"
            this.PersoanaSelect.KeyDown += new System.Windows.Input.KeyEventHandler(this.PersoanaSelect_KeyDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

