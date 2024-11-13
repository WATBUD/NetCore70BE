using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

public static class FileHelper
{
    public static async Task<List<string>> UploadImagesAsync(List<IFormFile> images, string rootPath)
    {
        var uploadResults = new List<string>();
        var imagesDirectory = Path.Combine(rootPath, "images");

        if (!Directory.Exists(imagesDirectory))
        {
            Directory.CreateDirectory(imagesDirectory);
        }

        foreach (var image in images)
        {
            if (image.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(imagesDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                uploadResults.Add($"/images/{fileName}");
            }
        }

        return uploadResults;
    }

    public static void DeleteImages(List<string> oldImagePaths, string rootPath)
    {
        foreach (var oldImagePath in oldImagePaths)
        {
            var fullPath = Path.Combine(rootPath, oldImagePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }

    public static string UploadImageFileToServer(IFormFile file, bool getDrivePath = false)
    {
        if (file != null && file.Length > 0)
        {
            // 验证文件类型
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (allowedExtensions.Contains(fileExtension))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"); // 使用时间戳生成唯一标识符
                var newFileName = $"{fileNameWithoutExtension}_{timeStamp}{fileExtension}";
                var newDrivePath = Path.Combine("F:\\Projectlibrary\\NetCoreBE\\UserImageLibrary", newFileName);

                using (var stream = new FileStream(newDrivePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return getDrivePath ? newFileName : "updated successfully";
            }
            else
            {
                return "Only supports uploading JPG and PNG files";
            }
        }
        else
        {
            return "No file uploaded";
        }
    }


}
