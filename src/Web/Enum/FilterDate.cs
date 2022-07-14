using System.ComponentModel.DataAnnotations;

namespace Web.Enum;

public enum FilterDate
{
    [Display(Name = "Today")]
    Today,
    [Display(Name = "This week")]
    ThisWeek,
    [Display(Name = "This month")]
    ThisMonth
}