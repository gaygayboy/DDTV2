using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DDTVLiveRecWebServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/log", async context =>
                {
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    if (File.Exists("./LOG/DDTVLiveRecLog.out"))
                    {
                        File.Delete("./LOG/DDTVLiveRecLog.out.bak");
                        File.Copy("./LOG/DDTVLiveRecLog.out", "./LOG/DDTVLiveRecLog.out.bak");
                        await context.Response.WriteAsync(File.ReadAllText("./LOG/DDTVLiveRecLog.out.bak", System.Text.Encoding.UTF8));
                        File.Delete("./LOG/DDTVLiveRecLog.out.bak");
                        return;
                    }
                    else
                    {
                        await context.Response.WriteAsync("û�л�ȡ����־�ļ�����ȷ��DDTVLive��������");
                    }
                });
                endpoints.MapGet("/file", async context =>
                {
                    string A = "��ǰ¼���ļ����ļ��б�:\r\n";
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    foreach (DirectoryInfo NextFolder1 in new DirectoryInfo("./tmp").GetDirectories())
                    {
                        A = A + "\r\n" + NextFolder1.Name;
                        foreach (FileInfo NextFolder2 in new DirectoryInfo("./tmp/" + NextFolder1.Name).GetFiles())
                        {
                            A = A + "\r\n����" + Math.Ceiling(NextFolder2.Length / 1024.0 / 1024.0) + " MB |" + NextFolder2.Name;
                        }
                        A = A + "\r\n";
                    }
                    await context.Response.WriteAsync(A, System.Text.Encoding.UTF8);
                });
                endpoints.MapGet("/list", async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(Auxiliary.InfoLog.DownloaderInfoPrintf(), System.Text.Encoding.UTF8);
                });
            });
        }
    }
}