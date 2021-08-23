using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace WePray.Repository._class
{
    public static class HtmlConvert
    {
        public static string Gettext(string content)
        {

            var html = content;
            var htmltext = new List<string>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var htmlBody = htmlDoc.DocumentNode.SelectNodes("//p");

            foreach (var item in htmlBody)
            {
                htmltext.Add(item.InnerText);
            }

            var ans = string.Concat(htmltext);
            return ans;
        }
    }
}
