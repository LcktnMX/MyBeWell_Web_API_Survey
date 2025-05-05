using System;
using System.Collections.Generic;

#nullable disable

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    public partial class Section
    {
        public Section()
        {
            Questions = new HashSet<Question>();
        }

        public Guid Id { get; set; }
        public Guid? IdSurvey { get; set; }
        public string Title { get; set; }
        public string TitleEs { get; set; }
        public string Description { get; set; }
        public string DescriptionEs { get; set; }
        public double? Position { get; set; }
        public bool? Active { get; set; }

        public virtual Survey IdSurveyNavigation { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
