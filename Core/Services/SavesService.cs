using Core.Interfaces.Driven;
using Core.Interfaces.Driving;
using Core.Models.Save;
using LanguageExt.Common;

namespace Core.Services;

public class SavesService : ISavesService
{
    private readonly ISavesStore _savesStore;
    private readonly IUserService _userService;

    public SavesService(ISavesStore savesStore, IUserService userService)
    {
        _savesStore = savesStore;
        _userService = userService;
    }

    public async Task<Result<IEnumerable<BookSaveItemModel>>> GetSavesOfThisUser(Guid genId)
    {
        var user = await _userService.GetUser();
        return await _savesStore.GetSavesByUser(user.Id, genId);
    }
}