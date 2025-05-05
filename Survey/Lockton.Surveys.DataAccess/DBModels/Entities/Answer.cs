using System;
using System.Collections.Generic;

#nullable disable

namespace Lockton.Surveys.DataAccess.DBModels.Entities
{
    public partial class Answer
    {
        public Guid Id { get; set; }
        public Guid? IdQuestion { get; set; }
        public double? Position { get; set; }
        public string Contents { get; set; }
        public string ContentsEs { get; set; }
        public double? Value { get; set; }
        public string Type { get; set; }
        public bool? Active { get; set; }

        public virtual Question IdQuestionNavigation { get; set; }
    }
}
