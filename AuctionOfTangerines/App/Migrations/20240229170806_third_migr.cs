using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuctionOfTangerines.Migrations
{
    /// <inheritdoc />
    public partial class third_migr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tangerines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CurrentPrice = table.Column<double>(type: "double precision", nullable: false),
                    BetId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tangerines", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_TangerineId",
                table: "Bets",
                column: "TangerineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Tangerines_TangerineId",
                table: "Bets",
                column: "TangerineId",
                principalTable: "Tangerines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Tangerines_TangerineId",
                table: "Bets");

            migrationBuilder.DropTable(
                name: "Tangerines");

            migrationBuilder.DropIndex(
                name: "IX_Bets_TangerineId",
                table: "Bets");
        }
    }
}
