using Donations_App.Dtos.PatientCaseDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;

namespace Donations_App.Repositories.PatientCaseServices
{
    public interface IPatientCaseRepository
    {
        Task<CaseResponse> GetAllPatientsCases(int page , int limit);
        Task<CaseResponse> CompletedPatientsCases(int page, int limit);
        Task<IEnumerable<PatientCase>> GetByCategoryId(int categoryId);
        Task<PatientCase> GetPatientCaseByID(int id);
        Task<GeneralRetDto> CreatePatientCase(PatientCaseDto Dto);
        Task<GeneralRetDto> UpdatePatientCase(int id , PatientCaseDto dto);
        Task<GeneralRetDto> DeletePatientCase(int id);
        Task<GeneralRetDto> IncreamentAmountPaid(int CartId);

        Task<CaseResponse> GetUserPatientCases(string UserId , int page, int limit);
        Task<CaseResponse> Search(string query , int page, int limit);
        Task<CaseResponse> Filter(FilterDto dto, int page, int limit);
    }
}
