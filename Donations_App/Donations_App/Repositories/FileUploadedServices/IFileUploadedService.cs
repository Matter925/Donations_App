namespace Donations_App.Repositories.FileUploadedServices
{
    public interface IFileUploadedService
    {
        Task <string> UploadCategoryImagesAsync(IFormFile file );

        //Task<string> GetUrlCategoryImage(string ImageName);
        Task<string> UploadCaseImagesAsync(IFormFile file);

        //Task<string> GetUrlCaseImage(string ImageName);

    }
}
