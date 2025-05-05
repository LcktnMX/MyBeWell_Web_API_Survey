using Lockton.Surveys.Services.Security;
using Lockton.Surveys.Services.Utils;
using Lockton.Surveys.DataAccess.DBModels;
using Lockton.Surveys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lockton.Surveys.Domain.Model;
using Lockton.Surveys.Services.Bussines;
using System.ComponentModel.DataAnnotations;

namespace Lockton.Surveys.API.Controllers
{
    [Route("api/Survey/[controller]")]
    [ApiController]
    //[Authorize]
    public class SurveyApplicationController : ControllerBase
    {
        private readonly ISurveyApplicationConfigManager _configManager;

        public SurveyApplicationController(ISurveyApplicationConfigManager configManager)
        {
            _configManager = configManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetAll()
        {
            try
            {
                var result = await _configManager.GetAll();

                return result.Any() ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDto>> GetById(Guid id)
        {
            try
            {
                var result = await _configManager.GetById(id);

                return result != null ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{lineId}/line")]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetByLineId(Guid lineId)
        {
            try
            {
                var result = await _configManager.GetByLineId(lineId);

                return result.Any() ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{participantId}/participant")]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetByParticipantId(Guid participantId)
        {
            try
            {
                var result = await _configManager.GetByParticipantId(participantId);

                return result.Any() ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upsert")]
        public async Task<ActionResult<ApplicationDto>> Upsert(ApplicationDto dto)
        {
            try
            {
                return Ok(await _configManager.Upsert(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("log")]
        public async Task<ActionResult> SaveLog(SurveyLogMailDto dto)
        {
            try
            {
                return Ok(_configManager.SaveLog(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("log/{id}")]
        public async Task<ActionResult<IEnumerable<SurveyLogMailDto>>> GetLogs(Guid id)
        {
            try
            {
                var result = await _configManager.GetLogBySurverApplication(id);

                return result.Any() ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<SurveyDto>> Delete(Guid id)
        {
            try
            {
                await _configManager.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
