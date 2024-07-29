using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prestamium.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Box_BoxId",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_LoanStatus_LoanStatusId1",
                table: "Loan");

            migrationBuilder.DropTable(
                name: "Box");

            migrationBuilder.DropTable(
                name: "LoanStatus");

            migrationBuilder.DropIndex(
                name: "IX_Loan_BoxId",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_Loan_LoanStatusId1",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "BoxId",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "LoanStatusId1",
                table: "Loan");

            migrationBuilder.RenameColumn(
                name: "LoanStatusId",
                table: "Loan",
                newName: "Frecuency");

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentAmount",
                table: "Loan",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingBalance",
                table: "Loan",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalInterestReceivable",
                table: "Loan",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentAmount",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "RemainingBalance",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "TotalInterestReceivable",
                table: "Loan");

            migrationBuilder.RenameColumn(
                name: "Frecuency",
                table: "Loan",
                newName: "LoanStatusId");

            migrationBuilder.AddColumn<int>(
                name: "BoxId",
                table: "Loan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoanStatusId1",
                table: "Loan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Box",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InitialAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Box", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameLoanStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loan_BoxId",
                table: "Loan",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_LoanStatusId1",
                table: "Loan",
                column: "LoanStatusId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Box_BoxId",
                table: "Loan",
                column: "BoxId",
                principalTable: "Box",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_LoanStatus_LoanStatusId1",
                table: "Loan",
                column: "LoanStatusId1",
                principalTable: "LoanStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
