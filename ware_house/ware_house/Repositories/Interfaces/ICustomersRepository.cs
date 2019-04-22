using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibrary.Repositories.Interfaces;
using ware_house.Models;
using ware_house.Models.Search;

namespace ware_house.Repositories.Interfaces
{
	public interface ICustomersRepository : IBaseRepository<Customer, CustomersSearchOptions>
	{
		/// <summary>
		/// Gets 
		/// </summary>
		/// <param name="longitude"></param>
		/// <param name="latitude"></param>
		/// <param name="offset"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		Task<List<NearestCustomer>> GetNearestCustomers(double longitude, double latitude, int offset = 0, int limit = 10);
	}
}