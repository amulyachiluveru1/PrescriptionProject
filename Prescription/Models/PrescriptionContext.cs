using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Prescription.Models
{
    public class PrescriptionContext : DbContext
        {
            public DbSet<Prescription> Prescriptions { get; set; } = null!;

            public PrescriptionContext(DbContextOptions<PrescriptionContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // Seed data
                modelBuilder.Entity<Prescription>().HasData(
                    new Prescription
                    {
                        PrescriptionId = 1,
                        MedicationName = "Atorvastatin",
                        FillStatus = "New",
                        Cost = 19.99,
                        RequestTime = new DateTime(2024, 10, 1, 9, 30, 0)
                    },
                    new Prescription
                    {
                        PrescriptionId = 2,
                        MedicationName = "Lisinopril",
                        FillStatus = "Filled",
                        Cost = 12.50,
                        RequestTime = new DateTime(2024, 11, 2, 14, 15, 0)
                    },
                    new Prescription
                    {
                        PrescriptionId = 3,
                        MedicationName = "Metformin",
                        FillStatus = "Pending",
                        Cost = 8.75,
                        RequestTime = new DateTime(2024, 12, 12, 10, 0, 0)
                    }
                );
            }
        }
    }
