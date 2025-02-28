using Azure;
using Azure.AI.TextAnalytics;

namespace Example
{
    class Program
    {
        // This example requires environment variables named "LANGUAGE_KEY" and "LANGUAGE_ENDPOINT"
        static string languageKey = "DBf9ZOhd0V6iWvwCfdMFPKN5EOvoeV9XbXLRbC4tkF8H3aMpDAe7JQQJ99BBACYeBjFXJ3w3AAAaACOGnRRX";
        static string languageEndpoint = "https://language-test132.cognitiveservices.azure.com/";

        private static readonly AzureKeyCredential credentials = new AzureKeyCredential(languageKey);
        private static readonly Uri endpoint = new Uri(languageEndpoint);

        // Example method for detecting sensitive information (PII) from text 
        static void RecognizePIIExample(TextAnalyticsClient client)
        {
            string document = "Call our office at 312-555-1234, or send an email to support@contoso.com.";
            string document2 = "Your ABA number - 111000025 - is the first 9 digits in the lower left hand corner of your personal check.";
            string document3 = "My Name Mohamed ELsayed Zonkol , Address:cairo , Phone: 01111111111 , Email:";
            PiiEntityCollection entities = client.RecognizePiiEntities(document3).Value;

            Console.WriteLine($"Redacted Text: {entities.RedactedText}");
            if (entities.Count > 0)
            {
                Console.WriteLine($"Recognized {entities.Count} PII entit{(entities.Count > 1 ? "ies" : "y")}:");
                foreach (PiiEntity entity in entities)
                {
                    Console.WriteLine($"Text: {entity.Text}, Category: {entity.Category}, SubCategory: {entity.SubCategory}, Confidence score: {entity.ConfidenceScore}");
                }
            }
            else
            {
                Console.WriteLine("No entities were found.");
            }
        }

        static void Main(string[] args)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);
            RecognizePIIExample(client);

            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }

    }
}