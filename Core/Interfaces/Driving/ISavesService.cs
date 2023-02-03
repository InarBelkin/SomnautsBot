using Core.Models.Executor;
using Core.Models.Save;
using LanguageExt.Common;

namespace Core.Interfaces.Driving;

public interface ISavesService
{
    Task<Result<IEnumerable<BookSaveItemModel>>> GetSavesOfThisUser(Guid genId);
    Task CreateNewSave(Guid genId);
    Task SwitchToSave(int saveId);
    Task<ReplicaModel> GetCurrentReplica();
    Task<ReplicaModel> NextReplica(string answerText);
}