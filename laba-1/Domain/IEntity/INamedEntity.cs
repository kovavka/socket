
namespace Domain.IEntity
{
    public interface INamedEntity: IEntity
    {
        string Name { get; set; }
    }
    public class NamedEntity : Entity, INamedEntity
    {
        public virtual string Name { get; set; }
    }
}
