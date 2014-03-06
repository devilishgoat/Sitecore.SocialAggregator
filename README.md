<h2>Sitecore.SocialAggregator</h2>
<span>
A framework that connects to social networks and downloads the data to Sitecore.  This removes the dependency on them and lets you present the information on your own terms.
</span>

<h3>
Installing
</h3>

The package is available from nuget.  Just run <code>Install-Package Sitecore.SocialAggregator</code> in your package manager.  You should make sure that the project you are installing the package into has been correctly <a href='http://www.sitecore.net/Community/Technical-Blogs/John-West-Sitecore-Blog/Posts/2011/06/Attach-a-Sitecore-Rocks-Connection-to-a-Visual-Studio-Project.aspx'>setup</a> with Sitecore Rocks to ensure that all of the items that are required are installed into your Sitecore CMS.

<h3>
Using it
</h3>

The framework currently connects to Facebook and Twitter but you will have to enter your details into the <code>Sitecore.SocialAggregator.config</code> file that you will fine in your App_Config\include folder.  In here you can also setup a proxy that will be used when connecting to the external sites.

In this file you also specify the sources that you are going to be using.  Just add in the types that you create.  The most important value in here is the root.  This is where you specify the GUID of the social aggregator source folder location.

So say you had a folder called my-sources and under that you had a social-aggragator-source called facebook with a GUID of <code>{295C5A91-8C80-4A35-9B8C-03C0ED99EA04}</code> you would pop that in the root attribute in the config

<h3>
Extending it
</h3>

I expect you will have a bunch more sources that you will be using and you can easily add new ones by inheriting from the class <code>Sitecore.SocialAggregator.Source</code>.  There are a couple of methods in there that you will need to implement.  You can then add your new custom type into the <code>Sitecore.SocialAggregator.config</code> file and specify the root that you have created and presto, you have another source
