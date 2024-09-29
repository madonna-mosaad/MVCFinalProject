using Microsoft.AspNetCore.Http;
using System;
using System.IO;
namespace MVC.Helpers
{
    public class UploadFile
    {
        public static string Upload(IFormFile formFile,string folderName)
        {
            string FolderPath= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);
            string FileName =$"{Guid.NewGuid()}{formFile.FileName}";
            string FilePath = Path.Combine(FolderPath, FileName);
            using var FileStream = new FileStream(FilePath, FileMode.Create);
            formFile.CopyTo(FileStream);
            return FileName;
        }
        public static void Delete(string fileName, string folderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
