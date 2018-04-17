using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class ForgotPassword
    {
        [Key]
        public string userId { get; set; }

        [Required]
        public DateTime createdDt { get; set; }

        [Required]
        public int attemptsCount { get; set; }
    }
}