using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Properties",
                newName: "LastModifiedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Leads",
                newName: "LastModifiedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "InteractionLogs",
                newName: "LastModifiedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Deals",
                newName: "LastModifiedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Appointments",
                newName: "LastModifiedAt");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Leads",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "InteractionLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InteractionLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "InteractionLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Deals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "InteractionLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InteractionLogs");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "InteractionLogs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "Properties",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "Leads",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "InteractionLogs",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "Deals",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "Appointments",
                newName: "UpdatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);
        }
    }
}
