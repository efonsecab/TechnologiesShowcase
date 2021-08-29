using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PTI.Microservices.Library.Services.Specialized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TechnologiesShowcase.Pages
{
    public partial class AudibleComputerVision
    {
        [Inject]
        private AudibleComputerVisionService AudibleComputerVisionService { get; set; }
        private bool IsLoading { get; set; }
        private string AudioBase64 { get; set; }
        private string ImageBase64 { get; set; }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            IsLoading = true;
            if (e.File != null)
            {
                int maxSizeInMB = (1024 * 1000) * 10; // 10MB
                var fileStream = e.File.OpenReadStream(maxAllowedSize: maxSizeInMB);
                MemoryStream sourceImageMemoryStream = new MemoryStream();
                await fileStream.CopyToAsync(sourceImageMemoryStream);
                this.ImageBase64 = Convert.ToBase64String(sourceImageMemoryStream.ToArray());
                MemoryStream outputStream = new MemoryStream();
                sourceImageMemoryStream.Position = 0;
                await this.AudibleComputerVisionService.DescribeImageToStreamAsync(sourceImageMemoryStream, outputStream,
                    e.File.Name);
                this.AudioBase64 = Convert.ToBase64String(outputStream.ToArray());
            }
            IsLoading = false;
        }

    }
}
