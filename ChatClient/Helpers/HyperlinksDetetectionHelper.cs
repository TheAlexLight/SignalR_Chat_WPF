using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ChatClient.Helpers
{
    public class HyperlinksDetetectionHelper
    {
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty ChromiumWebBrowserPanelProperty;

        static HyperlinksDetetectionHelper()
        {
            TextProperty = DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(HyperlinksDetetectionHelper),
            new PropertyMetadata(null, OnTextChanged));

            ChromiumWebBrowserPanelProperty = DependencyProperty.RegisterAttached(
                "ChromiumWebBrowserPanel",
                typeof(Panel),
                typeof(HyperlinksDetetectionHelper),
                new PropertyMetadata(null, OnChromiumWebBrowserPanelChanged));
        }

        private static void OnChromiumWebBrowserPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Panel panel)
            {
                _chromiumWebBrouserPanel = panel;
            }
        }

        private static readonly Regex _regexUrl = new Regex(@"(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?");
        
        private static Panel _chromiumWebBrouserPanel;
        private static ChromiumWebBrowser _webBrouser;
        private static bool _isFrameLoaded;
        private static Panel _panel;

        public static string GetText(DependencyObject d)
        { return d.GetValue(TextProperty) as string; }

        public static void SetText(DependencyObject d, string value)
        { d.SetValue(TextProperty, value); }

        public static Panel GetChromiumWebBrowserPanel(DependencyObject d)
        { return (Panel)d.GetValue(ChromiumWebBrowserPanelProperty); }

        public static void SetChromiumWebBrowserPanel(DependencyObject d, Panel value)
        { d.SetValue(ChromiumWebBrowserPanelProperty, value); }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBlock textBlock = d as TextBlock;

            if (textBlock == null)
            {
                return;
            }

            textBlock.Inlines.Clear();

            var newText = (string)e.NewValue;

            if (string.IsNullOrEmpty(newText))
            {
                return;
            }

            // Find all URLs using a regular expression
            int lastPosition = 0;

            foreach (Match match in _regexUrl.Matches(newText))
            {
                // Copy raw string from the last position up to the match
                if (match.Index != lastPosition)
                {
                    string rawText = newText.Substring(lastPosition, match.Index - lastPosition);
                    textBlock.Inlines.Add(new Run(rawText));
                }

                Hyperlink link = SetLinkAddress(match);

                if (link.NavigateUri != null)
                {
                    link.Click += OnUrlClick;

                    textBlock.Inlines.Add(link);
                }
                else
                {
                    textBlock.Inlines.Add(match.ToString());
                }

                

                

                // Update the last matched position
                lastPosition = match.Index + match.Length;
            }

            // Finally, copy the remainder of the string
            if (lastPosition < newText.Length)
            {
                textBlock.Inlines.Add(new Run(newText.Substring(lastPosition)));
            }

            if (_chromiumWebBrouserPanel != null && _regexUrl.Matches(newText).Count != 0)
            {
                Match lastRegex = _regexUrl.Matches(newText).Last();

                AddAdditionalInfo(lastRegex, textBlock);
            }
        }

        private static void AddAdditionalInfo(Match lastRegex, FrameworkElement textControl)
        {
            _webBrouser = new ChromiumWebBrowser()
            {
                Width = 1,
                Height = 1,
                Visibility = Visibility.Collapsed
            };

            Hyperlink link = SetLinkAddress(lastRegex);

            if (link.NavigateUri != null)
            {
                _webBrouser.Address = link.NavigateUri.ToString();

                if (textControl.Parent is Panel panel)
                {
                    _panel = panel;
                    panel.Children.Add(_webBrouser);
                }
                else
                {
                    return;
                }

                Panel mainPanel = GetChromiumWebBrowserPanel(_chromiumWebBrouserPanel);

                //mainPanel.Children.Add(_webBrouser);

                _webBrouser.Loaded += _webBrouser_Loaded;
                _webBrouser.FrameLoadEnd += _webBrouser_FrameLoadEnd;
            }
        }

        private static void ReadWebSource(string webPageSource)
        {
            const string PAGE_DESCRIPTION_START_KEY = "<meta name=\"description\" content=\"";
            const string PAGE_IMAGE_START_KEY = "og:image\" content=\"";

            string pageDescription;
            string pageImage;

            int pageDescriptionStartIndex = webPageSource.IndexOf(PAGE_DESCRIPTION_START_KEY);
            int pageImageStartIndex = webPageSource.IndexOf(PAGE_IMAGE_START_KEY);

            if (pageDescriptionStartIndex != -1 && pageImageStartIndex != -1)
            {
                string pageDescriptionStart = webPageSource
                    .Substring(pageDescriptionStartIndex)
                    .Replace(PAGE_DESCRIPTION_START_KEY, "");

                string pageImageStart = webPageSource
                    .Substring(pageImageStartIndex)
                    .Replace(PAGE_IMAGE_START_KEY, "");

                pageDescription = pageDescriptionStart.Substring(0, pageDescriptionStart.IndexOf("\">"));
                pageImage = pageImageStart.Substring(0, pageImageStart.IndexOf("\">"));

                TextBlock pageDescriptionChild = null; //= Application.Current.Dispatcher.Invoke(() => UIHelper.FindChild<TextBlock>(_panel, "pageDescription"));
                Image pageImageChild = null; //= Application.Current.Dispatcher.Invoke(() => UIHelper.FindChild<Image>(_panel, "pageImage"));
                                             //TextBlock pageDescriptionChild = UIHelper.FindChild<TextBlock>(_panel, "pageDescription");
                                             //Image pageImageChild = UIHelper.FindChild<Image>(_panel, "pageImage");

                Application.Current.Dispatcher.Invoke(() =>
                {
                    pageDescriptionChild = UIHelper.FindChild<TextBlock>(_panel, "pageDescription");
                    pageImageChild = UIHelper.FindChild<Image>(_panel, "pageImage");
                }, DispatcherPriority.ContextIdle);

                pageDescriptionChild.Dispatcher.Invoke(() =>
                {
                    //pageDescriptionChild = UIHelper.FindChild<TextBlock>(_panel, "pageDescription");
                    pageDescriptionChild.Text = pageDescription;
                    pageDescriptionChild.Visibility = Visibility.Visible;
                }, DispatcherPriority.ContextIdle);

                pageImageChild.Dispatcher.Invoke(() =>
                {
                    //pageImageChild = UIHelper.FindChild<Image>(_panel, "pageImage");
                    pageImageChild.Source = new BitmapImage(new Uri(pageImage));
                    pageImageChild.Visibility = Visibility.Visible;
                }, DispatcherPriority.ContextIdle);

                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    pageDescriptionChild = UIHelper.FindChild<TextBlock>(_panel, "pageDescription"); 
                //    pageImageChild = UIHelper.FindChild<Image>(_panel, "pageImage");
                    
                //    pageDescriptionChild.Text = pageDescription;
                //    pageDescriptionChild.Visibility = Visibility.Visible;
                    
                //    pageImageChild.Source = new BitmapImage(new Uri(pageImage));
                //    pageImageChild.Visibility = Visibility.Visible;

                //}, DispatcherPriority.ContextIdle);
 
            }
        }

        private static async void _webBrouser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            if (!_isFrameLoaded)
            {
                string webPageSource = await _webBrouser.GetSourceAsync();
                
                ReadWebSource(webPageSource);
                _isFrameLoaded = true;
            }
        }

        private static void _webBrouser_Loaded(object sender, RoutedEventArgs e)
        {
            _webBrouser.Visibility = Visibility.Hidden;
        }

        private static Hyperlink SetLinkAddress(Match match)
        {

            Hyperlink link = null;

            string[] possibleLinks = new string[2];

            possibleLinks[0] = "http://";
            possibleLinks[1] = "https://";

            try
            {
                for (int i = 0; i < possibleLinks.Length; i++)
                {
                    if (match.Value.Contains(possibleLinks[i]))
                    {
                        link = new Hyperlink(new Run(match.Value.Replace(possibleLinks[i], "")));
                        link.NavigateUri = new Uri(match.Value);
                    }
                }

                if (link == null)
                {
                    link = new Hyperlink(new Run(match.Value));
                    link.NavigateUri = new Uri(string.Format("{0}{1}", possibleLinks[0], match.Value));
                }

                link.TextDecorations = null;
            }
            catch (Exception)
            {
            }

            return link;
        }

        private static void OnUrlClick(object sender, RoutedEventArgs e)
        {
            var link = (Hyperlink)sender;
            // Do something with link.NavigateUri like:
            //Process.Start(link.NavigateUri.ToString());
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri)
            {
                UseShellExecute = true
            });

        }
    }
}
