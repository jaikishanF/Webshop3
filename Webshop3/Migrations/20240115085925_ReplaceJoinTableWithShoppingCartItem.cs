using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop3.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceJoinTableWithShoppingCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerProducts_Customer_CustomersId",
                table: "CustomerProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerProducts_Product_ShoppingCartId",
                table: "CustomerProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerProducts",
                table: "CustomerProducts");

            migrationBuilder.RenameTable(
                name: "CustomerProducts",
                newName: "CustomerProduct");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerProducts_ShoppingCartId",
                table: "CustomerProduct",
                newName: "IX_CustomerProduct_ShoppingCartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerProduct",
                table: "CustomerProduct",
                columns: new[] { "CustomersId", "ShoppingCartId" });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => new { x.CustomerId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ProductId",
                table: "ShoppingCartItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerProduct_Customer_CustomersId",
                table: "CustomerProduct",
                column: "CustomersId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerProduct_Product_ShoppingCartId",
                table: "CustomerProduct",
                column: "ShoppingCartId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerProduct_Customer_CustomersId",
                table: "CustomerProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerProduct_Product_ShoppingCartId",
                table: "CustomerProduct");

            migrationBuilder.DropTable(
                name: "ShoppingCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerProduct",
                table: "CustomerProduct");

            migrationBuilder.RenameTable(
                name: "CustomerProduct",
                newName: "CustomerProducts");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerProduct_ShoppingCartId",
                table: "CustomerProducts",
                newName: "IX_CustomerProducts_ShoppingCartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerProducts",
                table: "CustomerProducts",
                columns: new[] { "CustomersId", "ShoppingCartId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerProducts_Customer_CustomersId",
                table: "CustomerProducts",
                column: "CustomersId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerProducts_Product_ShoppingCartId",
                table: "CustomerProducts",
                column: "ShoppingCartId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
