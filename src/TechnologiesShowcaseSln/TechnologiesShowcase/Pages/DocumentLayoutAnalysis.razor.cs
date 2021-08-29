using Azure;
using Azure.AI.FormRecognizer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TechnologiesShowcase.Pages
{
    public partial class DocumentLayoutAnalysis
    {
        [Inject]
        private FormRecognizerClient FormRecognizerClient { get; set; }
        private Response<Azure.AI.FormRecognizer.Models.FormPageCollection> DocumentAnalysisResult { get; set; }
        private bool IsLoading { get; set; }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            IsLoading = true;
            var fileStream = e.File.OpenReadStream();
            MemoryStream memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var operation = await this.FormRecognizerClient.StartRecognizeContentAsync(memoryStream,
                recognizeContentOptions: new RecognizeContentOptions()
                {
                    ReadingOrder = FormReadingOrder.Natural
                });
            this.DocumentAnalysisResult = await operation.WaitForCompletionAsync();
            IsLoading = false;
        }
    }
}
