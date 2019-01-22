using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaOffer.DomainClasses
{
    public class User : BaseEntity
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsPhoneConfirmed { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsUserActive { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
