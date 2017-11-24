//using Microsoft.IdentityModel.Clients.ActiveDirectory;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ReseachPortal.API
//{
//    //public interface ICrmConfiguration
//    //{

//    //    string ClientId { get; set; }
//    //    string RedirectUrl { get; set; }
//    //    string ResourceUrl { get; set; }

//    //    AuthenticationContext AuthContext { get; set; }
//    //    string GetAuthorizationToken();

//    //}
//    public class CrmConfiguration
//    {
//        public CrmConfiguration() {
//            ResourceUrl = ConfigurationManager.AppSettings["Name"];
//            ResourceUrl = ConfigurationManager.AppSettings["Name"];
//            ResourceUrl = ConfigurationManager.AppSettings["Name"];
//        }
//        public string ClientId { get; set; }
//        public string RedirectUrl { get; set; }
//        public string ResourceUrl { get; set; }

//        //public AuthenticationContext AuthContext { get; set; }
//        /// <summary>
//        /// As recommended from Microsoft: https://msdn.microsoft.com/en-us/library/gg327838.aspx
//        /// The access token should always be refreshed before each call to Dynamics 365.
//        /// </summary>
//        /// <returns></returns>
//        //public string GetAuthorizationToken()
//        //{
//        //    // Authenticate the registered application with Azure Active Directory.
//        //    AuthenticationResult result = AuthContext.AcquireTokenAsync(ResourceUrl, ClientId, new Uri(RedirectUrl)).Result;
//        //    return result.AccessToken;

//        //}
//    }
//}
