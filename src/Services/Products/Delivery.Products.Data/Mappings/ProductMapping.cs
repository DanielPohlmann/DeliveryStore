using Delivery.Products.Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delivery.Orders.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(x=> x.Title)
                    .IsRequired()
                    .HasColumnType($"varchar(150)");

            builder.Property(x => x.Description)
                    .IsRequired()
                    .HasColumnType($"varchar(500)");

            builder.Property(x => x.Imagem)
                   .HasColumnType($"varchar(300)");

            builder.Property(x => x.Price)
                 .HasPrecision(19, 4);

            builder.ToTable("Products");
        }
    }
}
