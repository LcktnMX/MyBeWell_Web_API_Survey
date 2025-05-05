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
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantManager _participantManager;
        public ParticipantController(IParticipantManager participantManager)
        {
            _participantManager = participantManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetParticipant(Guid id)
        {
            try
            {
                var result = await _participantManager.GetById(id);

                return result != null ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetParticipants()
        {
            try
            {
                var result = await _participantManager.GetAll();

                return result.Any() ? Ok(result) : NoContent();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upsert")]
        public async Task<ActionResult<ParticipantDto>> save(ParticipantDto dto)
        {
            try
            {
                return Ok(await _participantManager.Upsert(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteParticipant(Guid id)
        {
            try
            {
                await _participantManager.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
