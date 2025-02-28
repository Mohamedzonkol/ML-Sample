using Azure;
using Azure.AI.Language.Conversations;
using Azure.Core;
using System.Text.Json;

namespace Example
{
    class Program
    {
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("DBf9ZOhd0V6iWvwCfdMFPKN5EOvoeV9XbXLRbC4tkF8H3aMpDAe7JQQJ99BBACYeBjFXJ3w3AAAaACOGnRRX");
        private static readonly Uri endpoint = new Uri("https://language-test132.cognitiveservices.azure.com/");

        static async Task Main(string[] args)
        {
            ConversationAnalysisClient client = new ConversationAnalysisClient(endpoint, credentials);
            await Summarization(client);
        }

        static async Task Summarization(ConversationAnalysisClient client)
        {
            var data = new
            {
                analysisInput = new
                {
                    conversations = new[]
                    {
                        new
                        {
                            conversationItems = new[]
                            {
                                new
                                {
                                    text = "Hello, you’re chatting with Rene. How may I help you?",
                                    id = "1",
                                    role = "Agent",
                                    participantId = "Agent",
                                },
                                new
                                {
                                    text = "Hi, I tried to set up wifi connection for Smart Brew 300 coffee machine, but it didn’t work.",
                                    id = "2",
                                    role = "Customer",
                                    participantId = "Customer",
                                },
                                new
                                {
                                    text = "I’m sorry to hear that. Let’s see what we can do to fix this issue. Could you please try the following steps for me? First, could you push the wifi connection button, hold for 3 seconds, then let me know if the power light is slowly blinking on and off every second?",
                                    id = "3",
                                    role = "Agent",
                                    participantId = "Agent",
                                },
                                new
                                {
                                    text = "Yes, I pushed the wifi connection button, and now the power light is slowly blinking?",
                                    id = "4",
                                    role = "Customer",
                                    participantId = "Customer",
                                },
                                new
                                {
                                    text = "Great. Thank you! Now, please check in your Contoso Coffee app. Does it prompt to ask you to connect with the machine?",
                                    id = "5",
                                    role = "Agent",
                                    participantId = "Agent",
                                },
                                new
                                {
                                    text = "No. Nothing happened.",
                                    id = "6",
                                    role = "Customer",
                                    participantId = "Customer",
                                },
                                new
                                {
                                    text = "I’m very sorry to hear that. Let me see if there’s another way to fix the issue. Please hold on for a minute.",
                                    id = "7",
                                    role = "Agent",
                                    participantId = "Agent",
                                }
                            },
                            id = "1",
                            language = "en",
                            modality = "text",
                        },
                    }
                },
                tasks = new[]
                {
                    new
                    {
                        parameters = new
                        {
                            summaryAspects = new[]
                            {
                                "issue",
                                "resolution",
                            }
                        },
                        kind = "ConversationalSummarizationTask",
                        taskName = "1",
                    },
                },
            };

            Operation<BinaryData> analyzeConversationOperation = await client.AnalyzeConversationsAsync(WaitUntil.Started, RequestContent.Create(data));
            analyzeConversationOperation.WaitForCompletion();

            using JsonDocument result = JsonDocument.Parse(analyzeConversationOperation.Value.ToStream());
            JsonElement jobResults = result.RootElement;
            foreach (JsonElement task in jobResults.GetProperty("tasks").GetProperty("items").EnumerateArray())
            {
                JsonElement results = task.GetProperty("results");

                Console.WriteLine("Conversations:");
                foreach (JsonElement conversation in results.GetProperty("conversations").EnumerateArray())
                {
                    Console.WriteLine($"Conversation: #{conversation.GetProperty("id").GetString()}");
                    Console.WriteLine("Summaries:");
                    foreach (JsonElement summary in conversation.GetProperty("summaries").EnumerateArray())
                    {
                        Console.WriteLine($"Text: {summary.GetProperty("text").GetString()}");
                        Console.WriteLine($"Aspect: {summary.GetProperty("aspect").GetString()}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}