using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FluxoDeCaixa.Lancamento.ORM.Migrations
{
    /// <inheritdoc />
    public partial class StoreFake : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                    table: "Stores",
                    columns: new[] { "Id", "Name", "Address", "CreatedAt", "UpdatedAt" },
                    values: new object[] {
                "862e08eb-fa5b-4393-9e82-983002538378", "Mercearia Maria", "Av Oliveira Freire nº 265", DateTime.UtcNow, DateTime.UtcNow
                    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM Products where StoreId = '862e08eb-fa5b-4393-9e82-983002538378'");
            migrationBuilder.Sql("DELETE FROM Products where Id = '862e08eb-fa5b-4393-9e82-983002538378'");
        }
    }
}
