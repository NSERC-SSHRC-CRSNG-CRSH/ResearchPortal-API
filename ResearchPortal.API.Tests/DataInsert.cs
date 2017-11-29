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
using System;
using System.Text;
using CsvHelper.Configuration;

namespace ResearchPortal.API.Tests.Controllers
{
    [TestClass]
    public class DataInsert
    {
        private readonly IOrganizationService service = null;
        public DataInsert()
        {
            service = ResearchPortal.API.Startup.CreateCrmServiceClient();
        }
        [TestMethod]
        public void CreateTestData()
        {
            string csvFile = @"C:\Dev\GitHub\NSERC-SSHRC\NSERC_GRT_FYR2016_AWARD.csv";
            //UpsertEntities<AccountMap, Account>(csvFile, "rp2_code");
            //UpsertEntities<ContactMap, Contact>(csvFile, "rp2_identifier");          
            //UpsertEntities<FundingOpportunityMap, rp2_fundingopportunity>(csvFile, "rp2_code");
            //UpsertEntities<FundingCycleMap, rp2_fundingcycle>(csvFile, "rp2_code");
            //UpsertEntities<ApplicationMap, rp2_application>(csvFile, "rp2_identifier");
            UpsertEntities<Rp2AwardMap, rp2_award>(csvFile, "rp2_identifier");

        }

        protected void UpsertEntities<TMap, TEntity>(string csvFile, string distinctColumn = "") where TEntity : Entity where TMap : ClassMap<TEntity>
        {
            IEnumerable<TEntity> entities = null;

            using (var streamReader = new StreamReader(csvFile, Encoding.GetEncoding("windows-1252")))
            {
                var csv = new CsvReader(streamReader);
                csv.Configuration.RegisterClassMap<TMap>();
                entities = csv.GetRecords<TEntity>().ToList();
            }

            if (!string.IsNullOrEmpty(distinctColumn))
            {
                // TODO filter list to distinct entities 
                var distinctIds = entities.Select(r => r[distinctColumn]?.ToString()).Distinct();
                HashSet<TEntity> filteredEntities = new HashSet<TEntity>();
                foreach (string id in distinctIds)
                {
                    TEntity firstDistinct = entities.FirstOrDefault(r => r[distinctColumn]?.ToString() == id).ToEntity<TEntity>();
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
            TEntity example = entities.FirstOrDefault();


            // split the list into sub lists with a maximum of 1000 records
            var subLists = entities.Select((e, i) => new { Index = i, Value = e })
               .GroupBy(x => x.Index / 1000)
               .Select(x => x.Select(v => v.Value).ToList()).ToList();

            foreach (var subEntities in subLists)
            {
                ExecuteMultipleRequest exMReq = new ExecuteMultipleRequest();
                exMReq.Requests = new OrganizationRequestCollection();
                exMReq.Settings = new ExecuteMultipleSettings();
                exMReq.Settings.ReturnResponses = true;

                foreach (var e in subEntities)
                {
                    UpsertRequest upsert = new UpsertRequest();

                    upsert.Target = e;
                    exMReq.Requests.Add(upsert);
                }

                var response = service.Execute(exMReq) as ExecuteMultipleResponse;
                if (response.IsFaulted)
                {
                    throw new Exception("");
                }
            }
        }
    }
}
