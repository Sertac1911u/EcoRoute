﻿// <auto-generated />
using System;
using EcoRoute.DataProcessing.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EcoRoute.DataProcessing.Migrations
{
    [DbContext(typeof(DataProcessingContext))]
    partial class DataProcessingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EcoRoute.DataProcessing.Entities.DataProcessingLog", b =>
                {
                    b.Property<Guid>("DataProcessingLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LogTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcessName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DataProcessingLogId");

                    b.ToTable("dataProcessingLogs");
                });

            modelBuilder.Entity("EcoRoute.DataProcessing.Entities.ProcessedData", b =>
                {
                    b.Property<Guid>("ProcessedDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("AverageFillLevel")
                        .HasColumnType("float");

                    b.Property<Guid>("BinId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PeriodEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PeriodStart")
                        .HasColumnType("datetime2");

                    b.Property<double>("PredictedFillLevel")
                        .HasColumnType("float");

                    b.Property<DateTime>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ProcessedDataId");

                    b.ToTable("processedDatas");
                });
#pragma warning restore 612, 618
        }
    }
}
