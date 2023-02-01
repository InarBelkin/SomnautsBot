namespace Core.Interfaces.Driven;

public interface ISavesStore
{
    public Task<Dictionary<Guid, int>> GetSavesCountByUserId(int userId);
}