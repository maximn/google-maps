using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.Entities.PlaceAutocomplete.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleMapsApi.PlaceAutocompleteTest
{
    public partial class PlaceAutocomplete : Form
    {
        public PlaceAutocomplete()
        {
            InitializeComponent();
        }

        private void TextBoxAddressEntry_TextChanged(object sender, EventArgs e)
        {
            string centreCoordinate = TextBoxCoordinate.Text;
            double latitude;
            double longitude;
            int radius;
            string[] components = centreCoordinate.Split(',');
            if ( (components.Length != 2) ||
                 !double.TryParse(components[0], out latitude) ||
                 !double.TryParse(components[1], out longitude) ||
                 !int.TryParse( TextBoxRadius.Text, out radius ) )
            {
                ListBoxResults.Items.Clear();
                ListBoxResults.Items.Add("Invalid centre coordinates or radius");
                ListBoxResults.ForeColor = Color.Red;
                lastResult = null;
                return;
            }

            var request = new PlaceAutocompleteRequest
            {
                ApiKey = TextBoxApiKey.Text,
                Input = TextBoxAddressEntry.Text,
                Location = new GoogleMapsApi.Entities.Common.Location(latitude, longitude),
                Radius = radius
            };

            lastResult = GoogleMaps.PlaceAutocomplete.Query(request);
            LabelQuota.Text = string.Format( "Total PlaceAutocomplete requests: {0}", ++totalRequests);

            if (lastResult.Status == Status.OK)
            {
                // Clear previous results
                ListBoxResults.Items.Clear();
                ListBoxResults.ForeColor = Color.Black;

                // Populate list box with results
                foreach (var address in lastResult.Results)
                {
                    ListBoxResults.Items.Add(address.Description);
                }

                // Select first result - allow user to select alternate result
                if (lastResult.Results.Count() > 0)
                {
                    ListBoxResults.SelectedIndex = 0;
                }
                    
            }
            else
            {
                ListBoxResults.Items.Clear();
                ListBoxResults.Items.Add(lastResult.Status.ToString());
                ListBoxResults.ForeColor = Color.Brown;
                lastResult = null;
            }
        }

        /// <summary>
        /// User chooses an Autocomplete result - show the details
        /// </summary>
        private void ListBoxResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear previous details
            TextBoxResultDetails.Text = string.Empty;

            if ((lastResult != null) && (ListBoxResults.SelectedIndex >= 0) && (ListBoxResults.SelectedIndex < lastResult.Results.Count()))
            {
                var details = lastResult.Results.ElementAt(ListBoxResults.SelectedIndex);
                TextBoxResultDetails.Text += ( string.Format("DESCRIPTION: {0}", details.Description) + Environment.NewLine );
                TextBoxResultDetails.Text += ( string.Format("REFERENCE: {0}", details.Reference) + Environment.NewLine );
                TextBoxResultDetails.Text += ( string.Format("ID: {0}", details.ID) + Environment.NewLine );

                if (details.Terms != null)
                {
                    foreach (Term term in details.Terms)
                    {
                        TextBoxResultDetails.Text += ( string.Format("TERM: {0} - offset {1}", term.Value, term.Offset) + Environment.NewLine );
                    }
                }

                if (details.Types != null)
                {
                    string allTypes = string.Join(", ", details.Types);
                    TextBoxResultDetails.Text += ( string.Format("TYPES: {0}", allTypes) + Environment.NewLine );
                }

                if (details.MatchedSubstrings != null)
                {
                    foreach (MatchedSubstring substring in details.MatchedSubstrings)
                    {
                        TextBoxResultDetails.Text += (string.Format("MATCHED SUBSTRING: offset {0} length {1}",  
                                                                    substring.Offset, substring.Length) + Environment.NewLine);
                    }
                }
            }
        }

        int totalRequests = 0;
        PlaceAutocompleteResponse lastResult = null;
    }
}
