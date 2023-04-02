using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DailyApartmentsMVC.Models.OwnerModel;

public partial class OwnerContext : DbContext
{
    public OwnerContext()
    {
    }

    public OwnerContext(DbContextOptions<OwnerContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
