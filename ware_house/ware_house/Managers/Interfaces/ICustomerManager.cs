using System.Collections.Generic;
using System.Threading.Tasks;
using ware_house.Models;

namespace ware_house.Managers.Interfaces
{
	public interface ICustomerManager
	{
		Task<Customer> GetCustomer(string customerId);

		/// <summary>
		/// Gets nearest Customer
		/// </summary>
		/// <param name="longitude"></param>
		/// <param name="latitude"></param>
		/// <param name="offset"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		Task<List<NearestCustomer>> GetNearestCustomers(double longitude, double latitude, int offset, int limit);
	}
}