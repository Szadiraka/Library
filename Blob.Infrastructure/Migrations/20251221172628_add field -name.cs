using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blob.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addfieldname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StorageName",
                table: "Buckets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageName",
                table: "Buckets");
        }
    }
}
