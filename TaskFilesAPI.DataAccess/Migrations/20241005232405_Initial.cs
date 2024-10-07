using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFilesAPI.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "task",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Идентификатор задачи"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование задачи"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Статус выполнения"),
                    RowChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Автор изменения записи"),
                    RowChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата изменения записи"),
                    RowCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Автор создания записи"),
                    RowCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата создания записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task", x => x.TaskId);
                },
                comment: "Задача");

            migrationBuilder.CreateTable(
                name: "file",
                columns: table => new
                {
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Идентификатор файла"),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Идентификатор задачи"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Имя файла"),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата загрузки файла"),
                    Length = table.Column<long>(type: "bigint", nullable: false, comment: "Размер файла"),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Тип контента файла")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file", x => x.FileId);
                    table.ForeignKey(
                        name: "fk_file_relations_task",
                        column: x => x.TaskId,
                        principalTable: "task",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Файл");

            migrationBuilder.CreateIndex(
                name: "IX_file_TaskId",
                table: "file",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "file");

            migrationBuilder.DropTable(
                name: "task");
        }
    }
}
