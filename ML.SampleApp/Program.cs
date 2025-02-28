// See https://aka.ms/new-console-template for more information
using ML_SampleApp;

Console.WriteLine("Hello, World!");
//Load sample data
var sampleData = new PredictiveModel.ModelInput()
{
    UDI = 2F,
    Product_ID = @"L47181",
    Air_temperature = 298.2F,
    Process_temperature = 308.7F,
    Rotational_speed = 1408F,
    Torque = 46.3F,
    Tool_wear = 3F,
};

//Load model and predict output
var result = PredictiveModel.Predict(sampleData);
Console.WriteLine("Using model to make single prediction -- Comparing actual Machine_failure with predicted Machine_failure from sample data...\n\n");


Console.WriteLine($"UDI: {2F}");
Console.WriteLine($"Product_ID: {@"L47181"}");
Console.WriteLine($"Air_temperature: {298.2F}");
Console.WriteLine($"Process_temperature: {308.7F}");
Console.WriteLine($"Rotational_speed: {1408F}");
Console.WriteLine($"Torque: {46.3F}");
Console.WriteLine($"Tool_wear: {3F}");
Console.WriteLine($"Machine_failure: {0F}");


var sortedScoresWithLabel = PredictiveModel.PredictAllLabels(sampleData);
Console.WriteLine($"{"Class",-40}{"Score",-20}");
Console.WriteLine($"{"-----",-40}{"-----",-20}");

foreach (var score in sortedScoresWithLabel)
{
    Console.WriteLine($"{score.Key,-40}{score.Value,-20}");
}

Console.WriteLine("=============== End of process, hit any key to finish ===============");
Console.ReadKey();
