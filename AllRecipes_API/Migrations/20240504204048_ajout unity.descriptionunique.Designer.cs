﻿// <auto-generated />
using System;
using AllRecipes_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AllRecipes_API.Migrations
{
    [DbContext(typeof(PostgresDbContext))]
    [Migration("20240504204048_ajout unity.descriptionunique")]
    partial class ajoutunitydescriptionunique
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AllRecipes_API.Models.Ingredient", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int?>("NameId")
                        .HasColumnType("integer");

                    b.Property<int?>("QuantityId")
                        .HasColumnType("integer");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("integer");

                    b.Property<int?>("UnityId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("NameId");

                    b.HasIndex("QuantityId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("UnityId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("AllRecipes_API.Models.Name", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Names");
                });

            modelBuilder.Entity("AllRecipes_API.Models.Quantity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Quantities");
                });

            modelBuilder.Entity("AllRecipes_API.Models.RecipeSql", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Directions")
                        .HasColumnType("text");

                    b.Property<string>("SubTitle")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RecipesSql");
                });

            modelBuilder.Entity("AllRecipes_API.Models.Unity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique();

                    b.ToTable("Unities");
                });

            modelBuilder.Entity("AllRecipes_API.Models.Ingredient", b =>
                {
                    b.HasOne("AllRecipes_API.Models.Name", "Name")
                        .WithMany("Ingredients")
                        .HasForeignKey("NameId");

                    b.HasOne("AllRecipes_API.Models.Quantity", "Quantity")
                        .WithMany("Ingredients")
                        .HasForeignKey("QuantityId");

                    b.HasOne("AllRecipes_API.Models.RecipeSql", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId");

                    b.HasOne("AllRecipes_API.Models.Unity", "Unity")
                        .WithMany("Ingredients")
                        .HasForeignKey("UnityId");

                    b.Navigation("Name");

                    b.Navigation("Quantity");

                    b.Navigation("Recipe");

                    b.Navigation("Unity");
                });

            modelBuilder.Entity("AllRecipes_API.Models.Name", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("AllRecipes_API.Models.Quantity", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("AllRecipes_API.Models.RecipeSql", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("AllRecipes_API.Models.Unity", b =>
                {
                    b.Navigation("Ingredients");
                });
#pragma warning restore 612, 618
        }
    }
}
