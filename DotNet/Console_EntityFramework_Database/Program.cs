using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

using (var db = new DataContext())
{
    db.Users.Add(new User() { Login = "abc4", Posts = new List<Post> { new Post { Message = "Message" } }  });
    db.Users.Add(new User() { Login = "abc3" });
    db.Users.Add(new User() { Login = "abc2" });
    db.Users.Add(new User() { Login = "abc1" });
}

using (var db = new DataContext())
{
    var users = db.Users.ToArray();
    ;
}

using (var db = new DataContext())
{
    var posts = db.Posts.ToArray();

    foreach (var post in posts)
    {
        var props = post.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(i =>
                i.PropertyType == typeof(User) &&
                i.GetCustomAttribute<ForeignKeyAttribute>() is not null
            )
            .ToArray();

    }

    ;
}

public enum Player
{
    [Display(Name = "Pierwszy")]
    First,

    [Display(Name = "Drugi")]
    Second,
}

public class DataContext : IDisposable, ISaveable
{
    public DbSet<User> Users { get; } = new DbSet<User>();
    public DbSet<Post> Posts { get; } = new DbSet<Post>();

    public void SaveChanges()
    {
        if (_isSaved)
            return;
        _isSaved = true;

        var dict = new Dictionary<Type, List<object>>();

        foreach (var i in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            object? obj = i.GetValue(this);
            //var elementType = i.PropertyType.GenericTypeArguments[0];
            //dict[elementType] = 

            if (obj is IDbSet set)
            {
                var posts = set.GetCollection(typeof(Post));
                foreach (var o in posts)
                    if (o is not null)
                    {
                        foreach (Post post in o)
                            Posts.Add(post);
                        Posts.SaveChanges();
                    }
                set.SaveChanges();
            }
        }
    }

    public void Dispose()
    {
        if (_isSaved)
            return;
        SaveChanges();
    }

    private bool _isSaved;
}

public class DbSet<T> : IEnumerable<T>, IDbSet
{
    public DbSet()
    {
        PropertyInfo prop = typeof(T)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Single(i => i.Name.EndsWith("Id"));
        primaryKeySetter = (item, id) => prop.SetValue(item, id);
    }

    public void Add(T item) => _newData.Add(item);

    public IEnumerable<IList<object>?>? GetCollection(Type type)
    {
        PropertyInfo? prop = typeof(T)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(i =>
                i.PropertyType.GenericTypeArguments.Length == 1 &&
                i.PropertyType.GenericTypeArguments[0] == type
            )
            .SingleOrDefault();

        if (prop is null)
            yield break;

        foreach (var i in _newData)
        {
            var obj = prop.GetValue(i);
            yield return (obj as IEnumerable)?.Cast<object>().ToList();
        }
    }

    public void SaveChanges()
    {
        int count = _data.Count + 1;
        for (int i = 0; i < _newData.Count; i++)
        {
            var item = _newData[i];
            primaryKeySetter(item, i + count);
            _data.Add(item);
        }
    }

    private readonly Action<T, int> primaryKeySetter;
    private readonly List<T> _newData = new();

    private readonly static List<T> _data = new();

    public IEnumerator<T> GetEnumerator() => _data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();
}

[DebuggerDisplay("{UserId}: '{Login,nq}/{Password,nq}', {Posts.Count}")]
public class User
{
    public int        UserId   { get; set; }
    public string     Login    { get; set; } = "";
    public string     Password { get; set; } = "";
    public List<Post> Posts    { get; set; } = new List<Post>();
}

[DebuggerDisplay("{Title,nq}:{Message,nq} {AuthorUser?.UserId}")]
public class Post
{
    public int    PostId      { get; set; }
    public string Title       { get; set; } = "";
    public string Message     { get; set; } = "";

    [ForeignKey]
    public User? AuthorUser   { get; set; }

    public User? RepliersUser { get; set; }
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
sealed class ForeignKeyAttribute : Attribute
{
}
