using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Donations_App.Repositories.FileUploadedServices
{
    public class FileUploadedService : IFileUploadedService
    {
        private readonly IHostingEnvironment _environment;
        public FileUploadedService(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> UploadCategoryImagesAsync(IFormFile file )
        {
            string Pathcom = Path.Combine("//CategoryImages/", file.FileName);
            string HostUrl = "http://MBrother.somee.com";
            string PathImage = HostUrl + Pathcom;

            string filePathImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CategoryImages/", file.FileName);
            using (var stream = System.IO.File.Create(filePathImage))
            {
                await file.CopyToAsync(stream);
                stream.Close();
            }
            
            return PathImage;
        }
        //public async Task<string> GetUrlCategoryImage(string ImageName)
        //{
        //    string filePathImage = Path.Combine("//CategoryImages/", ImageName);
        //    string imageUrl=string.Empty;
        //    string HostUrl = "http://MBrother.somee.com";
        //    imageUrl = HostUrl + filePathImage;
        //    return imageUrl;
        //}

        public async Task<string> UploadCaseImagesAsync(IFormFile file)
        {

            string Pathcom = Path.Combine("//PatientCaseImages/", file.FileName);
            string HostUrl = "http://MBrother.somee.com";
            string PathImage = HostUrl + Pathcom;

            string filePathImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PatientCaseImages/", file.FileName);
            using (var stream = System.IO.File.Create(filePathImage))
            {
                await file.CopyToAsync(stream);
                stream.Close();
            }
            return PathImage;
        }

        //public async Task<string> GetUrlCaseImage(string ImageName)
        //{
        //    string filePathImage = Path.Combine("//PatientCaseImages/", ImageName);
        //    string imageUrl = string.Empty;
        //    string HostUrl = "http://MBrother.somee.com";
        //    imageUrl = HostUrl + filePathImage;
        //    return imageUrl;
        //}
    }
}
