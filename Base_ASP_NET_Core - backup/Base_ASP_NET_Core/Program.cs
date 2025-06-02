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
        /// Конвеер обработки и middleware
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_0(ref WebApplication app)
        {
            // Примеры использвания\подключения middleware:
            app.MapGet("/", () => "Hello World!");        // метод расширения запросов HTTP GET
            app.MapGet("/test", () => "Hello Test!");     // метод расширения запросов HTTP GET
            app.MapGet("/admin", () => "Hello Admin!");   // метод расширения запросов HTTP GET

            // Middleware - компонент, обрабатывающий все запросы
            //app.UseWelcomePage(); // если расскамментировать, то MapGet не увидем
            app.UseCookiePolicy();  // не помешает увидеть зареганные MapGet
        }

        /// <summary>
        /// Терминальные Middleware
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_1(ref WebApplication app)
        {
            // Зарегистрируем терминальный middleware - компонент конвеера обработки запросов,
            // который не вызывает следующий middleware в цепочки  
            // - каждый запрос будет обработан этим терминальным middleware: /test, /admin и т.д. 
            // - людой другой после него middleware не сработает 

            // Пример 1
            //app.Run(
            //    async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    }
            //);

            //Пример 2 - заглужка тех обслуживания
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.StatusCode = 503;
            //        await context.Response.WriteAsync("The server is in service.");
            //    }
            //);

            // Пример 3 -  жизненный цикл middleware. 
            // Компоненты middleware создаются один раз и существуют на протяжении всего жизненного цикла
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
        /// Отправка ответов - HttpResponse
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_2(ref WebApplication app)
        {
            // Все middleware передают запросы через 
            // объект HttpContext. Этот объект инкапсулирует 
            // информацию о запросе, позволяет управлять ответом 
            // и многое другое 

            // Пример 1 - простая отправка ответа 
            //app.Run(async context =>
            //    await context.Response.WriteAsync("Example of simple Response!")
            //    //await context.Response.WriteAsync("Пример!", System.Text.Encoding.Default); - на РУ не пойдет 
            //);

            // Пример 2 - уставка заголовка
            //app.Run(async context =>
            //{
            //    var response = context.Response;
            //    response.Headers.ContentLanguage = "ru-RU";
            //    response.Headers.ContentType = "text/plain; charset=utf-8";
            //    response.Headers.Append("my-key", "-1");

            //    await response.WriteAsync("Привет мир!");
            //}
            //);

            // Пример 3 - установка кода статуса
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.StatusCode = 404; // 503 - The server is in service.
            //        await context.Response.WriteAsync("Resource not Found!");
            //    }    
            //);

            // Пример 4 - отправка html-кода
            app.Run(
                async context =>
                {
                    var response = context.Response;
                    response.ContentType = "text/html; charset=utf-8";
                    await response.WriteAsync(
                        "<h2>Пример №4</h2>" +
                        "<p>Некоторый текст</p>"
                        );
                }
            );
        }

        /// <summary>
        /// Получение данных запроса - HttpRequest
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_3(ref WebApplication app)
        {
            // Пример 1 - получение заголовков запроса 
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

            // Пример 2 - получить путь запроса 
            //app.Run(async context => await context.Response.WriteAsync($"Path: {context.Request.Path}"));

            // Пример 3 - фильтрация на основе указанного пути
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

            // Пример 4 - получение строки запроса (QueryString) из HttpRequest
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

            // Пример 5 - получение словаря параметров запроса 
            // GET запрос: ?name=Tom&age=37
            app.Run(
                async context =>
                {
                    var response = context.Response;
                    var requset = context.Request;

                    response.ContentType = "text/html; charset=utf-8";

                    StringBuilder stringBuilder = new("<h3>Параметры строки запроса</h3>");
                    stringBuilder.Append("<table>");
                    stringBuilder.Append("<tr><td>Параметр</td><td>Значение</td></tr>");

                    // I - коллекция всех параметров 
                    foreach (var param in requset.Query)
                    {
                        stringBuilder.Append($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");
                    }

                    stringBuilder.Append("</table>");

                    // II - значение отдельных параметров 
                    string name = requset.Query["name"];
                    string age = requset.Query["age"];

                    stringBuilder.Append($"<p>{name} - {age}</p>");

                    await response.WriteAsync(stringBuilder.ToString());
                }
            );


        }

        /// <summary>
        /// Отправка файлов 
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_4(ref WebApplication app)
        {
            // Пример 1 - отправка файла 
            //app.Run(
            //    //async context => await context.Response.SendFileAsync("D:\\tree.jpg") 
            //    async context => await context.Response.SendFileAsync("img\\tree.jpg")
            //);

            // Пример 2 - отправка html страницы
            //app.Run(
            //    async context =>
            //    {
            //        context.Response.ContentType = "text/html; charset=utf-8";
            //        await context.Response.SendFileAsync("html\\index.html");
            //    }
            //);

            // Пример 3 - обработка пути 
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

            // Пример 4 - авто-загрузка файла (сервер -> клиент)
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
        /// Отправка форм
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_5(ref WebApplication app)
        {
            // Пример 1 - обработка формы ввода
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
        /// Авторизация пользователя + EF Core
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
        /// Переадресация 
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_7(ref WebApplication app)
        {
            // Пример №1 - Было:
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

            // Стало:
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
        /// JSON - отправка и получения 
        /// </summary>
        /// <param name="app"></param>
        private static void Sample_8(ref WebApplication app)
        {
            // Пример №1 - отправка JSON (сервер -> клиент)
            //app.Run(
            //    async context =>
            //    {
            //        Person sam = new("Sam", 35);
            //        Cat vasy = new() { Age = 5 , Name = "vasy"};
            //        await context.Response.WriteAsJsonAsync(vasy);
            //    }
            //);

            // Пример №2 - получить JSON (клиент -> сервер)
            app.Run(
                async context =>
                {
                    var response = context.Response;
                    var request = context.Request;

                    if (request.Path == "/api/user")
                    {
                        var messageToClient = "Некорректные данные!";

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
