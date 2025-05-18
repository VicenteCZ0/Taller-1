using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CloudinaryDotNet.Actions;

namespace APITaller1.src.interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);

        Task<string> UploadImageAsync(IFormFile file); // devuelve URL
        Task<bool> DeleteImageAsync(string publicId);  // elimina usando el publicId

    }
}