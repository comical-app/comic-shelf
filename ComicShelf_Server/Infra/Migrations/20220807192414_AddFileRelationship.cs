using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddFileRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LibraryId",
                table: "File",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_File_LibraryId",
                table: "File",
                column: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_Library_LibraryId",
                table: "File",
                column: "LibraryId",
                principalTable: "Library",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Library_LibraryId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_LibraryId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "File");
        }
    }
}
