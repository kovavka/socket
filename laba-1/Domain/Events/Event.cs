using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IEntity;

namespace Domain.Events
{
    public class Event : NamedEntity
    {
        public virtual string Comment { get; set; }
        public virtual string Info { get; set; }

        public virtual Address Address { get; set; }
        public virtual DateTime Execution { get; set; }
    }

}
