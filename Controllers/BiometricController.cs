using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.Identity;
using Biometric.Services;
using SharedLib.APIs;
using SharedLib.Common;
using SharedLib.Security;
using Models.ViewModels.Biometric;
using Biometric.ViewModels;

namespace Biometric.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.JWT_BEARER_TOKEN_STATELESS)]
    public class BiometricController : ControllerBase
    {
        private readonly IBiometricService biometricService;
        private readonly BiometricTaskList biometricTaskList;
        public VwUser User
        {
            get
            {
                return (VwUser)this.Request.HttpContext.Items["User"];
            }
        }

        public BiometricController(IBiometricService biometricService, BiometricTaskList biometricTaskList)
        {
            this.biometricService = biometricService;
            this.biometricTaskList = biometricTaskList;
        }

        //[HttpGet(Name = "GetVehicleInfo")]
        //public async Task<ApiResponse> GetVehicleInfo(IMSaveBiometricInfo inputModel)
        //{
        //    this.biometricService.VwUser = this.User;

        //    var ds = await this.biometricService.GetVehicleInfo(inputModel);

        //    return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.NOT_FOUND_MESSAGE);
        //}

        //[HttpPost]
        //public async Task<ApiResponse> SaveBiometricInfo(VwBiometricInfo vwBiometricInfo)
        //{
        //    this.biometricService.VwUser = this.User;

        //    var ds = await this.biometricService.SaveBiometricInfo(vwBiometricInfo);

        //    return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.NOT_FOUND_MESSAGE);
        //}


        #region NADRA


        [HttpGet(Name = "GetVehicleInfo")]
        public async Task<ApiResponse> GetVehicleInfo(VwVehicleInfoIM vwVehicleInfoIM)
        {
            this.biometricService.VwUser = this.User;

            var ds = await this.biometricService.GetVehicleInfo(vwVehicleInfoIM);

            var vehicleInfoList = ds.Tables[0].ToList<VwVehicleInfo>();

            var vehicleInfo = vehicleInfoList.Count > 0 ? vehicleInfoList.FirstOrDefault() : null;

            var apiResponseType = vehicleInfo is not null ? ApiResponseType.SUCCESS : ApiResponseType.FAILED;
            var msg = vehicleInfo is not null ? Constants.RECORD_FOUND_MESSAGE : Constants.NOT_FOUND_MESSAGE;

            return ApiResponse.GetApiResponse(apiResponseType, vehicleInfo, msg);
        }

        [HttpPost]
        public async Task<ApiResponse> SaveNadraFranchise(VwNadraFranchise vwNadraFranchise)
        {
            //this.biometricService.VwUser = this.User;

            //var rowsAffected = await this.biometricService.SaveNadraFranchise(vwNadraFranchise);

            //var apiResponseType = rowsAffected > 0 ? ApiResponseType.SUCCESS : ApiResponseType.FAILED;
            //var msg = rowsAffected > 0 ? Constants.DATA_SAVED_MESSAGE : Constants.DATA_NOT_SAVED_MESSAGE;

            //return ApiResponse.GetApiResponse(apiResponseType, null, msg);

            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ApiResponse> SaveBiometricInfo(VwBiometricInfo vwBiometricInfo)
        {
            this.biometricService.VwUser = this.User;

            var ds = await this.biometricService.SaveBiometricInfo(vwBiometricInfo);

            var apiResponseType = ApiResponseType.SUCCESS;
            var msg = Constants.DATA_SAVED_MESSAGE;

            if (ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                var biometricIntimationId = Convert.ToInt64(ds.Tables[0].Rows[0][2].ToString());

                this.biometricTaskList.Tasks.TryAdd(biometricIntimationId, biometricIntimationId);

                apiResponseType = ApiResponseType.SUCCESS;
                msg = Constants.DATA_SAVED_MESSAGE;
            }
            else if (ds.Tables[0].Rows[0][0].ToString() == "1")
            {
                apiResponseType = ApiResponseType.FAILED;
                msg = ds.Tables[0].Rows[0][1].ToString();
            }
            else if (ds.Tables[0].Rows[0][0].ToString() == "2")
            {
                apiResponseType = ApiResponseType.FAILED;
                msg = ds.Tables[0].Rows[0][1].ToString();
            }
            else
            {
                throw new InvalidDataException();
            }

            return ApiResponse.GetApiResponse(apiResponseType, null, msg);
        }

        #endregion

        #region Organizational-Registration

        [AllowAnonymous]
        [HttpGet(Name = "GetAssociations")]
        public async Task<ApiResponse> GetAssociations(string ntn)
        {
            //this.biometricService.VwUser = this.User;

            //var ds = await this.biometricService.GetAssociations(ntn);

            //var repAssociations = ds.Tables[0].ToList<VwRepAssociation>();
            //var persons = ds.Tables[0].ToList<VwPerson>();  // dataset contains only one table. Tables[0]

            //foreach (var rep in repAssociations)
            //{
            //    if (rep.PersonId.HasValue)
            //    {
            //        rep.Person = persons.SingleOrDefault(x => x.PersonId == rep.PersonId);
            //    }
            //}

            //var apiResponseType = repAssociations is not null ? ApiResponseType.SUCCESS : ApiResponseType.FAILED;
            //var msg = repAssociations is not null ? Constants.RECORD_FOUND_MESSAGE : Constants.NOT_FOUND_MESSAGE;

            //return ApiResponse.GetApiResponse(apiResponseType, repAssociations, msg);

            throw new NotImplementedException();
        }

        #endregion
    }
}
