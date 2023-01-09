using Representative.ViewModels;
using Database;
using Models.ViewModels.Identity;
using SharedLib.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;
using SharedLib.Common;
using Representative.Models.udt;
using System.Runtime.InteropServices;

namespace Representative.Services
{
    public interface IRepresentativeService : ICurrentUser
    {
        Task<DataSet> GetApplicationsAgainstRegNo(string registrationNo);
        public Task<DataSet> GetPersonByCNIC(string cnic);
        Task<DataSet> SaveRepresentative(ImRepresentative imRepresentative, long UserId);
        Task<DataSet> GetBiometricApplicationsByRegNo(string registrationNo);
        Task<DataSet> GetAdrApplicationDetail(long applicationId, long UserId);
        Task<DataSet> MarkAdrApplicationStatus(ImRepresentative imRepresentative, long UserId);
    }

    public class RepresentativeService : IRepresentativeService
    {
        readonly IDBHelper dbHelper;
        public VwUser VwUser { get; set; }
        public RepresentativeService(IDBHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        #region public-Methods

        public async Task<DataSet> GetApplicationsAgainstRegNo(string registrationNo)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();

            paramDict.Add("@REG_NUM", registrationNo);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[GetAdrInfo]", paramDict);

            return ds;
        }
        public async Task<DataSet> GetPersonByCNIC(string cnic)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@PersonId", DBNull.Value);
            paramDict.Add("@CNIC", cnic);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[GetPerson]", paramDict);

            return ds;
        }
        public async Task<DataSet> SaveRepresentative(ImRepresentative imRepresentative,long UserId)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();

            paramDict.Add("@Person", new List<Person>()
            {
                new Person()
                {

                    PersonId = imRepresentative.Person.PersonId,
                    CountryId = imRepresentative.Person.CountryId,
                    PersonName = imRepresentative.Person.PersonName,
                    FatherHusbandName = imRepresentative.Person.FatherHusbandName,
                    CNIC = imRepresentative.Person.CNIC,
                    Email = imRepresentative.Person.Email,
                    OldCNIC = imRepresentative.Person.OldCNIC,
                    NTN = imRepresentative.Person.NTN,
                    FTN = imRepresentative.Person.FTN,
                    CreatedBy = UserId,
                    CreatedAt = DateTime.Now
                }

            }.ToDataTable());

            var addresses = new List<Address>();

            imRepresentative.Person.Addresses.ForEach(x =>
            {
                var address = new Address();
                EntityMapper<ImAddress, Address>.CopyByPropertyNameAndType(x, address);
                addresses.Add(address);
            });

            var phoneNumbers = new List<PhoneNumber>();

            imRepresentative.Person.PhoneNumbers.ForEach(x =>
            {
                var phoneNumber = new PhoneNumber();
                EntityMapper<ImPhoneNumber, PhoneNumber>.CopyByPropertyNameAndType(x, phoneNumber);
                phoneNumbers.Add(phoneNumber);
            });

            paramDict.Add("@Address", addresses.ToDataTable());
            paramDict.Add("@PhoneNumber", phoneNumbers.ToDataTable());
            paramDict.Add("@UserId", UserId);
            paramDict.Add("@VehicleId", imRepresentative.VehicleId);
            paramDict.Add("@REG_NUM", imRepresentative.RegistrationNo);
            paramDict.Add("@LimitedUpdate", imRepresentative.LimitedUpdate);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[SaveAlternativeRep]", paramDict);

            return ds;
        }
        public async Task<DataSet> GetBiometricApplicationsByRegNo(string registrationNo)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@REG_NUM", registrationNo);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[GetAdrApplications]", paramDict);

            return ds;
        }
        public async Task<DataSet> GetAdrApplicationDetail(long applicationId, long UserId)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@BioAltVerificationId", null);
            paramDict.Add("@ApplicationId", applicationId);
            paramDict.Add("@UserId", UserId);

            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[GetAdrApplicationDetail]", paramDict);

            return ds;
        }
        public async Task<DataSet> MarkAdrApplicationStatus(ImRepresentative imRepresentative, long UserId)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("@ApplicationId", imRepresentative.ApplicationId);
            paramDict.Add("@Status", imRepresentative.IsApproved);
            paramDict.Add("@UserId", UserId);
            paramDict.Add("@Remarks", imRepresentative.Remarks);
            paramDict.Add("@BusinessEventId", imRepresentative.BusinessEventId);


            var ds = await this.dbHelper.GetDataSetByStoredProcedure("[Biometric].[MarkAdrApplicationStatus]", paramDict);

            return ds;
        }
        #endregion
    }
}
