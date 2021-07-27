using System.Web.Mvc;
using AutoMapper;
using DTOModel;
using EmptyMVC.Models;
using Mapper;
using MechanicsModel;

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
    }
}