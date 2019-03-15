﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Player
    {
        public int ID { get; set; }

        [Display(Name ="Player")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(45, ErrorMessage = "First name cannot be more than 45 characters long.")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(45, ErrorMessage = "Middle name cannot be more than 45 characters long.")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(100, ErrorMessage = "Last name cannot be more than 100 characters long.")]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number (no spaces).")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64 Phone { get; set; }

        [Required(ErrorMessage = "Position is required.(1 to 4)")]
        [Range(1,4)]
        public int Position { get; set; }

        [Display(Name = "Win(s)")]
        public int Win { get; set; }

        [Display(Name = "Loss(es) ")]
        public int Loss { get; set; }

        [Required(ErrorMessage = "You must select a Primary Team")]
        [Display(Name ="Team")]
        public int TeamID { get; set; }

        [Required(ErrorMessage = "You must select if you want to hide your phone number from other players.")]
        [Display(Name = "Hide your phone number from other players?")]
        public bool PlayerWantsInfoHidden { get; set; }

        public virtual Team Team { get; set; }

        public ICollection<AssignedMatchPlayer> AssignedMatchPlayers { get; set; }

    }
}
