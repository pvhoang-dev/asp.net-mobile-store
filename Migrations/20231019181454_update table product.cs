using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_QuanLyBanDienThoai.Migrations
{
    /// <inheritdoc />
    public partial class updatetableproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price1",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "Price2",
                table: "Product",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ProductVariant",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Product",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Product",
                newName: "Price2");

            migrationBuilder.AddColumn<double>(
                name: "Price1",
                table: "Product",
                type: "float",
                nullable: true);
        }
    }
}
