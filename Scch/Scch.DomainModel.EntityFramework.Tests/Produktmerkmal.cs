using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Scch.Common.ComponentModel;
using Scch.DomainModel.EntityFramework.Tests.Properties;

namespace Scch.DomainModel.EntityFramework.Tests
{
    [LocalizedDisplayName(typeof(Produktmerkmal), typeof(Resources))]
    public class Produktmerkmal : EntityWithBezeichnung
    {
        private Merkmalsgruppe _merkmalsgruppe;
        private int _laenge;
        private TrackableCollection<long, Auspraegung> _auspraegungen;
        private long _merkmalsgruppeId;

        public Produktmerkmal()
        {
            Auspraegungen = new TrackableCollection<long, Auspraegung>();
        }

        [LocalizedDisplayName(typeof(Produktmerkmal), "Auspraegungen", typeof(Resources))]
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
            FixupOneToManyProperty<Produktmerkmal, Auspraegung>(e, x => x.Auspraegungen, x => x.Produktmerkmal);
        }

        [Range(FieldRange.MinLength, FieldRange.MaxLength, ErrorMessageResourceName = "Produktmerkmal_Laenge_Range", ErrorMessageResourceType = typeof(Resources))]
        [LocalizedDisplayName(typeof(Produktmerkmal), "Laenge", typeof(Resources))]
        public int Laenge
        {
            get { return _laenge; }
            set
            {
                if (_laenge == value)
                    return;

                _laenge = value;
                RaisePropertyChanged(() => Laenge);
            }
        }
        
        [LocalizedDisplayName(typeof(Produktmerkmal), "Merkmalsgruppe", typeof(Resources))]
        public Merkmalsgruppe Merkmalsgruppe
        {
            get { return _merkmalsgruppe; }
            set
            {
                if (_merkmalsgruppe == value)
                    return;

                var previousValue = _merkmalsgruppe;
                _merkmalsgruppe = value;
                FixupManyToOneProperty<Produktmerkmal, Merkmalsgruppe>(previousValue, x => x.Merkmalsgruppe, null);

                RaiseNavigationPropertyChanged(() => Merkmalsgruppe);
            }
        }

        [Browsable(false)]
        public long MerkmalsgruppeId
        {
            get { return _merkmalsgruppeId; }
            set
            {
                if (_merkmalsgruppeId == value)
                    return;

                ChangeTracker.RecordOriginalValue(() => MerkmalsgruppeId, _merkmalsgruppeId);

                _merkmalsgruppeId = value;
                RaisePropertyChanged(() => MerkmalsgruppeId);
            }
        }
    }
}
