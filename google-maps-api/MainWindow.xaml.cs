using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;

namespace google_maps_api
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            tb_origin.FilterMode = AutoCompleteFilterMode.Contains;
            tb_origin.ItemsSource = new string[] { "New York", "Ukraine", "Odesa" };
            tb_destination.FilterMode = AutoCompleteFilterMode.Contains;
            //tb_destination.ItemsSource = new string[] { "New York", "Kyiv", "Odesa" };
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_drawroute(object sender, RoutedEventArgs e)
        {
           
            //((MainViewModel)this.DataContext).ImageSource = result;
        }

        private async void tb_origin_TextChanged(object sender, RoutedEventArgs e)
        {   
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = ((MainViewModel)DataContext).apikey,
                Input = $"{tb_origin.Text}",
            };
            
            tb_origin.ItemsSource = (await GoogleMaps.PlaceAutocomplete.QueryAsync(request)).Results.Select(x => x.Description);
        }

        private async void tb_destination_TextChanged(object sender, RoutedEventArgs e)
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = ((MainViewModel)DataContext).apikey,
                Input = $"{tb_destination.Text}",
            };

            tb_destination.ItemsSource = (await GoogleMaps.PlaceAutocomplete.QueryAsync(request)).Results.Select(x => x.Description);
        }
    }
}