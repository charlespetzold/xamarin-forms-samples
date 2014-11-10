using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace RotatingSpokes
{
    public class App
    {
        public static Page GetMainPage()
        {
            return new RotatingSpokesPage();
        }
    }
}
