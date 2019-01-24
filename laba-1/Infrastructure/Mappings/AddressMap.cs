using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Events;

namespace Infrastructure.Mappings
{
    class AddressMap : EntityMap<Address>
    {
        public AddressMap()
        {
            Map(x => x.House);
            References(x => x.Street).Cascade.SaveUpdate().Cascade.Delete().ForeignKey("FK_House_Street");
        }
    }
    class StreetMap : NamedEntityMap<Street>
    {
        public StreetMap()
        {
            References(x => x.City).Cascade.SaveUpdate().Cascade.Delete().ForeignKey("FK_Street_City");
        }
    }
    class CityMap : NamedEntityMap<City>
    {
        public CityMap()
        {
            References(x => x.CityType).Cascade.SaveUpdate().Cascade.Delete().ForeignKey("FK_City_CityType");
            References(x => x.Region).Cascade.SaveUpdate().Cascade.Delete().ForeignKey("FK_City_Region");
        }
    }
    class CityTypeMap : NamedEntityMap<CityType>
    {
        public CityTypeMap()
        {
        }
    }
    class RegionMap : NamedEntityMap<Region>
    {
        public RegionMap()
        {
            References(x => x.Country).Cascade.SaveUpdate().Cascade.Delete().ForeignKey("FK_Region_Country");
        }
    }
    class CountryMap : NamedEntityMap<Country>
    {
        public CountryMap()
        {
        }
    }
}
