using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.SocialAggregator.Models;

namespace Sitecore.SocialAggregator
{
    public abstract class Source
    {
        public Database Database { get; set; }
        public string SaveLocation { get; set; }

        protected Source(Database database, string saveLocation)
        {
            this.Database = database;
            this.SaveLocation = saveLocation;
        }

        public Source()
        {

        }

        protected abstract IList<Entry> DownloadData(int count);

        public virtual void Update()
        {
            Log.Info(string.Format(CultureInfo.InvariantCulture, "Start updating SocialAggregator source {0}", this.GetType().FullName), this);
            var root = this.Database.GetItem(this.SaveLocation);
            if (root != null)
            {
                int entryCount = string.IsNullOrEmpty(root.Fields["number of entries"].Value)
                                     ? int.Parse(root.Fields["number of entries"].Value)
                                     : 5;
                var entries = this.DownloadData(entryCount);
                if (entries.Any())
                {
                    using (new SecurityDisabler())
                    {
                        var templateId = new TemplateID(new ID("{295C5A91-8C80-4A35-9B8C-03C0ED99EA04}"));
                        root.DeleteChildren();
                        foreach (var entry in entries)
                        {
                            if (!string.IsNullOrEmpty(entry.ExternalId))
                            {
                                Item itemToAdd = root.Add(entry.ExternalId, templateId);
                                itemToAdd.Editing.BeginEdit();
                                using (new EditContext(itemToAdd))
                                {
                                    entry.UpdateItem(itemToAdd);
                                }
                                itemToAdd.Editing.EndEdit();
                                foreach (var language in root.Database.GetLanguages())
                                {
                                    var publishOptions =
                                        new Publishing.PublishOptions(
                                            root.Database,
                                            Database.GetDatabase("web"),
                                            Publishing.PublishMode.SingleItem,
                                            language,
                                            System.DateTime.Now);
                                    var publisher = new Publishing.Publisher(publishOptions);

                                    // Choose where to publish from
                                    publisher.Options.RootItem = root;

                                    // Publish children as well?
                                    publisher.Options.Deep = true;

                                    // Do the publish!
                                    publisher.Publish();
                                }
                            }
                            else
                            {
                                Log.Error(
                                    string.Format(CultureInfo.InvariantCulture,
                                                  "Trying to create item for {0} source but the external id is empty",
                                                  GetType().FullName), this);
                            }
                        }
                    }
                }
                else
                {
                    Log.Error(string.Format(CultureInfo.InvariantCulture, "Tried processing the source {0} but the save location {1} was not found", this.GetType().FullName, this.SaveLocation), this);
                }
            }

            Log.Info(string.Format(CultureInfo.InvariantCulture, "Finished updating SocialAggregator source {0}", this.GetType().FullName), this);
        }
    }
}
