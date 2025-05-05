using System;
using System.Collections.Generic;

#nullable disable

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            Questions = new HashSet<Question>();
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Regex { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
