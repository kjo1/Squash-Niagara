using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
	public class News
	{
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
		public string Title { get; set; }

		[Required]
        [MaxLength(5000)]
		public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}
