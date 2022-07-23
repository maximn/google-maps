using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;

namespace GoogleMapsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        public BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private async void search_Click(object sender, RoutedEventArgs e)
        {
            DirectionsRequest directionsRequest = new DirectionsRequest()
            {
                Origin = origin.Text,
                Destination = destination.Text,
                TravelMode = (TravelMode)RadioButtonGroup.SelectedIndex,
                ApiKey = "AIzaSyDikeBAymgSWrWz-9Y7Danr2mNewZV_MwI"
            };

            DirectionsResponse directions = await GoogleMaps.Directions.QueryAsync(directionsRequest);

            // Static maps API - get static map of with the path of the directions request
            StaticMapsEngine staticMapGenerator = new StaticMapsEngine();

            //Path from previos directions request
            if (directions.Status == DirectionsStatusCodes.OK)
            {
                IEnumerable<Step> steps = directions.Routes.First().Legs.First().Steps;
                // All start locations
                IList<ILocationString> path = steps.Select(step => step.StartLocation).ToList<ILocationString>();
                // also the end location of the last step
                path.Add(steps.Last().EndLocation);
                // 4. Choose Map center
                int mid = path.Count / 2;
                string url = staticMapGenerator.GenerateStaticMapURL(new StaticMapRequest(path[mid], 15, new ImageSize(800, 400))
                {
                    Pathes = new List<GoogleMapsApi.StaticMaps.Entities.Path>(){ new GoogleMapsApi.StaticMaps.Entities.Path()
                {
                    Style = new PathStyle()
                     {
                            Color = "red"
                      },
                    Locations = path

                }},
                    ApiKey = "AIzaSyDikeBAymgSWrWz-9Y7Danr2mNewZV_MwI"
                });
                if (url != null)
                {
                    Uri uri = new Uri(url);
                    HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(uri);

                    HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    Stream imageStream = httpResponse.GetResponseStream();
                    Bitmap buddyIcon = new Bitmap(imageStream);
                    httpResponse.Close();
                    imageStream.Close();

                    image.Source = ToBitmapImage(buddyIcon);
                }
            }
            else MessageBox.Show("Нажаль, маршрут не знайдено");


        }

        private void swap_Click(object sender, RoutedEventArgs e)
        {
            origin.Text = origin.Text + destination.Text;
            destination.Text = origin.Text.Substring(0, origin.Text.Length - destination.Text.Length);
            origin.Text = origin.Text.Substring(destination.Text.Length);
        }
    }
}
