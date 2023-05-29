namespace Donations_App.Dtos.PatientCaseDtos
{
    public class FilterDto
    {
        public int ? CategoryId { get; set; }
        public double ? GreaterAmount { get; set; }
        public double ? lessAmount { get; set; }
        public bool IsComplete { get; set; }

    }
}
