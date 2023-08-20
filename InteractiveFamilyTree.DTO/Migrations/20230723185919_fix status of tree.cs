using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteractiveFamilyTree.DTO.Migrations
{
    /// <inheritdoc />
    public partial class fixstatusoftree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "FamilyTrees",
                type: "bit",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
