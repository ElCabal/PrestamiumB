using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prestamium.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class loan2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Installment_Loan_LoanId",
                table: "Installment");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Box_BoxId",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Client_ClientId",
                table: "Loan");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropColumn(
                name: "Frecuency",
                table: "Loan");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Loan",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Loan",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "BoxId",
                table: "Loan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "Loan",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Installment",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Installment",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Installment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Installment_Loan_LoanId",
                table: "Installment",
                column: "LoanId",
                principalTable: "Loan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Box_BoxId",
                table: "Loan",
                column: "BoxId",
                principalTable: "Box",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Client_ClientId",
                table: "Loan",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Installment_Loan_LoanId",
                table: "Installment");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Box_BoxId",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Client_ClientId",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Installment");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Installment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Loan",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Loan",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "BoxId",
                table: "Loan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Frecuency",
                table: "Loan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Installment",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstallmentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Installment_InstallmentId",
                        column: x => x.InstallmentId,
                        principalTable: "Installment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_InstallmentId",
                table: "Payment",
                column: "InstallmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Installment_Loan_LoanId",
                table: "Installment",
                column: "LoanId",
                principalTable: "Loan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Box_BoxId",
                table: "Loan",
                column: "BoxId",
                principalTable: "Box",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Client_ClientId",
                table: "Loan",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
