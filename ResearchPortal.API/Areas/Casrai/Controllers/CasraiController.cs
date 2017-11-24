using System.Collections.Generic;
using System.Linq;
using ResearchPortal.API.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Http;
using Microsoft.Crm.Sdk.Messages;
using System;
using Microsoft.Xrm.Tooling.Connector;
using ResearchPortal.Crm.Fetch;
using ResearchPortal.Entities;
using Microsoft.Xrm.Sdk;
using ResearchPortal.API.Models.Casrai;

namespace ResearchPortal.API.Controllers.Casrai
{

    public class CasraiController : ApiController
    {

        public CasraiController()
        {
        }

        /// <summary>
        /// Testing Method to retun the CRM Who Am I User Id
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/casrai/whoami")]
        [HttpGet()]
        public Guid WhoAmI()
        {
            CrmServiceClient service = HttpContext.Current.GetOwinContext().Get<CrmServiceClient>();
            WhoAmIResponse resp = service.Execute(new WhoAmIRequest()) as WhoAmIResponse;

            return resp.UserId;
        }

        /// <summary>
        /// Return the list of Funded Awards
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/casrai/FundedAwards")]
        [HttpGet()]
        public IEnumerable<FundedAward> FundingAwards()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
	<entity name='rp2_award'>
		<all-attributes />
		<order attribute='rp2_name' descending='false' />
		<link-entity name='rp2_application' from='rp2_applicationid' to='rp2_application' link-type='inner' alias='app'>
			<all-attributes />
			<link-entity name='contact' from='contactid' to='rp2_primaryapplicant' link-type='inner' alias='primaryapplicant' >
				<all-attributes />
			</link-entity>
			<link-entity name='account' from='accountid' to='rp2_administratingorganization' link-type='inner' alias='adminorg' >
				<all-attributes />
			</link-entity>
			<link-entity name='rp2_fundingcycle' from='rp2_fundingcycleid' to='rp2_fundingcycle' link-type='inner' alias='fc' >
				<all-attributes />
			</link-entity>
			<link-entity name='rp2_fundingopportunity' from='rp2_fundingopportunityid' to='rp2_fundingopportunity' link-type='inner' alias='fo' >
				<all-attributes />
			</link-entity>
		</link-entity>
	</entity>
</fetch>";

            var service = HttpContext.Current.GetOwinContext().Get<CrmServiceClient>();

            var rp2awards = service.FetchCompleteList<rp2_award>(fetchXml);

            // Cretae the list to return 
            return new HashSet<FundedAward>(rp2awards.Select(rp2Award =>
            {
                // Extract all the linked entities from the fetch query
                var linkedEntities = rp2Award.ExtractLinkedEntities();
                // extact the rp2_application entity type
                rp2_application rp2App = linkedEntities?["app"]?.ToEntity<rp2_application>();

                // extact the rp2_fundingcycle entity type
                rp2_fundingcycle rp2fc = linkedEntities?["fc"]?.ToEntity<rp2_fundingcycle>();

                // extact the rp2_fundingopportunity entity type
                rp2_fundingopportunity rp2fo = linkedEntities?["fo"]?.ToEntity<rp2_fundingopportunity>();

                // extact the Contact entity type
                Contact primaryApplicant = linkedEntities?["primaryapplicant"]?.ToEntity<Contact>();

                // extact the Account entity type
                Account adminOrg = linkedEntities?["adminorg"]?.ToEntity<Account>();


                FundedAward fundedAward = new FundedAward();

                #region Award Details
                fundedAward.AwardAmount = rp2Award.rp2_TotalAwardedAmount?.Value;
                fundedAward.Enddate = rp2Award.rp2_EndDate;
                fundedAward.PrimaryAwardeeName = rp2Award?.rp2_PrimaryAwardee?.Name;
                
                #endregion  Award Details

                #region Application Details
                fundedAward.ApplicationTitle = rp2Award.rp2_name;
                //fundedAward.Department = "";

                #endregion  Application Details

                #region Funding Opportunity Details
                fundedAward.ProgramId = rp2fo?.rp2_Code;
                fundedAward.ProgramName.En = rp2fo?.rp2_name;
                #endregion  Funding Opportunity Details

                #region Funding Cycle Details
                fundedAward.CompetitionYear = rp2fc?.rp2_CompetitionYear;
                //fundedAward.FiscalYear = rp2fc?.rp2_FiscalYear;

                #endregion  Funding Cycle Details

                #region Priamry Awardee
                fundedAward.PrimaryAwardee.Firstname = primaryApplicant?.FirstName;
                fundedAward.PrimaryAwardee.Lastname = primaryApplicant?.LastName;
                fundedAward.PrimaryAwardee.Salutation = primaryApplicant?.Salutation;
                fundedAward.PrimaryAwardee.Orcid = primaryApplicant?.rp2_orcid;
                fundedAward.PrimaryAwardee.FunderAccountPin = primaryApplicant?.rp2_Identifier;

                #endregion

                #region Admin Org Details 
                fundedAward.Institution = adminOrg?.Name;
                fundedAward.Country.En = adminOrg?.Address1_Country;
                //fundedAward.OrganizationID = adminOrg.Identifer;

                #endregion

                //award.Department
                return fundedAward;
            }));
        }



    }
}