namespace GoogleMapsApi.PlaceAutocompleteTest
{
    partial class PlaceAutocomplete
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelCoordinate = new System.Windows.Forms.Label();
            this.LabelAddress = new System.Windows.Forms.Label();
            this.LabelResults = new System.Windows.Forms.Label();
            this.TextBoxCoordinate = new System.Windows.Forms.TextBox();
            this.TextBoxAddressEntry = new System.Windows.Forms.TextBox();
            this.LabelApiKey = new System.Windows.Forms.Label();
            this.TextBoxApiKey = new System.Windows.Forms.TextBox();
            this.LabelQuota = new System.Windows.Forms.Label();
            this.ListBoxResults = new System.Windows.Forms.ListBox();
            this.TextBoxResultDetails = new System.Windows.Forms.TextBox();
            this.LabelDetails = new System.Windows.Forms.Label();
            this.LabelRadius = new System.Windows.Forms.Label();
            this.TextBoxRadius = new System.Windows.Forms.TextBox();
            this.PictureBoxCredits = new System.Windows.Forms.PictureBox();
            this.textBoxCountry = new System.Windows.Forms.TextBox();
            this.labelCountry = new System.Windows.Forms.Label();
            this.textBoxTiming = new System.Windows.Forms.TextBox();
            this.labelTiming = new System.Windows.Forms.Label();
            this.buttonClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxCredits)).BeginInit();
            this.SuspendLayout();
            // 
            // LabelCoordinate
            // 
            this.LabelCoordinate.AutoSize = true;
            this.LabelCoordinate.Location = new System.Drawing.Point(12, 64);
            this.LabelCoordinate.Name = "LabelCoordinate";
            this.LabelCoordinate.Size = new System.Drawing.Size(92, 13);
            this.LabelCoordinate.TabIndex = 0;
            this.LabelCoordinate.Text = "Centre Search On";
            // 
            // LabelAddress
            // 
            this.LabelAddress.AutoSize = true;
            this.LabelAddress.Location = new System.Drawing.Point(13, 93);
            this.LabelAddress.Name = "LabelAddress";
            this.LabelAddress.Size = new System.Drawing.Size(71, 13);
            this.LabelAddress.TabIndex = 1;
            this.LabelAddress.Text = "Address entry";
            // 
            // LabelResults
            // 
            this.LabelResults.AutoSize = true;
            this.LabelResults.Location = new System.Drawing.Point(13, 127);
            this.LabelResults.Name = "LabelResults";
            this.LabelResults.Size = new System.Drawing.Size(79, 13);
            this.LabelResults.TabIndex = 2;
            this.LabelResults.Text = "Search Results";
            // 
            // TextBoxCoordinate
            // 
            this.TextBoxCoordinate.Location = new System.Drawing.Point(155, 60);
            this.TextBoxCoordinate.Name = "TextBoxCoordinate";
            this.TextBoxCoordinate.Size = new System.Drawing.Size(157, 20);
            this.TextBoxCoordinate.TabIndex = 3;
            this.TextBoxCoordinate.Text = "53.4635332, -2.2419169";
            // 
            // TextBoxAddressEntry
            // 
            this.TextBoxAddressEntry.Location = new System.Drawing.Point(155, 90);
            this.TextBoxAddressEntry.Name = "TextBoxAddressEntry";
            this.TextBoxAddressEntry.Size = new System.Drawing.Size(265, 20);
            this.TextBoxAddressEntry.TabIndex = 4;
            this.TextBoxAddressEntry.TextChanged += new System.EventHandler(this.TextBoxAddressEntry_TextChanged);
            // 
            // LabelApiKey
            // 
            this.LabelApiKey.AutoSize = true;
            this.LabelApiKey.Location = new System.Drawing.Point(12, 15);
            this.LabelApiKey.Name = "LabelApiKey";
            this.LabelApiKey.Size = new System.Drawing.Size(69, 13);
            this.LabelApiKey.TabIndex = 0;
            this.LabelApiKey.Text = "Your API key";
            // 
            // TextBoxApiKey
            // 
            this.TextBoxApiKey.Location = new System.Drawing.Point(155, 12);
            this.TextBoxApiKey.Name = "TextBoxApiKey";
            this.TextBoxApiKey.Size = new System.Drawing.Size(265, 20);
            this.TextBoxApiKey.TabIndex = 3;
            this.TextBoxApiKey.Text = "Paste your Google API key here";
            // 
            // LabelQuota
            // 
            this.LabelQuota.AutoSize = true;
            this.LabelQuota.Location = new System.Drawing.Point(155, 39);
            this.LabelQuota.Name = "LabelQuota";
            this.LabelQuota.Size = new System.Drawing.Size(241, 13);
            this.LabelQuota.TabIndex = 6;
            this.LabelQuota.Text = "WARNING: this test will consume your API quota!";
            // 
            // ListBoxResults
            // 
            this.ListBoxResults.FormattingEnabled = true;
            this.ListBoxResults.Location = new System.Drawing.Point(16, 144);
            this.ListBoxResults.Name = "ListBoxResults";
            this.ListBoxResults.Size = new System.Drawing.Size(714, 121);
            this.ListBoxResults.TabIndex = 7;
            this.ListBoxResults.SelectedIndexChanged += new System.EventHandler(this.ListBoxResults_SelectedIndexChanged);
            // 
            // TextBoxResultDetails
            // 
            this.TextBoxResultDetails.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxResultDetails.Location = new System.Drawing.Point(16, 302);
            this.TextBoxResultDetails.Multiline = true;
            this.TextBoxResultDetails.Name = "TextBoxResultDetails";
            this.TextBoxResultDetails.ReadOnly = true;
            this.TextBoxResultDetails.Size = new System.Drawing.Size(380, 321);
            this.TextBoxResultDetails.TabIndex = 8;
            // 
            // LabelDetails
            // 
            this.LabelDetails.AutoSize = true;
            this.LabelDetails.Location = new System.Drawing.Point(13, 286);
            this.LabelDetails.Name = "LabelDetails";
            this.LabelDetails.Size = new System.Drawing.Size(70, 13);
            this.LabelDetails.TabIndex = 2;
            this.LabelDetails.Text = "Result details";
            // 
            // LabelRadius
            // 
            this.LabelRadius.AutoSize = true;
            this.LabelRadius.Location = new System.Drawing.Point(510, 64);
            this.LabelRadius.Name = "LabelRadius";
            this.LabelRadius.Size = new System.Drawing.Size(80, 13);
            this.LabelRadius.TabIndex = 0;
            this.LabelRadius.Text = "Radius (metres)";
            // 
            // TextBoxRadius
            // 
            this.TextBoxRadius.Location = new System.Drawing.Point(605, 60);
            this.TextBoxRadius.Name = "TextBoxRadius";
            this.TextBoxRadius.Size = new System.Drawing.Size(79, 20);
            this.TextBoxRadius.TabIndex = 3;
            this.TextBoxRadius.Text = "20000";
            // 
            // PictureBoxCredits
            // 
            this.PictureBoxCredits.BackColor = System.Drawing.Color.White;
            this.PictureBoxCredits.Image = global::GoogleMapsApi.PlaceAutocompleteTest.Properties.Resources.powered_by_google_on_white;
            this.PictureBoxCredits.Location = new System.Drawing.Point(576, 106);
            this.PictureBoxCredits.Name = "PictureBoxCredits";
            this.PictureBoxCredits.Padding = new System.Windows.Forms.Padding(20, 8, 20, 8);
            this.PictureBoxCredits.Size = new System.Drawing.Size(154, 32);
            this.PictureBoxCredits.TabIndex = 9;
            this.PictureBoxCredits.TabStop = false;
            // 
            // textBoxCountry
            // 
            this.textBoxCountry.Location = new System.Drawing.Point(407, 61);
            this.textBoxCountry.MaxLength = 2;
            this.textBoxCountry.Name = "textBoxCountry";
            this.textBoxCountry.Size = new System.Drawing.Size(79, 20);
            this.textBoxCountry.TabIndex = 11;
            this.textBoxCountry.Text = "GB";
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(330, 64);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(71, 13);
            this.labelCountry.TabIndex = 10;
            this.labelCountry.Text = "Country Code";
            // 
            // textBoxTiming
            // 
            this.textBoxTiming.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxTiming.Location = new System.Drawing.Point(407, 302);
            this.textBoxTiming.Multiline = true;
            this.textBoxTiming.Name = "textBoxTiming";
            this.textBoxTiming.ReadOnly = true;
            this.textBoxTiming.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTiming.Size = new System.Drawing.Size(327, 321);
            this.textBoxTiming.TabIndex = 13;
            // 
            // labelTiming
            // 
            this.labelTiming.AutoSize = true;
            this.labelTiming.Location = new System.Drawing.Point(404, 286);
            this.labelTiming.Name = "labelTiming";
            this.labelTiming.Size = new System.Drawing.Size(38, 13);
            this.labelTiming.TabIndex = 12;
            this.labelTiming.Text = "Timing";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(659, 273);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 14;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // PlaceAutocomplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 635);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.textBoxTiming);
            this.Controls.Add(this.labelTiming);
            this.Controls.Add(this.textBoxCountry);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.PictureBoxCredits);
            this.Controls.Add(this.TextBoxResultDetails);
            this.Controls.Add(this.ListBoxResults);
            this.Controls.Add(this.LabelQuota);
            this.Controls.Add(this.TextBoxAddressEntry);
            this.Controls.Add(this.TextBoxApiKey);
            this.Controls.Add(this.TextBoxRadius);
            this.Controls.Add(this.TextBoxCoordinate);
            this.Controls.Add(this.LabelDetails);
            this.Controls.Add(this.LabelResults);
            this.Controls.Add(this.LabelApiKey);
            this.Controls.Add(this.LabelRadius);
            this.Controls.Add(this.LabelAddress);
            this.Controls.Add(this.LabelCoordinate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlaceAutocomplete";
            this.ShowIcon = false;
            this.Text = "GoogleMapsApi PlaceAutocomplete Test";
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxCredits)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelCoordinate;
        private System.Windows.Forms.Label LabelAddress;
        private System.Windows.Forms.Label LabelResults;
        private System.Windows.Forms.TextBox TextBoxCoordinate;
        private System.Windows.Forms.TextBox TextBoxAddressEntry;
        private System.Windows.Forms.Label LabelApiKey;
        private System.Windows.Forms.TextBox TextBoxApiKey;
        private System.Windows.Forms.Label LabelQuota;
        private System.Windows.Forms.ListBox ListBoxResults;
        private System.Windows.Forms.TextBox TextBoxResultDetails;
        private System.Windows.Forms.Label LabelDetails;
        private System.Windows.Forms.Label LabelRadius;
        private System.Windows.Forms.TextBox TextBoxRadius;
        private System.Windows.Forms.PictureBox PictureBoxCredits;
        private System.Windows.Forms.TextBox textBoxCountry;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.TextBox textBoxTiming;
        private System.Windows.Forms.Label labelTiming;
        private System.Windows.Forms.Button buttonClear;
    }
}

