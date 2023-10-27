using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_QuanLyBanDienThoai.Migrations
{
    /// <inheritdoc />
    public partial class Add_Column_Create_At_In_Order_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Order",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Order");
        }
    }
}
