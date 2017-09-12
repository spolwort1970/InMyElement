using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace InMyElement
{
    public sealed partial class StudyElement : UserControl
    {
        public StudyElement()
        {
            this.InitializeComponent();
        }

        public Brush ElementBackground
        {
            get
            {
                return ElementBox.Fill;
            }
            set
            {
                ElementBox.Fill = value;
            }
        }

        public Brush ElementTextColor
        {
            get
            {
                return ElementSymbol.Foreground;
            }
            set
            {
                ElementNumber.Foreground = value;
                ElementSymbol.Foreground = value;
            }
        }

        public string Text
        {
            get
            {
                return ElementSymbol.Text;
            }
            set
            {
                ElementSymbol.Text = value;
            }
        }

        public string Number
        {
            get
            {
                return ElementNumber.Text;
            }
            set
            {
                ElementNumber.Text = value;
            }
        }

        public string ElementName
        {
            get
            {
                return ElementSymbol.Name;
            }
            set
            {
                ElementSymbol.Name = value;
            }
        }

        public Brush BorderStroke
        {
            get
            {
                return ElementBox.Stroke;
            }
            set
            {
                ElementBox.Stroke = value;
            }
        }

        public double BorderStrokeThickness
        {
            get
            {
                return ElementBox.StrokeThickness;
            }
            set
            {
                ElementBox.StrokeThickness = value;
            }
        }
    }
}
