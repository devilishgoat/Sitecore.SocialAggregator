using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sitecore.SocialAggregator.Models
{

    public class CategoryList
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class From
    {
        public string category { get; set; }
        public List<CategoryList> category_list { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Privacy
    {
        public string value { get; set; }
    }

    public class Shares
    {
        public int count { get; set; }
    }

    public class Application
    {
        public string name { get; set; }
        public string id { get; set; }
        public string @namespace { get; set; }
    }

    public class Datum2
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Cursors
    {
        public string after { get; set; }
        public string before { get; set; }
    }

    public class Paging
    {
        public Cursors cursors { get; set; }
    }

    public class Likes
    {
        public List<Datum2> data { get; set; }
        public Paging paging { get; set; }
    }

    public class __invalid_type__0
    {
        public string id { get; set; }
        public string name { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public string type { get; set; }
    }

    public class __invalid_type__37
    {
        public string id { get; set; }
        public string name { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public string type { get; set; }
    }

    public class __invalid_type__25
    {
        public string id { get; set; }
        public string name { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public string type { get; set; }
    }

    public class __invalid_type__40
    {
        public string id { get; set; }
        public string name { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public string type { get; set; }
    }

    public class StoryTags
    {
        public List<__invalid_type__0> __invalid_name__0 { get; set; }
        public List<__invalid_type__37> __invalid_name__37 { get; set; }
        public List<__invalid_type__25> __invalid_name__25 { get; set; }
        public List<__invalid_type__40> __invalid_name__40 { get; set; }
    }

    public class CategoryList2
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class From2
    {
        public string category { get; set; }
        public List<CategoryList2> category_list { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class MessageTag
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
    }

    public class Datum3
    {
        public string id { get; set; }
        public From2 from { get; set; }
        public string message { get; set; }
        public bool can_remove { get; set; }
        public string created_time { get; set; }
        public int like_count { get; set; }
        public bool user_likes { get; set; }
        public List<MessageTag> message_tags { get; set; }
    }

    public class Cursors2
    {
        public string after { get; set; }
        public string before { get; set; }
    }

    public class Paging2
    {
        public Cursors2 cursors { get; set; }
    }

    public class Comments
    {
        public List<Datum3> data { get; set; }
        public Paging2 paging { get; set; }
    }

    public class Datum4
    {
        public string category { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class To
    {
        public List<Datum4> data { get; set; }
    }

    public class __invalid_type__35
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
    }

    public class __invalid_type__38
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
    }

    public class MessageTags
    {
        public List<__invalid_type__35> __invalid_name__35 { get; set; }
        public List<__invalid_type__38> __invalid_name__38 { get; set; }
    }

    public class FacebookPost : Entry
    {
        public string id { get; set; }
        public From from { get; set; }
        public string message { get; set; }
        public string picture { get; set; }
        public string link { get; set; }
        public string icon { get; set; }
        public Privacy privacy { get; set; }
        public string type { get; set; }
        public string status_type { get; set; }
        public string object_id { get; set; }
        public string created_time { get; set; }
        public string updated_time { get; set; }
        public Shares shares { get; set; }
        public string story { get; set; }
        public Application application { get; set; }
        public Likes likes { get; set; }
        public StoryTags story_tags { get; set; }
        public Comments comments { get; set; }
        public To to { get; set; }
        public MessageTags message_tags { get; set; }
        public string name { get; set; }
        public string caption { get; set; }
        public string description { get; set; }

        public override DateTime Date
        {
            get { return DateTime.Parse(this.created_time); }
        }

        public override string ExternalId
        {
            get { return this.id; }
        }

        public override string Url
        {
            get { return GetPostUrl(this.id); }
        }

        public override string Message
        {
            get { return this.message; }
        }

        private static string GetPostUrl(string value)
        {
            var postPage = Configuration.Settings.GetSetting("SocialAggregator.Source.Facebook.PostUrl");
            var values = value.Split('_');
            var result = string.Empty;
            if (values.Length == 2)
            {
                result = string.Format(CultureInfo.InvariantCulture, postPage, values[1]);
            }

            return result;
        }
    }


    public class Paging3
    {
        public string previous { get; set; }
        public string next { get; set; }
    }

    public class FacebookPosts
    {
        public List<FacebookPost> data { get; set; }
        public Paging3 paging { get; set; }
    }

}
