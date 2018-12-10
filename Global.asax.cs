using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System.Reflection;
using System.Web.Http;

namespace RecruitmentQnA
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            RegisterBotDependencies();
            //Conversation.UpdateContainer(
            //    builder =>
            //    {
            //        builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

            //        // Bot Storage: Here we register the state storage for your bot. 
            //        // Default store: volatile in-memory store - Only for prototyping!
            //        // We provide adapters for Azure Table, CosmosDb, SQL Azure, or you can implement your own!
            //        // For samples and documentation, see: https://github.com/Microsoft/BotBuilder-Azure
            //        var store = new InMemoryDataStore();

            //        // Other storage options
            //        // var store = new TableBotDataStore("...DataStorageConnectionString..."); // requires Microsoft.BotBuilder.Azure Nuget package 
            //        // var store = new DocumentDbBotDataStore("cosmos db uri", "cosmos db key"); // requires Microsoft.BotBuilder.Azure Nuget package 

            //        builder.Register(c => store)
            //            .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
            //            .AsSelf()
            //            .SingleInstance();
            //    });

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        private void RegisterBotDependencies()
        {
            var builder = new ContainerBuilder();
            var store = new InMemoryDataStore();

            builder.RegisterModule<AgentModule>();
            builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));
            builder.Register(c => store)
                        .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                        .AsSelf()
                        .SingleInstance();

            builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            builder.Update(Conversation.Container);

            //DependencyResolver.SetResolver(new AutofacDependencyResolver(Conversation.Container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Conversation.Container);
        }
    }
}