﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using wescoast_education.api.Data;

#nullable disable

namespace wescoasteducation.api.Data.Migrations
{
    [DbContext(typeof(EducationContext))]
    [Migration("20230209155456_AddedRelationTeacherToCourse")]
    partial class AddedRelationTeacherToCourse
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("wescoast_education.api.Models.Course", b =>
                {
                    b.Property<Guid>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("CourseNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("CourseId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("wescoast_education.api.Models.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("BirthOfDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("wescoast_education.api.Models.Teacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("BirthOfDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("wescoast_education.api.Models.Course", b =>
                {
                    b.HasOne("wescoast_education.api.Models.Teacher", "Teacher")
                        .WithMany("Courses")
                        .HasForeignKey("TeacherId");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("wescoast_education.api.Models.Student", b =>
                {
                    b.HasOne("wescoast_education.api.Models.Course", "Course")
                        .WithMany("Students")
                        .HasForeignKey("CourseId");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("wescoast_education.api.Models.Course", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("wescoast_education.api.Models.Teacher", b =>
                {
                    b.Navigation("Courses");
                });
#pragma warning restore 612, 618
        }
    }
}
