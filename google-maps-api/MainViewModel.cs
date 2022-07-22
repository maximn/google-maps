using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace google_maps_api
{
    public class MainViewModel:INotifyPropertyChanged
    {
        private readonly string apikey = "AIzaSyDikeBAymgSWrWz-9Y7Danr2mNewZV_MwI";
        private RelayCommand _drawcommand;
        public RelayCommand DrawCommand
        {
            get => _drawcommand;
            set
            {
                _drawcommand = value;
                NotifyPropertyChanged();
            }
        }
        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                NotifyPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
        string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private string _origin;
        public string Origin
        {
            get => _origin;
            set
            {
                _origin = value;
                NotifyPropertyChanged();
            }
        }
        private string _destination;
        public string Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                NotifyPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ImageSource = "https://media.discordapp.net/attachments/917760526094856212/1000076529226752072/unknown.png";
            DrawCommand = new RelayCommand((_) => {

                RouteMapRequest routeMapRequest = new RouteMapRequest(new AddressLocation($"{Origin}"), new ImageSize(800, 400), $"{Origin}", $"{Destination}")
                { Scale = 2 };
                routeMapRequest.CalculateZoom = true;
                routeMapRequest.ApiKey = apikey;
                routeMapRequest.CalculateZoom = true;
                ImageSource = new RouteMapsEngine().GenerateRouteMapURL(routeMapRequest);
                
            });
        }
        
        
    }
}
