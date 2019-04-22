using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommonLibrary.Models.Errors;
using Microsoft.AspNetCore.Mvc;
using ware_house.Managers.Interfaces;
using ware_house.Models;
using ware_house.Models.QueryParams;
using ware_house.Models.Search;
using ware_house.Repositories.Interfaces;
using Warehouse.Models.Requests;
using Warehouse.Models.Responses;

namespace ware_house.Controllers
{
	/// <summary>
	/// Handles customers
	/// </summary>
	[Route("/api/v1/customers")]
	public class CustomersController : Controller
	{
		private readonly ICustomersRepository _customersRepository;
		private readonly ICustomerManager _customerManager;

		
		/// <summary>
		/// Constuctor
		/// </summary>
		/// <param name="customersRepository"></param>
		/// <param name="customerManager"></param>
		public CustomersController(ICustomersRepository customersRepository, ICustomerManager customerManager)
		{
			_customersRepository = customersRepository;
			_customerManager = customerManager;
		}

		/// <summary>
		/// Gets all customers
		/// </summary>
		/// <param name="queryParams"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(List<CustomerResponse>), 200)]
		public async Task<IActionResult> GetAllCustomers([FromQuery] CustomersQueryParams queryParams)
		{
			var customers = await _customersRepository.GetMultiple(queryParams != null ? new CustomersSearchOptions
			{
				Id = queryParams.Id,
				FirstName = queryParams.FirstName,
				LastName = queryParams.LastName,
				Age = queryParams.Age,
				CreationDate = queryParams.CreationDate
			} : null);

			return Ok(customers.Select(Mapper.Map<CustomerResponse>).ToList());
		}

		/// <summary>
		/// Gets single customer
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		[HttpGet("{customerId}")]
		[ProducesResponseType(typeof(CustomerResponse), 200)]
		[ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
		public async Task<IActionResult> GetCustomerById([FromRoute]string customerId)
		{
			var customer = await _customerManager.GetCustomer(customerId);

			if (customer == null)
				return NotFound(new NotFoundErrorResponse($"customer with id {customerId}"));

			return Ok(Mapper.Map<CustomerResponse>(customer));
		}

		/// <summary>
		/// Creates customer
		/// </summary>
		/// <param name="createCustomerRequest"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(CustomerResponse), 200)]
		[ProducesResponseType(typeof(BadRequestResponse), 400)]
		public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateRequest createCustomerRequest)
		{
			if (createCustomerRequest == null)
			{
				return BadRequest(new BadRequestResponse("invalid model"));
			}
			if (String.IsNullOrEmpty(createCustomerRequest.FirstName))
			{
				return BadRequest(new BadRequestResponse("Empty First Name"));
			}
			if (String.IsNullOrEmpty(createCustomerRequest.LastName))
			{
				return BadRequest(new BadRequestResponse("Empty Last Name"));
			}


			var newCustomer = Mapper.Map<Customer>(createCustomerRequest);
			newCustomer.Id = Guid.NewGuid().ToString("N");
			await _customersRepository.Create(newCustomer);

			return Ok(Mapper.Map<CustomerResponse>(newCustomer));
		}

		/// <summary>
		/// Updates or creates customer
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="updateCustomerRequest"></param>
		/// <returns></returns>
		[HttpPut("{customerId}")]
		[ProducesResponseType(typeof(CustomerResponse), 200)]
		[ProducesResponseType(typeof(BadRequestResponse), 400)]
		public async Task<IActionResult> UpdateOrCreateCustomer(string customerId, [FromBody] CustomerUpdateRequest updateCustomerRequest)
		{
			if (updateCustomerRequest == null)
			{
				return BadRequest(new BadRequestResponse("invalid model"));
			}

			if (updateCustomerRequest.Id != customerId)
			{
				return BadRequest(new BadRequestResponse("different id in body and path"));
			}
			if (String.IsNullOrEmpty(updateCustomerRequest.Id))
			{
				return BadRequest(new BadRequestResponse("empty id"));
			}
			if (String.IsNullOrEmpty(updateCustomerRequest.FirstName))
			{
				return BadRequest(new BadRequestResponse("empty FirstName"));
			}
			if (String.IsNullOrEmpty(updateCustomerRequest.LastName))
			{
				return BadRequest(new BadRequestResponse("empty LastName"));
			}

			var customer = Mapper.Map<Customer>(updateCustomerRequest);
			await _customersRepository.Replace(customer);

			return Ok(Mapper.Map<CustomerResponse>(customer));
		}

		/// <summary>
		/// Removes customer
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		[HttpDelete("{customerId}")]
		[ProducesResponseType(typeof(CustomerResponse), 200)]
		[ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
		public async Task<IActionResult> DeleteCustomerById([FromRoute]string customerId)
		{
			var removed = await _customersRepository.Remove(customerId);
			if (!removed)
				return NotFound(new NotFoundErrorResponse($"customer with id {customerId}"));

			return Ok();
		}


		/// <summary>
		/// Checks if customer exists
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		[HttpHead("{customerId}")]
		[ProducesResponseType( 200)]
		[ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
		public async Task<IActionResult> CustomerExists([FromRoute] string customerId)
		{
			var exists = await _customersRepository.Exists(customerId);
			if (!exists)
				return NotFound(new NotFoundErrorResponse($"customer with id {customerId}"));

			return Ok();
		}

		/// <summary>
		/// Gets nearest customers
		/// </summary>
		/// <param name="nearestQueryParams"></param>
		/// <returns></returns>
		[HttpGet("/nearest")]
		[ProducesResponseType(typeof(List<NearestCustomer>), 200)]
		[ProducesResponseType(typeof(BadRequestResponse), 400)]
		public async Task<IActionResult> GetNearestCustomers([FromQuery] NearestQueryParams nearestQueryParams)
		{
			//TODO: params validation
			var customers = await _customerManager.GetNearestCustomers(nearestQueryParams.Longitude,
				nearestQueryParams.Latitude, nearestQueryParams.Offset, nearestQueryParams.Limit);

			return Ok(customers);
		}

		/// <summary>
		/// Seeds data
		/// </summary>
		/// <returns></returns>
		[HttpPut("seed")]
		[ProducesResponseType(typeof(Customer), 200)]
		public async Task<IActionResult> Seed()
		{
			var customers = await _customersRepository.GetMultiple(null);
			foreach (var customer in customers)
			{
				if (await _customersRepository.Exists(customer.Id))
				{
					await _customersRepository.Replace(customer);
				}
				else
				{
					await _customersRepository.Create(customer);
				}
			}

			return Ok();
		}
	}
}