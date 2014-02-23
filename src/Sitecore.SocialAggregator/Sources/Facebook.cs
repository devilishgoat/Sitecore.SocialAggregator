using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.SocialAggregator.Models;

namespace Sitecore.SocialAggregator.Sources
{
    public class Facebook : Source
    {
        public Facebook(Database database, string saveLocation)
            : base(database, saveLocation)
        {
        }

        public Facebook()
        {

        }

        protected override IList<Entry> DownloadData(int count)
        {
            var endpoint = Configuration.Settings.GetSetting("SocialAggregator.Source.Facebook.EndPoint");
            if (string.IsNullOrEmpty(endpoint))
            {
                Log.Error(
                    string.Format(CultureInfo.InvariantCulture, "Facebook endpoint has not been set.  Unable to connect"),
                    this);
                return new List<Entry>();
            }

            var accessToken = Configuration.Settings.GetSetting("SocialAggregator.Source.Facebook.AppToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                Log.Error(
                    string.Format(CultureInfo.InvariantCulture,
                                  "Facebook accss token has not been set.  Unable to connect"), this);
                return new List<Entry>();
            }

            using (var client = new WebClient())
            {
                var proxy = Configuration.Settings.GetSetting("SocialAggregator.Proxy.Url");
                var proxyUsername = Configuration.Settings.GetSetting("SocialAggregator.Proxy.Username");
                var proxyPassword = Configuration.Settings.GetSetting("SocialAggregator.Proxy.Password");
                if (!string.IsNullOrEmpty(proxy))
                {
                    client.Proxy = new WebProxy(proxy);
                    if (!string.IsNullOrEmpty(proxyUsername) && !string.IsNullOrEmpty(proxyPassword))
                    {
                        client.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
                    }
                }

                var postsData =
                    client.DownloadData(string.Format(CultureInfo.InvariantCulture, endpoint, accessToken, count));
                var posts = JsonConvert.DeserializeObject<FacebookPosts>(Encoding.Default.GetString(postsData));
                return posts.data.Cast<Entry>().ToList();
            }
        }

    }
}
