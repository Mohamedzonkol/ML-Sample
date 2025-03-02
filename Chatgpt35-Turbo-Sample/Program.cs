using Azure;
using Azure.AI.OpenAI;
using OpenAI.Assistants;

string? endpoint = "https://20812-m7r4rn9n-swedencentral.cognitiveservices.azure.com/";
string? key = "Ah0V4M6RwZ4Lbt2kFSxMebS3o9a1CgQkP0tyruWsZGE1UZjL7bbUJQQJ99BCACfhMk5XJ3w3AAAAACOG5VKn";

if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
{
    Console.WriteLine("Please set the environment variables AZURE_OPENAI_ENDPOINT and AZURE_OPENAI_API_KEY.");
    return;
}

AzureOpenAIClient azureClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
#pragma warning disable OPENAI001
AssistantClient assistantClient = azureClient.GetAssistantClient();

AssistantCreationOptions assistantCreationOptions = new AssistantCreationOptions()
{
    Name = "Assistant390",
    Instructions = "",
    Tools = { },
    ToolResources = { },
    Temperature = 1
};

Assistant assistant = await assistantClient.CreateAssistantAsync("gpt-35-turbo", assistantCreationOptions);

ThreadInitializationMessage initialMessage = new ThreadInitializationMessage(MessageRole.User, new List<MessageContent>
{
    ("hi")
});
AssistantThread thread = await assistantClient.CreateThreadAsync(new ThreadCreationOptions()
{
    InitialMessages = { initialMessage },
});

RunCreationOptions runOptions = new RunCreationOptions()
{
    AdditionalInstructions = "When possible, talk like a pirate."
};

await foreach (StreamingUpdate streamingUpdate in assistantClient.CreateRunStreamingAsync(thread.Id, assistant.Id, runOptions))
{
    if (streamingUpdate.UpdateKind == StreamingUpdateReason.RunCreated)
    {
        Console.WriteLine("--- Run started! ---");
    }
    else if (streamingUpdate is MessageContentUpdate contentUpdate)
    {
        Console.Write(contentUpdate.Text);
        if (contentUpdate.ImageFileId is not null)
        {
            Console.WriteLine($"[Image content file ID: {contentUpdate.ImageFileId}]");
        }
    }
}