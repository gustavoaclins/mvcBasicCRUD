using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvcBasicCRUD.Data.Migrations
{
    public partial class AddedChoresAndChorestypesModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChoreTypes",
                columns: table => new
                {
                    ChoreTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoreTypes", x => x.ChoreTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Chores",
                columns: table => new
                {
                    ChoreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    ChoreTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chores", x => x.ChoreID);
                    table.ForeignKey(
                        name: "FK_Chores_ChoreTypes_ChoreTypeID",
                        column: x => x.ChoreTypeID,
                        principalTable: "ChoreTypes",
                        principalColumn: "ChoreTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chores_ChoreTypeID",
                table: "Chores",
                column: "ChoreTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chores");

            migrationBuilder.DropTable(
                name: "ChoreTypes");
        }
    }
}
