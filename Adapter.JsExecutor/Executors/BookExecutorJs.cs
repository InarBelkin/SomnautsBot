using System.Dynamic;
using Adapter.JsExecutor.Cache;
using Adapter.JsExecutor.Utils;
using Core.Interfaces.Driven;
using Core.Models.Book;
using Core.Models.Exceptions;
using Jint;
using Jint.Runtime;
using Microsoft.Extensions.Options;

namespace Adapter.JsExecutor.Executors;

public sealed class BookExecutorJs : IBookExecutor
{
    private readonly IBooksModulesPool _booksModulesPool;
    private readonly JsExecutorOptions _options;

    public BookExecutorJs(IBooksModulesPool booksModulesPool, IOptions<JsExecutorOptions> options)
    {
        _booksModulesPool = booksModulesPool;
        _options = options.Value;
    }

    public ValueTask<ExpandoObject> CreateInitState(BookHandleModel book)
    {
        try
        {
            var cached = _booksModulesPool.GetModule(book.Description.GenId, () =>
            {
                var pathToLauncher = Path.Combine(book.ContainingFolder, _options.PathToLauncher);
                var engine = new Engine(options => options.EnableModules(book.ContainingFolder));
                var moduleObject = engine.ImportModule(pathToLauncher);
                return new CachedJint(engine, moduleObject);
            });
            var initializer = cached.Module.Get("initializer");
            var fun = initializer.Get("createInitState");
            var stateJsValue = cached.Engine.Invoke(fun, new object?[] { null });
            var state = stateJsValue.ToObject();
            return new ValueTask<ExpandoObject>((ExpandoObject)state);
        }
        catch (JintException e)
        {
            throw new BookExecutionError("Init state error", e);
        }
    }
}