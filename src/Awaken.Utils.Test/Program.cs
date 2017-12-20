using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Text;

namespace Awaken.Utils.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //var c = Awaken.Utils.Widgets.CryptoAes.Encrypt("15801901535:AEF33", "0123456789123456", "1234560123456789");

            //var result=c.TrimEnd('=').Replace('+', '-').Replace('/', '_');

            var host = new WebHostBuilder()
                .UseUrls("http://*:5200/")
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
