﻿#pragma checksum "..\..\LocalizableText.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "07D0707F70D09D05B07DB4922C346595"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
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


namespace LayoutPanels {
    
    
    /// <summary>
    /// LocalizableText
    /// </summary>
    public partial class LocalizableText : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\LocalizableText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cmdPrev;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\LocalizableText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cmdNext;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\LocalizableText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkLongText;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\LocalizableText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cmdClose;
        
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
            System.Uri resourceLocater = new System.Uri("/LayoutPanels;component/localizabletext.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\LocalizableText.xaml"
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
            this.cmdPrev = ((System.Windows.Controls.Button)(target));
            return;
            case 2:
            this.cmdNext = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.chkLongText = ((System.Windows.Controls.CheckBox)(target));
            
            #line 18 "..\..\LocalizableText.xaml"
            this.chkLongText.Checked += new System.Windows.RoutedEventHandler(this.chkLongText_Checked);
            
            #line default
            #line hidden
            
            #line 18 "..\..\LocalizableText.xaml"
            this.chkLongText.Unchecked += new System.Windows.RoutedEventHandler(this.chkLongText_Unchecked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.cmdClose = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

