using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.Identity;
using Biometric.Services;
using Biometric.ViewModels;
using SharedLib.APIs;
using SharedLib.Common;
using SharedLib.Security;

namespace Biometric.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.JWT_BEARER_TOKEN_STATELESS)]
    public class BiometricController : ControllerBase
    {
        private readonly IBiometricService biometricService;
        public VwUser User
        {
            get
            {
                return (VwUser)this.Request.HttpContext.Items["User"];
            }
        }

        public BiometricController(IBiometricService biometricService)
        {
            this.biometricService = biometricService;
        }

        [HttpGet(Name = "GetVehicleInfo")]
        public async Task<ApiResponse> GetVehicleInfo(InputModel inputModel)
        {
            this.biometricService.VwUser = this.User;

            var ds = await this.biometricService.GetVehicleInfo(inputModel);

            return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.NOT_FOUND_MESSAGE);
        }

        [HttpPost]
        public async Task<ApiResponse> SaveBiometricInfo(InputModel inputModel)
        {
            this.biometricService.VwUser = this.User;

            var ds = await this.biometricService.SaveBiometricInfo(inputModel);

            return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.NOT_FOUND_MESSAGE);
        }
    }
}
