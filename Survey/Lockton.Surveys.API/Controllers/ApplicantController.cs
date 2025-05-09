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
using static System.Net.Mime.MediaTypeNames;

namespace Lockton.Surveys.API.Controllers
{
    [Route("api/Survey/[controller]")]
    [ApiController]
    //[Authorize]
    public class ApplicantController : ControllerBase
    {

        private readonly IApplicantManager _applicantManager;
        public ApplicantController(IApplicantManager applicantManager)
        {
            _applicantManager = applicantManager;
        }

        [HttpGet("{idApplication}")]
        public async Task<ActionResult<ApplicantContentDto>> GetById([Required] Guid idApplication)
        {
            try
            {
                var result = await _applicantManager.GetPartakerApplication(idApplication);

                return result != null ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("answer")]
        public async Task<ActionResult> SaveAnswer(AnsweredDto dto)
        {
            try
            {
                await _applicantManager.SaveAnswer(dto);

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logAnswer")]
        public async Task<ActionResult<LogAnswerDto>> SaveLogAnswer(LogAnswerDto dto)
        {
            try
            {
                return Ok(await _applicantManager.LogAnswer(dto));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Finish/{appId}")]
        public async Task<ActionResult> FinishApplication(Guid appId)
        {
            try
            {
                await _applicantManager.FinishApplication(appId);

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Progress/{appId}/Percent/{percent}")]
        public async Task<ActionResult> UpdateProgress(Guid appId, float percent)
        {
            try
            {
                await _applicantManager.UpdateProgress(appId, percent);

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Observation")]
        public async Task<ActionResult<AnsweredObservationDto>> UpsertObservation(AnsweredObservationDto dto)
        {
            try
            {
                var result = await _applicantManager.UpsertMessage(dto);

                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Observation/{idSurveyApplicationContents}/open/{open}")]
        public async Task<ActionResult<IEnumerable<AnsweredObservationDto>>> GetObservations(Guid idSurveyApplicationContents, bool open)
        {
            try
            {
                var result = await _applicantManager.GetObservations(idSurveyApplicationContents, open);

                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        //[HttpGet("Download/{appId}")]
        //public async Task<ActionResult> DownloadApp(Guid appId)
        //{
        //    try
        //    {
        //        var result = await _applicantManager.DownloadPartakerApplication(appId);

        //        return File(result, "application/zip", $"{appId}.zip");
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
