using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResearchPortal.API;
using ResearchPortal.API.Controllers;
using CsvHelper;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using ResearchPortal.Entities;
using ReseachPortal.API;

namespace ResearchPortal.API.Tests.Controllers
{
    [TestClass]
    public class DataInsert
    {
        IOrganizationService service = null;
        public DataInsert()
        {
            service = ResearchPortal.API.Startup.CreateCrmServiceClient();
        }
        [TestMethod]
        public void CreateTestData()
        {

            HashSet<IDictionary<string, object>> rows = new HashSet<IDictionary<string, object>>();
            using (TextReader fileReader = File.OpenText(@"C:\Dev\RP2.0\ReseachPortal-API\NSERC_GRT_FYR2016_AWARD.csv"))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.RegisterClassMap<AccountMap>();
                csv.GetRecords<Account>();
            }
            var organizations = new HashSet<Account>();
            ExecuteMultipleRequest exMReq = new ExecuteMultipleRequest();
            exMReq.Requests = new OrganizationRequestCollection();
            exMReq.Settings = new ExecuteMultipleSettings();
            exMReq.Settings.ReturnResponses = true;

            //Cle,Name-Nom,Department-Département,FiscalYear-Exercice financier,
            //CompetitionYear -Année de concours,AwardAmount,ProgramID,ProgramNaneEN,
            //ProgramNameFR,GroupEN,GroupFR,CommitteeCode,CommitteeNameEN,CommitteeNameFR,
            //AreaOfApplicationCode,AreaOfApplicationGroupEN,AreaOfApplicationGroupFR,
            //AreaOfApplicationEN,AreaOfApplicationFR,ResearchSubjectCode,ResearchSubjectGroupEN,
            //ResearchSubjectGroupFR,ResearchSubjectEN,ResearchSubjectFR,installment,Partie,
            //Num_Partie,Nb_Partie,ApplicationTitle,Keyword,ApplicationSummary

            #region Organizations
            // Organization Fields:
            //OrganizationID,Institution-Établissement,ProvinceEN,ProvinceFR,CountryEN,CountryFR,
            var distinctOrgIds = rows.Select(r => r["OrganizationID"]?.ToString()).Distinct();
            foreach (string orgId in distinctOrgIds)
            {
                var orgRow = rows.FirstOrDefault(r => r["OrganizationID"]?.ToString() == orgId);

                UpsertRequest upsert = new UpsertRequest();
                Account adminOrg = new Account("rp2_code", orgRow["OrganizationID"]);
                adminOrg.rp2_Code = orgRow["OrganizationID"]?.ToString();
                adminOrg.Name = orgRow["Institution-Établissement"]?.ToString();
                adminOrg.Address1_StateOrProvince = orgRow["ProvinceEN"]?.ToString();
                adminOrg.Address1_Country = orgRow["CountryEN"]?.ToString();
                exMReq.Requests.Add(upsert);
                organizations.Add(adminOrg);
            }
            var response = service.Execute(exMReq) as ExecuteMultipleResponse;
            for (int i = 0; i < exMReq.Requests.Count; i++)
            {
                UpsertResponse upResp = response.Responses.FirstOrDefault(r => r.RequestIndex == i)?.Response as UpsertResponse;
                (exMReq.Requests.ElementAt(i) as UpsertRequest).Target.Id = upResp.Target.Id;
            }

            #endregion

            #region Funding Opportunity

            #endregion

            #region   Funding Cycle
            #endregion

        }

        protected IEnumerable<Entity> blah(HashSet<IDictionary<string, object>> rows, string distinctColumn)
        {
            var entities = new HashSet<Entity>();
            ExecuteMultipleRequest exMReq = new ExecuteMultipleRequest();
            exMReq.Requests = new OrganizationRequestCollection();
            exMReq.Settings = new ExecuteMultipleSettings();
            exMReq.Settings.ReturnResponses = true;

            var distinctOrgIds = rows.Select(r => r[distinctColumn]?.ToString()).Distinct();
            foreach (string orgId in distinctOrgIds)
            {
                var orgRow = rows.FirstOrDefault(r => r[distinctColumn]?.ToString() == orgId);

                UpsertRequest upsert = new UpsertRequest();
                Account adminOrg = new Account("rp2_code", orgRow["OrganizationID"]);
                adminOrg.rp2_Code = orgRow["OrganizationID"]?.ToString();
                adminOrg.Name = orgRow["Institution-Établissement"]?.ToString();
                adminOrg.Address1_StateOrProvince = orgRow["ProvinceEN"]?.ToString();
                adminOrg.Address1_Country = orgRow["CountryEN"]?.ToString();
                exMReq.Requests.Add(upsert);
                entities.Add(adminOrg);
            }
            var response = service.Execute(exMReq) as ExecuteMultipleResponse;
            for (int i = 0; i < exMReq.Requests.Count; i++)
            {
                UpsertResponse upResp = response.Responses.FirstOrDefault(r => r.RequestIndex == i)?.Response as UpsertResponse;
                (exMReq.Requests.ElementAt(i) as UpsertRequest).Target.Id = upResp.Target.Id;
            }

            return entities;
        }
    }
}
