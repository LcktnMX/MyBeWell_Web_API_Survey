using System;

namespace Lockton.Surveys.Domain.Model
{
    public class LineDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
