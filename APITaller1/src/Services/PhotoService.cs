using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.interfaces;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace APITaller1.src.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IConfiguration config)
        {
            var acc = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            if (file.Length == 0 || file.Length > 100 * 1024 * 1024) // 100MB
                throw new ArgumentException("Archivo no válido o excede el tamaño permitido (100MB)");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Formato de imagen no compatible");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "products"
            };

            ImageUploadResult? uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult;
        }


        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Result == "not found")
                return new DeletionResult { Result = "ok" };

            return result;
        }


        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var uploadResult = await AddPhotoAsync(file);

            // ✅ Asegúrate de usar SecureUrl y convertirlo a string
            return uploadResult.SecureUrl?.ToString(); // <-- Esto devuelve la URL como string
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var result = await DeletePhotoAsync(publicId);
            return result.Result == "ok";
        }
    }
}