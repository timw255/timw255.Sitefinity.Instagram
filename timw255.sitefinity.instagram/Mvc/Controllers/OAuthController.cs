using Skybrud.Social.Instagram;
using Skybrud.Social.Instagram.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Telerik.Sitefinity.Configuration;
using timw255.Sitefinity.Instagram.Mvc.Configuration;

namespace timw255.Sitefinity.Instagram.Mvc.Controllers
{
    public class OAuthController : ApiController
    {
        ConfigManager manager = ConfigManager.GetManager();
        InstagramConfig instagramConfig;
        InstagramOAuthClient client;

        public OAuthController()
        {
            instagramConfig = manager.GetSection<InstagramConfig>();
            
            client = new InstagramOAuthClient
            {
                ClientId = instagramConfig.ClientID,
                ClientSecret = instagramConfig.ClientSecret,
                RedirectUri = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/instagram/oauth/"
            };
        }

        [Authorize]
        [HttpGet]
        public void Index()
        {
            string state = Guid.NewGuid().ToString();

            instagramConfig.AccessToken = state;
            manager.SaveSection(instagramConfig);

            string authorizationUrl = client.GetAuthorizationUrl(state, InstagramScope.PublicContent).Replace("publiccontent", "public_content");

            HttpContext.Current.Response.Redirect(authorizationUrl);
        }

        [HttpGet]
        public void Index(string code)
        {
            var response = client.GetAccessTokenFromAuthCode(code);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                instagramConfig.AccessToken = response.Body.AccessToken;
                manager.SaveSection(instagramConfig);

                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Sitefinity");
            }
        }
    }
}
