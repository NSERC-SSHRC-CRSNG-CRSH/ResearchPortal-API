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
                csv.Configuration.RegisterClassMap<FundingOpportunityMap>();
                csv.Configuration.RegisterClassMap<FundingCycleMap>();
                csv.Configuration.RegisterClassMap<ContactMap>();



                var accounts = csv.GetRecords<Account>();
                UpsertEntities(accounts, "rp2_code");

                var contacts = csv.GetRecords<Contact>();
                UpsertEntities(contacts);
            }

        }

        protected void UpsertEntities(IEnumerable<Entity> entities, string distinctColumn = "")
        {
            if (!string.IsNullOrEmpty(distinctColumn))
            {
                // TODO filter list to distinct entities 
                var distinctIds = entities.Select(r => r[distinctColumn]?.ToString()).Distinct();
                HashSet<Entity> filteredEntities = new HashSet<Entity>();
                foreach (string id in distinctIds)
                {
                    var firstDistinct = entities.FirstOrDefault(r => r[distinctColumn]?.ToString() == id);
                    if (firstDistinct == null)
                    {
                        continue;
                    }
                    if (firstDistinct.KeyAttributes == null)
                    {
                        firstDistinct.KeyAttributes = new KeyAttributeCollection();
                    }
                    firstDistinct.KeyAttributes[distinctColumn] = firstDistinct[distinctColumn];
                    filteredEntities.Add(firstDistinct);
                }
                entities = filteredEntities;
            }

            ExecuteMultipleRequest exMReq = new ExecuteMultipleRequest();
            exMReq.Requests = new OrganizationRequestCollection();
            exMReq.Settings = new ExecuteMultipleSettings();
            exMReq.Settings.ReturnResponses = true;

            foreach (var e in entities)
            {
                UpsertRequest upsert = new UpsertRequest();
                upsert.Target = e;
                exMReq.Requests.Add(upsert);
            }
            var response = service.Execute(exMReq) as ExecuteMultipleResponse;
        }
    }
}
