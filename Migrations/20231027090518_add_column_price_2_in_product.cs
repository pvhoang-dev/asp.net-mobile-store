using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_QuanLyBanDienThoai.Migrations
{
    /// <inheritdoc />
    public partial class add_column_price_2_in_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price2",
                table: "Product",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price2",
                table: "Product");
        }
    }
}
