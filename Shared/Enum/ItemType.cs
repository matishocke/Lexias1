using System.Text.Json.Serialization;

namespace Shared.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ItemType
    {
        Tops = 1,
        Bottoms = 2,
        Dresses = 3,
        Outerwear = 4,
        Accessories = 5,
        Shoes = 6,
    }
}
