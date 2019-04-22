using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace ware_house.Models
{
	/// <summary>
	/// Customer entity
	/// </summary>
    public class Customer : BaseModelWithId
    {
		// public string Id { get; set; }

		[BsonElement("firstName")]
		public string FirstName { get; set; }

	    [BsonElement("lastName")]
		public string LastName { get; set; }

	    [BsonElement("age")]
		public byte Age { get; set; }

	    [BsonElement("creationDate")]
		public DateTime CreationDate { get; set; }

	    /// <summary>
	    /// Location
	    /// </summary>
	    [BsonElement("location")]
	    public List<double> Location { get; set; }
	}
}
