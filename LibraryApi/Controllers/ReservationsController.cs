using LibraryApi.Domain;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
	public class ReservationsController : Controller
	{

		LibraryDataContext Context;
		public ReservationsController(LibraryDataContext context)
		{
			Context = context;
		}

		[HttpPost("/reservations")]
		[ValidateModel]

		public async Task<ActionResult> AddAReservation([FromBody] PostReservationRequest request)
		{
			//1. map to a reservation
			var reservation = new Reservation
			{
				For = request.For,
				Books = string.Join(',', request.Books),
				ReservationCreated = DateTime.Now,
				Status = ReservationStatus.Pending


			};
			//2. Add to Database
			Context.Reservations.Add(reservation);
			await Context.SaveChangesAsync();

			var response = MapIt(reservation);
			return CreatedAtRoute("reservations#getbyid", new { id = response.ID }, response);

		}

		[HttpGet("/reservations/{id:int}", Name="reservations#getbyid")]

		public async Task<ActionResult<GetReservationItemResponse>> GetById(int id)
		{
			return Ok();
		}

		private GetReservationItemResponse MapIt(Reservation reservation)
		{
			var response = new GetReservationItemResponse
			{
				ID = reservation.ID,
				For = reservation.For,
				ReservationCreated = DateTime.Now,
				Status = reservation.Status,
				Books = reservation.Books.Split(',')
				.Select (id => Url.ActionLink("GetBookById", "Books", new { id = id })).ToList() 
			};

			return response;
		}
	}
}
