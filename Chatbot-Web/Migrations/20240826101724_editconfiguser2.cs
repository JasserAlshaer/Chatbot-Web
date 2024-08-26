using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatbot_Web.Migrations
{
    /// <inheritdoc />
    public partial class editconfiguser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDateTime",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginDateTime",
                table: "Users");
        }
    }
}
