/*******************************************************************************\
Class: Element
Created by: Shaun Cobb
Date: 3/25/14
 * *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

namespace InMyElement.DataModel
{
    public class Element
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        [PrimaryKey]
        public int AtomicNumber { get; set; }
        public string AtomicWeight { get; set; }
        public string Density { get; set; }
        public string MeltingPoint { get; set; }
        public string BoilingPoint { get; set; }
        public string AtomicRadius { get; set; }
        public string CovalentRadius { get; set; }
        public string IonicRadius { get; set; }
        public string SpecificVolume { get; set; }
        public string SpecificHeat { get; set; }
        public string HeatFusion { get; set; }
        public string HeatEvaporation { get; set; }
        public string ThermalConductivity { get; set; }
        public string PaulingElectronnegativity { get; set; }
        public string FirstIonizationEnergy { get; set; }
        public string OxidationStates { get; set; }
        public string ElectronicConfiguration { get; set; }
        public string Lattice { get; set; }
        public string LatticeConstant { get; set; }
        private string image;
        private Brush groupColor;
        public string Image
        {
            get { return image; }
            set
            {
                image = "/Assets/Elements/" + Name + ".jpg";
            }
        }
        public Brush GroupColor
        {
            get { return groupColor; }
            set
            {
                groupColor = value;
            }
        }

        
    }
}
