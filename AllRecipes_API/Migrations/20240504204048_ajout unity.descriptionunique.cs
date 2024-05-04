using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllRecipes_API.Migrations
{
    /// <inheritdoc />
    public partial class ajoutunitydescriptionunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "UnityId",
                table: "Ingredients",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "Ingredients",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "QuantityId",
                table: "Ingredients",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "NameId",
                table: "Ingredients",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Unities_Description",
                table: "Unities",
                column: "Description",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Names_NameId",
                table: "Ingredients",
                column: "NameId",
                principalTable: "Names",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Quantities_QuantityId",
                table: "Ingredients",
                column: "QuantityId",
                principalTable: "Quantities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_RecipesSql_RecipeId",
                table: "Ingredients",
                column: "RecipeId",
                principalTable: "RecipesSql",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Unities_UnityId",
                table: "Ingredients",
                column: "UnityId",
                principalTable: "Unities",
                principalColumn: "Id");
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
                name: "IX_Unities_Description",
                table: "Unities");

            migrationBuilder.AlterColumn<int>(
                name: "UnityId",
                table: "Ingredients",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "Ingredients",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuantityId",
                table: "Ingredients",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NameId",
                table: "Ingredients",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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
    }
}
