using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Services.Cover;

public class Cover: IIdMarker
{
    public const decimal DailyPremiumBase = 1250;
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
        Type = coverType;

        var calculator = PremiumCalculatorFactory.Create(coverType, DailyPremiumBase);
        Premium = calculator.CalculatePremium(startDate, endDate);
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
        var dateFrom = startDate.ToDateTime(TimeOnly.MinValue).Date;
        var dateTo = endDate.ToDateTime(TimeOnly.MaxValue).Date;

        return dateTo <= dateFrom.AddYears(1);
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