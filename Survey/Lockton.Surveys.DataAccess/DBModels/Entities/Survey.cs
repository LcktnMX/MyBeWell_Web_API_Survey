using System;
using System.Collections.Generic;

#nullable disable

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    public partial class Survey
    {
        public Survey()
        {
            Sections = new HashSet<Section>();
            SurveyApplicationContents = new HashSet<SurveyApplicationContent>();
        }

        public Guid Id { get; set; }
        public Guid FingerPrint { get; set; }
        public string Name { get; set; }
        public string NameEs { get; set; }
        public string Description { get; set; }
        public string DescriptionEs { get; set; }
        public int? Version { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public Guid? LineId { get; set; }
        public bool? Active { get; set; }

        public virtual Line Line { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
        public virtual ICollection<SurveyApplicationContent> SurveyApplicationContents { get; set; }
    }
}
