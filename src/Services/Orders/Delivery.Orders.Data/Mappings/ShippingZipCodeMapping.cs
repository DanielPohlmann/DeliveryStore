using Delivery.Core.DomainObjects;
using Delivery.Orders.Domain;
using Delivery.Orders.Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Delivery.Orders.Data.Mappings
{
    public class ShippingZipCodeMapping : IEntityTypeConfiguration<ShippingZipCode>
    {
        public void Configure(EntityTypeBuilder<ShippingZipCode> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(x => x.Price)
                 .HasPrecision(8, 4);

            builder.OwnsOne(c => c.ZipCodeBegin,
            tf =>
            {
                tf.Property(c => c.Number)
                    .IsRequired()
                    .HasMaxLength(ZipCode.ZipCodeLength)
                    .HasColumnName(nameof(ShippingZipCode.ZipCodeBegin))
                    .HasColumnType($"varchar({ZipCode.ZipCodeLength})");
            });

            builder.OwnsOne(c => c.ZipCodeEnd,
            tf => {
               tf.Property(c => c.Number)
                    .IsRequired()
                    .HasMaxLength(ZipCode.ZipCodeLength)
                    .HasColumnName(nameof(ShippingZipCode.ZipCodeEnd))
                    .HasColumnType($"varchar({ZipCode.ZipCodeLength})");
           });

           builder.ToTable("ShippingZipCodes");
        }
    }
}
