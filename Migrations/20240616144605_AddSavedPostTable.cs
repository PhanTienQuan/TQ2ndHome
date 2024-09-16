using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TQuanHome.Migrations
{
    /// <inheritdoc />
    public partial class AddSavedPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SavedPosts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SaveDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedPosts", x => new { x.PostId, x.UserName });
                    table.ForeignKey(
                        name: "FK_SavedPosts_PostInfo_PostId",
                        column: x => x.PostId,
                        principalTable: "PostInfo",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_PostId",
                table: "SavedPosts",
                column: "PostId",
                unique: true); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedPosts");

        }
    }
}
