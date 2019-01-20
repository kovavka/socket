using System;
using System.Collections.Generic;
using Domain.IEntity;

namespace Domain.Events
{
    public class EventDate : Entity
    {
        public virtual TimeSpan? StartTime { get; set; }
        public virtual TimeSpan? EndTime { get; set; }
    }

    public class EventExecution : Entity
    {
        public virtual ICollection<EventDate> Dates { get; set; }
        public virtual Event Event { get; set; }
    }
}
