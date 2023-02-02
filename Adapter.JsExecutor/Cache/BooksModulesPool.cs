using System.Collections.Concurrent;
using Core.Interfaces.Driven;

namespace Adapter.JsExecutor.Cache;

public interface IBooksModulesPool : INeedCleanAfterBooksRescan
{
    CachedJint GetModule(Guid genId, Func<CachedJint> moduleFactory);
    Task<CachedJint> GetModuleAsync(Guid genId, Func<Task<CachedJint>> moduleFactory);
    void Return(Guid genId, CachedJint item);
}

public sealed class BooksModulesPool : IBooksModulesPool
{
    private readonly ConcurrentDictionary<Guid, ConcurrentBag<CachedJint>> _bookModules;

    public BooksModulesPool()
    {
        _bookModules = new ConcurrentDictionary<Guid, ConcurrentBag<CachedJint>>();
    }

    public async Task<CachedJint> GetModuleAsync(Guid genId, Func<Task<CachedJint>> moduleFactory)
    {
        if (_bookModules.TryGetValue(genId, out var bag))
            return bag.TryTake(out var instance) ? instance : await moduleFactory();

        return await moduleFactory();
    }

    public CachedJint GetModule(Guid genId, Func<CachedJint> moduleFactory)
    {
        if (_bookModules.TryGetValue(genId, out var bag))
            return bag.TryTake(out var instance) ? instance : moduleFactory();

        return moduleFactory();
    }

    public void Return(Guid genId, CachedJint item)
    {
        var bag = _bookModules.GetOrAdd(genId, _ => new ConcurrentBag<CachedJint>());
        bag.Add(item);
    }

    public ValueTask Clean()
    {
        _bookModules.Clear();
        return new ValueTask();
    }
}