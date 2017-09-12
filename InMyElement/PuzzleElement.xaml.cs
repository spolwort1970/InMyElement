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
using Windows.UI;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace InMyElement
{
    public sealed partial class PuzzleElement : UserControl
    {
        private Brush defaultColor;

        public PuzzleElement()
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
                return ElementText.Foreground;
            }
            set
            {
                ElementText.Foreground = value;
            }
        }

        public string Text
        {
            get
            {
                return ElementText.Text;
            }
            set
            {
                ElementText.Text = value;
            }
        }

        public string ElementName
        {
            get
            {
                return ElementText.Name;
            }
            set
            {
                ElementText.Name = value;
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
                if (defaultColor==null)
                {
                    defaultColor = value;
                }
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

        public void ResetToDefault()
        {
            if (defaultColor != null)
            {
                BorderStroke = defaultColor;
            }
            BorderStrokeThickness = 5;
            ElementBackground = new SolidColorBrush(Colors.Black);
            ElementTextColor = new SolidColorBrush(Colors.White);
        }

    }
}
