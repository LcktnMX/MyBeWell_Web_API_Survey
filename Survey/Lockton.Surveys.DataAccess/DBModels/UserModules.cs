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
    [Table("Seguridad_UsuariosModulos")]
    public class UserModules : AuditModel
    {
        [Key]
        [Column("Seguridad_UsuariosModulos_Id")]
        public int Id { get; set; }
        
        [ForeignKey("User")]
        [Column("Seguridad_UsuariosModulos_UsuarioId")]
        public int UserId { get; set; }

        [ForeignKey("Module")]
        [Column("Seguridad_UsuariosModulos_ModuloId")]
        public int ModuleId { get; set; }

        [Column("Seguridad_UsuariosModulos_Privilegios")]
        public string Privilege { get; set; }
        public virtual User User { get; set; }

        public virtual Module Module { get; set; }

    }
}
