namespace GameOfLife
{
    using System;
    using System.Windows;
    using System.Diagnostics;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;

    internal class AdWindow : Window
    {
        private readonly DispatcherTimer adTimer;
        private int imgNmb;     // the number of the image currently shown
        private string link;    // the URL where the currently shown ad leads to

        private readonly BitmapImage[] adImages;

        public AdWindow(Window owner)
        {
            var rnd = new Random();
            this.Owner = owner;
            this.Width = 350;
            this.Height = 100;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.ToolWindow;
            this.Title = "Support us by clicking the ads";
            this.Cursor = Cursors.Hand;
            this.ShowActivated = false;
            this.MouseDown += this.OnClick;

            adImages = new BitmapImage[]
            {
                new BitmapImage(new Uri("ad1.jpg", UriKind.Relative)),
                new BitmapImage(new Uri("ad2.jpg", UriKind.Relative)),
                new BitmapImage(new Uri("ad3.jpg", UriKind.Relative))
            };

            this.imgNmb = rnd.Next(1, 4);
            this.ChangeAds(this, new EventArgs());

            // Run the timer that changes the ad's image 
            this.adTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            this.adTimer.Tick += this.ChangeAds;
            this.adTimer.Start();
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start(this.link);
            this.Close();
        }
        
        protected override void OnClosed(EventArgs e)
        {
            //Unsubscribe();
            this.Unsubscribe();
            base.OnClosed(e);
        }

        public void Unsubscribe()
        {
            this.adTimer.Tick -= this.ChangeAds;
        }

        private void ChangeAds(object sender, EventArgs eventArgs)
        {
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = adImages[imgNmb];

            Background = myBrush;
            link = "http://example.com";
            imgNmb = (imgNmb + 1) % adImages.Length;
        }
    }
}