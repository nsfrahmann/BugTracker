using BugTracker.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        [MaxFileSize((1024 * 1024 * 5))]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile Avatar { get; set; }
        public string AvatarName { get; set; }
        public byte[] AvatarData { get; set; }
        public string AvatarContentType { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        [MaxFileSize((1024 * 1024 * 5))]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile BannerImage { get; set; }
        public string BannerName { get; set; }
        public byte[] BannerData { get; set; }
        public string BannerContentType { get; set; }
        public string Bio { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public BTUser User { get; set; }
    }
}
