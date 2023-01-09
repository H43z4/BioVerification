namespace Representative.Models.DatabaseModels.AddRep
{
    public class Tehsil
    {
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int ModifiedBy { get; set; }
    }

}
