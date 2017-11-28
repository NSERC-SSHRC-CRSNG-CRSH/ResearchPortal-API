using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Xrm.Sdk.WebServiceClient;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;

[assembly: OwinStartup(typeof(ResearchPortal.API.Startup))]
namespace ResearchPortal.API
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
          
                app.CreatePerOwinContext<CrmServiceClient>(CreateCrmServiceClient);
          
        }
        public static CrmServiceClient CreateCrmServiceClient()
        {
            string connectionstring = ConfigurationManager.AppSettings["rp2:ConnectionString"];
            CrmServiceClient client = new CrmServiceClient(connectionstring);
        
            var resp = client.Execute(new WhoAmIRequest()) as WhoAmIResponse;
            return client;
        }
    }
}