using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WindowsFormsApplication1
{
    class UI
    {
        private static Form1 f1;
        public static Form1 GetF1()
        {
            if (f1 == null)
            {
                f1 = new Form1();
            }
            return f1;
        }
    }
    //Restful风格的WebApi服务
    [RoutePrefix("api/AttrOrder")]
    public class OrderController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        [Route("")]
        public IHttpActionResult GetByIdz(int id)
        {
            return Ok<string>("Success" + id);
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok<string>("Success");
        }

        [Route("{id:int=3}/OrderDetailById")]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            return Ok<string>("Success" + id);
        }

        [Route("{no}/OrderDetailByNo")]
        [HttpGet]
        public IHttpActionResult GetByNO(string no)
        {
            return Ok<string>("Success" + no);
        }

        [Route("{name}/OrderDetailByName")]
        [HttpGet]
        public IHttpActionResult GetByName(string name)
        {
            return Ok<string>("Success" + name);
        }

        [Route("postdata")]
        [HttpPost]
        public HttpResponseMessage PostData(int id)
        {
            return Request.CreateResponse();
        }

        [Route("Test/AttrRoute")]
        [HttpPost]
        public HttpResponseMessage SavaData(ORDER order)
        {
            return Request.CreateResponse();
        }
    }

    public class queryController : ApiController
    {
        //get api
        public string Get(string id)
        {
            return id;
        }
        // POST api
        public string Post([FromBody] string value)
        {
            return value;
        }
        // PUT api
        public void Put(int id, string value)
        {
        }
        // DELETE api
        public void Delete(int id)
        {
        }
    }

    public class PersonController : ApiController
    {
        Person[] personList = new Person[] {
        new Person { Id= 1,Age = 2,Name="DANNY"},
        new Person { Id = 2,Age = 3,Name = "Danny123"},
        new Person { Id =3,Age = 4,Name = "dANNY456"}
        };

        [HttpGet]
        [Route("api/person/getAll")]
        public List<Person> GetListAll()
        {
            return personList.ToList();
        }

        public List<Person> Get(string id)
        {
            return personList.ToList();
        }
    }
    public class Person
    {
        public int Id { set; get; }
        public string Name { get; set; }
        public int Age { set; get; }
    }
    public class LogController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok("Hello World!");
        }

        // GET
        //这里用户请求的时候,如果不提交msg参数,那么将会404,建议将参数改成这种方式[FromUri] Obj obj
        //[HttpGet]
        //public string Get([FromUri] Obj obj)
        //{
        //    if (obj == null)
        //    {
        //        return "msg参数不能为空!";
        //    }
        //    UI.GetF1().AppendText("用户发起了Get请求(http://xxx.xxx.x.xxx:9000/api/log/Get),msg参数:" + obj.msg);
        //    return "msg:" + obj.msg;
        //}

        [HttpPost]
        public string Post([FromBody]Obj obj)
        {
            if (obj == null)
            {
                return "msg参数不能为空!";
            }
            UI.GetF1().AppendText("用户发起了Post请求(http://xxx.xxx.x.xxx:9000/api/log/Post),msg参数:" + obj.msg);
            return "msg:" + obj.msg;
        }
        [HttpPost]
        [ActionName("Thumbnail")]
        public HttpResponseMessage log([FromBody] Obj obj) 
        {
            HttpResponseMessage result;
            string response = "";
            if (obj == null)
            {
                response = "参数不能为空!";
            }
            else
            {
                if (obj.msg == null || obj.msg.Length == 0)
                {
                    response += "msg参数不能为空";
                }
                if (obj.log == null || obj.log.Length == 0)
                {
                    response += " log参数不能为空";
                }
            }
            if (response.Length > 0)
            {
                result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(response, System.Text.Encoding.UTF8)
                };
                return result;
            }
            UI.GetF1().AppendText("用户发起了Post请求(http://xxx.xxx.x.xxx:9000/api/log/log),msg=" + obj.msg + ",log=" + obj.log);
            result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("这是post log:msg=" + obj.msg + ",log=" + obj.log)
            };
            return result;
        }

        public class Obj
        {
            public string msg { get; set; }
            public string log { get; set; }
        }

    }

}
