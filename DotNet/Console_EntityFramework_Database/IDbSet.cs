
public interface IDbSet : ISaveable
{
    IEnumerable<IList<object>?>? GetCollection(Type type);
}