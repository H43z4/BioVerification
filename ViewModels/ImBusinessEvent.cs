using Models.ViewModels.VehicleRegistration.Core;

namespace Representative.ViewModels
{
    public class ImBusinessEvent:CommonFeature
    {
        public long BusinessEventId { get; set; }
        public string ButtonText { get; set; }
        public string ButtonClass { get; set; }

    }
}
