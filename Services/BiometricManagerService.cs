using Biometric.ViewModels;

namespace Biometric.Services
{
    public class BiometricManagerService : BackgroundService
    {
        readonly BiometricTaskList biometricTaskList;
        readonly IBiometricService biometricService;

        public BiometricManagerService(IBiometricService biometricService, BiometricTaskList biometricTaskList)
        {
            this.biometricTaskList = biometricTaskList;
            this.biometricService = biometricService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Console.WriteLine("backgroup service has started.");

            try
            {
                var ds = await this.biometricService.GetBiometricIntimation();

                for(int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    var biometricIntimationId = Convert.ToInt64(ds.Tables[0].Rows[row][0].ToString());

                    this.biometricTaskList.Tasks.TryAdd(biometricIntimationId, biometricIntimationId);
                }

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var biometricIntimationId = this.biometricTaskList.Tasks.Values.FirstOrDefault();

                        if (biometricIntimationId > 0)
                        {
                            var dsSaveBiometricInfo = await this.biometricService.SaveBiometricInfo(biometricIntimationId);

                            this.biometricTaskList.Tasks.TryRemove(biometricIntimationId, out biometricIntimationId);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
