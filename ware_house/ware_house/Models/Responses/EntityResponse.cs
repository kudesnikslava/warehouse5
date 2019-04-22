﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Warehouse.Models
{
	/// <summary>
	/// Entiry response
	/// </summary>
    public class EntityResponse
    {
	    [JsonProperty(Required = Required.Always)]
		public string Id { get; set; }

	    [JsonProperty(Required = Required.Always)]
		public string Name { get; set; }

	    [JsonProperty(Required = Required.Always)]
		public DateTime CreatedDate { get; set; }

	    [JsonProperty(Required = Required.Always)]
		public int AvailableQuantity { get; set; }
	}
}
