using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWEB_P6.Data.Migrations
{
    public partial class FixTypoInCurso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cursos",
                newName: "Nome");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Cursos",
                newName: "Name");
        }
    }
}
