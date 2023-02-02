using System.Dynamic;
using Core.Models.Exceptions;
using Core.Models.Save;
using LanguageExt.Common;
using Utils.Language;

namespace Core.Interfaces.Driven;

public interface ISavesStore
{
    Task<Result<IEnumerable<BookSaveItemModel>>> GetSavesByUser(int userId, Guid genId);

    ///<exception cref="Core.Models.Exceptions.BookDoesntExistException"></exception>
    Task CreateNewSave(int userId, Guid genId, ExpandoObject state, LangEnum language);

    ///<exception cref="SaveDoesntExistException"></exception>
    Task<SaveStateModel> GetStateBySaveId(int saveId);
}