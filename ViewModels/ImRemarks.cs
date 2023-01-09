using Models.ViewModels.VehicleRegistration.Core;
using System.ComponentModel.DataAnnotations;

namespace Representative.ViewModels
{
    public class ImRemarks:CommonFeature
    {
        [Required]
        public long? ApplicationId { get; set; }

        [Required]
        [StringLength(500)]
        public string RemarksStatement { get; set; }

        public string UserRole { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
