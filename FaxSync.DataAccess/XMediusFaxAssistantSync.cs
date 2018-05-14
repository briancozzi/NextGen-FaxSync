namespace FaxSync.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("XMediusFaxAssistantSync")]
    public partial class XMediusFaxAssistantSync
    {
        [Key]
        [StringLength(100)]
        public string AttorneyUserID { get; set; }

        [StringLength(100)]
        public string PreviousAssistantUserId { get; set; }

        [StringLength(100)]
        public string CurrentAssistantUserId { get; set; }

        [StringLength(100)]
        public string PreviousFaxNumber { get; set; }

        [StringLength(100)]
        public string CurrentFaxNumber { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DateUpdated { get; set; }
    }
}
