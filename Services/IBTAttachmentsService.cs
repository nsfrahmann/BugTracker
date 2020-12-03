using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public interface IBTAttachmentsService
    {
        public Task<byte[]> EncodeAttachmentAsync(IFormFile file);
        public string DecodeAttachment(byte[] data, string imagePath);
        public string GetFileIcon(string file);
        public string FormatFileSize(long bytes);
    }
}
