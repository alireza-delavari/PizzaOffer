using PizzaOffer.Areas.Web.Models.Common;
using System;

namespace PizzaOffer.Areas.Web.Models.Admin
{
    public class UserViewModel : BaseViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AvatarImage { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsPhoneConfirmed { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsUserActive { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LastVisitDate { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string SerialNumber { get; set; }
    }
}
