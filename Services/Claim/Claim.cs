using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Services.Claim;

public class Claim: IIdMarker
{
    [JsonProperty(PropertyName = "id")] 
    public string Id { get; set; } = null!;
        
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

}

[PublicAPI]
public enum ClaimType
{
    Collision = 0,
    Grounding = 1,
    BadWeather = 2,
    Fire = 3
}