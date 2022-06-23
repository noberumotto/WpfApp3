using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp3.core;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var test = Application.GetResourceStream(new Uri("pack://application:,,,/WpfApp3;component/Views/index.vue", UriKind.RelativeOrAbsolute));

            StreamReader reader = new StreamReader(test.Stream);
            string text = reader.ReadToEnd();

            var template = Regex.Match(text, @"<template>(?<text>[\s\S]*)</template>");
            var style = Regex.Match(text, @"<style(.*?)>(?<text>[\s\S]*)</style>");

            var tagBuilder = new core.TagBuilder(template.Groups["text"].Value);
            tagBuilder.Render();

            var styleBuilder = new core.StyleBuilder(style.Groups["text"].Value);
            styleBuilder.Render();

            var tags = tagBuilder.GetTags();
            var styles = styleBuilder.GetStyles();

            var render = new Render(tags, styles);
            render.Handle();

            Content = render.GetContent();
        }


    }
}
