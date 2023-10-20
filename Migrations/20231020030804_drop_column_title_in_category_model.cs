using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_QuanLyBanDienThoai.Migrations
{
    /// <inheritdoc />
    public partial class drop_column_title_in_category_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
