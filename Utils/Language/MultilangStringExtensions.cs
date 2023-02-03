namespace Utils.Language;

public static class MultilangStringExtensions
{
    public static string WithErrorString(this Dictionary<string, string> dictionary, LangEnum key)
    {
        return dictionary.TryGetValue(key.Value, out var fromDict) ? fromDict : key.ErrorMessage;
    }

    public static string NearestLang(this Dictionary<string, string> dictionary, LangEnum key)
    {
        if (dictionary.TryGetValue(key.Value, out var startLang)) return startLang;

        foreach (var lang in LangEnum.List.OrderBy(l => l.Priority))
            if (dictionary.TryGetValue(lang, out var priorLang))
                return priorLang;

        return key.ErrorMessage;
    }
}