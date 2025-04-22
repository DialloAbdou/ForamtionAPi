
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;
using System.Text;

namespace Mod3ASPNET
{
    /// <summary>
    ///  pour lire html
    /// </summary>
    public class HtmlResult : IResult
    {
         private readonly string _html;
        public HtmlResult(string html)
        {
            _html = html;   
        }
        public  async Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
            httpContext.Response.ContentType = MediaTypeNames.Text.Html;
            await  httpContext.Response.WriteAsync(_html);
        }
    }
}
