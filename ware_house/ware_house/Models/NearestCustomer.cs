using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace ware_house.Models
{
    public class NearestCustomer : Customer
    {
	    /// <summary>
	    /// Distance
	    /// </summary>
	    [BsonElement("distance")]
	    public double Distance { get; set; }
	}
}
