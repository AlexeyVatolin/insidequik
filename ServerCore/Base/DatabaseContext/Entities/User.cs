using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using Inside.Core.Base.DatabaseContext.Entities;

namespace ServerCore.Base.DatabaseContext.Entities
{
    public class User : IdentityUser//, IInsideUser
    {
        private DateTime? _createdDate;
        public DateTime? CreatedDate
        {
            set { _createdDate = value; }
            get
            {
                if (_createdDate == null)
                {
                    _createdDate = DateTime.UtcNow;
                }
                return _createdDate.Value;
            }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public decimal? MaxOrderLimit { get; set; }
        public decimal? MinOrderLimit { get; set; }
        public decimal? MaxDayLimit { get; set; }
        public DateTime? LastSuccessLoginTime { get; set; }
        public UserStatus UserStatus { get; set; }
        public DateTime? FreezeEndDateTime { set; get; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        //TODO:посмотреть нужны ли они
        //public ICollection<Event> Events { set; get; }
        public ICollection<OrderHistory> OrderHistories { set; get; }
    }
}
