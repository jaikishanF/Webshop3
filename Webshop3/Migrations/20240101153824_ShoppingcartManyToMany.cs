using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop3.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingcartManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingCart",
                table: "Customer");

            migrationBuilder.CreateTable(
                name: "CustomerProducts",
                columns: table => new
                {
                    CustomersId = table.Column<int>(type: "int", nullable: false),
                    ShoppingCartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProducts", x => new { x.CustomersId, x.ShoppingCartId });
                    table.ForeignKey(
                        name: "FK_CustomerProducts_Customer_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProducts_Product_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_ShoppingCartId",
                table: "CustomerProducts",
                column: "ShoppingCartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProducts");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCart",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
