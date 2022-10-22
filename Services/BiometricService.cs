using Database;
using Models.ViewModels.Identity;
using SharedLib.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;
using Models.ViewModels.Biometric;

namespace Biometric.Services
{
    public interface IBiometricService : ICurrentUser
    {
        Task<DataSet> GetVehicleInfo(VwVehicleInfoIM vwVehicleInfoIM);
        Task<DataSet> SaveBiometricInfo(VwBiometricInfo vwBiometricInfo);
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

            paramDict.Add("@CNIC", vwVehicleInfoIM.RegNo);
            paramDict.Add("@ApplicationId", vwVehicleInfoIM.MvrsTransId);
            paramDict.Add("@UserId", this.VwUser.UserId);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[GetVehicleInfo]", paramDict);

            return ds;
        }
        
        public async Task<DataSet> SaveBiometricInfo(VwBiometricInfo vwBiometricInfo)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();

            paramDict.Add("@ApplicationId", vwBiometricInfo.MvrsTransId);
            paramDict.Add("@CNIC", vwBiometricInfo.CNIC);
            paramDict.Add("@RegNo", vwBiometricInfo.RegNo);
            paramDict.Add("@NadraTransId", vwBiometricInfo.NadraTransId);
            paramDict.Add("@NadraFranchiseId", vwBiometricInfo.NadraFranchiseId);
            paramDict.Add("@IsVerified", vwBiometricInfo.IsVerified);
            paramDict.Add("@UserId", this.VwUser.UserId);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[SaveBiometricInfo]", paramDict);

            return ds;
        }

        #endregion
    }
}
