using System;
using System.Web;

namespace Scch.Common.Web
{
    public static class HtmlHelper
    {
        public static string HtmlEncode(string value)
        {
            return HttpUtility.HtmlEncode(value).Replace(Environment.NewLine, "<br>");
        }

        public static string SpaceEncode(string value)
        {
            return value.Replace(" ", "&nbsp;");
        }

        public static string HtmlDecode(string value)
        {
            return HttpUtility.HtmlDecode(value.Replace("<br>", Environment.NewLine).Replace("&nbsp;", " "));
        }
    }
}
