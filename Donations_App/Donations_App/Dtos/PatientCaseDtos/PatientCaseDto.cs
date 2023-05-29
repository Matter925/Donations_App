namespace Donations_App.Dtos.PatientCaseDtos
{
    public class PatientCaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public double Amount { get; set; }
        public int CategoryId { get; set; }
        public string Address { get; set; }
        public int LimitTime { get; set; }
        public int Rate { get; set; }
        public string UserId { get; set; }
    }
}
