namespace Donations_App.Dtos.PatientCaseDtos
{
    public class PatientCaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public double Amount { get; set; }
        public int CategoryId { get; set; }
    }
}
