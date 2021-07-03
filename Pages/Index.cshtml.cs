using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace WebApplicationTwitterV1
{
    public class IndexModel : PageModel
    {
        private AppSettings AppSettings { get; set; }

        public IndexModel(IOptions<AppSettings> settings)
        {
            AppSettings = settings.Value;
        }
        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var stremw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jsonw = new JsonTextWriter(stremw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jsonw, value);
                jsonw.Flush();
            }
        }
        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var memorystresm = new MemoryStream();
                SerializeJsonIntoStream(content, memorystresm);
                memorystresm.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(memorystresm);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }


            return httpContent;
        }
        public async Task<IActionResult> OnPostAsync(string name)
        {
            var Url = AppSettings.MyFunctionURL;

            dynamic content = new { name = name};

            CancellationToken cancellationToken;

            using (var myclient = new HttpClient())
            using (var myrequest = new HttpRequestMessage(HttpMethod.Post, Url))
            using (var httpContent = CreateHttpContent(content))
            {
                myrequest.Content = httpContent;

                using (var newresponse = await myclient
                    .SendAsync(myrequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false))
                {
                     var MyData = newresponse.Content.ReadAsAsync<TwitterUser>();
                     ViewData["Name"] = MyData.Result;
                     return Page();
                }
            }

        }
    }
}
