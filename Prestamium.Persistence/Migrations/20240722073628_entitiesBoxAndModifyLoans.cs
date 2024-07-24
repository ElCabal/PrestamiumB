using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prestamium.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class entitiesBoxAndModifyLoans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_InterestRate_InterestRateId",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_PaymentDeadline_PaymentDeadlineId",
                table: "Loan");

            migrationBuilder.DropTable(
                name: "InterestRate");

            migrationBuilder.DropTable(
                name: "PaymentDeadline");

            migrationBuilder.DropIndex(
                name: "IX_Loan_InterestRateId",
                table: "Loan");

            migrationBuilder.RenameColumn(
                name: "StatusLoan",
                table: "Loan",
                newName: "LoanStatusId");

            migrationBuilder.RenameColumn(
                name: "PaymentDeadlineId",
                table: "Loan",
                newName: "LoanStatusId1");

            migrationBuilder.RenameColumn(
                name: "InterestRateId",
                table: "Loan",
                newName: "Fees");

            migrationBuilder.RenameColumn(
                name: "DeadlineMonths",
                table: "Loan",
                newName: "InterestRate");

            migrationBuilder.RenameIndex(
                name: "IX_Loan_PaymentDeadlineId",
                table: "Loan",
                newName: "IX_Loan_LoanStatusId1");

            migrationBuilder.AddColumn<int>(
                name: "BoxId",
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InitialAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "BoxId",
                table: "Loan");

            migrationBuilder.RenameColumn(
                name: "LoanStatusId1",
                table: "Loan",
                newName: "PaymentDeadlineId");

            migrationBuilder.RenameColumn(
                name: "LoanStatusId",
                table: "Loan",
                newName: "StatusLoan");

            migrationBuilder.RenameColumn(
                name: "InterestRate",
                table: "Loan",
                newName: "DeadlineMonths");

            migrationBuilder.RenameColumn(
                name: "Fees",
                table: "Loan",
                newName: "InterestRateId");

            migrationBuilder.RenameIndex(
                name: "IX_Loan_LoanStatusId1",
                table: "Loan",
                newName: "IX_Loan_PaymentDeadlineId");

            migrationBuilder.CreateTable(
                name: "InterestRate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestRate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDeadline",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDeadline", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loan_InterestRateId",
                table: "Loan",
                column: "InterestRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_InterestRate_InterestRateId",
                table: "Loan",
                column: "InterestRateId",
                principalTable: "InterestRate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_PaymentDeadline_PaymentDeadlineId",
                table: "Loan",
                column: "PaymentDeadlineId",
                principalTable: "PaymentDeadline",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
