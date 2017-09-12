using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

namespace InMyElement.DataModel
{
    static class GroupColorMatching
    {
        static public readonly Brush ActinoidBrush = new SolidColorBrush(Colors.Violet);
        static public readonly Brush AlkaliMetalBrush = new SolidColorBrush(Colors.Orange);
        static public readonly Brush AlkalineEarthBrush = new SolidColorBrush(Colors.Yellow);
        static public readonly Brush HalogenBrush = new SolidColorBrush(Colors.Aquamarine);
        static public readonly Brush LanthanoidBrush = new SolidColorBrush(Colors.PaleGoldenrod);
        static public readonly Brush MetalloidBrush = new SolidColorBrush(Colors.MediumSeaGreen);
        static public readonly Brush NobleBrush = new SolidColorBrush(Colors.DeepSkyBlue);
        static public readonly Brush NonMetalBrush = new SolidColorBrush(Colors.LightGreen);
        static public readonly Brush PostTransitionMetalBrush = new SolidColorBrush(Colors.Green);
        static public readonly Brush TransitionMetalBrush = new SolidColorBrush(Colors.PaleVioletRed);
        static public readonly Brush Default = new SolidColorBrush(Color.FromArgb(255, 237, 28, 36));

        static public void GetColor(Element element)
        {
            if (GroupMatching.Actinoid.Contains(element.AtomicNumber))
            {
                element.GroupColor = ActinoidBrush;
            }
            else if (GroupMatching.AlkaliMetal.Contains(element.AtomicNumber))
            {
                element.GroupColor = AlkaliMetalBrush;
            }
            else if (GroupMatching.AlkalineEarth.Contains(element.AtomicNumber))
            {
                element.GroupColor = AlkalineEarthBrush;
            }
            else if (GroupMatching.Halogen.Contains(element.AtomicNumber))
            {
                element.GroupColor = HalogenBrush;
            }
            else if (GroupMatching.Lanthanoid.Contains(element.AtomicNumber))
            {
                element.GroupColor = LanthanoidBrush;
            }
            else if (GroupMatching.Metalloid.Contains(element.AtomicNumber))
            {
                element.GroupColor = MetalloidBrush;
            }
            else if (GroupMatching.Noble.Contains(element.AtomicNumber))
            {
                element.GroupColor = NobleBrush;
            }
            else if (GroupMatching.NonMetal.Contains(element.AtomicNumber))
            {
                element.GroupColor = NonMetalBrush;
            }
            else if (GroupMatching.PostTransitionMetal.Contains(element.AtomicNumber))
            {
                element.GroupColor = PostTransitionMetalBrush;
            }
            else if (GroupMatching.TransitionMetal.Contains(element.AtomicNumber))
            {
                element.GroupColor = TransitionMetalBrush;
            }
            else
            {
            }
        }
    }
}
