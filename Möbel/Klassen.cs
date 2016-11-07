using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ObjekteVererbungXMLSerialisierung
{
    [Flags]
    public enum TürArt { undefiniert = -1, Keine = 0, Flügeltür = 1, Doppeltür = 2, Schiebetür = 4, Spiegeltür = 8, Ofentür = 16 }
    public enum AufstellungsOrte { Keiner, Flur, Küche, Schlafzimmer, Wohnzimmer }

    interface Kühlung
    {
        Boolean IsMaxTemperaturGesetzt();
        Boolean IsMinTemperaturGesetzt();
    }

    public static class erweiterung
    {
        static String ToTemperatur(this Int16 temperatur) => $"{temperatur}°C";
    }

    // um alle verschiedenen Typen zusammen zu serialisieren, muss hier etwas mehr Aufwand getrieben werden
    // den spart man dafür bei den einzelnen Ableitungen
    [XmlInclude(typeof(Schrank))]
    [XmlInclude(typeof(Truhe))]
    [XmlInclude(typeof(Kommode))]
    [XmlInclude(typeof(Küchenschrank))]
    [XmlInclude(typeof(Kleiderschrank))]
    [XmlInclude(typeof(Kühlschrank))]
    [XmlInclude(typeof(Kühltruhe))]
    [XmlInclude(typeof(Wäschetruhe))]
    [Serializable]
    public abstract class Möbel
    {
        private AufstellungsOrte aufstellungsOrt = AufstellungsOrte.Keiner;
        private UInt16 höhe = 0, länge = 0, breite = 0;
        abstract public TürArt Tür { get; set; }

        public String Bezeichnung { get; set; } = String.Empty;

        public UInt16 Länge
        {
            get
            {
                return länge;
            }

            set
            {
                länge = value;
            }
        }

        public UInt16 Breite
        {
            get
            {
                return breite;
            }

            set
            {
                breite = value;
            }
        }

        public UInt16 Höhe
        {
            get
            {
                return höhe;
            }

            set
            {
                höhe = value;
            }
        }

        public AufstellungsOrte AufstellungsOrt
        {
            get
            {
                return aufstellungsOrt;
            }

            set
            {
                aufstellungsOrt = value;
            }
        }

        protected Boolean IsBezeichnungGesetzt => !String.IsNullOrWhiteSpace(Bezeichnung);
        protected Boolean IsAufstellungsOrtGesetzt => AufstellungsOrt != AufstellungsOrte.Keiner;
        protected Boolean IsLängeGesetzt => 0 < Länge;
        protected Boolean IsBreiteGesetzt => 0 < Breite;
        protected Boolean IsHöheGesetzt => 0 < Höhe;
        public virtual Boolean AlleEigenschaftenGesetzt => IsBezeichnungGesetzt && IsAufstellungsOrtGesetzt && IsLängeGesetzt && IsBreiteGesetzt && IsHöheGesetzt;

        public UInt32 Volumen => (UInt32)(Länge * Breite * Höhe);
        public UInt32 Umfang => (UInt32)(Länge + Breite) * 2;

        public String GetShortType() =>  GetType().ToString().Substring(GetType().ToString().LastIndexOf('.')+1);
        public override String ToString() => (IsBezeichnungGesetzt ? Bezeichnung : "keine Bezeichnung vorhanden!")
                + (AlleEigenschaftenGesetzt ? $" [{Länge}*{Breite}*{Höhe}] {AufstellungsOrt.ToString()} ({GetShortType()})" : ": fehlerhafte Daten!");
    }

    public class Schrank : Möbel
    {
        private Int16 anzahlTüren = -1;
        private TürArt tür = TürArt.undefiniert;
        protected Boolean IsAnzahlTürenGesetzt => -1 < AnzahlTüren;
        protected Boolean IsTürGesetzt => Tür != TürArt.undefiniert;

        public override Boolean AlleEigenschaftenGesetzt => base.AlleEigenschaftenGesetzt && IsAnzahlTürenGesetzt && IsTürGesetzt;

        public override TürArt Tür
        {
            get
            {
                return tür;
            }

            set
            {
                tür = value;
            }
        }

        public Int16 AnzahlTüren
        {
            get
            {
                return anzahlTüren;
            }

            set
            {
                anzahlTüren = value;
            }
        }
        public override String ToString() => base.ToString() + $" {AnzahlTüren}*{Tür}"; //TODO Tür-Kombinationen einzeln ausgeben
    }

    public class Truhe : Möbel
    {
        private TürArt tür = TürArt.undefiniert;

        public override TürArt Tür
        {
            get
            {
                return tür;
            }

            set
            {
                if ((value & (TürArt.Doppeltür | TürArt.Spiegeltür)) == TürArt.Keine)
                    tür = value;
                else
                    throw new ArgumentException($@"""{value}"" ist für ""{GetType()}"" nicht zulässig!");
            }
        }

        protected Boolean IsTürGesetzt => Tür != TürArt.undefiniert;

        public override Boolean AlleEigenschaftenGesetzt => base.AlleEigenschaftenGesetzt && IsTürGesetzt;
        public override String ToString() => base.ToString() + $" {Tür}"; //TODO Tür-Kombinationen einzeln ausgeben
    }

    public class Kommode : Möbel
    {
        private String errorMessage => $"Die Schubladenhöhe * Anzahl ({{0}} * {{1}}) übersteigt die Höhe der {GetShortType()} von {Höhe}!";
        private Byte schubladenHöhe = 0;
        private Byte schubladenAnzahl = 0;
        protected Boolean IsSchubladenAnzahlGesetzt => 0 < SchubladenAnzahl;
        protected Boolean IsSchubladenHöheGesetzt => 0 < SchubladenHöhe;

        public new UInt16 Höhe
        {
            get
            {
                return base.Höhe;
            }

            set
            {
                if (IsSchubladenAnzahlGesetzt && IsSchubladenHöheGesetzt && (value < (SchubladenHöhe * SchubladenAnzahl)))
                    throw new ArgumentException(String.Format(errorMessage, SchubladenHöhe, SchubladenAnzahl));
                else
                    base.Höhe = value;
            }
        }

        public Byte SchubladenAnzahl
        {
            get
            {
                return schubladenAnzahl;
            }

            set
            {
                if (IsHöheGesetzt && IsSchubladenHöheGesetzt && (Höhe < (value * SchubladenHöhe)))
                    throw new ArgumentException(String.Format(errorMessage, SchubladenHöhe, value));
                else
                    schubladenAnzahl = value;
            }
        }

        public Byte SchubladenHöhe
        {
            get
            {
                return schubladenHöhe;
            }

            set
            {
                if (IsHöheGesetzt && IsSchubladenAnzahlGesetzt && (Höhe < (value * SchubladenAnzahl)))
                    throw new ArgumentException(String.Format(errorMessage, value, SchubladenAnzahl));
                else
                    schubladenHöhe = value;
            }
        }

        public override Boolean AlleEigenschaftenGesetzt => base.AlleEigenschaftenGesetzt && IsSchubladenAnzahlGesetzt && IsSchubladenHöheGesetzt;

        public override TürArt Tür
        {
            get
            {
                return TürArt.undefiniert;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override String ToString() => base.ToString() + "; "
            + (IsSchubladenAnzahlGesetzt && IsSchubladenHöheGesetzt ? $"{SchubladenAnzahl} Schubladen á {SchubladenHöhe} hoch" : "Schubladen nicht korrekt gesetzt");
    }

    public class Küchenschrank : Schrank
    {
        private Int16 anzahlEinlegeböden = -1;
        protected Boolean IsAnzahlEinlegebödenGesetzt => -1 < anzahlEinlegeböden;

        public Int16 AnzahlEinlegeböden
        {
            get
            {
                return anzahlEinlegeböden;
            }

            set
            {
                anzahlEinlegeböden = value;
            }
        }

        public override Boolean AlleEigenschaftenGesetzt => base.AlleEigenschaftenGesetzt && IsAnzahlEinlegebödenGesetzt;
        public override String ToString() => base.ToString() + $"; {(IsAnzahlEinlegebödenGesetzt ? AnzahlEinlegeböden.ToString() : "keine ")} Einlegeböden";
    }

    public class Kleiderschrank : Schrank
    {
        private Int16 stangenbreite = -1;
        protected Boolean IsStangenbreiteGesetzt => -1 < stangenbreite;

        public Int16 Stangenbreite
        {
            get
            {
                return stangenbreite;
            }

            set
            {
                stangenbreite = value;
            }
        }

        public override Boolean AlleEigenschaftenGesetzt => base.AlleEigenschaftenGesetzt && IsStangenbreiteGesetzt;
        public override String ToString() => base.ToString() + $"; Stange: {(IsStangenbreiteGesetzt ? Stangenbreite.ToString() : "nicht gesetzt")}";
    }

    public class Kühlschrank : Küchenschrank, Kühlung
    {
        private String errorMessage => $"Bei einem {GetShortType()} muss die Minimaltemperatur von {{0}}°C kleiner als die Maximaltemperatur von {{1}}°C sein!";
        private Int16 maximalTemperatur = Int16.MinValue;
        private Int16 minimalTemperatur = Int16.MaxValue;
        protected Boolean IsMaximalTemperaturGesetzt => Int16.MinValue < MaximalTemperatur;
        protected Boolean IsMinimalTemperaturGesetzt => Int16.MaxValue > MinimalTemperatur;
        Boolean IsMaxTemperaturGesetzt() => IsMaximalTemperaturGesetzt;
        Boolean IsMinTemperaturGesetzt() => IsMinimalTemperaturGesetzt;


        String MaximalTemperaturString => $"{nameof(MaximalTemperatur)}: {MaximalTemperatur}";
        String MinimalTemperaturString => $"{nameof(MinimalTemperatur)}: {MinimalTemperatur}";

        public Int16 MaximalTemperatur
        {
            get
            {
                return maximalTemperatur;
            }

            set
            {
                if (IsMinimalTemperaturGesetzt && value < MinimalTemperatur)
                    throw new ArgumentException(String.Format(errorMessage, MinimalTemperatur, value));
                else
                    maximalTemperatur = value;
            }
        }

        public Int16 MinimalTemperatur
        {
            get
            {
                return minimalTemperatur;
            }

            set
            {
                if (IsMaximalTemperaturGesetzt && MaximalTemperatur < value)
                    throw new ArgumentException(String.Format(errorMessage, value, MaximalTemperatur));
                else
                    minimalTemperatur = value;
            }
        }

        Boolean Kühlung.IsMaxTemperaturGesetzt() => IsMaximalTemperaturGesetzt;

        Boolean Kühlung.IsMinTemperaturGesetzt() => IsMinimalTemperaturGesetzt;

        public override Boolean AlleEigenschaftenGesetzt => base.AlleEigenschaftenGesetzt && IsMaximalTemperaturGesetzt && IsMinimalTemperaturGesetzt;

        public override String ToString() => base.ToString() + " " + MaximalTemperaturString + " " + MinimalTemperaturString;
    }

    public class Kühltruhe : Truhe, Kühlung
    {
        private String errorMessage => $"Bei einem {GetShortType()} muss die Minimaltemperatur von {{0}}°C kleiner als die Maximaltemperatur von {{1}}°C sein!";
        private Int16 maximalTemperatur = Int16.MinValue;
        private Int16 minimalTemperatur = Int16.MaxValue;
        protected Boolean IsMaximalTemperaturGesetzt => Int16.MinValue < MaximalTemperatur;
        protected Boolean IsMinimalTemperaturGesetzt => Int16.MaxValue > MinimalTemperatur;
        Boolean IsMaxTemperaturGesetzt() => IsMaximalTemperaturGesetzt;
        Boolean IsMinTemperaturGesetzt() => IsMinimalTemperaturGesetzt;


        String MaximalTemperaturString => $"{nameof(MaximalTemperatur)}: {MaximalTemperatur}";
        String MinimalTemperaturString => $"{nameof(MinimalTemperatur)}: {MinimalTemperatur}";

        public Int16 MaximalTemperatur
        {
            get
            {
                return minimalTemperatur;
            }

            set
            {
                if (IsMinimalTemperaturGesetzt && value < MinimalTemperatur)
                    throw new ArgumentException(String.Format(errorMessage, MinimalTemperatur, value));
                else
                    maximalTemperatur = value;
            }
        }

        public Int16 MinimalTemperatur
        {
            get
            {
                return minimalTemperatur;
            }

            set
            {
                if (IsMaximalTemperaturGesetzt && MaximalTemperatur < value)
                    throw new ArgumentException(String.Format(errorMessage, value, MaximalTemperatur));
                else
                    minimalTemperatur = value;
            }
        }

        Boolean Kühlung.IsMaxTemperaturGesetzt() => IsMaximalTemperaturGesetzt;

        Boolean Kühlung.IsMinTemperaturGesetzt() => IsMinimalTemperaturGesetzt;

        public override Boolean AlleEigenschaftenGesetzt => base.AlleEigenschaftenGesetzt && IsMaximalTemperaturGesetzt && IsMinimalTemperaturGesetzt;

        public override String ToString() => base.ToString() + " " + MaximalTemperaturString + " " + MinimalTemperaturString;
    }

    public class Wäschetruhe : Truhe
    {
        Boolean? insektendicht = null;
        protected Boolean IsInsektendichtGesetzt => insektendicht != null;

        public Boolean Insektendicht
        {
            get
            {
                return IsInsektendichtGesetzt ? (Boolean)insektendicht : false;
            }

            set
            {
                insektendicht = value;
            }
        }

        public override Boolean AlleEigenschaftenGesetzt => base.AlleEigenschaftenGesetzt && IsInsektendichtGesetzt;

        public override String ToString() => base.ToString() + (Insektendicht ? " ist insektendicht" : " Vorsicht vor Insekten");
    }
}