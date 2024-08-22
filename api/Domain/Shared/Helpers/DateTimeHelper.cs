
namespace api.Domain.Shared.Helpers;

public static class DateTimeHelper
{
    public static bool IsDataValidaFromString(this string data)
    {
        try
        {

            var date = DateTime.ParseExact(data, "yyyyMMdd", null);

            var minDate = new DateTime(1753, 1, 1);
            var maxDate = new DateTime(9999, 12, 31);

            return date > minDate && date < maxDate;
        }
        catch
        {
            return false;
        }
    }

    public static DateTime DataFromString(this string data)
    {
        try
        {
            return DateTime.ParseExact(data, "yyyyMMdd", null);
        }
        catch
        {
            return new DateTime(1900, 01, 01);
        }
    }

    public static DateTime? ConvertToDateTime(this string? data)
    {
        try
        {
            return DateTime.ParseExact(data ?? "", "yyyyMMdd", null);
        }
        catch
        {
            return null;
        }
    }

    public static DateTime DataHoraDeBrasilia
    {
        get
        {
            DateTime dateTime = DateTime.UtcNow;

            try
            {
                dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, Timezone);
            }
            catch
            {
                dateTime = DateTime.UtcNow;
            }

            return dateTime;
        }
    }

    public static TimeZoneInfo Timezone
    {
        get
        {
            var timeZoneId = Environment.GetEnvironmentVariable("TIME_ZONE_ID") ?? "America/Sao_Paulo";
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
    }
}
