using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.API.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }

		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		public string? UserName { get; set; }

		public string? Password { get; set; }

		public string? Token { get; set; }

		public string? Role { get; set; }

		public string? Email { get; set; }
	}
}

