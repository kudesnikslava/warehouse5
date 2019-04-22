using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary.Config;
using CommonLibrary.Repositories.Implementations;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ware_house.Models;
using ware_house.Models.Search;
using ware_house.Repositories.Interfaces;

namespace ware_house.Repositories.Implementations
{
    public class MongoDbCustomersRepository : BaseMongoDbRepository<Customer, CustomersSearchOptions>, ICustomersRepository
	{
		private readonly IMongoDatabase _db;

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="client"></param>    
		public MongoDbCustomersRepository(IOptions<MongoConfiguration> configuration, IMongoClient client)
		{
			_db = client.GetDatabase(configuration.Value.DbName);
			Collection.Indexes.CreateOne(
				new CreateIndexModel<Customer>(Builders<Customer>.IndexKeys.Geo2DSphere(f => f.Location)));
		}

		/// <inheritdoc />
		protected override IMongoCollection<Customer> Collection => _db.GetCollection<Customer>("customers");

		/// <inheritdoc />
		protected override FilterDefinition<Customer> GetMultipleItemsQuery(CustomersSearchOptions searchOptions)
		{
			var query = FilterDefinition<Customer>.Empty;
			if (searchOptions != null)
			{
				if (!String.IsNullOrEmpty(searchOptions.Id))
					query &= Builders<Customer>.Filter.Eq(c => c.Id, searchOptions.Id);
				if (!String.IsNullOrEmpty(searchOptions.FirstName))
					query &= Builders<Customer>.Filter.Eq(c => c.FirstName, searchOptions.FirstName);
				if (!String.IsNullOrEmpty(searchOptions.LastName))
					query &= Builders<Customer>.Filter.Eq(c => c.LastName, searchOptions.LastName);
				//if (searchOptions.CreationDate.HasValue)
				//	query &= Builders<Customer>.Filter.Lte(c => c.Date, searchOptions.DateTo.Value);
				//if (searchOptions.DateFrom.HasValue)
				//	query &= Builders<Customer>.Filter.Gte(c => c.Date, searchOptions.DateFrom.Value);
			}

			return query;
		}

		/// <inheritdoc />
		public Task<List<NearestCustomer>> GetNearestCustomers(double longitude, double latitude, int offset = 0,
			int limit = 10)
		{
			var geoNearOptions = new BsonDocument
			{
				{
					"near", new BsonDocument
					{
						{"type", "Point"},
						{"coordinates", new BsonArray {longitude, latitude}},
					}
				},
				{"distanceField", "distance"},
				{"distanceMultiplier", 0.001},
                //{"maxDistance", 1000 * 1000},
                //{"minDistance", 0 * 1000},
                {"spherical", true},
				{"num", 1000}
			};
			var aggregation = Collection.Aggregate()
				.AppendStage<NearestCustomer>(new BsonDocument { { "$geoNear", geoNearOptions } })
				.Skip(offset)
				.Limit(limit);

			return aggregation.ToListAsync();

		}
	}
}
