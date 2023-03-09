using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using NotableAthletes.Models.ConData;

namespace NotableAthletes.Data
{
  public partial class ConDataContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public ConDataContext(DbContextOptions<ConDataContext> options):base(options)
    {
    }

    public ConDataContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<NotableAthletes.Models.ConData.Athlete>()
              .HasOne(i => i.Country)
              .WithMany(i => i.Athletes)
              .HasForeignKey(i => i.CountryID)
              .HasPrincipalKey(i => i.CountryID);


        builder.Entity<NotableAthletes.Models.ConData.Athlete>()
              .Property(p => p.AthleteID)
              .HasPrecision(19, 0);

        builder.Entity<NotableAthletes.Models.ConData.Athlete>()
              .Property(p => p.CountryID)
              .HasPrecision(10, 0);

        builder.Entity<NotableAthletes.Models.ConData.Country>()
              .Property(p => p.CountryID)
              .HasPrecision(10, 0);
        this.OnModelBuilding(builder);
    }


    public DbSet<NotableAthletes.Models.ConData.Athlete> Athletes
    {
      get;
      set;
    }

    public DbSet<NotableAthletes.Models.ConData.Country> Countries
    {
      get;
      set;
    }
  }
}
