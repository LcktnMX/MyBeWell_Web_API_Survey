using System;
using System.Collections.Generic;

#nullable disable

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    public partial class Participant
    {
        public Participant()
        {
            SurveyApplications = new HashSet<SurveyApplication>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RazonSocial { get; set; }
        public string Email { get; set; }
        public string Aviso { get; set; }
        public string Terminos { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<SurveyApplication> SurveyApplications { get; set; }
    }
}
