using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wescoasteducation.api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedManySkillsToManyTeachers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SkillName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillTeacher",
                columns: table => new
                {
                    SkillsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeachersId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillTeacher", x => new { x.SkillsId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_SkillTeacher_Skill_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillTeacher_Teachers_TeachersId",
                        column: x => x.TeachersId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillTeacher_TeachersId",
                table: "SkillTeacher",
                column: "TeachersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillTeacher");

            migrationBuilder.DropTable(
                name: "Skill");
        }
    }
}
