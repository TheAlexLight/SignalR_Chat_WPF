﻿using CefSharp;
using CefSharp.Wpf;
using SharedItems.Models;
using Syroot.Windows.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
                "ChromiumWebBrowser",
                typeof(ChromiumWebBrowser),
                typeof(HyperlinksDetetectionHelper),
                new PropertyMetadata(null, OnChromiumWebBrowserPanelChanged));
        }

        private static void OnChromiumWebBrowserPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ChromiumWebBrowser chromiumBrowser)
            {
                WebBrowser = chromiumBrowser;

                WebBrowser.FrameLoadEnd += WebBrowser_FrameLoadEnd;
            }
        }

        private static readonly Regex _regexUrl = new Regex(@"(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?");

        public static ChromiumWebBrowser WebBrowser { get; set; }
        private static bool _isFrameLoaded;

        public static string GetText(DependencyObject d)
        { return d.GetValue(TextProperty) as string; }

        public static void SetText(DependencyObject d, string value)
        { d.SetValue(TextProperty, value); }

        public static ChromiumWebBrowser GetChromiumWebBrowser(DependencyObject d)
        { return (ChromiumWebBrowser)d.GetValue(ChromiumWebBrowserPanelProperty); }

        public static void SetChromiumWebBrowser(DependencyObject d, ChromiumWebBrowser value)
        { d.SetValue(ChromiumWebBrowserPanelProperty, value); }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBlock textBlock = d as TextBlock;

            if (textBlock == null)
            {
                return;
            }

            textBlock.Inlines.Clear();

            string newText = (string)e.NewValue;

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
        }

        public static async Task<HyperlinkDescriptionModel> ReceivePageSource(Match lastRegex)
        {
            Hyperlink link = SetLinkAddress(lastRegex);
            WebBrowser.Address = string.Empty;

            if (link.NavigateUri != null)
            {
                WebBrowser.Address = link.NavigateUri.ToString();

                _isFrameLoaded = false;

                while (!_isFrameLoaded)
                {
                    await Task.Delay(25);
                }
            }

            string webPageSource = await WebBrowser.GetSourceAsync();

            HyperlinkDescriptionModel hyperlinkDescription = new HyperlinkDescriptionModel();

            if (webPageSource != string.Empty)
            {
               hyperlinkDescription = ReadWebSource(webPageSource);
            }

            return hyperlinkDescription;
        }

        private static HyperlinkDescriptionModel ReadWebSource(string webPageSource)
        {
            const string PAGE_TITLE_START_KEY = "og:site_name\" content=\"";
            const string PAGE_DESCRIPTION_START_KEY = "<meta name=\"description\" content=\"";
            const string PAGE_IMAGE_START_KEY = "og:image\" content=\"";

            string pageTitle = ReceiveTagDescription(webPageSource, PAGE_TITLE_START_KEY);
            string pageDescription = ReceiveTagDescription(webPageSource, PAGE_DESCRIPTION_START_KEY);
            string pageImage = ReceiveTagDescription(webPageSource, PAGE_IMAGE_START_KEY);

            HyperlinkDescriptionModel hyperlinkDescription = new HyperlinkDescriptionModel();

            if (pageTitle != string.Empty
                && pageDescription != string.Empty
                && pageImage != string.Empty)
            {
                hyperlinkDescription.HyperlinkTitle = pageTitle;
                hyperlinkDescription.HyperlinkDescription = pageDescription;

                string downloadsPath = new KnownFolder(KnownFolderType.Downloads).Path;

                string temporaryImagePath = string.Format("{0}\\lastSentMessageDescriptionImage.jpg", downloadsPath);

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(pageImage), temporaryImagePath);
                }

                hyperlinkDescription.HyperlinkImage = File.ReadAllBytes(temporaryImagePath);

                try
                {
                    File.Delete(temporaryImagePath);
                }
                catch (Exception)
                {
                    //Do something
                }
            }

            return hyperlinkDescription;
        }

        private static string ReceiveTagDescription(string webPageSource, string tagName)
        {
            int pageTagDescriptionIndex = webPageSource.IndexOf(tagName);

            if (pageTagDescriptionIndex == -1)
            {
                return string.Empty;
            }

            string pageTagDescriptionStart = webPageSource
                   .Substring(pageTagDescriptionIndex)
                   .Replace(tagName, "");

           string pageTagDescription = pageTagDescriptionStart.Substring(0, pageTagDescriptionStart.IndexOf("\">"));

            return pageTagDescription;
        }

        private static void WebBrowser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            if (!_isFrameLoaded)
            {
                _isFrameLoaded = true;
            }
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
            Hyperlink link = (Hyperlink)sender;

            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri)
            {
                UseShellExecute = true
            });

        }
    }
}