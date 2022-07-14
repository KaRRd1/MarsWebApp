using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Extensions;

public static class HtmlHelperExtensions
{
    public static IEnumerable<SelectListItem> GetEnumValueTextSelectList<TEnum>(this IHtmlHelper htmlHelper)
        where TEnum : System.Enum
    {
        var enumType = typeof(TEnum);
        
        return new SelectList(System.Enum.GetValues(typeof(TEnum)).OfType<System.Enum>()
            .Select(x =>
            {
                var memInfo = enumType.GetMember(x.ToString()).First();
                var attribute = memInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
                var displayName = attribute.Length > 0 ? ((DisplayAttribute)attribute[0]).Name : null;

                return new SelectListItem
                {
                    Text = displayName ?? x.ToString(),
                    Value = x.ToString()
                };
            }), "Value", "Text");
    }
}