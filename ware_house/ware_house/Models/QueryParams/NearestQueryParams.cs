using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ware_house.Models.QueryParams
{
    public class NearestQueryParams
    {
	    /// <summary>
	    /// Lng
	    /// </summary>
	    [JsonProperty]
	    public double Longitude { get; set; }

	    /// <summary>
	    /// Ltd
	    /// </summary>
	    [JsonProperty]
	    public double Latitude { get; set; }

	    /// <summary>
	    /// Offset
	    /// </summary>
	    [JsonProperty]
	    public int Offset { get; set; } = 0;

	    /// <summary>
	    /// Limit
	    /// </summary>
	    [JsonProperty]
	    public int Limit { get; set; } = 10;
	}
}
