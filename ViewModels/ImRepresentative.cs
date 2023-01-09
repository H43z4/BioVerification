using Models.ViewModels.VehicleRegistration.Core;
using System.ComponentModel.DataAnnotations;

namespace Representative.ViewModels
{
    public class ImRepresentative : CommonFeature
    {
        public long VehicleId { get; set; }
        public long BiometricAlternativeVerificationId { get; set; }
        public string RegistrationNo { get; set; }
        public long ApplicationId { get; set; }
        public long PersonId { get; set; }
        public long AlternativePersonId { get; set; }
        public bool? IsApproved { get; set; }
        public string Owner { get; set; }
        public string OwnerCNIC { get; set; }
        public string Representative { get; set; }
        public string RepresentativecCNIC { get; set; }
        public string ApplicationStatus { get; set; }
        public string BusinessProcess { get; set; }
        public string BusinessPhase { get; set; }
        public string BusinessPhaseStatus { get; set; }
        public string Remarks { get; set; }
        public long BusinessEventId { get; set; }

        //public string REG_NUM { get; set; }
        public long LimitedUpdate { get; set; }
        public ImPerson? Person { get; set; }
        public List<ImRemarks>? RemarksObj { get; set; }
        public List<ImBusinessEvent>? BusinessEvent { get; set; }
        public long UserId { get; set; }
    }
}
