using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Scch.Common.ComponentModel;
using Scch.DomainModel.EntityFramework.Tests.Properties;

namespace Scch.DomainModel.EntityFramework.Tests
{
    [LocalizedDisplayName(typeof(Produktversion), typeof(Resources))]
    public class Produktversion : EntityFrameworkEntity<long>
    {
        private int _nummer;
        private string _kommentar;
        private Produkt _produkt;
        private long _produktId;
        private TrackableCollection<long, Auspraegung> _auspraegungen;

        public Produktversion()
        {
            Auspraegungen = new TrackableCollection<long, Auspraegung>();
        }

        [Range(FieldRange.MinPositive, FieldRange.MaxPositive, ErrorMessageResourceName = "Produktversion_Nummer_Range", ErrorMessageResourceType = typeof(Resources))]
        [LocalizedDisplayName(typeof(Produktversion), "Nummer", typeof(Resources))]
        public int Nummer
        {
            get { return _nummer; }
            set
            {
                if (_nummer == value)
                    return;

                _nummer = value;
                RaisePropertyChanged(() => Nummer);
            }
        }

        [Required(ErrorMessageResourceName = "Produktversion_Kommentar_Required", ErrorMessageResourceType = typeof(Resources))]
        [LocalizedDisplayName(typeof(Produktversion), "Kommentar", typeof(Resources))]
        public string Kommentar
        {
            get { return _kommentar; }
            set
            {
                if (_kommentar == value)
                    return;

                _kommentar = value;
                RaisePropertyChanged(() => Kommentar);
            }
        }

        [LocalizedDisplayName(typeof(Produktversion), "Produkt", typeof(Resources))]
        public Produkt Produkt
        {
            get { return _produkt; }
            set
            {
                if (_produkt == value)
                    return;

                var previousValue = _produkt;
                _produkt = value;
                FixupManyToOneProperty(previousValue, x => x.Produkt, y => y.Produktversionen);

                RaiseNavigationPropertyChanged(() => Produkt);
            }
        }

        [Browsable(false)]
        public long ProduktId
        {
            get { return _produktId; }
            set
            {
                if (_produktId == value)
                    return;

                ChangeTracker.RecordOriginalValue(() => ProduktId, _produktId);

                _produktId = value;
                RaisePropertyChanged(() => ProduktId);
            }
        }

        [LocalizedDisplayName(typeof(Produktversion), "Auspraegungen", typeof(Resources))]
        public TrackableCollection<long, Auspraegung> Auspraegungen
        {
            get
            {
                return _auspraegungen;
            }
            private set
            {
                if (Auspraegungen == value)
                    return;

                ThrowExceptionIfChangeTrackingEnabled();

                if (Auspraegungen != null)
                {
                    Auspraegungen.CollectionChanged -= FixupAuspraegungen;
                }

                _auspraegungen = value;

                if (Auspraegungen != null)
                {
                    Auspraegungen.CollectionChanged += FixupAuspraegungen;
                }

                RaiseNavigationPropertyChanged(() => Auspraegungen);
            }
        }

        private void FixupAuspraegungen(object sender, NotifyCollectionChangedEventArgs e)
        {
            FixupManyToManyProperty<Produktversion, Auspraegung>(e, x => x.Auspraegungen, null);
        }

        public override string ToString()
        {
            return Nummer + ": " + (Kommentar ?? string.Empty);
        }
    }
}
