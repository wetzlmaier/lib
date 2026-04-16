using System;
using System.ComponentModel.DataAnnotations;
using Scch.Common.ComponentModel;
using Scch.Common.ComponentModel.DataAnnotations;
using Scch.DomainModel.EntityFramework.Tests.Properties;

namespace Scch.DomainModel.EntityFramework.Tests
{
    public abstract class EntityWithBezeichnung : EntityFrameworkEntity<long>, IComparable
    {
        private string _bezeichnung;

        [Required(ErrorMessageResourceName = "EntityWithBezeichnung_Bezeichnung_Required", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(FieldLength.Bezeichnung, ErrorMessageResourceName = "EntityWithBezeichnung_Bezeichnung_Length", ErrorMessageResourceType = typeof(Resources))]
        [LocalizedDisplayName(typeof(EntityWithBezeichnung), "Bezeichnung", typeof(Resources))]
        [Index(IsUnique = true)]
        public string Bezeichnung
        {
            get { return _bezeichnung; }
            set
            {
                if (_bezeichnung == value)
                    return;

                _bezeichnung = value;
                RaisePropertyChanged(() => Bezeichnung);
            }
        }

        public override string ToString()
        {
            return Bezeichnung;
        }

        public virtual int CompareTo(object obj)
        {
            if (obj!=null && obj.GetType()==GetType())
            {
                return string.Compare(Bezeichnung, ((EntityWithBezeichnung)obj).Bezeichnung, StringComparison.CurrentCulture);
            }
            return -1;
        }
    }
}
