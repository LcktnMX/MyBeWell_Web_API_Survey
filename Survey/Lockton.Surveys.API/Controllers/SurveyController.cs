using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lockton.Surveys.Domain.Model;
using Lockton.Surveys.Services.Bussines;
using System.Linq;
using Lockton.Surveys.DataAccess.DBModels.Entities;

namespace Lockton.Surveys.API.Controllers
{
    [Route("api/Survey/[controller]")]
    [ApiController]
    //[Authorize]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyManager _surveyManager;

        public SurveyController(ISurveyManager surveyManager)
        {
            _surveyManager = surveyManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SurveyDto>>> GetAll()
        {
            try
            {
                var result = await _surveyManager.GetAll();

                return result.Any() ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyDto>> GetById(Guid id)
        {
            try
            {
                var result = await _surveyManager.GetById(id);

                return result != null ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        [HttpGet("{lineId}/all")]
        public async Task<ActionResult<IEnumerable<SurveyDto>>> GetByLineId(Guid lineId)
        {
            try
            {
                var result = await _surveyManager.GetByLineId(lineId);

                return result.Any() ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/versions")]
        public async Task<ActionResult<IEnumerable<SurveyDto>>> GetVersions(Guid id)
        {
            try
            {
                var result = await _surveyManager.GetVersions(id);

                return result.Any() ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("upsert")]
        public async Task<ActionResult<SurveyDto>> Upsert(SurveyDto dto)
        {
            try
            {
                return Ok(await _surveyManager.Upsert(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SurveyDto>> Delete(Guid id)
        {
            try
            {
                await _surveyManager.DeleteSurvey(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
