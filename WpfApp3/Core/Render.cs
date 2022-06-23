using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp3.Controls;
using WpfApp3.Core.Style;

namespace WpfApp3.core
{
    public class Render
    {
        private VirtualizingStackPanel content;
        private List<core.Tags.Tag> tags;
        private List<Core.Style.Style> styles;
        public Render(List<core.Tags.Tag> tags, List<Core.Style.Style> styles)
        {
            content = new VirtualizingStackPanel();

            this.tags = tags;
            this.styles = styles;
        }

        public VirtualizingStackPanel GetContent()
        {
            return content;
        }
        public void Handle()
        {

            foreach (var tag in tags)
            {
                var el = new Div();
                el.Name = tag.Name;
                el.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                el.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                HandlePropertie(el, tag.Properties);

                el.Content = new TextBlock()
                {
                    Text = tag.Content.ToString()
                };

                content.Children.Add(el);
            }
        }

        private void HandlePropertie(Div control, List<Tags.TagProperty> propties)
        {
            foreach (var propertie in propties)
            {
                switch (propertie.Name)
                {
                    case "class":
                        SetStyles(control, propertie.Value);
                        break;
                }
            }
        }

        private void SetStyles(Div control, string className)
        {
            Debug.WriteLine("给：" + control.Name + "，设置样式：" + className);

            var style = styles.Where(m => m.Name == className).FirstOrDefault();
            foreach (var p in style!.Properties)
            {
                Debug.WriteLine("给：" + control.Name + "，设置样式：" + className + " -> " + p.Name + " > " + p.Value);

                switch (p.Name)
                {
                    case "background":
                        control.Background = GetBrush(p.Value);
                        break;
                    case "color":
                        control.Foreground = GetBrush(p.Value);
                        break;
                    case "width":
                        control.Width = GetDouble(p.Value);
                        break;
                    case "height":
                        control.Height = GetDouble(p.Value);
                        break;
                }

            }
        }

        private double GetDouble(string value)
        {
            return double.Parse(value.Replace("px", ""));
        }

        private Brush GetBrush(string color)
        {
            if (color.ToLower() == "red")
            {
                return new SolidColorBrush(Colors.Red);
            }
            else if (color.ToLower() == "black")
            {
                return new SolidColorBrush(Colors.Black);
            }
            else if (color.ToLower() == "white")
            {
                return new SolidColorBrush(Colors.White);
            }
            else if (color.ToLower() == "green")
            {
                return new SolidColorBrush(Colors.Green);
            }
            return new SolidColorBrush(Colors.Green);
        }

    }
}
