using System.Net.Http.Headers;
using System.Web.Http;
using Owin;

namespace WindowsFormsApplication1
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.

        /// 配置webApi文本格式、路由规则、跨域规则等参数
        public void Configuration(Owin.IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();


            //// Web API 路由
            //config.MapHttpAttributeRoutes();

            ////1.默认路由
            //config.Routes.MapHttpRoute(
            //name: "DefaultApi",
            //routeTemplate: "api/{controller}/{id}",
            //defaults: new { id = RouteParameter.Optional }
            //);

            ////2.自定义路由一：匹配到action
            //config.Routes.MapHttpRoute(
            //name: "ActionApi",
            //routeTemplate: "actionapi/{controller}/{action}/{id}",
            //defaults: new { id = RouteParameter.Optional }
            //);

            ////3.自定义路由二
            //config.Routes.MapHttpRoute(
            //name: "TestApi",
            //routeTemplate: "testapi/{controller}/{ordertype}/{id}",
            //defaults: new { ordertype = "aa", id = RouteParameter.Optional }
            //);

            //默认路由
            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}", 
            defaults: new { id = RouteParameter.Optional }
            );
            //定义Action 路由
            config.Routes.MapHttpRoute(
            name: "WithActionApi",
            routeTemplate: "actionapi/{controller}/{action}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );
 

            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));//解决跨域问题，需要nuget Cors
            //config.Routes.MapHttpRoute(
            //name: "DefaultApi",
            //routeTemplate: "api/{controller}/{id}",
            //defaults: new { id = RouteParameter.Optional }

            //);
            //将默认xml返回数据格式改为json
            //config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //config.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("datatype", "json", "application/json"));
            //json 序列化设置  
            //config.Formatters.JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings() {
            //        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore //设置忽略值为 null 的属性  
            //    };
            appBuilder.UseWebApi(config);
        }

    }
}