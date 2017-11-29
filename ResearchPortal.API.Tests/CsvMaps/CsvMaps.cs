using CsvHelper.Configuration;
using ResearchPortal.API.Models;
using ResearchPortal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReseachPortal.API
{

    public sealed class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            // Organization Fields:
            //OrganizationID,Institution,ProvinceEN,ProvinceFR,CountryEN,CountryFR,
            Map(m => m.Name).Name("Institution");
            Map(m => m.rp2_Code).Name("OrganizationID");
            Map(m => m.Address1_StateOrProvince).Name("ProvinceEN");
            Map(m => m.Address1_Country).Name("CountryEN");
        }
    }
    public sealed class ContactMap : ClassMap<Contact>
    {
        public ContactMap()
        {
            // Fields:
            Map(m => m.Last_FirstName).Name("Name-Nom");
            Map(m => m.rp2_Identifier).Name("Cle");
            Map(m => m.Address1_StateOrProvince).Name("ProvinceEN");
            Map(m => m.Address1_Country).Name("CountryEN");
        }
    }


    public sealed class ApplicationMap : ClassMap<rp2_application>
    {
        public ApplicationMap()
        {
            // Fields:
            //AreaOfApplicationCode,AreaOfApplicationGroupEN,AreaOfApplicationGroupFR,
            //AreaOfApplicationEN,AreaOfApplicationFR,ResearchSubjectCode,ResearchSubjectGroupEN,
            //ResearchSubjectGroupFR,ResearchSubjectEN,ResearchSubjectFR,installment,Partie,
            //Num_Partie,Nb_Partie,ApplicationTitle,Keyword,ApplicationSummary
            Map(m => m.rp2_name).Name("ApplicationTitle");
            Map(m => m.rp2_Identifier).Name("ApplicationId");
            Map(m => m.rp2_PrimaryAppilcant_Identifier).Name("Cle");
            Map(m => m.rp2_Organization_Code).Name("OrganizationID");
            Map(m => m.rp2_FundingCycle_Code).Name("FCCode");
            Map(m => m.rp2_FundingOpportunity_Code).Name("ProgramID");

            // Map(m => m.).Name("Keyword");
            // Map(m => m.).Name("ApplicationSummary");
        }
    }

    public sealed class FundingOpportunityMap : ClassMap<rp2_fundingopportunity>
    {
        public FundingOpportunityMap()
        {
            // Fields:
            //ProgramID,ProgramNaneEN,ProgramNameFR
            Map(m => m.rp2_name).Name("ProgramNaneEN");
            Map(m => m.rp2_Code).Name("ProgramID");
        }
    }

    public sealed class FundingCycleMap : ClassMap<rp2_fundingcycle>
    {
        public FundingCycleMap()
        {
            // Fields:
            //ProgramID,ProgramNaneEN,ProgramNameFR
            Map(m => m.rp2_FiscalYear).Name("FiscalYear");
            Map(m => m.rp2_FiscalYearDisplay).Name("FiscalYear");
            Map(m => m.rp2_CompetitionYearDisplay).Name("CompetitionYear");
            Map(m => m.rp2_CompetitionYear).Name("CompetitionYear");
            Map(m => m.rp2_FundingOpportunity_Code).Name("ProgramID");
            Map(m => m.rp2_Code).Name("FCCode");
            Map(m => m.rp2_name).Name("FCCode");
        }
    }

    public sealed class Rp2AwardMap : ClassMap<rp2_award>
    {
        //Cle,Name-Nom,Department,FiscalYear,
        //CompetitionYear,AwardAmount,ProgramID,ProgramNaneEN,
        //ProgramNameFR,GroupEN,GroupFR,CommitteeCode,CommitteeNameEN,CommitteeNameFR,
        //AreaOfApplicationCode,AreaOfApplicationGroupEN,AreaOfApplicationGroupFR,
        //AreaOfApplicationEN,AreaOfApplicationFR,ResearchSubjectCode,ResearchSubjectGroupEN,
        //ResearchSubjectGroupFR,ResearchSubjectEN,ResearchSubjectFR,installment,Partie,

        public Rp2AwardMap()
        {
            Map(m => m.rp2_Identifier).Name("ApplicationId");
            Map(m => m.rp2_name).Name("ApplicationId");
            Map(m => m.rp2_Application_Identifier).Name("ApplicationId");
            Map(m => m.rp2_TotalAwardedAmount.Value).Name("AwardAmount");
            Map(m => m.rp2_PrimaryAwardee_Identifier).Name("Cle");
            Map(m => m.rp2_Organization_Code).Name("OrganizationID");
        }
    }
}
