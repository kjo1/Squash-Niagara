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
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
