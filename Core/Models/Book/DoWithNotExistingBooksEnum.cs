using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;

namespace Core.Models.Book;

[JsonConverter(typeof(SmartEnumValueConverter<DoWithNotExistingBooksEnum, string>))]
public sealed class DoWithNotExistingBooksEnum : SmartEnum<DoWithNotExistingBooksEnum, string>
{
    public static readonly DoWithNotExistingBooksEnum Nothing = new(nameof(Nothing), "nothing");
    public static readonly DoWithNotExistingBooksEnum Delete = new(nameof(Delete), "delete");
    public static readonly DoWithNotExistingBooksEnum Invisible = new(nameof(Invisible), "invisible");

    private DoWithNotExistingBooksEnum(string name, string value) : base(name, value)
    {
    }
}