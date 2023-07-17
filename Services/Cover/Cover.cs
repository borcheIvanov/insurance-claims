using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Services.Cover;

public class Cover: IIdMarker
{
    public Cover(DateOnly startDate,  DateOnly endDate, CoverType coverType)
    {
        if (!DateIsNotInPast(startDate)) {
            throw new ArgumentException("Start date must not be in the past");
        }

        if (!PeriodIsUnderYear(startDate, endDate)) {
            throw new ArgumentException("Total insurance period cannot exceed 1 year");
        }

        StartDate = startDate;
        EndDate = endDate;
        Premium = ComputePremium(startDate, endDate, coverType);
    }

    public Cover() { }
    
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty(PropertyName = "startDate")]
    public DateOnly StartDate { get; set; }
    
    [JsonProperty(PropertyName = "endDate")]
    public DateOnly EndDate { get; set; }
    
    [JsonProperty(PropertyName = "claimType")]
    public CoverType Type { get; set; }

    [JsonProperty(PropertyName = "premium")]
    public decimal Premium { get; set; }

    public static bool DateIsNotInPast(DateOnly startDate)
    {
        var dateTimeNow = DateTime.UtcNow;
        var dateOnlyNow = new DateOnly(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day);
        return startDate >= dateOnlyNow;
    }
    
    public static bool PeriodIsUnderYear(DateOnly startDate, DateOnly endDate)
    {
        var dateFrom = startDate.ToDateTime(TimeOnly.MinValue);
        var dateTo = endDate.ToDateTime(TimeOnly.MaxValue);

        return dateTo < dateFrom.AddYears(1);
    }
    
    private static decimal ComputePremium(DateOnly startDate,  DateOnly endDate, CoverType coverType)
    {
        var multiplier = 1.3m;
        if (coverType == CoverType.Yacht)
        {
            multiplier = 1.1m;
        }

        if (coverType == CoverType.PassengerShip)
        {
            multiplier = 1.2m;
        }

        if (coverType == CoverType.Tanker)
        {
            multiplier = 1.5m;
        }

        var premiumPerDay = 1250 * multiplier;
        var insuranceLength = endDate.DayNumber - startDate.DayNumber;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            if (i < 30) totalPremium += premiumPerDay;
            if (i < 180 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
            else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
            if (i < 365 && coverType != CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
            else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
        }

        return totalPremium;
    }
}

[PublicAPI]
public enum CoverType
{
    Yacht = 0,
    PassengerShip = 1,
    ContainerShip = 2,
    BulkCarrier = 3,
    Tanker = 4
}