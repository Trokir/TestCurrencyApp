using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestCurrency.Core;
using TestCurrency.Models;

namespace TestCurrency.Configurations
{
    public class CurrencyConfiguration: IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> entity)
        {
            entity.ToTable("Currency");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.TypeOfCurrency).HasColumnName("TypeOfCurrency").HasDefaultValue(CurrencyType.Eur).IsRequired();
            entity.Property(c => c.Count).HasColumnName("Count").IsRequired();
            entity.Property(c => c.UserId).HasColumnName("UserId").IsRequired();
        }
    }
}
