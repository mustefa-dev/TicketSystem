using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class notification2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolverDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketCount = table.Column<int>(type: "integer", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolverDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolverDatas_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f4832aba-9784-4f7f-ace8-b5b251fa4322"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 17, 22, 58, 0, 147, DateTimeKind.Utc).AddTicks(6497));

            migrationBuilder.CreateIndex(
                name: "IX_SolverDatas_UserId",
                table: "SolverDatas",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolverDatas");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f4832aba-9784-4f7f-ace8-b5b251fa4322"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 17, 22, 15, 55, 62, DateTimeKind.Utc).AddTicks(3511));
        }
    }
}
