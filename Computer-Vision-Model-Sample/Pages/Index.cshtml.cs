using Azure;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Computer_Vision_Model_Sample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ImageAnalysisClient _imageAnalysisClient;

        public class ImageCaptionModel
        {
            public IFormFile? Image { get; set; }
            public string? ImageData { get; set; }
            public string? ImageCaption { get; set; }
            public List<DetectedObject>? Objects { get; set; }
        }

        public class DetectedObject
        {
            public string Name { get; set; } = string.Empty;
            public double Confidence { get; set; }
            public BoundingBox Box { get; set; } = new();
        }
        public class BoundingBox
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        [BindProperty]
        public ImageCaptionModel Input { get; set; } = new();

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;

            string endpointUrl = "https://vision-test2132.cognitiveservices.azure.com/";
            string key = "7hVdDnnNaB5ETDHcJqr85v0tgm6ions9sBbZIVBL35xhI0TfeAyAJQQJ99BBACYeBjFXJ3w3AAAFACOG4tD0";

            _imageAnalysisClient = new ImageAnalysisClient(new Uri(endpointUrl), new AzureKeyCredential(key));
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Input.Image == null || Input.Image.Length == 0)
            {
                ModelState.AddModelError("Input.Image", "Please upload a valid image file.");
                return Page();
            }

            try
            {
                using MemoryStream ms = new();
                await Input.Image.CopyToAsync(ms);
                ms.Position = 0;

                Input.ImageData = Convert.ToBase64String(ms.ToArray());


                ImageAnalysisResult analysisResult = await _imageAnalysisClient.AnalyzeAsync(
                    BinaryData.FromStream(ms),
                    VisualFeatures.Caption | VisualFeatures.Objects | VisualFeatures.Tags | VisualFeatures.People | VisualFeatures.SmartCrops,
                    new ImageAnalysisOptions { GenderNeutralCaption = true }
                );

                // Extract Caption
                Input.ImageCaption = analysisResult.Caption is not null
                    ? $"Caption: {analysisResult.Caption.Text} | Confidence: {analysisResult.Caption.Confidence:P2}"
                    : "No caption detected.";
                // Extract Object Detection
                Input.Objects = analysisResult.Objects.Values.Select(obj => new DetectedObject
                {
                    Name = obj.BoundingBox.X.ToString(),
                    Confidence = obj.BoundingBox.Height,
                    Box = new BoundingBox
                    {
                        X = obj.BoundingBox.X,
                        Y = obj.BoundingBox.Y,
                        Width = obj.BoundingBox.Width,
                        Height = obj.BoundingBox.Height
                    }
                }).ToList();


                _logger.LogInformation("Image processed successfully with {ObjectCount} objects detected.", Input.Objects.Count);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing image.");
                ModelState.AddModelError(string.Empty, "An error occurred while processing the image. Please try again.");
            }

            return Page();
        }
    }
}
