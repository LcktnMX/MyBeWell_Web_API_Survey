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
    [Table("Seguridad_Usuarios")]
    public class User : AuditModel
    {
        [Key]
        [Column("Seguridad_Usuarios_Id")]
        public int Id { get; set; }

        [Column("Seguridad_Usuarios_Email")]
        public string Email { get; set; }

        [Column("Seguridad_Usuarios_Nombre")]
        public string Names { get; set; }

        [Column("Seguridad_Usuarios_Password")]
        public string Password { get; set; }

        [Column("Seguridad_Usuarios_IntentosFallidosBloqueo")]
        public int FailedAttemptsToBlock { get; set; }

        [Column("Seguridad_Usuarios_IntentosFallidosDeshabilitar")]
        public int FailedAttemptsToDisable { get; set; }

        [Column("Seguridad_Usuarios_UltimoCambioPass")]
        public DateTime LastPasswordChanged { get; set; }

        [Column("Seguridad_Usuarios_UltimoLogin")]
        public DateTime? LastLogin { get; set; }

        [Column("Seguridad_Usuarios_TiempoEsperaFecha")]
        public DateTime WaitTimeLoginDate { get; set; }

        [ForeignKey("Profile")]
        [Column("Seguridad_Usuarios_PerfilId")]
        public int ProfileId { get; set; }

        [ForeignKey("Group")]
        [Column("Seguridad_Usuarios_GrupoId")]
        public int GroupId { get; set; }

        [ForeignKey("Estatus")]
        [Column("Seguridad_Usuarios_EstatusId")]
        public int EstatusId { get; set; }

        [Column("Seguridad_Usuarios_TokenReset")]
        public string TokenReset { get; set; }

        [Column("Seguridad_Usuarios_TokenCaducidad")]
        public DateTime? TokenValidUntil { get; set; }

        [Column("Seguridad_Usuarios_Interno")]
        public bool? IsInternal { get; set; }

        [Column("Seguridad_Usuarios_NombreUsuario")]
        public string UserName { get; set; }

        [Column("Seguridad_Usuarios_AppId")]
        public Guid AppId { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Group Group { get; set; }
        public virtual UserStatus Estatus { get; set; }        
        public ICollection<UserModules> Modules { get; set; }
        public ICollection<PasswordHistory> Passwords { get; set; }



    }
}
