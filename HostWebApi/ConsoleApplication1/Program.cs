using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication1
{
    class Program
    {

        static void Main(string[] args)
        {
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration("http://localhost:9000");
            config.Routes.MapHttpRoute("API Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            HttpSelfHostServer server = new HttpSelfHostServer(config);
            //OpenAsync()屬非同步呼叫，加上Wait()則等待開啟完成才往下執行
            server.OpenAsync().Wait();
            Console.WriteLine("Web API host started...");
            string line = null;
            do
            {
                line = Console.ReadLine();
            }
            while (line != "exit");
            //結束链接
            server.CloseAsync().Wait();
        }
    }


    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        [HttpGet]
        [Route("st")]
        public string GetServerTim(string id)
        {
            try
            {
                return DateTime.Today.ToString("yyyy/MM/dd");
            }
            catch
            {
                return "failed :" + id;
            }
        }

        [HttpPost]
        public object SaveData(dynamic obj)
        {
            var strObj = Convert.ToString(obj);
            var desObj = JsonConvert.DeserializeObject(strObj);
            RequestContext rContext = new RequestContext();
            foreach (JProperty tm in desObj)
            {
                switch (tm.Name)
                {
                    case nameof(rContext.XmlTempleId):
                        rContext.XmlTempleId = tm.Value.ToString();
                        break;

                    case nameof(rContext.DataProperty):
                        rContext.DataProperty = tm.Value.ToString();
                        break;
                    case nameof(rContext.Header):
                        if (tm.Value != null)
                        {
                            rContext.Header = JsonConvert.DeserializeObject<Dictionary<string, string>>(tm.Value.ToString());
                        }
                        break;
                    case nameof(rContext.Pages):

                        rContext.Pages.Clear();
                        foreach (JToken v in tm.Value)
                        {
                            #region 真的 页数 key value 模式
                            //测试json 数据结构
                            /*
                            {
        "DataProperty": "TChart",
		"XmlTempleId": "xmlid",
		"Header": { "James": 9001,    "Jo": 3474, "Jess": 11926 },
		"pages":[
			{
				"page1":
						[  
						    {"DATE":"2019-09-18","ARMPITTEMP1":35.2,"ARMPITTEMP2":40.4,"ARMPITTEMP3":41.1,"ARMPITTEMP4":41.1,"ARMPITTEMP5":35.6,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":80,"SPHYGMUS2":85,"SPHYGMUS3":70,"SPHYGMUS4":100,"SPHYGMUS5":90,"SPHYGMUS6":77,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-19","ARMPITTEMP1":37.4,"ARMPITTEMP2":38.4,"ARMPITTEMP3":37.4,"ARMPITTEMP4":36.2,"ARMPITTEMP5":36.2,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":83,"SPHYGMUS2":97,"SPHYGMUS3":78,"SPHYGMUS4":102,"SPHYGMUS5":101,"SPHYGMUS6":96,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-20","ARMPITTEMP1":40.6,"ARMPITTEMP2":39.8,"ARMPITTEMP3":39.9,"ARMPITTEMP4":37.3,"ARMPITTEMP5":40.3,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":99,"SPHYGMUS2":102,"SPHYGMUS3":84,"SPHYGMUS4":111,"SPHYGMUS5":90,"SPHYGMUS6":78,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-21","ARMPITTEMP1":36.3,"ARMPITTEMP2":41.2,"ARMPITTEMP3":40.3,"ARMPITTEMP4":38,"ARMPITTEMP5":39.6,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":80,"SPHYGMUS2":85,"SPHYGMUS3":70,"SPHYGMUS4":100,"SPHYGMUS5":90,"SPHYGMUS6":77,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-22","ARMPITTEMP1":39.3,"ARMPITTEMP2":35.5,"ARMPITTEMP3":40.2,"ARMPITTEMP4":40.4,"ARMPITTEMP5":38.4,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":80,"SPHYGMUS2":85,"SPHYGMUS3":70,"SPHYGMUS4":100,"SPHYGMUS5":90,"SPHYGMUS6":77,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-23","ARMPITTEMP1":35.8,"ARMPITTEMP2":41,"ARMPITTEMP3":36.7,"ARMPITTEMP4":37.7,"ARMPITTEMP5":39.9,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":80,"SPHYGMUS2":85,"SPHYGMUS3":70,"SPHYGMUS4":100,"SPHYGMUS5":90,"SPHYGMUS6":77,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-24","ARMPITTEMP1":39.4,"ARMPITTEMP2":41,"ARMPITTEMP3":37.8,"ARMPITTEMP4":35.2,"ARMPITTEMP5":36.2,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":80,"SPHYGMUS2":85,"SPHYGMUS3":70,"SPHYGMUS4":100,"SPHYGMUS5":90,"SPHYGMUS6":77,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"}
						]  
			},
			{ 
				"page2":[
					
					  {"DATE":"2019-09-18","ARMPITTEMP1":35.2,"ARMPITTEMP2":40.4,"ARMPITTEMP3":41.1,"ARMPITTEMP4":41.1,"ARMPITTEMP5":35.6,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":80,"SPHYGMUS2":85,"SPHYGMUS3":70,"SPHYGMUS4":100,"SPHYGMUS5":90,"SPHYGMUS6":77,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-19","ARMPITTEMP1":37.4,"ARMPITTEMP2":38.4,"ARMPITTEMP3":37.4,"ARMPITTEMP4":36.2,"ARMPITTEMP5":36.2,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":83,"SPHYGMUS2":97,"SPHYGMUS3":78,"SPHYGMUS4":102,"SPHYGMUS5":101,"SPHYGMUS6":96,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-20","ARMPITTEMP1":40.6,"ARMPITTEMP2":39.8,"ARMPITTEMP3":39.9,"ARMPITTEMP4":37.3,"ARMPITTEMP5":40.3,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":99,"SPHYGMUS2":102,"SPHYGMUS3":84,"SPHYGMUS4":111,"SPHYGMUS5":90,"SPHYGMUS6":78,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-21","ARMPITTEMP1":36.3,"ARMPITTEMP2":41.2,"ARMPITTEMP3":40.3,"ARMPITTEMP4":38,"ARMPITTEMP5":39.6,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":80,"SPHYGMUS2":85,"SPHYGMUS3":70,"SPHYGMUS4":100,"SPHYGMUS5":90,"SPHYGMUS6":77,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-22","ARMPITTEMP1":39.3,"ARMPITTEMP2":35.5,"ARMPITTEMP3":40.2,"ARMPITTEMP4":40.4,"ARMPITTEMP5":38.4,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":80,"SPHYGMUS2":85,"SPHYGMUS3":70,"SPHYGMUS4":100,"SPHYGMUS5":90,"SPHYGMUS6":77,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-23","ARMPITTEMP1":35.8,"ARMPITTEMP2":41,"ARMPITTEMP3":36.7,"ARMPITTEMP4":37.7,"ARMPITTEMP5":39.9,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":80,"SPHYGMUS2":85,"SPHYGMUS3":70,"SPHYGMUS4":100,"SPHYGMUS5":90,"SPHYGMUS6":77,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"},
					        {"DATE":"2019-09-24","ARMPITTEMP1":39.4,"ARMPITTEMP2":41,"ARMPITTEMP3":37.8,"ARMPITTEMP4":35.2,"ARMPITTEMP5":36.2,"ARMPITTEMP6":65534,"COOLTEMP1":null,"HEARTRATE1":80,"HEARTRATE2":85,"HEARTRATE3":70,"HEARTRATE4":100,"HEARTRATE5":90,"HEARTRATE6":77,"SPHYGMUS1":80,"SPHYGMUS2":85,"SPHYGMUS3":70,"SPHYGMUS4":100,"SPHYGMUS5":90,"SPHYGMUS6":77,"INTAKEVOL1":"1200","OUTPUTVOL1":"1400","EVENT1":"事件1234","DOWN1":"降温提示"}
					    ]
				
			}
			]

}
    */
                            //var page = JsonConvert.DeserializeObject<Dictionary<string, Newtonsoft.Json.Linq.JToken>>(v.ToString()).Values;
                            //var days = page.Children<JToken>();
                            #endregion 

                            //集合结构 
                            var days = v.Children<JToken>();
                            List<Dictionary<string, string>> dayLst = new List<Dictionary<string, string>>();
                            foreach (var day in days)
                            {
                                var dDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(day.ToString());
                                dayLst.Add(dDic);//添加天数据
                            }
                            rContext.Pages.Add(dayLst);
                        }
                        break;
                }

            } 
            //连接文件服务取根据模板那id 取出 xml 模板

            //根据模板和数据生成pdf

            //上传pdf文件到 文件服务  

            //返回 pdf 文件id
            return strObj;
        }


        protected HttpRequestBase GetRequest()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_RequestContext"];//获取传统context

            HttpRequestBase request = context.Request;//定义传统request对象

            return request;
        }
        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return string.Format("Address:{0}, Port: {1}", prop.Address, prop.Port.ToString());
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
    }


    public class RequestContext
    {
        public string XmlTempleId { get; set; }

        public string DataProperty { get; set; }

        public Dictionary<string, string> Header { get; set; }

        public List<List<Dictionary<string, string>>> Pages { get; } = new List<List<Dictionary<string, string>>>();
    }


    #region test class
    public class Token
    {
        public string token { set; get; }
    }
    public class InnerData
    {
        public string ts { set; get; }
        public string id { set; get; }
        public string val { set; get; }
    }


    #endregion
}
