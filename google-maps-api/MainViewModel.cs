using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace google_maps_api
{
    public class MainViewModel:INotifyPropertyChanged
    {
        private RouteMapRequest _routemaprequest;
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
        private RelayCommand _zoomincommand;
        public RelayCommand ZoomInCommand
        {
            get => _zoomincommand;
            set
            {
                _zoomincommand = value;
                NotifyPropertyChanged();
            }
        }
        private RelayCommand _zoomoutcommand;
        public RelayCommand ZoomOutCommand
        {
            get => _zoomoutcommand;
            set
            {
                _zoomoutcommand = value;
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

                try
                {
                _routemaprequest = new RouteMapRequest(new AddressLocation($"{Origin}"), new ImageSize(800, 400), $"{Origin}", $"{Destination}")
                { Scale = 2 };
                _routemaprequest.CalculateZoom = true;
                _routemaprequest.ApiKey = apikey;
                //ImageSource = new RouteMapsEngine().GenerateRouteMapURL(_routemaprequest);
                ImageSource = new RouteMapsEngine().GenerateRouteMapURL(_routemaprequest).URL;
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Route not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            ZoomInCommand = new RelayCommand((_) =>
            {
                try
                {
                    if (_routemaprequest is null) return;
                    _routemaprequest.CalculateZoom = false;
                    _routemaprequest.Zoom += 1;
                    ImageSource = new RouteMapsEngine().GenerateRouteMapURL(_routemaprequest).URL;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            });
            ZoomOutCommand = new RelayCommand((_) =>
            {
                try
                {
                    if (_routemaprequest is null) return;
                    _routemaprequest.CalculateZoom = false;
                    _routemaprequest.Zoom -= 1;
                    ImageSource = new RouteMapsEngine().GenerateRouteMapURL(_routemaprequest).URL;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });


        }
        
        
    }
}
