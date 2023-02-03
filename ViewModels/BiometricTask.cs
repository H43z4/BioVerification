using System.Collections.Concurrent;

namespace Biometric.ViewModels
{
    public class BiometricTaskList
    {
        public ConcurrentDictionary<long, long> Tasks { get; set; }
        public BiometricTaskList()
        {
            this.Tasks = new ConcurrentDictionary<long, long>();
        }
    }
}
