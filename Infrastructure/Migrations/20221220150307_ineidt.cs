using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ineidt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobTimeHistories",
                table: "JobTimeHistories");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "JobTimeHistories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "JobTimeHistoryId",
                table: "JobTimeHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobTimeHistories",
                table: "JobTimeHistories",
                column: "JobTimeHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobTimeHistories",
                table: "JobTimeHistories");

            migrationBuilder.DropColumn(
                name: "JobTimeHistoryId",
                table: "JobTimeHistories");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "JobTimeHistories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobTimeHistories",
                table: "JobTimeHistories",
                column: "EmployeeId");
        }
    }
}
