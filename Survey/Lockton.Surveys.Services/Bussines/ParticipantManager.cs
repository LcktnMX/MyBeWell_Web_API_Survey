using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lockton.Surveys.DataAccess.DBModels.Entities;
using Lockton.Surveys.DataAccess.Repositories;
using Lockton.Surveys.Domain.Model;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lockton.Surveys.Services.Bussines
{
    public interface IParticipantManager
    {
        Task<ParticipantDto> Upsert(ParticipantDto dto);
        Task<ParticipantDto> GetById(Guid id);
        Task<IEnumerable<ParticipantDto>> GetAll();
        Task Delete(Guid id);
    }
    public class ParticipantManager : IParticipantManager
    {
        private readonly IRepository<Participant> _participantRepository;
        public ParticipantManager(IRepository<Participant> participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<ParticipantDto> Upsert(ParticipantDto dto)
        {
            Participant participantDb = null;
            if (dto.Id != null || dto.Id != Guid.Empty)
                participantDb = await _participantRepository.GetByCondition(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (participantDb == null)
            {
                participantDb = (await _participantRepository.Create(new()
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Terminos = dto.Terminos,
                    RazonSocial = dto.RazonSocial,
                    Aviso = dto.Aviso,
                    Active = dto.Active,
                })).Entity;
            }
            else
            {
                participantDb.Name = dto.Name;
                participantDb.RazonSocial = dto.RazonSocial;
                participantDb.Email = dto.Email;
                participantDb.Terminos = dto.Terminos;
                participantDb.Aviso = dto.Aviso;
                participantDb.Active = dto.Active;
                _participantRepository.Update(participantDb);
            }

            await _participantRepository.SaveChanges();

            return await GetById(participantDb.Id);

        }
        public async Task<ParticipantDto> GetById(Guid id)
        {
            var participantDb = await _participantRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();

            return participantDb?.Adapt<ParticipantDto>() ?? null;
        }

        public async Task<IEnumerable<ParticipantDto>> GetAll()
        {
            var participantsDB = await _participantRepository.GetAll().ToListAsync();

            return participantsDB?.Adapt<List<ParticipantDto>>().OrderByDescending(x => x.Name) ?? null;
        }
        public async Task Delete(Guid id)
        {
            var participantDb = await _participantRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();

            if (participantDb == null)
                throw new Exception("No existe el participante");

            participantDb.Active = false;
            _participantRepository.Update(participantDb);
            await _participantRepository.SaveChanges();
        }
    }
}
