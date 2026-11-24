using Identity.CoreModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.CoreModels.Configurations
{
    public class OtpManagerConfiguration : IEntityTypeConfiguration<OtpManager>
    {
        public void Configure(EntityTypeBuilder<OtpManager> builder)
        {
            builder.ToTable("OtpManager");
            builder.HasKey(x=>x.OtpId);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.EmailId).IsRequired();
            builder.Property(x => x.OtpType).IsRequired();
            builder.Property(x => x.OtpText).IsRequired();
            builder.Property(x => x.CreatdAt).IsRequired();
            builder.Property(x => x.ExpiredTime).IsRequired();

        }
    }
}
