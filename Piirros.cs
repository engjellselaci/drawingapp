using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows

namespace WpfPiirustusOhjelma
{
   class Piirros
        class Piirros
    {
        public int Muoto { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }
        public double StrokeThickness { get; set; }
        public bool Fill { get; set; }
        public Piirros()
    }
}
