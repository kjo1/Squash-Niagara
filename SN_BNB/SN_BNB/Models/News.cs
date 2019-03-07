﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class News
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}