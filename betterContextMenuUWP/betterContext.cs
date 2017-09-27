using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Media.Core;
using Windows.Media.SpeechSynthesis;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace betterContextMenuUWP
{
   public class contextMenu
    {
        
      static  DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
       static TextBlock ogTextBlock;
        private static Flyout meaningFlyout = new Flyout();
        static string selectedText = "";

        public static void setContextMenu(TextBlock textBlockToTrack)
        {
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            ogTextBlock = textBlockToTrack;
            string textToUse = textBlockToTrack.SelectedText.Trim();
            if (textToUse != "" || textToUse != string.Empty)
            {
                Button copyButton = new Button()
                {
                    Width = 120,
                    Height = 50,
                    Content = "Copy",
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Background = new SolidColorBrush(Colors.Transparent)
                };
                copyButton.Click += CopyButton_Click;
                copyButton.Margin = new Thickness(0);

                Button speakButton = new Button()
                {
                    Width = 120,
                    Height = 50,
                    Content = "Speak",
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Background = new SolidColorBrush(Colors.Transparent)
                };

                speakButton.Click += SpeakButon_Click;
                speakButton.Margin = new Thickness(0);

                Button searchButton = new Button()
                {
                    Width = 120,
                    Height = 50,
                    Content = "Search Bing™",
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Background = new SolidColorBrush(Colors.Transparent)
                };

                searchButton.Click += SearchButton_Click;
                searchButton.Margin = new Thickness(0);

                Button shareButton = new Button()
                {
                    Width = 120,
                    Height = 50,
                    Content = "Share",
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Background = new SolidColorBrush(Colors.Transparent)
                };

                shareButton.Click += ShareButton_Click;
                shareButton.Margin = new Thickness(0);

                Button selectAllButton = new Button()
                {
                    Width = 120,
                    Height = 50,
                    Content = "Select All",
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Background = new SolidColorBrush(Colors.Transparent)
                };

                selectAllButton.Click += SelectAllButton_Click;
                selectAllButton.Margin = new Thickness(0);

                var myFlyoutPresenterStyle = createFlyoutPresenterStyle();
                meaningFlyout.FlyoutPresenterStyle = myFlyoutPresenterStyle;

                StackPanel horizontalPanel = new StackPanel();

                horizontalPanel.Padding = new Thickness(0);
                horizontalPanel.Margin = new Thickness(0);
                horizontalPanel.Children.Add(copyButton);
                horizontalPanel.Children.Add(speakButton);
                horizontalPanel.Children.Add(searchButton);
                horizontalPanel.Children.Add(shareButton);
                horizontalPanel.Children.Add(selectAllButton);
                meaningFlyout.Content = horizontalPanel;



                if (textBlockToTrack.SelectionStart != null && textToUse != string.Empty)
                {
                    selectedText = textToUse;
                    meaningFlyout.Placement = FlyoutPlacementMode.Bottom;
                    meaningFlyout.AllowFocusOnInteraction = false;
                    meaningFlyout.ShowAt(textBlockToTrack.SelectionStart.VisualParent);

                }
            }
        }

        private static void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.SetText(selectedText);
            request.Data.Properties.Title = "Share Selected Text";
            request.Data.Properties.Description = "Share the text you selected with another app.";
        }

        private static void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            ogTextBlock.SelectAll();
        }

        private static void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private static async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri($"https://www.bing.com/search?q={selectedText}"));
        }

        private static Style createFlyoutPresenterStyle()
        {
            var style = new Style(typeof(FlyoutPresenter));
            style.Setters.Add(new Setter(FlyoutPresenter.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch));
            style.Setters.Add(new Setter(FlyoutPresenter.VerticalContentAlignmentProperty, VerticalAlignment.Stretch));
            style.Setters.Add(new Setter(FlyoutPresenter.IsTabStopProperty, false));
            style.Setters.Add(new Setter(FlyoutPresenter.BackgroundProperty, App.Current.Resources["SystemControlBackgroundChromeMediumLowBrush"]));
            style.Setters.Add(new Setter(FlyoutPresenter.BorderBrushProperty, Colors.Transparent));
            style.Setters.Add(new Setter(FlyoutPresenter.BorderThicknessProperty, new Thickness(0, 0, 0, 0)));
            style.Setters.Add(new Setter(FlyoutPresenter.PaddingProperty, new Thickness(0, 0, 0, 0)));
            style.Setters.Add(new Setter(FlyoutPresenter.MarginProperty, new Thickness(0, 0, 0, 0)));
            style.Setters.Add(new Setter(FlyoutPresenter.MinWidthProperty, 96));
            style.Setters.Add(new Setter(FlyoutPresenter.MinHeightProperty, 44));
            //style.Setters.Add(new Setter(FlyoutPresenter... //Do It For All Properties

            return style;
        }

        private static async void SpeakButon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                meaningFlyout.Hide();
                // The media object for controlling and playing audio.
                MediaPlayerElement mediaElement = new MediaPlayerElement();

                // The object for controlling the speech synthesis engine (voice).
                var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();

                // Generate the audio stream from plain text.
                SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(selectedText);


                // Send the stream to the media object.

                mediaElement.Source = MediaSource.CreateFromStream(stream, stream.ContentType);
                mediaElement.MediaPlayer.Play();
            }
            catch (Exception ex)
            {


            }
        }

        private static void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                meaningFlyout.Hide();

            }
            catch (Exception ex)
            {


            }
        }
    }
}
