using Ganss.XSS;
using AngleSharp.Text;

namespace Core.Shared.Tools;

public static class MySanitizer
{
    public static string SanitizeText(this string text)
    {
        var htmlSanitizer = new HtmlSanitizer();

        htmlSanitizer.KeepChildNodes = true;

        htmlSanitizer.AllowDataAttributes = true;

        return htmlSanitizer.Sanitize(text);
    }

    public static bool SanitizeBool(this string text)
    {
        try
        {
            var htmlSanitizer = new HtmlSanitizer();

            htmlSanitizer.KeepChildNodes = true;

            htmlSanitizer.AllowDataAttributes = true;

            return htmlSanitizer.Sanitize(text).ToBoolean();
        }
        catch
        {
            return false;
        }
    }
}