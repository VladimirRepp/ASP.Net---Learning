using System.Text;

namespace Sample_0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            Sample_4(ref app);

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
                    foreach(var param in requset.Query)
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
            app.Run(
                //async context => await context.Response.SendFileAsync("D:\\tree.jpg") 
                async context => await context.Response.SendFileAsync("img\\tree.jpg")
            );
        }
    }
}
