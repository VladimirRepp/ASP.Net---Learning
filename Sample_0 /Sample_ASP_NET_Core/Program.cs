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
    }
}
