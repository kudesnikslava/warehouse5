using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ware_house.Managers.Cache.Interfaces;
using ware_house.Managers.Interfaces;
using ware_house.Models;
using ware_house.Repositories.Interfaces;

namespace ware_house.Managers
{
	public class CustomerManager : ICustomerManager
	{
		private readonly ICustomersRepository _CustomersRepository;
		private readonly ICustomersCacheManager _CustomersCacheManager;

		public CustomerManager(ICustomersRepository CustomersRepository,
			ICustomersCacheManager CustomersCacheManagerManager)
		{
			_CustomersRepository = CustomersRepository;
			_CustomersCacheManager = CustomersCacheManagerManager;
		}

		public async Task<Customer> GetCustomer(string CustomerId)
		{
			var cached = await _CustomersCacheManager.GetCustomer(CustomerId);
			if (cached == null)
			{
				var result = await _CustomersRepository.GetById(CustomerId);
				if (result != null)
				{
					await _CustomersCacheManager.AddCustomer(result);
				}

				return result;
			}

			return cached;
		}


		public Task<List<NearestCustomer>> GetNearestCustomers(double longitude, double latitude, int offset, int limit)
		{
			return _CustomersRepository.GetNearestCustomers(longitude, latitude, offset, limit);
		}
	}
}
