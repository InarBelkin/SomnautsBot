using Core.Models.Save;
using LanguageExt.Common;

namespace Core.Interfaces.Driven;

public interface ISavesStore
{
    Task<Result<IEnumerable<BookSaveItemModel>>> GetSavesByUser(int userId, Guid genId);
}