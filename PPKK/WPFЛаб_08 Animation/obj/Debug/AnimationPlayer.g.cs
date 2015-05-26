﻿#pragma checksum "..\..\AnimationPlayer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0595F5C1F4FB4E4A5ACDE13C830B448F"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.34209
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
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


namespace Animation {
    
    
    /// <summary>
    /// AnimationPlayer
    /// </summary>
    public partial class AnimationPlayer : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\AnimationPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.Animation.BeginStoryboard fadeStoryboardBegin;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\AnimationPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.Animation.Storyboard fadeStoryboard;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\AnimationPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgDay;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\AnimationPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cmdStart;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\AnimationPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cmdPause;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\AnimationPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cmdResume;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\AnimationPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cmdStop;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\AnimationPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cmdMiddle;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\AnimationPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblTime;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\AnimationPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sldSpeed;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\AnimationPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar progressBar;
        
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
            System.Uri resourceLocater = new System.Uri("/Animation;component/animationplayer.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AnimationPlayer.xaml"
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
            this.fadeStoryboardBegin = ((System.Windows.Media.Animation.BeginStoryboard)(target));
            return;
            case 2:
            this.fadeStoryboard = ((System.Windows.Media.Animation.Storyboard)(target));
            
            #line 12 "..\..\AnimationPlayer.xaml"
            this.fadeStoryboard.CurrentTimeInvalidated += new System.EventHandler(this.storyboard_CurrentTimeInvalidated);
            
            #line default
            #line hidden
            return;
            case 3:
            this.imgDay = ((System.Windows.Controls.Image)(target));
            return;
            case 4:
            this.cmdStart = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.cmdPause = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.cmdResume = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.cmdStop = ((System.Windows.Controls.Button)(target));
            return;
            case 8:
            this.cmdMiddle = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.lblTime = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.sldSpeed = ((System.Windows.Controls.Slider)(target));
            
            #line 73 "..\..\AnimationPlayer.xaml"
            this.sldSpeed.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.sldSpeed_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.progressBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

