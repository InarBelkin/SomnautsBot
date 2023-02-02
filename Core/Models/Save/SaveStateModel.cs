using System.Dynamic;
using Utils.Language;

namespace Core.Models.Save;

public record SaveStateModel(int Id, int UserId, Guid GenId, LangEnum Language, ExpandoObject State);