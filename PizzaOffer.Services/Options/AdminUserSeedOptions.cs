using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaOffer.Services.Options
{
    public class AdminUserSeedOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
    }
}
