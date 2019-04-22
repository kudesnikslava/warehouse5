using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ware_house.Models.QueryParams
{
    public class CustomersQueryParams
    {
	    /// <summary>
	    /// id
	    /// </summary>
	    [JsonProperty]
	    public string Id { get; set; }

		/// <summary>
		/// First name
		/// </summary>
		[JsonProperty]
		public string FirstName { get; set; }

		/// <summary>
		/// Last name
		/// </summary>
		[JsonProperty]
		public string LastName { get; set; }

		/// <summary>
		/// Age
		/// </summary>
		[JsonProperty]
		public byte Age { get; set; }

		/// <summary>
		/// Creation date
		/// </summary>
		[JsonProperty]
		public DateTime CreationDate { get; set; }
	}
}
