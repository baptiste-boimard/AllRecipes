using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllRecipes_API.Migrations
{
    /// <inheritdoc />
    public partial class AddManyRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ingredients",
                table: "RecipesSql");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_NameId",
                table: "Ingredients",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_QuantityId",
                table: "Ingredients",
                column: "QuantityId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_UnityId",
                table: "Ingredients",
                column: "UnityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Names_NameId",
                table: "Ingredients",
                column: "NameId",
                principalTable: "Names",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Quantities_QuantityId",
                table: "Ingredients",
                column: "QuantityId",
                principalTable: "Quantities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_RecipesSql_RecipeId",
                table: "Ingredients",
                column: "RecipeId",
                principalTable: "RecipesSql",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Unities_UnityId",
                table: "Ingredients",
                column: "UnityId",
                principalTable: "Unities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Names_NameId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Quantities_QuantityId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_RecipesSql_RecipeId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Unities_UnityId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_NameId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_QuantityId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_UnityId",
                table: "Ingredients");

            migrationBuilder.AddColumn<string>(
                name: "Ingredients",
                table: "RecipesSql",
                type: "text",
                nullable: true);
        }
    }
}
