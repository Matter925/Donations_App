namespace Donations_App.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int governorateId { get; set; }
        public Governorate governorate { get; set; }
        
    }
}
