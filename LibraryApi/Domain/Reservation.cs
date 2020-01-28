﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryApi.Domain

{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum ReservationStatus { Pending, Approved, Cancelled }
	public class Reservation
	{

		public int ID { get; set; }

		public string For { get; set; }
		public ReservationStatus Status { get; set; }
		public DateTime ReservationCreated { get; set; }

		public string Books { get; set; }

	}
}

