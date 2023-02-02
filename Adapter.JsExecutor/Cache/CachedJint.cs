using Jint;
using Jint.Native.Object;

namespace Adapter.JsExecutor.Cache;

public record CachedJint(Engine Engine, ObjectInstance Module);