using Skybrud.Social.Instagram;
using Skybrud.Social.Instagram.OAuth;
using Skybrud.Social.Instagram.Objects;
using Skybrud.Social.Instagram.Options.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Caching;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Services;
using timw255.Sitefinity.Instagram.Mvc.Configuration;
using timw255.Sitefinity.Instagram.Mvc.Models;

namespace timw255.Sitefinity.Instagram.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "InstagramFeed", Title = "Instagram Feed", SectionName = "Social")]
    public class InstagramController : Controller
    {
        public string Username { get; set; }
        
        /// <summary>
        /// This is the default Action.
        /// </summary>
        public ActionResult Index()
        {
            if (SystemManager.IsDesignMode || SystemManager.IsPreviewMode)
            {
                Guid output;
                if (Guid.TryParse(instagramConfig.AccessToken, out output) || string.IsNullOrWhiteSpace(instagramConfig.AccessToken))
                {
                    return View("Authorize");
                }
            }

            var model = new InstagramModel();

            model.Media = GetMedia();
            
            return View("Default", model);
        }

        private List<InstagramMedia> GetMedia()
        {
            var media = new List<InstagramMedia>();

            if (!string.IsNullOrWhiteSpace(Username))
            {
                if ((List<InstagramMedia>)CacheManager[GetCacheKey()] != null)
                {
                    media = (List<InstagramMedia>)CacheManager[GetCacheKey()];
                }
                else
                {
                    media = GetFeedDataFromInstagram();
                }
            }
            
            return media;
        }

        #region caching

        private List<InstagramMedia> GetFeedDataFromInstagram()
        {
            var service = InstagramService.CreateFromAccessToken(instagramConfig.AccessToken);

            var userResponse = service.Users.Search(Username);

            // Temporary list for storing the retrieved media
            var media = new List<InstagramMedia>();

            // Find the first user with the specified username
            var user = userResponse.Body.Data.FirstOrDefault(x => x.Username == Username);
            
            if (user != null)
            {
                // Declare the initial search options
                var options = new InstagramUserRecentMediaOptions
                {
                    Count = instagramConfig.MaxItems,
                };

                var mediaResponse = service.Users.GetRecentMedia(user.Id, options);

                // Add the media to the list
                media.AddRange(mediaResponse.Body.Data);
            }
            
            CacheManager.Add(
                GetCacheKey(),
                media,
                CacheItemPriority.Normal,
                null,
                new AbsoluteTime(TimeSpan.FromMinutes(instagramConfig.Expiration)));

            return media;
        }

        #endregion

        #region private

        private ICacheManager CacheManager
        {
            get
            {
                return SystemManager.GetCacheManager(CacheManagerInstance.Global);
            }
        }

        private string GetCacheKey()
        {
            return string.Format("{0}{1}", "instagramMedia", Username);
        }

        InstagramConfig instagramConfig = Config.Get<InstagramConfig>();
        #endregion
    }
}