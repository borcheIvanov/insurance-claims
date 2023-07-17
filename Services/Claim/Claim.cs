using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Services.Claim;

public class Claim: IIdMarker
{
    public const decimal MaximumDamageAllowed = 100_000;
    
    [JsonProperty(PropertyName = "id")] 
    public string Id { get; set; } = Guid.NewGuid().ToString();
        
    [JsonProperty(PropertyName = "coverId")]
    public string CoverId { get; set; } = null!;

    [JsonProperty(PropertyName = "created")]
    public DateTime Created { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = null!;

    [JsonProperty(PropertyName = "claimType")]
    public ClaimType Type { get; set; }

    [JsonProperty(PropertyName = "damageCost")]
    public decimal DamageCost { get; set; }

    public Claim(Cover.Cover cover, DateTime created, string name, ClaimType claimType, decimal damage)
    {
        if (damage > 100_000) {
            throw new ArgumentException("DamageCost cannot not exceed 100.000");
        }

        if (!CreatedIsInCoverPeriod(cover, created)) {
            throw new ArgumentException("Created Date must be in Cover Period");
        }

        CoverId = cover.Id;
        Created = created;
        Name = name;
        Type = claimType;
        DamageCost = damage;
    }

    public Claim() { }
    
    public static bool CreatedIsInCoverPeriod(Cover.Cover cover, DateTime created)
    {
        return cover.StartDate.ToDateTime(TimeOnly.MinValue) < created
               && cover.EndDate.ToDateTime(TimeOnly.MaxValue) > created;
    }
}

[PublicAPI]
public enum ClaimType
{
    Collision = 0,
    Grounding = 1,
    BadWeather = 2,
    Fire = 3
}