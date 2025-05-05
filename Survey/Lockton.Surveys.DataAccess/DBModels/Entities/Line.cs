using System;
using System.Collections.Generic;

#nullable disable

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    public partial class Line
    {
        public Line()
        {
            Surveys = new HashSet<Survey>();
        }

        public Guid Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Survey> Surveys { get; set; }
    }
}
