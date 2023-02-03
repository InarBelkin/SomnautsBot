namespace Core.Models.Executor;

public class ReplicaModel
{
    public required string Text { get; init; }
    public required bool TakesFreeText { get; init; }
    public required AnswerModel[] Answers { get; init; }
}