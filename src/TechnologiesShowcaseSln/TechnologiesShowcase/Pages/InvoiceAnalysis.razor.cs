using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PTI.Microservices.Library.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TechnologiesShowcase.Pages
{
    public partial class InvoiceAnalysis
    {
        [Inject]
        private FormRecognizerClient FormRecognizerClient { get; set; }
        private Response<RecognizedFormCollection> InvoiceAnalysisResult { get; set; }
        private bool IsLoading { get; set; }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            IsLoading = true;
            var fileStream = e.File.OpenReadStream();
            MemoryStream memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var invoiceAnalysisOperation = await this.FormRecognizerClient.StartRecognizeInvoicesAsync(memoryStream,
                recognizeInvoicesOptions: new RecognizeInvoicesOptions()
                {
                    IncludeFieldElements = true
                });
            this.InvoiceAnalysisResult=await invoiceAnalysisOperation.WaitForCompletionAsync();
            IsLoading = false;
        }
    }
}
