namespace Web.Extensions;

public static class DateTimeExtensions
{
    public static string ToRelativeDate(this DateTime dateTime)
    {
        var delta = DateTime.Now.Subtract(dateTime);
        var totalDays = (int)delta.TotalDays;

        var years = totalDays / 365;
        if (years > 0)
            return $"{years} {Pluralize(years, "year")} ago";

        var months = totalDays / 30;
        if (months > 0)
            return $"{months} {Pluralize(months, "month")} ago";

        if (totalDays > 0)
            return $"{totalDays} {Pluralize(totalDays, "day")} ago";
        
        if (delta.Hours > 0)
            return $"{delta.Hours} {Pluralize(delta.Days, "hour")} ago";

        if (delta.Minutes == 0)
            return "Now";

        return $"{delta.Minutes} {Pluralize(delta.Minutes, "minute")} ago";
    }

    private static string Pluralize(int value, string word)
    {
        return value == 1 ? word : $"{word}s";
    }
}