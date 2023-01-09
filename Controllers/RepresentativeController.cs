using Representative.Services;
using Representative.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.Identity;
using SharedLib.APIs;
using SharedLib.Common;
using SharedLib.Security;
using Models.ViewModels.VehicleRegistration.Core;
using Representative.Models.udt;
using System;

namespace Representative.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.JWT_BEARER_TOKEN_STATELESS)]
    public class RepresentativeController : ControllerBase
    {
        private readonly IRepresentativeService representativeService;
        public VwUser User
        {
            get
            {
                return (VwUser)this.Request.HttpContext.Items["User"];
            }
        }

        public RepresentativeController(IRepresentativeService representativeService)
        {
            this.representativeService = representativeService;
        }


        [HttpGet(Name = "GetApplicationsAgainstRegNo")]
        public async Task<ApiResponse> GetApplicationsAgainstRegNo(string registrationNo)
        {
            this.representativeService.VwUser = this.User;

            var ds = await this.representativeService.GetApplicationsAgainstRegNo(registrationNo);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.NOT_FOUND_MESSAGE);
            }

            var data = ds.Tables[0].ToList<OwnerApplicationInfo>();

            return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, Constants.RECORD_FOUND_MESSAGE);
        }

        [HttpGet(Name = "GetPersonByCNIC")]
        public async Task<ApiResponse> GetPersonByCNIC(string cnic)
        {
            var ds = await this.representativeService.GetPersonByCNIC(cnic);

            var person = SharedLib.Common.Extentions.ToList<ImPerson>(ds.Tables[0]).FirstOrDefault();

            if (person is not null)
            {
                person.Addresses = SharedLib.Common.Extentions.ToList<ImAddress>(ds.Tables[1]).ToList();
                person.PhoneNumbers = SharedLib.Common.Extentions.ToList<ImPhoneNumber>(ds.Tables[2]).ToList();
            }

            var data = new { person };

            var status = person is not null ? Constants.RECORD_FOUND_MESSAGE : Constants.NOT_FOUND_MESSAGE;

            return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, status);
        }

        [HttpGet(Name = "GetBiometricApplicationsByRegNo")]
        public async Task<ApiResponse> GetBiometricApplicationsByRegNo(string registrationNo)
        {
            this.representativeService.VwUser = this.User;

            var ds = await this.representativeService.GetBiometricApplicationsByRegNo(registrationNo);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.NOT_FOUND_MESSAGE);
            }

            var data = ds.Tables[0].ToList<ImRepresentative>();

            return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, Constants.RECORD_FOUND_MESSAGE);
        }

        [HttpGet(Name = "GetAdrApplicationDetail")]
        public async Task<ApiResponse> GetAdrApplicationDetail(long applicationId)
        {
            this.representativeService.VwUser = this.User;

            var ds = await this.representativeService.GetAdrApplicationDetail(applicationId,this.User.UserId);

            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    return ApiResponse.GetApiResponse(ApiResponseType.FAILED, null, Constants.NOT_FOUND_MESSAGE);
            //}

            //var representative = ds.Tables[0].ToList<ImRepresentative>();
            var representative = SharedLib.Common.Extentions.ToList<ImRepresentative>(ds.Tables[0]).FirstOrDefault() ;
            if (representative is not null)
            {
                representative.RemarksObj = SharedLib.Common.Extentions.ToList<ImRemarks>(ds.Tables[1]).ToList();
                representative.BusinessEvent = SharedLib.Common.Extentions.ToList<ImBusinessEvent>(ds.Tables[2]).ToList();
                representative.BusinessEventId = representative.BusinessEvent.FirstOrDefault().BusinessEventId;
                representative.Remarks = representative.RemarksObj.Count>0? representative.RemarksObj.FirstOrDefault().RemarksStatement:"";
            }
            var data = new { representative };
            var status = representative is not null ? Constants.RECORD_FOUND_MESSAGE : Constants.NOT_FOUND_MESSAGE;

            return ApiResponse.GetApiResponse(ApiResponseType.SUCCESS, data, status);
        }

        [HttpPost]
        public async Task<ApiResponse> AddRepresentative(ImRepresentative imRepresentative)
        {
            this.representativeService.VwUser = this.User;

            var ds = await this.representativeService.SaveRepresentative(imRepresentative, this.User.UserId);

            var apiResponseType = ApiResponseType.SUCCESS;
            var msg = Constants.DATA_SAVED_MESSAGE;

            if (ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                apiResponseType = ApiResponseType.SUCCESS;
                msg = Constants.DATA_SAVED_MESSAGE;
            }
            else if (ds.Tables[0].Rows[0][0].ToString() == "U")
            {
                apiResponseType = ApiResponseType.UN_AUTHORIZED;
                msg = Constants.UN_AUTHORIZED_MESSAGE;
                Response.StatusCode = 401;
            }
            else
            {
                apiResponseType = ApiResponseType.FAILED;
                msg = Constants.DATA_NOT_SAVED_MESSAGE;
            }

            return ApiResponse.GetApiResponse(apiResponseType, null, msg);
        }

        [HttpPost(Name = "MarkAdrApplicationStatus")]
        public async Task<ApiResponse> MarkAdrApplicationStatus(ImRepresentative ImRepresentative)
        {
            this.representativeService.VwUser = this.User;

            var ds = await this.representativeService.MarkAdrApplicationStatus(ImRepresentative, this.User.UserId);

            var apiResponseType = ApiResponseType.SUCCESS;
            var msg = Constants.DATA_SAVED_MESSAGE;

            if (ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                apiResponseType = ApiResponseType.SUCCESS;
                msg = Constants.DATA_SAVED_MESSAGE;
            }
            else if (ds.Tables[0].Rows[0][0].ToString() == "U")
            {
                apiResponseType = ApiResponseType.UN_AUTHORIZED;
                msg = Constants.UN_AUTHORIZED_MESSAGE;
                Response.StatusCode = 401;
            }
            else
            {
                apiResponseType = ApiResponseType.FAILED;
                msg = Constants.DATA_NOT_SAVED_MESSAGE;
            }

            return ApiResponse.GetApiResponse(apiResponseType, null, msg);
        }
    }
}
