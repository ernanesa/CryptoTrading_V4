using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataCollection.Migrations
{
    /// <inheritdoc />
    public partial class Symbols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BaseCurrency = table.Column<string>(type: "varchar(25)", nullable: true),
                    Currency = table.Column<string>(type: "varchar(25)", nullable: true),
                    DepositMinimum = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", nullable: true),
                    ExchangeListed = table.Column<bool>(type: "boolean", nullable: false),
                    ExchangeTraded = table.Column<bool>(type: "boolean", nullable: false),
                    MinMovement = table.Column<double>(type: "double precision", nullable: false),
                    PriceScale = table.Column<long>(type: "bigint", nullable: false),
                    SessionRegular = table.Column<string>(type: "text", nullable: true),
                    SymbolName = table.Column<string>(type: "text", nullable: true),
                    Timezone = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    WithdrawMinimum = table.Column<double>(type: "double precision", nullable: false),
                    WithdrawalFee = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Symbols");
        }
    }
}
