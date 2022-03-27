namespace Boilerplate.Common.Data;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}