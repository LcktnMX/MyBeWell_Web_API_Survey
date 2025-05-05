using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lockton.Surveys.DataAccess.DBModels.Base;

namespace Lockton.Surveys.DataAccess.DBModels
{
    [Table("Seguridad_Perfiles")]
    public class Profile : AuditModel
    {
        [Key]
        [Column("Seguridad_Perfiles_Id")]
        public int Id { get; set; }
        [Column("Seguridad_Perfiles_Nombre")]
        public string Name { get; set; }
        [Column("Seguridad_Perfiles_Activo")]
        public bool Active { get; set; }
        public ICollection<User> Users { get; set; }

    }
}
