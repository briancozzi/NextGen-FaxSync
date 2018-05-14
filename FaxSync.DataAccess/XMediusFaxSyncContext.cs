namespace FaxSync.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class XMediusFaxSyncContext : DbContext
    {
        public XMediusFaxSyncContext()
            : base("name=XMediusFaxSync")
        {
        }

        public virtual DbSet<XMediusFaxAssistantSync> XMediusFaxAssistantSyncs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<XMediusFaxAssistantSync>()
                        .HasKey(e => e.AttorneyUserID);

            modelBuilder.Entity<XMediusFaxAssistantSync>()
                .Property(e => e.AttorneyUserID)
                .IsUnicode(false);
                

            modelBuilder.Entity<XMediusFaxAssistantSync>()
                .Property(e => e.PreviousAssistantUserId)
                .IsUnicode(false);

            modelBuilder.Entity<XMediusFaxAssistantSync>()
                .Property(e => e.CurrentAssistantUserId)
                .IsUnicode(false);

            modelBuilder.Entity<XMediusFaxAssistantSync>()
                .Property(e => e.PreviousFaxNumber)
                .IsUnicode(false);

            modelBuilder.Entity<XMediusFaxAssistantSync>()
                .Property(e => e.CurrentFaxNumber)
                .IsUnicode(false);
        }
    }
}
