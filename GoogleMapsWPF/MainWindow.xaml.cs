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
using MaterialDesignThemes.Wpf;

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
            if (String.IsNullOrEmpty(origin.Text) || String.IsNullOrEmpty(destination.Text))
                return;

            string[] waypoints = new string[destinations.Children.Count - 2];
            for (int i = 1; i < destinations.Children.Count - 1; i++)
            {
                waypoints[i - 1] = ((destinations.Children[i] as StackPanel).Children[0] as TextBox).Text;
            }

            DirectionsRequest directionsRequest = new DirectionsRequest()
            {
                Origin = origin.Text,
                Destination = destination.Text,
                Waypoints = waypoints,
                TravelMode = (TravelMode)RadioButtonGroup.SelectedIndex,
                ApiKey = "AIzaSyDikeBAymgSWrWz-9Y7Danr2mNewZV_MwI"
            };

            DirectionsResponse directions = await GoogleMaps.Directions.QueryAsync(directionsRequest);

            // Static maps API - get static map of with the path of the directions request
            StaticMapsEngine staticMapGenerator = new StaticMapsEngine();

            //Path from previos directions request
            if (directions.Status == DirectionsStatusCodes.OK)
            {
                List<Step> steps = new List<Step>();
                
                foreach (Route route in directions.Routes)
                {
                    foreach (Leg leg in route.Legs)
                    {
                        steps.AddRange(leg.Steps);
                    }
                }

                // All start locations
                IList<ILocationString> path = steps.Select(step => step.StartLocation).ToList<ILocationString>();
                // also the end location of the last step
                path.Add(steps.Last().EndLocation);

                // Choose Map center
                int mid = path.Count / 2;

                int sum = steps.Sum(step => step.Distance.Value);
                int zoom = sum > 45000 ? (45000 / sum) : 15; //needs to be completed
                string url = staticMapGenerator.GenerateStaticMapURL(new StaticMapRequest(path[mid], zoom, new ImageSize(800, 400))
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

        private void addDestination_Click(object sender, MouseButtonEventArgs e)
        {
            if (destinations.Children.Count < 9)
            {
                swap.Visibility = Visibility.Collapsed;

                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;

                TextBox destinationTB = new TextBox();
                destinationTB.Margin = new Thickness(5);
                destinationTB.Width = 170;
                destinationTB.Padding = new Thickness(10);
                HintAssist.SetHint(destinationTB, "Пункт призначення");

                PackIcon packIcon = new PackIcon();
                packIcon.Kind = PackIconKind.DeleteCircleOutline;
                packIcon.Width = 20;
                packIcon.Height = 20;
                packIcon.VerticalAlignment = VerticalAlignment.Center;
                packIcon.Cursor = Cursors.Hand;
                packIcon.MouseLeftButtonUp += delete_Click;

                sp.Children.Add(destinationTB);
                sp.Children.Add(packIcon);

                destinations.Children.Insert(destinations.Children.Count - 1, sp);
            }
            else
            {
                addDestination.Visibility = Visibility.Collapsed;
            }
        }

        private void delete_Click(object sender, MouseButtonEventArgs e)
        {
            StackPanel sp = (sender as PackIcon).Parent as StackPanel;
            destinations.Children.Remove(sp);
        }

    }
}
