using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Drawing
{
    /// <summary>
    /// Interaction logic for RotateElement.xaml
    /// </summary>

    public partial class RotateElement : System.Windows.Window
    {

        public RotateElement()
        {
            InitializeComponent();
        }

        private void SliderValue(object sender, RoutedPropertyChangedEventArgs<double> e) {
            var slider = sender as Slider;
            double value = slider.Value;
            this.RotatingdButton.RenderTransform = new RotateTransform(value, 45, 5);
        }

    }
}