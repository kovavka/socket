using Domain;
using Domain.Events;
using Domain.IEntity;
using FluentNHibernate.Mapping;
using NHibernate.Type;

namespace Infrastructure.Mappings.Events
{
    class EventMap : NamedEntityMap<Event>
    {
        public EventMap()
        {
            Map(x => x.Comment).Nullable();
            Map(x => x.Info);
            Map(x => x.Execution);
            References(x => x.Address).Cascade.SaveUpdate().ForeignKey("FK_EventExecution_Address");
        }
    }
}
