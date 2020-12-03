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
    public class TicketAttachment
    {
        public int Id { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        [MaxFileSize((1024 * 1024 * 5))]
        [MaxFileCount(3)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".pdf" })]
        public List<IFormFile> FormFile { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public int TicketId { get; set; }

        public string UserId { get; set; }
        public virtual BTUser User { get; set; }
    }
}
