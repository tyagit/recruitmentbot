using System.Configuration;

namespace RecruitmentQnA
{
    public static class Settings
    {
        public static string GetSubscriptionKey()
        {
            return ConfigurationManager.AppSettings["SubscriptionKey"];
        }
        public static string GetCognitiveServicesTokenUri()
        {
            return ConfigurationManager.AppSettings["CognitiveServicesTokenUri"];
        }
        public static string GetTranslatorUri()
        {
            return ConfigurationManager.AppSettings["TranslatorUri"];
        }
        public static string GetDirectLineSecret()
        {
            return ConfigurationManager.AppSettings["testdirectsecret"];
        }
    }
}