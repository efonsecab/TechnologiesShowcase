using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PTI.Microservices.Library.Services.Specialized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TechnologiesShowcase.Pages
{
    public partial class AudibleWeather
    {
        [Inject]
        private AudibleWeatherService AudibleWeatherService { get; set; }
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private double? Latitude { get; set; }
        private double? Longitude { get; set; }
        private string CoordinateAudioBase64 { get; set; }

        // Load the module and keep a reference to it
        // You need to use .AsTask() to convert the ValueTask to Task as it may be awaited multiple times
        private Task<IJSObjectReference> _geoCoordinatesInfoModule;
        private Task<IJSObjectReference> GeoCoordinatesInfoModule => _geoCoordinatesInfoModule ??=
            JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/geoCoordinatesInfo.js").AsTask();

        private static Action<double, double> UpdateCoordinatesDisplayAction { get; set; }
        private bool IsCoordinatesAudioLoaded { get; set; } = false;

        private bool IsLoading { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    IsLoading = true;
                    StateHasChanged();
                    await GetCurrentLocation();
                    UpdateCoordinatesDisplayAction = UpdateCoordinatesDisplay;
                }
                finally
                {
                    IsLoading = false;
                    StateHasChanged();
                }
            }
        }

        private async void UpdateCoordinatesDisplay(double latitude, double longitude)
        {
            try
            {
                IsLoading = true;
                StateHasChanged();
                this.Latitude = latitude;
                this.Longitude = longitude;
                MemoryStream outputStream = new MemoryStream();
                await this.AudibleWeatherService.SpeakCurrentWeatherToStreamAsync(new PTI.Microservices.Library.Models.Shared.GeoCoordinates()
                {
                    Latitude = latitude,
                    Longitude = longitude
                }, outputStream);
                var base64Audio = Convert.ToBase64String(outputStream.ToArray());
                this.CoordinateAudioBase64= base64Audio;
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private async Task GetCurrentLocation()
        {
            var module = await GeoCoordinatesInfoModule;
            await module.InvokeVoidAsync("getCurrentLocation");
        }

        [JSInvokable]
        public static void ongetCurrentLocationSuccess(double latitude, double longitude)
        {
            UpdateCoordinatesDisplayAction(latitude, longitude);
        }
    }
}
