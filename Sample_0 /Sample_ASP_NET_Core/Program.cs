namespace Sample_ASP_NET_Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            Sample_2(ref app);

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
    }
}
