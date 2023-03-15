using Donations_App.Models;

namespace Donations_App.Dtos.PatientCaseDtos
{
    public class CaseResponse
    {
        public IEnumerable<PatientCase> patientCases { get; set; }
        public int CurrentPage { get; set; }

        public int Pages { get; set; }
    }
}
