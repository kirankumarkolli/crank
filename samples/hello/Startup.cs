// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;

namespace hello
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
                endpoints.MapGet("/", async context =>
                {
                    IHeaderDictionary inputHeaders = context.Request.Headers;
                    if (inputHeaders != null)
                    {
                        StringValues authHeaderValue;
                        StringValues dateHeaderValue;

                        inputHeaders.TryGetValue("authorization", out authHeaderValue);
                        inputHeaders.TryGetValue("x-ms-date", out dateHeaderValue);

                        if (authHeaderValue.Count != 1
                            || dateHeaderValue.Count != 1)
                        {
                            context.Response.StatusCode = 400;
                            return;
                        }
                    }

                    await context.Response.WriteAsync("Hello World!");
                });
            });

            Console.WriteLine($"AspNetCore location: {typeof(IWebHostBuilder).GetTypeInfo().Assembly.Location}");
            Console.WriteLine($"AspNetCore version: {typeof(IWebHostBuilder).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}");

            Console.WriteLine($"NETCoreApp location: {typeof(object).GetTypeInfo().Assembly.Location}");
            Console.WriteLine($"NETCoreApp version: {typeof(object).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}");
    
        }
    }
}
