using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace betterContextMenuUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            string textToUse = "Somebody once told me the world is gonna roll me\nI ain't the sharpest tool in the shed\nShe was looking kind of dumb with her finger and her thumb\nIn the shape of an \"L\" on her forehead\n\nWell the years start coming and they don't stop coming\nFed to the rules and I hit the ground running\nDidn't make sense not to live for fun\nYour brain gets smart but your head gets dumb\nSo much to do, so much to see\nSo what's wrong with taking the back streets?\nYou'll never know if you don't go\nYou'll never shine if you don't glow\nHey now, you're an all-star, get your game on, go play\nHey now, you're a rock star, get the show on, get paid\nAnd all that glitters is gold\nOnly shooting stars break the mold";
            myTextBlock.Text = textToUse;
        }

        

        private void myTextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

            var textblockToUse = (TextBlock)sender;
          
            contextMenu.setContextMenu(textblockToUse);
           
        }

       
    }
}
