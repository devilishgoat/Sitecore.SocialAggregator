using System;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Sitecore.SocialAggregator
{
    public abstract class Entry
    {
        public abstract DateTime Date { get; }

        public abstract string ExternalId { get; }

        public abstract string Url { get; }

        public abstract string Message { get; }      

        public Item UpdateItem(Item item)
        {

            item.Fields["Date"].Value = this.Date.ToString("yyyyMMddThhmmss");
            item.Fields["External Id"].Value = this.ExternalId;
            LinkField link = item.Fields["Url"];
            link.Url = this.Url;
            link.LinkType = "external";
            item.Fields["Message"].Value = this.Message;            

            return item;
        }
    }
}

