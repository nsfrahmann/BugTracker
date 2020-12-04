using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class BTUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "FirstName")]
        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public char FirstLetter { get { return FirstName.ElementAt(0); } }
        public char LastLetter { get { return LastName.ElementAt(0); } }
        public string Initials { get { return $"{FirstLetter}{LastLetter}"; } }
        public string AvatarName { get; set; }
        public byte[] AvatarData { get; set; }
        public string AvatarContentType { get; set; }

        public List<ProjectUser> ProjectUsers { get; set; }
    }
}
