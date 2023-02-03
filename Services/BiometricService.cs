using Database;
using Models.ViewModels.Identity;
using SharedLib.Interfaces;
using System.Data;
using Models.ViewModels.Biometric;

namespace Biometric.Services
{
    public interface IBiometricService : ICurrentUser
    {
        Task<DataSet> GetVehicleInfo(VwVehicleInfoIM vwVehicleInfoIM);
        Task<DataSet> GetBiometricIntimation();
        Task<DataSet> SaveBiometricInfo(ViewModels.VwBiometricInfo vwBiometricInfo);
        Task<DataSet> SaveBiometricInfo(long biometricIntimationId);
    }

    public class BiometricService : IBiometricService
    {
        readonly IDBHelper dbHelper;
        public VwUser VwUser { get; set; }
        public BiometricService(IDBHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        #region public-Methods

        public async Task<DataSet> GetVehicleInfo(VwVehicleInfoIM vwVehicleInfoIM)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();

            paramDict.Add("@ApplicationId", vwVehicleInfoIM.MvrsTransId);
            paramDict.Add("@CNIC", vwVehicleInfoIM.CNIC);
            paramDict.Add("@RegNo", vwVehicleInfoIM.RegNo);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[GetVehicleInfo]", paramDict);

            return ds;
        }

        public async Task<DataSet> GetBiometricIntimation()
        {
            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[GetBiometricIntimation]", null);

            return ds;
        }

        public async Task<DataSet> SaveBiometricInfo(ViewModels.VwBiometricInfo vwBiometricInfo)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();

            paramDict.Add("@ApplicationId", vwBiometricInfo.MvrsTransId);
            paramDict.Add("@CNIC", vwBiometricInfo.CNIC);
            paramDict.Add("@RegNo", vwBiometricInfo.RegNo);
            paramDict.Add("@NadraTransId", vwBiometricInfo.NadraTransId);
            paramDict.Add("@NadraFranchiseId", vwBiometricInfo.NadraFranchiseId);
            paramDict.Add("@IsVerified", true);
            paramDict.Add("@UserId", this.VwUser.UserId);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[SaveBiometricIntimation]", paramDict);

            return ds;
        }
        
        public async Task<DataSet> SaveBiometricInfo(long biometricIntimationId)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();

            paramDict.Add("@BiometricIntimationId", biometricIntimationId);
            //paramDict.Add("@UserId", this.VwUser.UserId);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[OnBiometricIntimation]", paramDict);

            return ds;
        }

        #endregion
    }
}
