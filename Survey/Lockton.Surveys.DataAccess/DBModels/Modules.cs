using Lockton.Surveys.DataAccess.DBModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockton.Surveys.DataAccess.DBModels
{
    [Table("Seguridad_Modulos")]
    public class Module:AuditModel
    {
        [Key]
        [Column("Seguridad_Modulos_Id")]
        public int Id { get; set; }
        [Column("Seguridad_Modulos_Nombre")]
        public string Name { get; set; }
        [Column("Seguridad_Modulos_Activo")]
        public bool Active { get; set; }

        public ICollection<UserModules> Users { get; set; }

    }
}
