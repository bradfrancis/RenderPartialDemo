using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RenderPartialDemo.Models
{
    public class Greeting
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [AllowHtml]
        public string RawHtml { get; set; }

        public DateTime? DateProcessed { get; set; }
    }
}