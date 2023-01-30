using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;

namespace Utils.Language;

[JsonConverter(typeof(SmartEnumValueConverter<LangEnum, string>))]
public sealed class LangEnum : SmartEnum<LangEnum, string>
{
    public static readonly LangEnum En = new(nameof(En), "en", "eng", "english")
        { ErrorMessage = "String resource not found", LangName = "English", Priority = 0 };

    public static readonly LangEnum Ru = new(nameof(Ru), "ru", "rus", "russian")
        { ErrorMessage = "Строка не найдена", LangName = "Русский", Priority = 1 };

    public static readonly LangEnum De = new(nameof(De), "de")
        { ErrorMessage = "Zeichenkette nicht gefunden", LangName = "Deutch", Priority = 2 };

    private readonly string[] _aliases;

    private LangEnum(string name, string value, params string[] aliases) : base(name, value)
    {
        _aliases = new[] { value }.Concat(aliases).ToArray();
    }

    public IEnumerable<string> Aliases => _aliases;

    public required string ErrorMessage { get; init; }
    public required string LangName { get; init; }
    public required int Priority { get; set; }

    public static LangEnum GetNearestLang(string languageCode)
    {
        TryFromValue(languageCode, out var result);
        result ??= En;
        return result;
    }

    public static LangEnum GetAvailableLang(LangEnum neededLang, LangEnum[] availableLangs)
    {
        return availableLangs.Contains(neededLang) ? neededLang : availableLangs.OrderBy(l => l.Value).First();
    }

    public static LangEnum DefineLanguageOrEng(string? langCode)
    {
        var lang = langCode == null ? List.FirstOrDefault(l => l.Aliases.Contains(langCode)) ?? En : En;
        return lang;
    }
}