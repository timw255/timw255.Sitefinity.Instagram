using System;
using System.Configuration;
using System.Linq;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;

namespace timw255.Sitefinity.Instagram.Mvc.Configuration
{
    [ObjectInfo(Title = "Instagram", Description = "Instagram")]
    public class InstagramConfig : ConfigSection
    {
        [ObjectInfo(Title = "Expiration", Description = "How long to cache Instagram feed data (in minutes)")]
        [ConfigurationProperty("Expiration", DefaultValue = 10)]
        public int Expiration
        {
            get
            {
                return (int)this["Expiration"];
            }
            set
            {
                this["Expiration"] = value;
            }
        }

        [ObjectInfo(Title = "Max Items", Description = "Maximum number of items to show")]
        [ConfigurationProperty("MaxItems", DefaultValue = 5)]
        public int MaxItems
        {
            get
            {
                return (int)this["MaxItems"];
            }
            set
            {
                this["MaxItems"] = value;
            }
        }

        [ObjectInfo(Title = "Client ID")]
        [ConfigurationProperty("ClientID")]
        public string ClientID
        {
            get
            {
                return (string)this["ClientID"];
            }
            set
            {
                this["ClientID"] = value;
            }
        }

        [ObjectInfo(Title = "Client Secret")]
        [ConfigurationProperty("ClientSecret")]
        public string ClientSecret
        {
            get
            {
                return (string)this["ClientSecret"];
            }
            set
            {
                this["ClientSecret"] = value;
            }
        }

        [ObjectInfo(Title = "Access Token")]
        [ConfigurationProperty("AccessToken")]
        public string AccessToken
        {
            get
            {
                return (string)this["AccessToken"];
            }
            set
            {
                this["AccessToken"] = value;
            }
        }
    }
}