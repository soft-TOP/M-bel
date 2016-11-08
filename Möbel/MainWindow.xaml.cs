using ObjekteVererbungXMLSerialisierung;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace ObjekteVererbungXMLSerialisierung
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Die aktuell vom Bediener ausgewählten Türarten
        /// </summary>
        TürArt AusgwählteTüren = TürArt.Keine;
        /// <summary>
        /// Der aktuell vom Bediener ausgewählte Aufstellungsort
        /// </summary>
        AufstellungsOrte AusgewählterAufstellungsOrt = AufstellungsOrte.Keiner;
        /// <summary>
        /// Die Liste der Möbel, welche den vom Bediener gewählten Kriterien entspricht
        /// </summary>
        private List<Möbel> AusgwählteMöbelliste = new List<Möbel>();
        /// <summary>
        /// Die Liste aller verfügbaren Möbel
        /// </summary>
        private List<Möbel> Möbelliste = new List<Möbel>();

        public MainWindow()
        {
            InitializeComponent();
            // die Möbelstücke anlegen

            //Führt zu Exception:
            //Kommode Kom = new Kommode();
            //Kom.Bezeichnung = "Kommode";
            //Kom.SchubladenHöhe = 10;
            //Kom.SchubladenAnzahl = 10;
            //Kom.Höhe = 1;


            //Führt zu Exception:
            //Kühlschrank Kühl = new Kühlschrank();
            //Kühl.Bezeichnung = "Kühlscrank";
            //Kühl.MinimalTemperatur = 8;
            //Kühl.MaximalTemperatur = 4;
            #region MöbelDefinition
            Schrank schrank = new Schrank();
            schrank.Bezeichnung = "Wohnzimmerschrank";
            schrank.AufstellungsOrt = AufstellungsOrte.Wohnzimmer;
            schrank.Länge = 200;
            schrank.Breite = 60;
            schrank.Höhe = 230;
            schrank.AnzahlTüren = 3;
            schrank.Tür = TürArt.Doppeltür | TürArt.Flügeltür;

            Möbelliste.Add(schrank);


            schrank = new Schrank();
            schrank.Bezeichnung = "Büroschrank";
            schrank.AufstellungsOrt = AufstellungsOrte.Wohnzimmer;
            schrank.Länge = 120;
            schrank.Breite = 60;
            schrank.Höhe = 71;
            schrank.AnzahlTüren = 2;
            schrank.Tür = TürArt.Schiebetür;

            Möbelliste.Add(schrank);


            Truhe truhe = new Truhe();
            truhe.Bezeichnung = "Kleidertruhe";
            truhe.AufstellungsOrt = AufstellungsOrte.Schlafzimmer;
            truhe.Länge = 90;
            truhe.Breite = 55;
            truhe.Höhe = 48;
            truhe.Tür = TürArt.Schiebetür;

            Möbelliste.Add(truhe);

            Kommode kommode = new Kommode();
            kommode.Bezeichnung = "Kinderkommode";
            kommode.AufstellungsOrt = AufstellungsOrte.Flur;
            kommode.Länge = 78;
            kommode.Breite = 45;
            kommode.Höhe = 68;
            kommode.SchubladenAnzahl = 3;
            kommode.SchubladenHöhe = 22;

            Möbelliste.Add(kommode);


            Küchenschrank küchenschrank = new Küchenschrank();
            küchenschrank.Bezeichnung = "Tellerschrank";
            küchenschrank.AufstellungsOrt = AufstellungsOrte.Küche;
            küchenschrank.Länge = 110;
            küchenschrank.Breite = 60;
            küchenschrank.Höhe = 210;
            küchenschrank.AnzahlTüren = 2;
            küchenschrank.Tür = TürArt.Doppeltür;
            küchenschrank.AnzahlEinlegeböden = 5;

            Möbelliste.Add(küchenschrank);


            Kleiderschrank kleiderschrank = new Kleiderschrank();
            kleiderschrank.Bezeichnung = "ihre Klamotten";
            kleiderschrank.AufstellungsOrt = AufstellungsOrte.Flur;
            kleiderschrank.Länge = 310;
            kleiderschrank.Breite = 80;
            kleiderschrank.Höhe = 230;
            kleiderschrank.AnzahlTüren = 2;
            kleiderschrank.Tür = TürArt.Schiebetür | TürArt.Spiegeltür | TürArt.Flügeltür;
            kleiderschrank.Stangenbreite = 80;

            Möbelliste.Add(kleiderschrank);


            Kühlschrank kühlschrank = new Kühlschrank();
            kühlschrank.Bezeichnung = "Obst und Gemüse";
            kühlschrank.AufstellungsOrt = AufstellungsOrte.Küche;
            kühlschrank.Länge = 60;
            kühlschrank.Breite = 60;
            kühlschrank.Höhe = 200;
            kühlschrank.AnzahlTüren = 1;
            kühlschrank.Tür = TürArt.Flügeltür;
            kühlschrank.MaximalTemperatur = 10;
            kühlschrank.MinimalTemperatur = -32;
            kühlschrank.AnzahlEinlegeböden = 7;

            Möbelliste.Add(kühlschrank);


            kühlschrank = new Kühlschrank();
            kühlschrank.Bezeichnung = "Milch und Eier";
            kühlschrank.AufstellungsOrt = AufstellungsOrte.Küche;
            kühlschrank.Länge = 60;
            kühlschrank.Breite = 60;
            kühlschrank.Höhe = 100;
            kühlschrank.AnzahlTüren = 1;
            kühlschrank.Tür = TürArt.Flügeltür;
            kühlschrank.MaximalTemperatur = 10;
            kühlschrank.MinimalTemperatur = 4;
            kühlschrank.AnzahlEinlegeböden = 4;

            Möbelliste.Add(kühlschrank);


            Kühltruhe kühltruhe = new Kühltruhe();
            kühltruhe.Bezeichnung = "Wild und Fisch";
            kühltruhe.AufstellungsOrt = AufstellungsOrte.Küche;
            kühltruhe.Länge = 90;
            kühltruhe.Breite = 60;
            kühltruhe.Höhe = 80;
            kühltruhe.Tür = TürArt.Schiebetür;
            kühltruhe.MaximalTemperatur = -18;
            kühltruhe.MinimalTemperatur = -44;

            Möbelliste.Add(kühltruhe);


            Wäschetruhe wäschetruhe = new Wäschetruhe();
            wäschetruhe.Bezeichnung = "ihre UWäsche";
            wäschetruhe.AufstellungsOrt = AufstellungsOrte.Schlafzimmer;
            wäschetruhe.Länge = 90;
            wäschetruhe.Breite = 60;
            wäschetruhe.Höhe = 80;
            wäschetruhe.Tür = TürArt.Schiebetür;
            wäschetruhe.Insektendicht = true;

            Möbelliste.Add(wäschetruhe);


            wäschetruhe = new Wäschetruhe();
            wäschetruhe.Bezeichnung = "seine UWäsche";
            wäschetruhe.AufstellungsOrt = AufstellungsOrte.Schlafzimmer;
            wäschetruhe.Länge = 90;
            wäschetruhe.Breite = 60;
            wäschetruhe.Höhe = 80;
            wäschetruhe.Tür = TürArt.Schiebetür;
            wäschetruhe.Insektendicht = false;

            Möbelliste.Add(wäschetruhe);

            #endregion

            // Die Listboxen mit den Auswahlmöglichkeiten für Tür und Aufstellungsort befüllen
            // sowie alle Möbelstücke anzeigen

            // Alle Aufzählungen der Türarten als array für die Anzeige
            // der Eintrag "undefiniert" wird weggefiltert
            listBoxTürArten.ItemsSource = Enum.GetValues(typeof(TürArt)).Cast<TürArt>().Where(tA => tA != TürArt.undefiniert).ToArray(); ;
            // Alle Aufstellungsorte der Möbelstücke
            // der Eintrag "keiner" wird weggefiltert
            listBoxAufstellungsorte.ItemsSource = Enum.GetValues(typeof(AufstellungsOrte)).Cast<AufstellungsOrte>().Where(tA => tA != AufstellungsOrte.Keiner).ToArray();
            // Alle Aufstellungsorte der Möbelstücke
            listBox_Möbel.ItemsSource = Möbelliste;
        }


        /// <summary>
        /// überträgt die vom Bediener getätigte Türauswahl ins entsprechende Feld für die eingeschränkte Auswahl
        /// Ruft anschließend die tatsächliche Auswahlselektion auf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxTürArten_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            AusgwählteTüren = TürArt.Keine;
            foreach (TürArt türAuswahl in ((ListBox)sender).SelectedItems)
            {
                AusgwählteTüren |= türAuswahl;
            }
            SelectionChanged();
        }

        /// <summary>
        /// überträgt die vom Bediener getätigte Auswahl des Aufstellungsortes ins entsprechende Feld für die eingeschränkte Auswahl
        /// Ruft anschließend die tatsächliche Auswahlselektion auf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxAufstellungsorte_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            AusgewählterAufstellungsOrt = (AufstellungsOrte)((ListBox)sender).SelectedItem;
            SelectionChanged();
        }

        /// <summary>
        /// übernimmt die tatsächliche Auswahlselektion
        /// der anzuzeigenden Möbelstücke
        /// </summary>
        private void SelectionChanged()
        {
            // Anzeige der tatsächlich ausgewählten Möbelstücke
            // Besonderheiten von WPF nicht optimal umgesetzt!
            // Hier kann man noch Optimieren...
            listBox_AusgewählteMöbel.ItemsSource = null;
            AusgwählteMöbelliste.Clear();

            // Alle Möbelstücke, die beiden Bedingungen entsprechen in einer neuen Liste zusammenfassen
            foreach (var möbel in Möbelliste)
            {
                if ((möbel.AufstellungsOrt == AusgewählterAufstellungsOrt)
                    && ((TürArt.Keine != (möbel.Tür & AusgwählteTüren))))
                {
                    AusgwählteMöbelliste.Add(möbel);
                }
            }
            listBox_AusgewählteMöbel.ItemsSource = AusgwählteMöbelliste;
        }

        /// <summary>
        /// Speichern der ausgwählten Bücher in einer XML-Datei
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Speichern_Click(Object sender, RoutedEventArgs e)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Möbel));
            using (FileStream datei = new FileStream(System.IO.Path.Combine(Environment.CurrentDirectory, "Moebel.XML"), FileMode.Append))
            {
                try
                {
                    foreach (var möbel in AusgwählteMöbelliste)
                    {
                        xml.Serialize(datei, möbel);
                    }
                }
                finally
                {
                    datei.Close();
                }
            }
        }
    }
}