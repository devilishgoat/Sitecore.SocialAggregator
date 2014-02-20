using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.SocialAggregator.Models;

namespace Sitecore.SocialAggregator.Sources
{
    public class TwitterAuthenticateResponse
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
    }

    public class Twitter : Source
    {
        public Twitter(Database database, string saveLocation)
            : base(database, saveLocation)
        {
        }

        public Twitter()
        {
            
        }

        protected override IList<Entry> DownloadData(int count)
        {            
            var oAuthConsumerKey = Configuration.Settings.GetSetting("SocialAggregator.Source.Twitter.SecretKey");
            var oAuthConsumerSecret = Configuration.Settings.GetSetting("SocialAggregator.Source.Twitter.ConsumerSecret");
            var oAuthUrl = Configuration.Settings.GetSetting("SocialAggregator.Source.Twitter.AuthenticationEndpoint");
            var username = Configuration.Settings.GetSetting("SocialAggregator.Source.Twitter.TargetAccount");

            
            Assert.IsNotNullOrEmpty(oAuthConsumerKey, "No setting found with the name SocialAggregator.Source.Twitter.SecretKey.  Please provide one in order to use the Twitter SocialAggregator source");
            Assert.IsNotNullOrEmpty(oAuthConsumerSecret, "No setting found with the name SocialAggregator.Source.Twitter.ConsumerSecret.  Please provide one in order to use the Twitter SocialAggregator source");
            Assert.IsNotNullOrEmpty(oAuthUrl, "No setting found with the name SocialAggregator.Source.Twitter.AuthenticationEndpoint.  Please provide one in order to use the Twitter SocialAggregator source");
            Assert.IsNotNullOrEmpty(oAuthUrl, "No setting found with the name SocialAggregator.Source.Twitter.TargetAccount.  Please provide one in order to use the Twitter SocialAggregator source");
            Log.Info(string.Format(CultureInfo.InvariantCulture, "Starting to download the latest Tweets for {0}", username), this);

            var authenticationHeader = string.Format("Basic {0}",
                                           System.Convert.ToBase64String(
                                               Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
                                                                      Uri.EscapeDataString((oAuthConsumerSecret)))
                                               ));

            const string postBody = "grant_type=client_credentials";

            var authorisationRequest = (HttpWebRequest) WebRequest.Create(oAuthUrl);
            authorisationRequest.Headers.Add("Authorization", authenticationHeader);
            authorisationRequest.Method = "POST";
            authorisationRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            authorisationRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (Stream stream = authorisationRequest.GetRequestStream())
            {
                byte[] content = Encoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }

            authorisationRequest.Headers.Add("Accept-Encoding", "gzip");
                       
            // deserialize into an object
            TwitterAuthenticateResponse twitterAuthResponse;
            using (WebResponse authenticationResponse = authorisationRequest.GetResponse())
            {
                using (var reader = new StreamReader(authenticationResponse.GetResponseStream()))
                {
                    var objectText = reader.ReadToEnd();
                    twitterAuthResponse = JsonConvert.DeserializeObject<TwitterAuthenticateResponse>(objectText);
                }
            }

            // Do the timeline
            var timelineFormat = Configuration.Settings.GetSetting("SocialAggregator.Source.Twitter.TimelineEndPoint");
            Assert.IsNotNullOrEmpty(oAuthUrl, "No setting found with the name SocialAggregator.Source.Twitter.TimelineEndPoint.  Please provide one in order to use the Twitter SocialAggregator source");
            var timelineUrl = string.Format(timelineFormat, username, count);
            var timeLineRequest = (HttpWebRequest) WebRequest.Create(timelineUrl);
            const string timelineHeaderFormat = "{0} {1}";
            timeLineRequest.Headers.Add("Authorization", 
                string.Format(timelineHeaderFormat, twitterAuthResponse.token_type,
                                                      twitterAuthResponse.access_token));
            timeLineRequest.Method = "Get";
            
            string timeLineJson;
            using (WebResponse timeLineResponse = timeLineRequest.GetResponse())
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    timeLineJson = reader.ReadToEnd();
                }
            }

            var results = JsonConvert.DeserializeObject<IList<Tweet>>(timeLineJson);
            Log.Info(string.Format(CultureInfo.InvariantCulture, "Finished download the latest Tweets for {0}", username), this);
            return results.Cast<Entry>().ToList();
        }
    }
}
