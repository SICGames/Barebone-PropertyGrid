using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Drawing;

namespace TestingForm01
{

    public class Stuff
    {
        [Category("Person"), DisplayName("Name")]
        public string Name { get; set; }
        [Category("Person")]
        public int Age { get; set; }
        [Category("Person")]
        public string City { get; set; }
        [Category("Person")]
        public string State { get; set; }
        [Category("Person")]
        public string Zipcode { get; set; }
        [Category("Person"), Description("Person's ID File."), Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string IDFile { get; set; }
        [Category("Person"), Description("Has person got cats?")]
        public bool hasCats { get; set; }
        [Category("Person"), Description("Person's hair color")]
        public Color HairColor { get; set; }
    }
}
