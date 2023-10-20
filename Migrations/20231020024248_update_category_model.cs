using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_QuanLyBanDienThoai.Migrations
{
    /// <inheritdoc />
    public partial class update_category_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Category");
        }
    }
}
