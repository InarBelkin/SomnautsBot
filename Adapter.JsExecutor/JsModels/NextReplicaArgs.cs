using System.Dynamic;
using Jint.Native.Object;

namespace Adapter.JsExecutor.JsModels;

public class NextReplicaArgs
{
    public required string? answerId { get; init; }
    public required string? answerText { get; init; }

    public required string language { get; init; }

    public required ExpandoObject state { get; init; }
    public required ObjectInstance engineModule { get; init; }
}