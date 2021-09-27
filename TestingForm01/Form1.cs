/*
    Simple Demonstration how to generate controls like PropertyGrid. May not be all fancy. But this includes: color picker, filepicker, checkbox on booleans.
    This was just created to figure out some stuff.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Reflection;

namespace TestingForm01
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BuildControls(object o)
        {
            PropertyInfo[] properties = o.GetType().GetProperties();
            
            var lineY = 0;
            var fileBrowserRequested = false;
            var index = 0;

            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(int))
                {
                    System.Diagnostics.Debug.WriteLine($"{prop.Name}'s value is a integer.");
                }

                //-- testing purposes to try to get attributes, description, and editorattributes.
                AttributeCollection attributecollection = TypeDescriptor.GetProperties(o)[prop.Name].Attributes;
                DescriptionAttribute descriptionAttribute = (DescriptionAttribute)attributecollection[typeof(DescriptionAttribute)];
                System.Diagnostics.Debug.WriteLine($"{prop.Name}'s description: {descriptionAttribute.Description}");
                EditorAttribute editorAttribute = (EditorAttribute)attributecollection[typeof(EditorAttribute)];
                if (editorAttribute != null)
                {
                    if (editorAttribute.EditorTypeName.Contains("FileNameEditor"))
                    {
                        fileBrowserRequested = true;
                        //-- System.Diagnostics.Debug.WriteLine($"{prop.Name}'s requires file to be browsed.");
                    }
                }
                //-- System.Diagnostics.Debug.WriteLine($"{prop.Name}: {prop.GetValue(o)}");
                Label label = new Label();
                label.Text = prop.Name;
                label.Location = new Point(0, lineY);
                label.Height = 24;
                this.Controls.Add(label);

                if (prop.PropertyType == typeof(bool))
                {
                    CheckBox checkbox = new CheckBox();
                    checkbox.Checked = (bool)prop.GetValue(o);
                    checkbox.Location = new Point(100, lineY);
                    checkbox.Name = $"checkbox{index}";
                    this.Controls.Add(checkbox);
                }
                if (fileBrowserRequested)
                {
                    TextBox textbox = new TextBox();
                    textbox.Text = prop.GetValue(o).ToString();
                    textbox.Location = new Point(100, lineY);
                    textbox.BackColor = Color.White;
                    textbox.ForeColor = Color.Black;
                    textbox.Width = 100;
                    textbox.Height = 24;
                    textbox.Tag = prop.Name;
                    textbox.Name = $"textbox{index}";
                    this.Controls.Add(textbox);

                    Button browseBtn = new Button();
                    browseBtn.Text = "...";
                    browseBtn.Width = 32;
                    browseBtn.Location = new Point(200, lineY);
                    browseBtn.ForeColor = Color.Black;
                    browseBtn.BackColor = SystemColors.ControlLight;

                    browseBtn.Name = $"button{index}";
                    browseBtn.Click += BrowseBtn_Click1;
                    this.Controls.Add(browseBtn);
                    fileBrowserRequested = false;

                }
                if(prop.PropertyType == typeof(Color))
                {
                    Panel colorpanel = new Panel();
                    colorpanel.Height = 16;
                    colorpanel.Width = 100;
                    colorpanel.BackColor = (Color)prop.GetValue(o);
                    colorpanel.Location = new Point(100, lineY);
                    colorpanel.Click += Colorpanel_Click;
                    this.Controls.Add(colorpanel);
                }
                else
                {
                    TextBox textbox = new TextBox();
                    textbox.Text = prop.GetValue(o).ToString();
                    textbox.Location = new Point(100, lineY);
                    textbox.BackColor = Color.White;
                    textbox.ForeColor = Color.Black;
                    textbox.Width = 100;
                    textbox.Height = 24;
                    this.Controls.Add(textbox);
                }
                lineY += label.Height;
                index++;
            }
        }
        //--- ColorPickerEditor.
        private void Colorpanel_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if(c.ShowDialog() == DialogResult.OK)
            {
                Panel p = (Panel)sender;
                p.BackColor = c.Color;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Stuff stuff = new Stuff();
            stuff.Name = "John Doe";
            stuff.Age = 50;
            stuff.City = "New Lawn";
            stuff.State = "AL";
            stuff.Zipcode = "01234";
            stuff.IDFile = "id.card";
            stuff.hasCats = true;
            stuff.HairColor = Color.Red;
            
            BuildControls(stuff);
        }

        //-- FilePicker Editor
        private void BrowseBtn_Click1(object sender, EventArgs e)
        {

            //-- probably better way to get sibling (previous) control.
            OpenFileDialog opf = new OpenFileDialog();
            if(opf.ShowDialog() == DialogResult.OK)
            {
                Button b = (Button)sender;
                var i = 0;
                
                foreach(Control c in this.Controls)
                {
                    if(c.Name == b.Name)
                    {
                        if (this.Controls[i - 1].GetType() == typeof(TextBox))
                        {
                            TextBox t = this.Controls[i - 1] as TextBox;
                            t.Text = opf.FileName.Substring(opf.FileName.LastIndexOf(@"\") + 1);
                            break;
                        }
                    }
                    i++;
                }
            }
            
        }

    }
}
