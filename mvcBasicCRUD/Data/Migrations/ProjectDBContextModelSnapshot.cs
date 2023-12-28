﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using mvcBasicCRUD.Data;

#nullable disable

namespace mvcBasicCRUD.Data.Migrations
{
    [DbContext(typeof(ProjectDBContext))]
    partial class ProjectDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("mvcBasicCRUD.Models.Chore", b =>
                {
                    b.Property<int>("ChoreID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChoreID"), 1L, 1);

                    b.Property<int?>("ChoreTypeID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ChoreID");

                    b.HasIndex("ChoreTypeID");

                    b.ToTable("Chores");
                });

            modelBuilder.Entity("mvcBasicCRUD.Models.ChoreType", b =>
                {
                    b.Property<int>("ChoreTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChoreTypeID"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ChoreTypeID");

                    b.ToTable("ChoreTypes");
                });

            modelBuilder.Entity("mvcBasicCRUD.Models.Chore", b =>
                {
                    b.HasOne("mvcBasicCRUD.Models.ChoreType", "ChoreType")
                        .WithMany("Chores")
                        .HasForeignKey("ChoreTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChoreType");
                });

            modelBuilder.Entity("mvcBasicCRUD.Models.ChoreType", b =>
                {
                    b.Navigation("Chores");
                });
#pragma warning restore 612, 618
        }
    }
}