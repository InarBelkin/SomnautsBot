using System.Dynamic;
using Adapter.JsExecutor.Cache;
using Adapter.JsExecutor.JsModels;
using Adapter.JsExecutor.Utils;
using Core.Interfaces.Driven;
using Core.Models.Book;
using Core.Models.Executor;
using Jint;
using Jint.Native;
using Microsoft.Extensions.Options;
using Utils.Language;

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
        return "Init state error".WrapJintExceptions(() =>
        {
            var cached = GetModuleFromPool(book);
            var initializer = cached.Module.Get("initializer");
            var fun = initializer.Get("createInitState");
            var stateJsValue = cached.Engine.Invoke(fun, new object?[] { null });
            var state = stateJsValue.ToObject();
            return new ValueTask<ExpandoObject>((ExpandoObject)state);
        });
    }

    public ValueTask<ReplicaModel> GetCurrentReplica(BookHandleModel book, ExpandoObject state, LangEnum language)
    {
        return "Get replica error".WrapJintExceptions(() =>
        {
            var cached = GetModuleFromPool(book);
            var replicaHandler = cached.Module.Get("replicaHandler");
            var fun = replicaHandler.Get("getCurrentReplica");
            var arg = new ReplicaArgs
            {
                language = language,
                state = state,
                engineModule = JsValue.FromObject(cached.Engine, new { }).AsObject()
            };
            var replicaJsValue = cached.Engine.Invoke(fun, arg);
            dynamic replicaExpando = replicaJsValue.ToObject();
            ReplicaModel replica = JsConversions.ConvertDynamicToReplica(replicaExpando);
            return new ValueTask<ReplicaModel>(replica);
        });
    }

    public ValueTask<ReplicaModel> NextReplica(BookHandleModel book, ExpandoObject state, LangEnum language,
        string? answerId, string answerText)
    {
        return "Next replica error".WrapJintExceptions(() =>
        {
            var cached = GetModuleFromPool(book);
            var replicaHandler = cached.Module.Get("replicaHandler");
            var fun = replicaHandler.Get("nextReplica");
            var arg = new NextReplicaArgs
            {
                answerId = answerId,
                answerText = answerText,
                language = language,
                state = state,
                engineModule = JsValue.FromObject(cached.Engine, new { }).AsObject()
            };
            var nextReplicaJsValue = cached.Engine.Invoke(fun, arg);
            dynamic replicaExpando = nextReplicaJsValue.ToObject();
            ReplicaModel replica = JsConversions.ConvertDynamicNextReplicaToReplica(replicaExpando);
            return new ValueTask<ReplicaModel>(replica);
        });
    }

    private CachedJint GetModuleFromPool(BookHandleModel book)
    {
        return _booksModulesPool.GetModule(book.Description.GenId, () =>
        {
            var pathToLauncher = Path.Combine(book.ContainingFolder, _options.PathToLauncher);
            var engine = new Engine(options => options.EnableModules(book.ContainingFolder));
            var moduleObject = engine.ImportModule(pathToLauncher);
            return new CachedJint(engine, moduleObject);
        });
    }
}