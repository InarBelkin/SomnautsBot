using Core.Interfaces.Driven;
using Core.Interfaces.Driving;
using Core.Models.Save;
using LanguageExt.Common;
using Utils;

namespace Core.Services;

public class SavesService : ISavesService
{
    private readonly IBookExecutor _bookExecutor;
    private readonly IBooksStore _booksStore;
    private readonly ISavesStore _savesStore;
    private readonly IUserService _userService;

    public SavesService(ISavesStore savesStore, IUserService userService, IBooksStore booksStore,
        IBookExecutor bookExecutor)
    {
        _savesStore = savesStore;
        _userService = userService;
        _booksStore = booksStore;
        _bookExecutor = bookExecutor;
    }

    public async Task<Result<IEnumerable<BookSaveItemModel>>> GetSavesOfThisUser(Guid genId)
    {
        var user = await _userService.GetUser();
        return await _savesStore.GetSavesByUser(user.Id, genId);
    }

    public async Task CreateNewSave(Guid genId)
    {
        //Experiment is obviosly unsuccessful, so i'll throw expetions 
        var user = await _userService.GetUser();
        var book = (await _booksStore.GetOne(genId)).GetValOrThrow();
        var state = await _bookExecutor.CreateInitState(book);
        await _savesStore.CreateNewSave(user.Id, genId, state,
            book.Description.Languages.OrderBy(l => l.Priority).First());
    }
}