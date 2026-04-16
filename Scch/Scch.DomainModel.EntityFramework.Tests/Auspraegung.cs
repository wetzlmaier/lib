using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Scch.Common.ComponentModel;
using Scch.Common.ComponentModel.DataAnnotations;
using Scch.DomainModel.EntityFramework.Tests.Properties;

namespace Scch.DomainModel.EntityFramework.Tests
{
    [LocalizedDisplayName(typeof(Auspraegung), typeof(Resources))]
    public class Auspraegung : EntityWithBezeichnung
    {
        private Produktmerkmal _produktmerkmal;
        private long _produktmerkmalId;
        private string _bestellcode;

        [LocalizedDisplayName(typeof(Auspraegung), "Produktmerkmal", typeof(Resources))]
        public Produktmerkmal Produktmerkmal
        {
            get { return _produktmerkmal; }
            set
            {
                if (_produktmerkmal == value)
                    return;

                var previousValue = _produktmerkmal;
                _produktmerkmal = value;
                FixupManyToOneProperty(previousValue, x => x.Produktmerkmal, x => x.Auspraegungen);

                RaiseNavigationPropertyChanged(() => Produktmerkmal);
            }
        }

        [Browsable(false)]
        [Index(IndexName = "IX_Auspraegung_Bezeichnung", IsUnique = true, OrdinalPoistion = 1)]
        [Index(IndexName = "IX_Auspraegung_Bestellcode", IsUnique = true)]
        public long ProduktmerkmalId
        {
            get { return _produktmerkmalId; }
            set
            {
                if (_produktmerkmalId == value)
                    return;

                ChangeTracker.RecordOriginalValue(() => ProduktmerkmalId, _produktmerkmalId);

                _produktmerkmalId = value;
                RaisePropertyChanged(() => ProduktmerkmalId);
            }
        }

        [Required(ErrorMessageResourceName = "Auspraegung_Bestellcode_Required", ErrorMessageResourceType = typeof(Resources))]
        [LocalizedDisplayName(typeof(Auspraegung), "Bestellcode", typeof(Resources))]
        [Index(IndexName = "IX_Auspraegung_Bestellcode", IsUnique = true, OrdinalPoistion = 1)]
        [StringLength(FieldRange.MaxLength, ErrorMessageResourceName = "Auspraegung_Bestellcode_Length", ErrorMessageResourceType = typeof(Resources))]
        public string Bestellcode
        {
            get { return _bestellcode; }
            set
            {
                if (_bestellcode == value)
                    return;

                _bestellcode = value;
                RaisePropertyChanged(() => Bestellcode);
            }
        }

        /// <summary>
        /// <see cref="object.ToString"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", Bezeichnung, Bestellcode);
        }

        /// <summary>
        /// <see cref="EntityWithBezeichnung.CompareTo(object)"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int CompareTo(object obj)
        {
            if (obj != null && obj.GetType() == GetType())
            {
                return string.Compare(Bestellcode, ((Auspraegung)obj).Bestellcode, StringComparison.CurrentCulture);
            }
            return -1;
        }
    }
}
