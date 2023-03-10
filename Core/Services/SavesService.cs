using Core.Interfaces.Driven;
using Core.Interfaces.Driving;
using Core.Models.Exceptions;
using Core.Models.Executor;
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
        var user = await _userService.GetUser();
        //Experiment is obviosly unsuccessful, so i'll just throw expetions 
        var book = (await _booksStore.GetOne(genId)).GetValOrThrow();
        var state = await _bookExecutor.CreateInitState(book);
        await _savesStore.CreateNewSave(user.Id, genId, state,
            book.Description.Languages.OrderBy(l => l.Priority).First());
    }

    public async Task SwitchToSave(int saveId)
    {
        var user = await _userService.GetUser();
        var save = await _savesStore.GetStateBySaveId(saveId);
        if (save.UserId != user.Id) throw new SaveDoesntExistException();
        await _userService.UpdateCurrentSaveId(saveId);
    }

    public async Task<ReplicaModel> GetCurrentReplica()
    {
        var user = await _userService.GetStoredUser();
        var save = await _savesStore.GetStateBySaveId(user.CurrentSaveId ?? throw new UserCurrentSaveIsNull());
        if (save.UserId != user.Id) throw new SaveDoesntExistException();

        var book = (await _booksStore.GetOne(save.GenId)).GetValOrThrow();

        var replica = await _bookExecutor.GetCurrentReplica(book, save.State, save.Language);
        return replica;
    }

    public async Task<ReplicaModel> NextReplica(string answerText)
    {
        var user = await _userService.GetStoredUser();
        var save = await _savesStore.GetStateBySaveId(user.CurrentSaveId ?? throw new UserCurrentSaveIsNull());
        if (save.UserId != user.Id) throw new SaveDoesntExistException();
        var book = (await _booksStore.GetOne(save.GenId)).GetValOrThrow();

        var replica = await _bookExecutor.NextReplica(book, save.State, save.Language, null, answerText);
        await _savesStore.UpdateState(user.CurrentSaveId.Value, save.State);
        return replica;
    }
}