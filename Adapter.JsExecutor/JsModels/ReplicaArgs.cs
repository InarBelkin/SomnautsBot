using System.Dynamic;
using Jint.Native.Object;

namespace Adapter.JsExecutor.JsModels;

public class ReplicaArgs
{
    /// <summary>
    ///     Two-letters code
    /// </summary>
    public required string language { get; init; }

    public required ExpandoObject state { get; init; }
    public required ObjectInstance engineModule { get; init; }
}