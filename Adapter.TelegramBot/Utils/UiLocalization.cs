namespace Adapter.TelegramBot.Utils;

public class UiLocalization
{
    public required Dictionary<string, string> Help { get; init; }
    public required Dictionary<string, string> SelectBook { get; init; }
    public required Dictionary<string, string> SelectBookDescription { get; init; }
    public required Dictionary<string, string> SelectBookSave { get; init; }
    public required Dictionary<string, string> SelectBookSaveItem { get; init; }
    public required Dictionary<string, string> SelectBookSaveCreate { get; init; }
    public required Dictionary<string, string> SelectUiLang { get; init; }
    public required Dictionary<string, string> UiLangSelected { get; init; }

    public required Dictionary<string, string> SelectBookLang { get; init; }
    public required Dictionary<string, string> SelectBookLangNoCurrentBook { get; init; }
}