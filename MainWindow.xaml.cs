using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfPiirustusOhjelma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private double x;
        private double y;
        private bool piirtotila = false;
        private Color vari = Colors.Blue;
        private Brush sivellin;
        private int paksuus;
        private int muoto = 1;
        private Ellipse ellipsi;
        private Rectangle suorakaide;
        private Piirros piirros;
        private List<Piirros> piirustukset = new List<Piirros>();
       
        public MainWindow()
        {
            InitializeComponent();
            sivellin = new SolidColorBrush(vari);
            paksuus = 1;

            for(int i = 1; i < 50; i++)
            {
                MenuItem menuitem = new MenuItem();
                menuitem.Header = i.ToString();
                menuitem.Click += new RoutedEventHandler(KynaPaksuus);
                this.MenuItemPaksuus.Items.Add(menuitem);
                this.ComboBoxPaksuudet.Items.Add(i);
            }
            this.ComboBoxPaksuudet.SelectedIndex = 0;

            this.ComboBoxMuodot.Items.Add("Viiva");
            this.ComboBoxMuodot.Items.Add("Ellipsi");
            this.ComboBoxMuodot.Items.Add("Suorakaide");
            this.ComboBoxMuodot.Items.Add("Ellipsejä");
            this.ComboBoxMuodot.Items.Add("Täytetty ellipsi");
        }

        private void KynaPaksuus(object sender, RoutedEventArgs e)
        {
            foreach(MenuItem item in MenuItemPaksuus.Items)
            {
                item.IsChecked = false;
            }
            (sender as MenuItem).IsChecked = true;
            int i = 0;
            int valittu = 0;
            foreach (MenuItem item in MenuItemPaksuus.Items)
            {
                if (item.IsChecked)
                {
                    valittu = i;
                }
                item.IsChecked = false;
                i++;
            }
            this.ComboBoxPaksuudet.SelectedIndex = valittu;
            paksuus = int.Parse((sender as MenuItem).Header.ToString());
        }

        private void GridPiirto_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!piirtotila)
            {
                piirtotila = true;
                x = e.GetPosition(this.GridPiirto).X;
                y = e.GetPosition(this.GridPiirto).Y;
             
            }

        }
        private void GridPiirto_MouseMove(object sender, MouseEventArgs e)
        {
            if (piirtotila)
            {
                switch(muoto)
                {
                    case 1:
                 Line viiva = new Line();
                viiva.StrokeThickness = paksuus;
                viiva.Stroke = sivellin;
                viiva.X1 = x;
                viiva.Y1 = y;
                viiva.X2 = e.GetPosition(this.GridPiirto).X;
                viiva.Y2 = e.GetPosition(this.GridPiirto).Y;
                this.GridPiirto.Children.Add(viiva);
                piirros = new Piirros;
                piirros.Muoto = 1;
                piirros.R = vari.R;
                piirros.G = vari.G;
                piirros.B = vari.B;
                piirros.A = vari.A;
                piirros.StrokeThickness = paksuus;
                piirros.X = x;
                piirros.Y = y;
                piirros.X2 = e.GetPosition(this.GridPiirto).X;
                piirros.Y2 = e.GetPosition(this.GridPiirto).Y;
                piirros.Fill = false;
                piirustukset.Add(piirros);
                x = e.GetPosition(this.GridPiirto).X;
                y = e.GetPosition(this.GridPiirto).Y;
                        break;
                    case 2:
                        if (ellipsi == null)
                        {
                            ellipsi = new Ellipse();
                            ellipsi.Stroke = sivellin;
                            ellipsi.HorizontalAlignment = HorizontalAlignment.Left;
                            ellipsi.VerticalAlignment = VerticalAlignment.Top;
                            ellipsi.StrokeThickness = paksuus;
                        }
                        if (x > e.GetPosition(this.GridPiirto).X)
                        {
                            if (y > e.GetPosition(this.GridPiirto).Y)
                            {
                                ellipsi.Margin = new Thickness(e.GetPosition(this.GridPiirto).X, e.GetPosition(this.GridPiirto).Y, 0, 0);   
                            }
                            else
                            {
                                ellipsi.Margin = new Thickness(e.GetPosition(this.GridPiirto).X, y, 0, 0);
                            }
                        }
                        else
                        {
                            if (y > e.GetPosition(this.GridPiirto).Y)
                            {
                                ellipsi.Margin = new Thickness(x, e.GetPosition(this.GridPiirto).Y, 0, 0);
                            }
                            else
                            {
                                ellipsi.Margin = new Thickness(x, y, 0, 0);
                            }
                        }
                        ellipsi.Width = Math.Abs(x - e.GetPosition(this.GridPiirto).X);
                        ellipsi.Height = Math.Abs(y - e.GetPosition(this.GridPiirto).Y);
                            try
                        {
                            this.GridPiirto.Children.Add(ellipsi);
                        }
                        catch { }
                        break;
                    case 3:
                        if (suorakaide == null)
                        {
                            suorakaide = new Rectangle();
                            suorakaide.Stroke = sivellin;
                            suorakaide.HorizontalAlignment = HorizontalAlignment.Left;
                            suorakaide.VerticalAlignment = VerticalAlignment.Top;
                            suorakaide.StrokeThickness = paksuus;
                        }
                        if (x > e.GetPosition(this.GridPiirto).X)
                        {
                            if (y > e.GetPosition(this.GridPiirto).Y)
                            {
                                suorakaide.Margin = new Thickness(e.GetPosition(this.GridPiirto).X, e.GetPosition(this.GridPiirto).Y, 0, 0);
                          }
                            else
                            {
                                suorakaide.Margin = new Thickness(e.GetPosition(this.GridPiirto).X, y, 0, 0);
                            }
                        }
                        else
                        {
                            if (y > e.GetPosition(this.GridPiirto).Y)
                            {
                                suorakaide.Margin = new Thickness(x, e.GetPosition(this.GridPiirto).Y, 0, 0);
                            }
                            else
                            {
                                suorakaide.Margin = new Thickness(x, y, 0, 0);
                            }
                        }
                        suorakaide.Width = Math.Abs(x - e.GetPosition(this.GridPiirto).X);
                        suorakaide.Height = Math.Abs(y - e.GetPosition(this.GridPiirto).Y);
                        try
                        {
                            this.GridPiirto.Children.Add(suorakaide);
                                }
                        catch { }
                        break;
                    case 4:
                        ellipsi = new Ellipse();
                        ellipsi.Stroke = sivellin;
                        ellipsi.HorizontalAlignment = HorizontalAlignment.Left;
                        ellipsi.VerticalAlignment = VerticalAlignment.Top;
                        ellipsi.StrokeThickness = paksuus;

                        if (x > e.GetPosition(this.GridPiirto).X)
                        {
                            if (y > e.GetPosition(this.GridPiirto).Y)
                            {
                                ellipsi.Margin = new Thickness(e.GetPosition(this.GridPiirto).X, e.GetPosition(this.GridPiirto).Y, 0, 0);
                            }
                            else
                            {
                                ellipsi.Margin = new Thickness(e.GetPosition(this.GridPiirto).X, y, 0, 0);
                            }
                        }
                        else
                        {
                            if (y > e.GetPosition(this.GridPiirto).Y)
                            {
                                ellipsi.Margin = new Thickness(x, e.GetPosition(this.GridPiirto).Y, 0, 0);
                            }
                            else
                            {
                                ellipsi.Margin = new Thickness(x, y, 0, 0);
                            }
                            
                        }
                        ellipsi.Width = Math.Abs(x - e.GetPosition(this.GridPiirto).X);
                        ellipsi.Height = Math.Abs(y - e.GetPosition(this.GridPiirto).Y);
                        this.GridPiirto.Children.Add(ellipsi);
                        piirros = new Piirros();
                        piirros.Muoto = 4;
                        piirros.R = vari.R;
                        piirros.G = vari.G;
                        piirros.B = vari.B;
                        piirros.A = vari.A;
                        piirros.StrokeThickness = paksuus;
                        piirros.X = x;
                        piirros.Y = y;
                        piirros.Width = ellipsi.Width;
                        piirros.Height = ellipsi.Height;
                        piirros.Fill = false;
                        piirustukset.Add(piirros);
                        break;
                    case 5:
                        if (ellipsi == null)
                        {
                            ellipsi = new Ellipse();
                            ellipsi.Stroke = sivellin;
                            ellipsi.Fill = sivellin;
                            ellipsi.HorizontalAlignment = HorizontalAlignment.Left;
                            ellipsi.VerticalAlignment = VerticalAlignment.Top;
                            ellipsi.StrokeThickness = paksuus;
                        }
                        if (x > e.GetPosition(this.GridPiirto).X)
                        {
                            if (y > e.GetPosition(this.GridPiirto).Y)
                            {
                                ellipsi.Margin = new Thickness(e.GetPosition(this.GridPiirto).X, e.GetPosition(this.GridPiirto).Y, 0, 0);
                            }
                            else
                            {
                                ellipsi.Margin = new Thickness(e.GetPosition(this.GridPiirto).X, y, 0, 0);
                            }
                        }
                        else
                        {
                            if (y > e.GetPosition(this.GridPiirto).Y)
                            {
                                ellipsi.Margin = new Thickness(x, e.GetPosition(this.GridPiirto).Y, 0, 0);
                            }
                            else
                            {
                                ellipsi.Margin = new Thickness(x, y, 0, 0);
                            }
                        }
                        ellipsi.Width = Math.Abs(x - e.GetPosition(this.GridPiirto).X);
                        ellipsi.Height = Math.Abs(x - e.GetPosition(this.GridPiirto).Y);
                        try
                        {
                            this.GridPiirto.Children.Add(ellipsi);
                        }
                        catch { }
                        break;
            }
               
            }
        }
        private void GridPiirto_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (muoto)
            {
                case 2:
                    piirros = new Piirros();
                    piirros.Muoto = 2;
                    piirros.R = vari.R;
                    piirros.G = vari.G;
                    piirros.B = vari.B;
                    piirros.A = vari.A;
                    piirros.StrokeThickness = paksuus;
                    piirros.X = x;
                    piirros.Y = y;
                    piirros.Width = ellipsi.Width;
                    piirros.Height = ellipsi.Height;
                    piirros.Fill = false;
                    piirustukset.Add(piirros);
                    //this.GridPiirto.Children.Add(ellipsi);
                    ellipsi = null;
                    break;
                case 3:
                    piirros = new Piirros();
                    piirros.Muoto = 3;
                    piirros.R = vari.R;
                    piirros.G = vari.G;
                    piirros.B = vari.B;
                    piirros.A = vari.A;
                    piirros.StrokeThickness = paksuus;
                    piirros.X = x;
                    piirros.Y = y;
                    piirros.Width = suorakaide.Width;
                    piirros.Height = suorakaide.Height;
                    piirros.Fill = false;
                    piirustukset.Add(piirros);
                    //this.GridPiirto.Children.Add(suorakaide);
                    suorakaide = null;
                    break;
                case 4:
                    break;
                case 5:
                    piirros = new Piirros();
                    piirros.Muoto = 5;
                    piirros.R = vari.R;
                    piirros.G = vari.G;
                    piirros.B = vari.B;
                    piirros.A = vari.A;
                    piirros.StrokeThickness = paksuus;
                    piirros.X = x;
                    piirros.Y = y;
                    piirros.Width = ellipsi.Width;
                    piirros.Height = ellipsi.Height;
                    piirros.Fill = true;
                    piirustukset.Add(piirros);
                    //this.GridPiirto.Children.Add(ellipsi);
                    ellipsi = null;
                    break;
            }
            piirtotila = false;

        }

        private void MenuItemUusi_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemAvaa_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemTalleta_Click(object sender, RoutedEventArgs e)
        {
            string tiedostonnimi;
            System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML-tiedosto (.xml)|*.xml";
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tiedostonnimi = dlg.FileName;
                this.Title = tiedostonnimi;
            }
            else
            {
                return;
            }
            XDocument xdoc = new XDocument();

            XElement xeRoot = new XElement("Data");
            XElement xeSubRoot = new XElement("Rows");
            
            foreach(var item in piirustukset)
            {
                Piirros pii = (Piirros)item;

                XElement xRow = new XElement("Row");
                xRow.Add(new XElement("Muoto", pii.Muoto));
                xRow.Add(new XElement("X", pii.X));
                xRow.Add(new XElement("Y", pii.Y));
                xRow.Add(new XElement("X2", pii.X2));
                xRow.Add(new XElement("Y2", pii.Y2));
                xRow.Add(new XElement("Width", pii.Width));
                xRow.Add(new XElement("Height", pii.Height));
                xRow.Add(new XElement("R", pii.R));
                xRow.Add(new XElement("G", pii.G));
                xRow.Add(new XElement("B", pii.B));
                xRow.Add(new XElement("A", pii.A));
                xRow.Add(new XElement("StrokeThickness", pii.StrokeThickness));
                xRow.Add(new XElement("Fill", pii.Fill));
                xeSubRoot.Add(xRow);
            }
            xeRoot.Add(xeSubRoot);
            xdoc.Add(xeRoot);

            xdoc.Save(tiedostonnimi);
        }

        private void MenuItemSulje_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemVari_Click(object sender, RoutedEventArgs e)
        {
            VaihdaVari();
        }
        private void VaihdaVari()
        {
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
            dlg.Color = System.Drawing.Color.FromArgb(vari.A, vari.R, vari.G, vari.B);

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                vari.R = dlg.Color.R;
                vari.G = dlg.Color.G;
                vari.B = dlg.Color.B;
                vari.A = dlg.Color.A;
                sivellin = new SolidColorBrush(vari);
                MenuItemSuorakaide = null;
            }
        }

        private void MenuItemPaksuus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemMuoto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemViiva_Click(object sender, RoutedEventArgs e)
        {
            muoto = 1;
        }

        private void MenuItemEllipsi_Click(object sender, RoutedEventArgs e)
        {
            muoto = 2;
            ellipsi = null;
        }

        private void MenuItemSuorakaide_Click(object sender, RoutedEventArgs e)
        {
            muoto = 3;
            suorakaide = null;
        }

        private void MenuItemEllipsit_Click(object sender, RoutedEventArgs e)
        {
            muoto = 4;
        }

        private void MenuItemTaytettyEllipsi_Click(object sender, RoutedEventArgs e)
        {
            muoto = 5;
            ellipsi = null;
        }

        private void ButtonVari_Click(object sender, RoutedEventArgs e)
        {
            VaihdaVari();
        }

        private void ComboBoxPaksuudet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VaihdaPaksuus();
            int laskuri = 0;
            foreach (MenuItem item in this.MenuItemPaksuus.Items)
            {
                if (item.IsChecked)
                {
                    item.IsChecked = false;
                }
                if (laskuri == int.Parse(this.ComboBoxPaksuudet.SelectedValue.ToString()) - 1)
                {
                    item.IsChecked = true;
                }
                laskuri++;
            }
        }
        private void VaihdaPaksuus()
        {
            paksuus = int.Parse(this.ComboBoxPaksuudet.SelectedValue.ToString());
        }

        private void ComboBoxMuodot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ComboBoxMuodot.SelectedIndex != 1)
            {
                muoto = this.ComboBoxMuodot.SelectedIndex + 1;
            }
        }
        public void Piirra(MainWindow parent)
        {
            switch (Muoto)
            {
                case 1:
                    Line viiva = new Line();
                    viiva.StrokeThickness = StrokeThickness;
                    viiva.Stroke = new SolidColorBrush(Color.FromArgb(A, R, G, B));
                    viiva.X1 = X;
                    viiva.Y1 = Y;
                    viiva.X2 = X2;
                    viiva.Y2 = Y2;
                    parent.GridPiirto.Children.Add(viiva);
                    break;
                    case 2:
                    
                    case 4:
                    Ellipse ellipsi = new Ellipse();
                    ellipsi.StrokeThickness = StrokeThickness;
                    ellipsi.Stroke = new SolidColorBrush(Color.FromArgb(A, R, G, B));
                    ellipsi.HorizontalAlignment = HorizontalAlignment.Left;
                    ellipsi.VerticalAlignment = VerticalAlignment.Top;
                    ellipsi.Margin = new Thickness(X, Y, 0, 0);
                    ellipsi.Width = Width;
                    ellipsi.Height = Height;
                    parent.GridPiirto.Children.Add(ellipsi);
                    break;
                case 3:
                    Rectangle rect = new Rectangle();
                    rect.StrokeThickness = StrokeThickness;
                    rect.Stroke = new SolidColorBrush(Color.FromArgb(A, R, G, B));
                    rect.HorizontalAlignment = HorizontalAlignment.Left;
                    rect.VerticalAlignment = VerticalAlignment.Top;
                    rect.Margin = new Thickness(X, Y, 0, 0);
                    rect.Width = Width;
                    rect.Height = Height;
                    parent.GridPiirto.Children.Add(rect);
                    break;
                case 5:
                    ellipsi = new Ellipse();
                    ellipsi.StrokeThickness = StrokeThickness;
                    ellipsi.Stroke = new SolidColorBrush(Color.FromArgb(A, R, G, B));
                    ellipsi.HorizontalAlignment = HorizontalAlignment.Left;
                    ellipsi.VerticalAlignment = VerticalAlignment.Top;
                    ellipsi.Margin = new Thickness(X, Y, 0, 0);
                    ellipsi.Width = Width;
                    ellipsi.Height = Height;
                    ellipsi.Fill = new SolidColorBrush(Color.FromArgb(A, R, G, B));
                    parent.GridPiirto.Children.Add(ellipsi);
                    break;
                    default:
                    break;
            }
        }
            
    }


}
