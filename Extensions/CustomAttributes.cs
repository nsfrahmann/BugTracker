﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Extensions
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }
        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is { _maxFileSize} bytes.";
        }
    }

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;       
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage(extension));
                }
            }
            return ValidationResult.Success;
        }
        public string GetErrorMessage(string ext)
        {
            return $"The file extension {ext} is not allowed!";
        }
    }

    public class MaxFileCountAttribute : ValidationAttribute
    {
        private readonly int _maxFileCount;
        public MaxFileCountAttribute(int maxFileCount)
        {
            _maxFileCount = maxFileCount;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as List<IFormFile>;
            if (file != null)
            {
                if (file.Count > _maxFileCount)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }
        public string GetErrorMessage()
        {
            return $"Cannot upload more than { _maxFileCount} attachments.";
        }
    }
}
