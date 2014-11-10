
using Xamarin.Forms;

namespace KwazyThreeDee
{
    public class App
    {
        public static Page GetMainPage()
        {
            return new NavigationPage(new HomePage());
        }
    }
}
