using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
	public class News : IValidatableObject
	{
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
		public string Title { get; set; }

		[Required]
        [MaxLength(500)]
		public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date > DateTime.Today)
            {
                yield return new ValidationResult("Date of post cannot be in the past");
            }
        }
    }
}
