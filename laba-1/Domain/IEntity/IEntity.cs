
namespace Domain.IEntity
{
    public interface IEntity
    {
        long Id { get; set; }
    }

    public class Entity: IEntity
    {
       public virtual long Id { get; set; }
    }
}
