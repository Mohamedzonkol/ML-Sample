using Azure;
using Azure.AI.TextAnalytics;

namespace Example
{
    class Program
    {
        // This example requires environment variables named "LANGUAGE_KEY" and "LANGUAGE_ENDPOINT"
        private static string languageKey =
            "DBf9ZOhd0V6iWvwCfdMFPKN5EOvoeV9XbXLRbC4tkF8H3aMpDAe7JQQJ99BBACYeBjFXJ3w3AAAaACOGnRRX";

        private static string languageEndpoint = "https://language-test132.cognitiveservices.azure.com/";

        private static readonly AzureKeyCredential credentials = new AzureKeyCredential(languageKey);
        private static readonly Uri endpoint = new Uri(languageEndpoint);

        // Example method for detecting opinions text. 
        static void SentimentAnalysisWithOpinionMiningExample(TextAnalyticsClient client)
        {
            var documents = new List<string>
            {
                "The food and service were unacceptable. The concierge was nice, however.",
                "test test"
            };

            AnalyzeSentimentResultCollection reviews = client.AnalyzeSentimentBatch(documents, options: new AnalyzeSentimentOptions()
            {
                IncludeOpinionMining = true
            });

            foreach (AnalyzeSentimentResult review in reviews)
            {
                Console.WriteLine($"Document sentiment: {review.DocumentSentiment.Sentiment}\n");
                Console.WriteLine($"\tPositive score: {review.DocumentSentiment.ConfidenceScores.Positive:0.00}");
                Console.WriteLine($"\tNegative score: {review.DocumentSentiment.ConfidenceScores.Negative:0.00}");
                Console.WriteLine($"\tNeutral score: {review.DocumentSentiment.ConfidenceScores.Neutral:0.00}\n");
                foreach (SentenceSentiment sentence in review.DocumentSentiment.Sentences)
                {
                    Console.WriteLine($"\tText: \"{sentence.Text}\"");
                    Console.WriteLine($"\tSentence sentiment: {sentence.Sentiment}");
                    Console.WriteLine($"\tSentence positive score: {sentence.ConfidenceScores.Positive:0.00}");
                    Console.WriteLine($"\tSentence negative score: {sentence.ConfidenceScores.Negative:0.00}");
                    Console.WriteLine($"\tSentence neutral score: {sentence.ConfidenceScores.Neutral:0.00}\n");

                    foreach (SentenceOpinion sentenceOpinion in sentence.Opinions)
                    {
                        Console.WriteLine($"\tTarget: {sentenceOpinion.Target.Text}, Value: {sentenceOpinion.Target.Sentiment}");
                        Console.WriteLine($"\tTarget positive score: {sentenceOpinion.Target.ConfidenceScores.Positive:0.00}");
                        Console.WriteLine($"\tTarget negative score: {sentenceOpinion.Target.ConfidenceScores.Negative:0.00}");
                        foreach (AssessmentSentiment assessment in sentenceOpinion.Assessments)
                        {
                            Console.WriteLine($"\t\tRelated Assessment: {assessment.Text}, Value: {assessment.Sentiment}");
                            Console.WriteLine($"\t\tRelated Assessment positive score: {assessment.ConfidenceScores.Positive:0.00}");
                            Console.WriteLine($"\t\tRelated Assessment negative score: {assessment.ConfidenceScores.Negative:0.00}");
                        }
                    }
                }
                Console.WriteLine($"\n");
            }
        }

        static void Main(string[] args)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);
            SentimentAnalysisWithOpinionMiningExample(client);

            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }

    }
}