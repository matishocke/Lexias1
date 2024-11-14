using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    //redd
    public enum ResultState
    {
        Succeeded,
        Failed
    }
}
