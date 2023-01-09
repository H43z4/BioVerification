using Models.ViewModels.VehicleRegistration.Core;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Representative.ViewModels
{
    public class ImPhoneNumber:CommonFeature
    {
        public long PhoneNumberId { get; set; }

        [Required]
        [StringLength(500)]
        public string PhoneNumberValue { get; set; }

        public long CountryId { get; set; }

        [Required]
        public long? PhoneNumberTypeId { get; set; }

        public string PhoneNumberType { get; set; }

        [JsonIgnore]
        public long PersonId { get; set; }

        [JsonIgnore]
        public long BusinessId { get; set; }
    }
}
