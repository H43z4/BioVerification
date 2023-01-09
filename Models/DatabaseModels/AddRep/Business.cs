namespace Representative.Models.DatabaseModels.AddRep
{
    public class Business
    {
        public int BusinessId { get; set; }
        public int BusinessTypeId { get; set; }
        public int BusinessSectorId { get; set; }
        public int BusinessStatusId { get; set; }
        public string BusinessRegNo { get; set; }
        public string BusinessName { get; set; }
        public string Email { get; set; }
        public string NTN { get; set; }
        public string FTN { get; set; }
        public string STN { get; set; }
        public string AccountPassword { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
    }

}
