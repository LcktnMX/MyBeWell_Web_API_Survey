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
    [Table("Seguridad_Grupos")]
    public class Group : AuditModel
    {
        [Key]
        [Column("Seguridad_Grupos_Id")]
        public int Id { get; set; }

        [Column("Seguridad_Grupos_NombreLegadoId")]
        public int? LegacyGroupId { get; set; }

        [Column("Seguridad_Grupos_NombreLegado")]
        public string LegacyGroupName { get; set; }

        [Column("Seguridad_Grupos_Activo")]
        public bool Active { get; set; }

        [Column("Seguridad_Grupos_DiasParaCambioPass")]
        public int DaysToChangePassword { get; set; }

        [Column("Seguridad_Grupos_IntentosParaBloqueo")]
        public int AttemptsToBlock { get; set; }

        [Column("Seguridad_Grupos_IntentosParaDeshabilitar")]
        public int AttemptsToDisable { get; set; }

        [Column("Seguridad_Grupos_MaxLongitudPass")]
        public int MaxLengthPassword { get; set; }

        [Column("Seguridad_Grupos_MinLogitudPass")]
        public int MinLengthPassword { get; set; }

        [Column("Seguridad_Grupos_RegistrosHistoricoPass")]
        public int NumberHistoryPassword { get; set; }

        [Column("Seguridad_Grupos_PalabrasReservadas")]
        public string ReservedWords { get; set; }

        [Column("Seguridad_Grupos_TiempoVidaSesionInactiva")]
        public int SessionInactiveLiveTime { get; set; }

        [Column("Seguridad_Grupos_TiempoVidaSession")]
        public int SessionLiveTime { get; set; }

        [ForeignKey("PasswordType")]
        [Column("Seguridad_Grupos_TipoPassword")]
        public int PasswordTypeId { get; set; }

        [Column("Seguridad_Grupos_TiempoEsperaLogin")]
        public int WaitTimeLogin { get; set; }

        [Column("Seguridad_Grupos_Multisesion")]
        public bool? MultiSession { get; set; }

        [Column("Seguridad_Grupos_CodigoVerificacionTipoId")]
        public int? VerificationCodeType { get; set; }

        [Column("Seguridad_Grupos_CodigoVerificacionLongitud")]
        public int? Len { get; set; }

        [Column("Seguridad_Grupos_CodigoVerificacionExpiracion")]
        public int? Expiration { get; set; }

        public virtual PasswordType PasswordType { get; set; }
        public ICollection<User> Users { get; set; }

    }
}
