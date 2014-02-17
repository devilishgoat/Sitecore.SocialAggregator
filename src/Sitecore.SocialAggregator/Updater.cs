using System;
using System.Collections.Generic;
using System.Xml;
using Sitecore.Data;

namespace Sitecore.SocialAggregator
{
    public class Updater
    {        
        private readonly List<Source> sources;

        public Updater()
        {
            this.sources = new List<Source>();
        }

        public void AddSource(XmlNode source)
        {
            if (source.Attributes != null)
            {
                string type = source.Attributes["type"].Value;
                var instance = Activator.CreateInstance(Type.GetType(type)) as Source;
                if (instance != null)
                {
                    instance.SaveLocation = source.Attributes["root"].Value;
                    instance.Database = Database.GetDatabase("master");
                    this.sources.Add(instance);
                }
            }
        }

        public void Run()
        {
            this.sources.ForEach(source =>
                {
                    try
                    {
                        source.Update();
                    }
                    catch (Exception)
                    {
                    }
                });
        }
    }
}
