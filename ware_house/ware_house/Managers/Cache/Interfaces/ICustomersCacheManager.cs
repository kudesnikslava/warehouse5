using System.Threading.Tasks;
using ware_house.Models;

namespace ware_house.Managers.Cache.Interfaces
{
	public interface ICustomersCacheManager
	{
		/// <summary>
		/// Adds to cache
		/// </summary>
		/// <param name="customer"></param>
		/// <returns></returns>
		Task<bool> AddCustomer(Customer customer);

		/// <summary>
		/// Gets from cache
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<Customer> GetCustomer(string id);
	}
}