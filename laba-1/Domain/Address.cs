using Domain.IEntity;

namespace Domain
{
    public class Address: Entity
    {
        public virtual string House { get; set; }

        public virtual Street Street { get; set; }

        public virtual Country Country => Region.Country;

        public virtual Region Region => City.Region;

        public virtual CityType CityType => City.CityType;

        public virtual City City => Street.City;
    }

    
    public class Country : NamedEntity
    {
    }

    public class Region : NamedEntity
    {
        public virtual Country Country { get; set; }
    }

    public class CityType : NamedEntity
    {
    }

    public class City : NamedEntity
    {
        public virtual CityType CityType { get; set; }
        public virtual Region Region { get; set; }
    }

    public class Street : NamedEntity
    {
        public virtual City City { get; set; }
    }

}
