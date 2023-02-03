using System.ComponentModel.DataAnnotations;

namespace Biometric.ViewModels
{
    public class VwBiometricInfo
    {
        [Required]
        public string RegNo { get; set; }

        [Required]
        public string CNIC { get; set; }

        [Required]
        public long? MvrsTransId { get; set; }

        [Required]
        public long? NadraTransId { get; set; }

        [Required]
        public long? NadraFranchiseId { get; set; }
    }
}
