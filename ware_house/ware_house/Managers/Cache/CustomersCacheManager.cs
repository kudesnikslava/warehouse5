using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary.Cache.Interfaces;
using ware_house.Managers.Cache.Interfaces;
using ware_house.Models;

namespace ware_house.Managers.Cache
{
	class CustomersCacheManager : ICustomersCacheManager
	{
		private readonly IBaseCache _baseCache;

		public CustomersCacheManager(IBaseCache baseCache)
		{
			_baseCache = baseCache;
		}

		public Task<bool> AddCustomer(Customer customer)
		{
			return _baseCache.SetObjectToCache(GetCustomerCacheKey(customer.Id), customer, TimeSpan.FromMinutes(5));
		}

		public Task<Customer> GetCustomer(string id)
		{
			return _baseCache.GetObjectFromCache<Customer>(GetCustomerCacheKey(id));
		}

		private string GetCustomerCacheKey(string customerId)
		{
			return $"customer:{customerId}";
		}
	}
}
