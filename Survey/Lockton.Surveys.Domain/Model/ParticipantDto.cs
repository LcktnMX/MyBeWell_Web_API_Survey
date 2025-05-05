using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.Domain.Model
{
    public class ParticipantDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string RazonSocial { get; set; }
        public string Email { get; set; }
        public string Aviso { get; set; }
        public string Terminos { get; set; }
        public bool Active { get; set; }
    }
}
