using System;
using System.IO;
using System.Web.Mvc;
using AutoMapper;
using DTOModel;
using EmptyMVC.Models;
using Mapper;
using MechanicsModel;
using Newtonsoft.Json;

namespace EmptyMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpGet]
        public ActionResult Index(int id)
        {
            var converter = new StringToCombinationConverter(CombinationStringFormat.Short);
            var combStr = $"1r 2r 3r {id}r";
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            }).CreateMapper();
            var combModel = converter.StringToCombination(combStr);
            var combDto = mapper.Map<CombinationDto>(combModel);
            return new JsonNetResult<CombinationDto>(combDto);
        }

        [HttpGet]
        [ActionName("simple_result")]
        public ViewResult SimpleExample()
        {
            return View("~/Views/Home/SimpleExample.html");
        }

        [HttpPost]
        [ActionName("post_check")]
        public ActionResult PostCheck()
        {
            var json = new StreamReader(Request.InputStream).ReadToEnd();
            var postData = JsonConvert.DeserializeObject<TestPostData>(json);
            return new JsonNetResult<TestResult>(new TestResult {Result = postData.Time + " " + postData.Data});
        }

        /*public ActionResult GameCheck()
        {
            var json = new StreamReader(Request.InputStream).ReadToEnd();
            GameModel game;
            try
            {
                game = JsonConvert.DeserializeObject<GameModel>(json);
            }
            catch (Exception ex)
            {
                
            }
        }*/
    }

    [JsonObject]
    public class TestResult
    {
        [JsonProperty("result")]
        public string Result { get; set; }
    }

    [JsonObject]
    public class TestPostData
    {
        [JsonProperty("data_value")]
        public int Data { get; set; }

        [JsonProperty("time_value")]
        public string Time { get; set; }
    }
}