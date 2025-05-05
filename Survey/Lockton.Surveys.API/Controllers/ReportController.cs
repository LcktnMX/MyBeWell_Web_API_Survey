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
using System.Text.Json.Serialization;

namespace Lockton.Surveys.API.Controllers
{
    [Route("api/Survey/[controller]")]
    [ApiController]    
    //[Authorize]
    public class ReportController : ControllerBase
    {


        public ReportController()
        {
        }

        [HttpGet("participantId")]
        public async Task<ActionResult<ApplicantContentDto>> GetByParticipant([Required][FromQuery] Guid participantId)
            => Ok(new ApplicantContentDto());

        [HttpGet("participantId/surveyId")]
        public async Task<ActionResult<ApplicantContentDto>> GetByParticipantAndSurveyId([Required][FromQuery] Guid participantId, [Required][FromQuery] Guid surveyId)
            => Ok(new ApplicantContentDto());

    }
}
