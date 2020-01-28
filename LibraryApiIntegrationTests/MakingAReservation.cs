using LibraryApi;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests
{
	public class MakingAReservation : IClassFixture<CustomWebApplicationFactory<Startup>>
	{
		private readonly CustomWebApplicationFactory<Startup> Factory;

		public MakingAReservation(CustomWebApplicationFactory<Startup> factory)
		{
			Factory = factory;
		}

		[Fact]
		public async Task ServiceIsCalled()
		{
			var mockReservationProcessor = new Mock<ISendMessagesToTheReservationProcessor>();

			var client = Factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureTestServices(services =>
				{
					services.AddSingleton<ISendMessagesToTheReservationProcessor>(mockReservationProcessor.Object);
				});
			}).CreateClient();

			var reservation = new FakeReservation { For = "Sam", Books = new string[] { "1", "42" } };
			var response = await client.PostAsJsonAsync("/reservations", reservation);

			var responseReservation = response.Content.ReadAsAsync<FakeReservationResponse>();
			mockReservationProcessor.Verify(p => p.SendForProcessing(It.IsAny<GetReservationItemResponse>()), Times.Once());
		}
	}

	public class FakeReservation
	{
		public string For { get; set; }
		public string[] Books { get; set; }
	}


	public class FakeReservationResponse
	{
		public int id { get; set; }
		public string _for { get; set; }
		public string status { get; set; }
		public DateTime reservationCreated { get; set; }
		public string[] books { get; set; }
	}




}
