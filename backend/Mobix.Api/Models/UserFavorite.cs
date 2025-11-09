using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mobix.Api.Models
{
    public class UserFavorite
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public int SmartphoneId { get; set; }

        public User User { get; set; }
        public Smartphone Smartphone { get; set; }
    }
}