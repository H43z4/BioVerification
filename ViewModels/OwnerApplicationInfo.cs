namespace Representative.ViewModels
{
    public class OwnerApplicationInfo
    {
        public Int64 ApplicationId { get; set; }
        public string RegistrationNo { get; set; }
        public string PersonName { get; set; }
        public string CNIC { get; set; }
        public Int64 VehicleId { get; set; }
        public Int64 PersonId { get; set; }
        public bool IsVerified { get; set; }
    }
}
