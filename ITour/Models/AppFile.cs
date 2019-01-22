using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace ITour.Models
{
    public class AppFile
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
    }

    public class AppFileOptions
    {
        public string RootPath { get; set; }
        public string FilesPath { get; set; }
        public string ThumbnailsPath { get; set; }
        public int ThumbnailsSize { get; set; } = 75;
        public int ThumbnailsQuality { get; set; } = 75;
    }

    public interface IAppFileHandler
    {
        AppFileOptions AppFileOptions { get; }
        Task CreateFileAsync(AppFile appFile, IFormFile uploadedFile, ModelStateDictionary modelState);
        string GetFilePath(AppFile appFile);
        string GetThumbnailPath(AppFile appFile);
    }

    public class AppFileHandler : IAppFileHandler
    {
        public AppFileOptions AppFileOptions { get; }

        public AppFileHandler(IOptionsSnapshot<AppFileOptions> namedOptionsAccessor)
        {
            AppFileOptions = namedOptionsAccessor.Get("AppFiles");
        }

        public async Task CreateFileAsync(AppFile appFile, IFormFile uploadedFile, ModelStateDictionary modelState)
        {

            if (uploadedFile == null)
                modelState.AddModelError("UploadedFileIsNull", "Загружаемый файл не найден");

            if (uploadedFile.Length == 0)
                modelState.AddModelError("UploadedFileIsEmpty", "Загружаемый файл пустой");

            if (uploadedFile.Length > 1048576)
                modelState.AddModelError("UploadedFileToLarge", "Загружаемый файл слишком большой");

            if (uploadedFile.ContentType.ToLower() != "image/jpeg")
                modelState.AddModelError("UploadedFileNotJpeg", "Загружаемый файл не jpeg");

            if (modelState.IsValid)
            {
                string fileName = $"{appFile.Id}_{appFile.Name}";

                using (var fileStream = new FileStream(GetFilePath(appFile), FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                CreateThumbnail(GetFilePath(appFile), GetThumbnailPath(appFile));
            }
        }

        private void CreateThumbnail(string inputPath, string outputPath)
        {
            try
            {
                using (var image = new MagickImage(inputPath))
                {
                    image.Resize(AppFileOptions.ThumbnailsSize, AppFileOptions.ThumbnailsSize);

                    image.Strip();
                    image.Quality = AppFileOptions.ThumbnailsQuality;
                    image.Write(outputPath);
                }
            }
            catch (MagickMissingDelegateErrorException) { throw; }
            catch (MagickDelegateErrorException) { throw; }
            catch { throw; }
        }

        public string GetFilePath(AppFile appFile) =>
            $"{AppFileOptions.RootPath}\\{appFile.TenantId}\\{AppFileOptions.FilesPath}\\{appFile.Id}_{appFile.Name}";

        public string GetThumbnailPath(AppFile appFile) =>
            $"{AppFileOptions.RootPath}\\{appFile.TenantId}\\{AppFileOptions.ThumbnailsPath}\\{appFile.Id}_{appFile.Name}";
    }
}
