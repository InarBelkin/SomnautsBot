using System.Collections.Concurrent;

namespace Utils;

public sealed class ObjectPool<T>
{
    private readonly Func<T> _objectGenerator;
    private readonly ConcurrentBag<T> _objects;

    public ObjectPool(Func<T> objectGenerator)
    {
        _objectGenerator = objectGenerator ?? throw new ArgumentException(nameof(objectGenerator));
        _objects = new ConcurrentBag<T>();
    }

    public T Get()
    {
        return _objects.TryTake(out var item) ? item : _objectGenerator();
    }

    public void Return(T item)
    {
        _objects.Add(item);
    }
}