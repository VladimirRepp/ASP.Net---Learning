using System;
using System.Text;
using TestDb.DatabaseLogic.Contexts;
using TestDb.DatabaseLogic.Controllers;
using TestDb.DatabaseLogic.Models;

namespace Sample_0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            Sample_8(ref app);

            app.Run();
        }

        /// <summary>
        /// ������� ��������� � middleware
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_0(ref WebApplication app)
        {
            // ������� ������������\����������� middleware:
            app.MapGet("/", () => "Hello World!");        // ����� ���������� �������� HTTP GET
            app.MapGet("/test", () => "Hello Test!");     // ����� ���������� �������� HTTP GET
            app.MapGet("/admin", () => "Hello Admin!");   // ����� ���������� �������� HTTP GET

            // Middleware - ���������, �������������� ��� �������
            //app.UseWelcomePage(); // ���� ������������������, �� MapGet �� ������
            app.UseCookiePolicy();  // �� �������� ������� ���������� MapGet
        }

        /// <summary>
        /// ������������ Middleware
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_1(ref WebApplication app)
        {
            // �������������� ������������ middleware - ��������� �������� ��������� ��������,
            // ������� �� �������� ��������� middleware � �������  
            // - ������ ������ ����� ��������� ���� ������������ middleware: /test, /admin � �.�. 
            // - ����� ������ ����� ���� middleware �� ��������� 

            // ������ 1
            //app.Run(
            //    async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    }
            //);

            //������ 2 - �������� ��� ������������
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.StatusCode = 503;
            //        await context.Response.WriteAsync("The server is in service.");
            //    }
            //);

            // ������ 3 -  ��������� ���� middleware. 
            // ���������� middleware ��������� ���� ��� � ���������� �� ���������� ����� ���������� �����
            int x = 2;
            app.Run(
                async context =>
                {
                    x *= 2;
                    await context.Response.WriteAsync($"Result = {x}");
                }
            );
        }

        /// <summary>
        /// �������� ������� - HttpResponse
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_2(ref WebApplication app)
        {
            // ��� middleware �������� ������� ����� 
            // ������ HttpContext. ���� ������ ������������� 
            // ���������� � �������, ��������� ��������� ������� 
            // � ������ ������ 

            // ������ 1 - ������� �������� ������ 
            //app.Run(async context =>
            //    await context.Response.WriteAsync("Example of simple Response!")
            //    //await context.Response.WriteAsync("������!", System.Text.Encoding.Default); - �� �� �� ������ 
            //);

            // ������ 2 - ������� ���������
            //app.Run(async context =>
            //{
            //    var response = context.Response;
            //    response.Headers.ContentLanguage = "ru-RU";
            //    response.Headers.ContentType = "text/plain; charset=utf-8";
            //    response.Headers.Append("my-key", "-1");

            //    await response.WriteAsync("������ ���!");
            //}
            //);

            // ������ 3 - ��������� ���� �������
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.StatusCode = 404; // 503 - The server is in service.
            //        await context.Response.WriteAsync("Resource not Found!");
            //    }    
            //);

            // ������ 4 - �������� html-����
            app.Run(
                async context =>
                {
                    var response = context.Response;
                    response.ContentType = "text/html; charset=utf-8";
                    await response.WriteAsync(
                        "<h2>������ �4</h2>" +
                        "<p>��������� �����</p>"
                        );
                }
            );
        }

        /// <summary>
        /// ��������� ������ ������� - HttpRequest
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_3(ref WebApplication app)
        {
            // ������ 1 - ��������� ���������� ������� 
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.ContentType = "text/html; charset=utf-8";
            //        StringBuilder stringBuilder = new("<table>");

            //        foreach (var header in context.Request.Headers)
            //        {
            //            stringBuilder.Append(
            //                $"<tr>" +
            //                $"<td>{header.Key}</td>" +
            //                $"<td>{header.Value}</td>" +
            //                $"</tr>"
            //                );
            //        }

            //        stringBuilder.Append("</table>");
            //        await context.Response.WriteAsync(stringBuilder.ToString());
            //    }
            //);

            // ������ 2 - �������� ���� ������� 
            //app.Run(async context => await context.Response.WriteAsync($"Path: {context.Request.Path}"));

            // ������ 3 - ���������� �� ������ ���������� ����
            //app.Run(
            //    async context =>
            //    {
            //        var path = context.Request.Path;
            //        var date_now = DateTime.Now;
            //        var response = context.Response;

            //        if (path == "/date")
            //            await response.WriteAsync($"Date: {date_now.ToShortDateString()}");
            //        else if (path == "/time")
            //            await response.WriteAsync($"Time: {date_now.ToShortTimeString()}");
            //        else if (path == "/time/date")
            //            await response.WriteAsync($"Time: {date_now.ToShortTimeString()}\n\r" +
            //                $"Date: {date_now.ToShortDateString()}");
            //        else if (path == "/date/time")
            //            await response.WriteAsync($"Date: {date_now.ToShortDateString()}\n\r" +
            //                $"Time: {date_now.ToShortTimeString()}");
            //        else
            //            await response.WriteAsync($"Hellow world!");
            //    }
            //);

            // ������ 4 - ��������� ������ ������� (QueryString) �� HttpRequest
            //app.Run(
            //    async context =>
            //    {
            //        var response = context.Response;
            //        response.ContentType = "text/html; charset=utf-8";
            //        await response.WriteAsync(
            //            $"<p>Path: {context.Request.Path}</p>" +
            //            $"<p>QueryString: {context.Request.QueryString}</p>"
            //            );
            //    }
            //);

            // ������ 5 - ��������� ������� ���������� ������� 
            // GET ������: ?name=Tom&age=37
            app.Run(
                async context =>
                {
                    var response = context.Response;
                    var requset = context.Request;

                    response.ContentType = "text/html; charset=utf-8";

                    StringBuilder stringBuilder = new("<h3>��������� ������ �������</h3>");
                    stringBuilder.Append("<table>");
                    stringBuilder.Append("<tr><td>��������</td><td>��������</td></tr>");

                    // I - ��������� ���� ���������� 
                    foreach (var param in requset.Query)
                    {
                        stringBuilder.Append($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");
                    }

                    stringBuilder.Append("</table>");

                    // II - �������� ��������� ���������� 
                    string name = requset.Query["name"];
                    string age = requset.Query["age"];

                    stringBuilder.Append($"<p>{name} - {age}</p>");

                    await response.WriteAsync(stringBuilder.ToString());
                }
            );


        }

        /// <summary>
        /// �������� ������ 
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_4(ref WebApplication app)
        {
            // ������ 1 - �������� ����� 
            //app.Run(
            //    //async context => await context.Response.SendFileAsync("D:\\tree.jpg") 
            //    async context => await context.Response.SendFileAsync("img\\tree.jpg")
            //);

            // ������ 2 - �������� html ��������
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.ContentType = "text/html; charset=utf-8";
            //        await context.Response.SendFileAsync("html\\index.html");
            //    }
            //);

            // ������ 3 - ��������� ���� 
            //app.Run(
            //    async context =>
            //    {
            //        var path = context.Request.Path;
            //        var fullPath = $"html/{path}.html";
            //        var response = context.Response;

            //        response.ContentType = "text/html; charset=utf-8";

            //        if (File.Exists(fullPath))
            //        {
            //            await response.SendFileAsync(fullPath);
            //        }
            //        else
            //        {
            //            response.StatusCode = 404;
            //            await response.WriteAsync("<h2>Not Found</h2>");
            //        }
            //    }
            //);

            // ������ 4 - ����-�������� ����� (������ -> ������)
            app.Run(
                async context =>
                {
                    var path = context.Request.Path;

                    if (path == "/")
                    {
                        await context.Response.WriteAsync("Input to '/download' for download \'tree.jpg\'");
                    }

                    if (path == "/download")
                    {
                        context.Response.Headers.ContentDisposition = "attachment; filename=img_tree.jpg";
                        await context.Response.SendFileAsync("img/tree.jpg");
                    }
                }
            );
        }

        /// <summary>
        /// �������� ����
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_5(ref WebApplication app)
        {
            // ������ 1 - ��������� ����� �����
            app.Run(
                async context =>
                {
                    var response = context.Response;
                    var request = context.Request;

                    response.ContentType = "text/html; charset=utf-8";

                    if (request.Path == "/" || request.Path == "/login")
                    {
                        await response.SendFileAsync("html/login.html");
                    }
                    else if (request.Path == "/post_user_login")
                    {
                        var form = request.Form;
                        string login = form["login"];
                        string password = form["password"];

                        if (login == "log1" && password == "1111")
                        {
                            await response.SendFileAsync("html/index.html");
                        }
                        else
                        {
                            await response.WriteAsync($"<div><h3>{login} - {password}: Not Found!</h3></div>");
                        }
                    }
                    else if (request.Path == "/post_user")
                    {
                        var form = request.Form;

                        string name = form["name"];
                        string age = form["age"];

                        string[] languages = form["languages"];

                        string langStr = "";
                        foreach (var lang in languages)
                        {
                            langStr += lang + " ";
                        }

                        await response.WriteAsync(
                            $"<div> " +
                            $"<p> Name: {name} </p>" +
                            $"<p> Age: {age} </p>" +
                            $"<p> Languages: {langStr} </p>" +
                            $"</div>"
                            );
                    }
                }
            );
        }

        /// <summary>
        /// ����������� ������������ + EF Core
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_6(ref WebApplication app)
        {
            DbAppContext dbContext = new();
            DbUserController userController = new(dbContext);

            app.Run(
               async context =>
               {
                   var response = context.Response;
                   var request = context.Request;

                   response.ContentType = "text/html; charset=utf-8";

                   if (request.Path == "/" || request.Path == "/login")
                   {
                       await response.SendFileAsync("html/login.html");
                   }
                   else if (request.Path == "/post_user_login")
                   {
                       var form = request.Form;
                       string login = form["login"];
                       string password = form["password"];

                       if (userController.IsAuthorized(new DbUser() { Id = -1, Login = login, Password = password }))
                       {
                           await response.SendFileAsync("html/index.html");
                       }
                       else
                       {
                           await response.WriteAsync($"<div><h3>{login} - {password}: Not Found!</h3></div>");
                       }
                   }
               }
           );
        }

        /// <summary>
        /// ������������� 
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_7(ref WebApplication app)
        {
            // ������ �1 - ����:
            //app.Run(
            //    async context =>
            //    {
            //        if (context.Request.Path == "/old")
            //        {
            //            await context.Response.WriteAsync("Old page");
            //        }
            //        else
            //        {
            //            await context.Response.WriteAsync("Main page");
            //        }
            //    }
            //);

            // �����:
            app.Run(
                async context =>
                {
                    if (context.Request.Path == "/old")
                    {
                        context.Response.Redirect("/in_work");
                    }
                    else if (context.Request.Path == "/contacts")
                    {
                        context.Response.Redirect("/in_work");
                    }
                    else if (context.Request.Path == "/about")
                    {
                        context.Response.Redirect("/in_work");
                    }

                    else if (context.Request.Path == "/in_work")
                    {
                        await context.Response.WriteAsync("InWork page");
                    }
                    else if (context.Request.Path == "/search")
                    {
                        context.Response.Redirect("https:google.com");
                    }
                    else
                    {
                        await context.Response.WriteAsync("Main page");
                    }
                }
            );
        }

        /// <summary>
        /// JSON - �������� � ��������� 
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_8(ref WebApplication app)
        {
            // ������ �1 - �������� JSON (������ -> ������)
            //app.Run(
            //    async context =>
            //    {
            //        Person sam = new("Sam", 35);
            //        Cat vasy = new() { Age = 5 , Name = "vasy"};
            //        await context.Response.WriteAsJsonAsync(vasy);
            //    }
            //);

            // ������ �2 - �������� JSON (������ -> ������)
            app.Run(
                async context =>
                {
                    var response = context.Response;
                    var request = context.Request;

                    if (request.Path == "/api/user")
                    {
                        var messageToClient = "������������ ������!";

                        try
                        {
                            var person = await request.ReadFromJsonAsync<Person>();

                            if (person != null)
                                messageToClient = $"Name: {person.Name} Age: {person.Age}";
                        }
                        catch { }

                        await response.WriteAsJsonAsync(new { text = messageToClient });
                    }
                    else
                    {
                        response.ContentType = "text/html; charset=utf-8";
                        await response.SendFileAsync("html/index-JSON.html");
                    }
                }
            );
        }

        public record Person(string Name, int Age);
        public class Cat
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
