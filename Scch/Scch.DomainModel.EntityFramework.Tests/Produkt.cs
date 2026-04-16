using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Scch.Common.ComponentModel;
using Scch.Common.ComponentModel.DataAnnotations;
using Scch.DomainModel.EntityFramework.Tests.Properties;

namespace Scch.DomainModel.EntityFramework.Tests
{
    [LocalizedDisplayName(typeof(Produkt), typeof(Resources))]
    public class Produkt : EntityWithBezeichnung
    {
        private decimal _grundpreis;
        private string _nummer;
        private Produktgruppe _produktgruppe;
        private long _produktgruppeId;
        private TrackableCollection<long, Produktversion> _produktversionen;

        public Produkt()
        {
            Produktversionen = new TrackableCollection<long, Produktversion>();
        }

        [Required(ErrorMessageResourceName = "Produkt_Nummer_Required", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(FieldLength.Produktnummer, ErrorMessageResourceName = "Produkt_Nummer_Length", ErrorMessageResourceType = typeof(Resources))]
        [LocalizedDisplayName(typeof(Produkt), "Nummer", typeof(Resources))]
        [Index(IsUnique = true)]
        public string Nummer
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

        [Range(FieldRange.MinPrice, FieldRange.MaxPrice, ErrorMessageResourceName = "Produkt_Grundpreis_Range", ErrorMessageResourceType = typeof(Resources))]
        [LocalizedDisplayName(typeof(Produkt), "Grundpreis", typeof(Resources))]
        public decimal Grundpreis
        {
            get { return _grundpreis; }
            set
            {
                if (_grundpreis == value)
                    return;

                _grundpreis = value;
                RaisePropertyChanged(() => Grundpreis);
            }
        }

        [LocalizedDisplayName(typeof(Produkt), "Produktgruppe", typeof(Resources))]
        public Produktgruppe Produktgruppe
        {
            get { return _produktgruppe; }
            set
            {
                if (_produktgruppe == value)
                    return;

                var previousValue = _produktgruppe;
                _produktgruppe = value;
                FixupManyToOneProperty<Produkt, Produktgruppe>(previousValue, x => x.Produktgruppe, null);

                RaiseNavigationPropertyChanged(() => Produktgruppe);
            }
        }

        [Browsable(false)]
        public long ProduktgruppeId
        {
            get { return _produktgruppeId; }
            set
            {
                if (_produktgruppeId == value)
                    return;

                ChangeTracker.RecordOriginalValue(() => ProduktgruppeId, _produktgruppeId);

                _produktgruppeId = value;
                RaisePropertyChanged(() => ProduktgruppeId);
            }
        }

        [LocalizedDisplayName(typeof(Produkt), "Produktversionen", typeof(Resources))]
        public TrackableCollection<long, Produktversion> Produktversionen
        {
            get
            {
                return _produktversionen;
            }
            private set
            {
                if (Produktversionen == value)
                    return;

                ThrowExceptionIfChangeTrackingEnabled();

                if (Produktversionen != null)
                {
                    Produktversionen.CollectionChanged -= FixupProduktversionen;
                }

                _produktversionen = value;

                if (Produktversionen != null)
                {
                    Produktversionen.CollectionChanged += FixupProduktversionen;
                }

                RaiseNavigationPropertyChanged(() => Produktversionen);
            }
        }

        private void FixupProduktversionen(object sender, NotifyCollectionChangedEventArgs e)
        {
            FixupOneToManyProperty<Produkt, Produktversion>(e, x => x.Produktversionen, y => y.Produkt);
        }
    }
}
