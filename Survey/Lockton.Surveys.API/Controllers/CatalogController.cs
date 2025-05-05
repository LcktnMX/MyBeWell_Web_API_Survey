using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lockton.Surveys.Domain.Model;
using Lockton.Surveys.Services.Bussines;

namespace Lockton.Surveys.API.Controllers
{
    [Route("api/Survey/[controller]")]
    [ApiController]
    //[Authorize]
    public class CatalogController : ControllerBase
    {

        private readonly ICatalogManager _catalogManager;
        public CatalogController(ICatalogManager catalogManager)
        {
            _catalogManager = catalogManager;
        }

        [HttpGet("Lines")]
        public async Task<ActionResult<IEnumerable<LineDto>>> GetLines()
        {
            try
            {
                var result = await _catalogManager.GetLines();

                return result.Any() ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("QuestionTypes")]
        public async Task<ActionResult<IEnumerable<QuestionTypesDto>>> GetQuestionTypes()
        {
            try
            {
                var result = await _catalogManager.GetQuestionTypes();

                return result.Any() ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
