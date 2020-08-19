using Microsoft.EntityFrameworkCore;
using XCore.Services.Hiring.Core.Constants;
using XCore.Services.Hiring.Core.Models.Relations;

namespace XCore.Services.Hiring.Core.DataLayer.Context
{
    public static class AdvertisementsSkillsDBConfigurations
    {
        public static void HandleAdvertisementsSkillsDBConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdvertisementSkill>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.AdvertisementsSkillsTable, DBConstants.Schema)
                      .HasKey(x => new { x.AdvertisementId, x.SkillId });

                #endregion
                #region relations.

                entity.HasOne(x => x.Advertisement)
                      .WithMany(x => x.Skills)
                      .HasForeignKey(x => x.AdvertisementId);

                entity.HasOne(x => x.Skill)
                     .WithMany()
                     .HasForeignKey(x => x.SkillId);

                #endregion
                #region props.

                #endregion
            });
        }
    }
}
