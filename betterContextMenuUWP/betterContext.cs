using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
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
        private static MenuFlyout meaningFlyout = new MenuFlyout();
        static string selectedText = "";

        public static void setContextMenu(TextBlock textBlockToTrack)
        {
            meaningFlyout = new MenuFlyout();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            ogTextBlock = textBlockToTrack;
            string textToUse = textBlockToTrack.SelectedText.Trim();
            if (textToUse != "" || textToUse != string.Empty)
            {
                MenuFlyoutItem copyItem = new MenuFlyoutItem { Text = "Copy"};
                copyItem.Click += CopyItem_Click;
                MenuFlyoutItem speakItem = new MenuFlyoutItem { Text = "Speak" };
                speakItem.Click += SpeakItem_Click;
                MenuFlyoutItem searchItem = new MenuFlyoutItem { Text = "Search" };
                searchItem.Click += SearchItem_Click;
                MenuFlyoutItem shareItem = new MenuFlyoutItem { Text = "Share" };
                shareItem.Click += ShareItem_Click;
                MenuFlyoutItem selectAllItem = new MenuFlyoutItem { Text = "Select All" };
                selectAllItem.Click += SelectAllItem_Click;

                meaningFlyout.Items.Add(copyItem);
                meaningFlyout.Items.Add(speakItem);
                meaningFlyout.Items.Add(searchItem);
                meaningFlyout.Items.Add(shareItem);
                meaningFlyout.Items.Add(selectAllItem);


                if (textBlockToTrack.SelectionStart != null && textToUse != string.Empty)
                {
                    selectedText = textToUse;
                    meaningFlyout.Placement = FlyoutPlacementMode.Bottom;
                    meaningFlyout.AllowFocusOnInteraction = false;
                    var rect = textBlockToTrack.SelectionEnd.GetCharacterRect(Windows.UI.Xaml.Documents.LogicalDirection.Forward);
                    Point p = new Point(rect.Right, rect.Bottom);
                    try
                    {
                        meaningFlyout.ShowAt(textBlockToTrack, p);
                    }
                    catch
                    {
                        meaningFlyout.ShowAt(textBlockToTrack);
                    }


                }
            }
        }

        private static void SelectAllItem_Click(object sender, RoutedEventArgs e)
        {
            ogTextBlock.SelectAll();
        }

        private static void ShareItem_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private async static void SearchItem_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri($"https://www.bing.com/search?q={selectedText}"));
        }

        private async static void SpeakItem_Click(object sender, RoutedEventArgs e)
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

        private static void CopyItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataPackage dataPackage = new DataPackage();
                dataPackage.RequestedOperation = DataPackageOperation.Copy;
                dataPackage.SetText(selectedText);
                Clipboard.SetContent(dataPackage);
            }
            
            catch(Exception ex)
            {

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
    }
}
