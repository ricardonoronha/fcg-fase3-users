using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FIAP.MicroService.Usuario.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DataAtualizacao", "DataCriacao", "Email", "Password", "Role", "Username", "UsuarioAtualizador", "UsuarioCriador" },
                values: new object[,]
                {
                    { new Guid("11111111-9c51-42b2-86bd-701c9f61ddca"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 7, 3, 17, 37, 330, DateTimeKind.Utc).AddTicks(6369), "user1@email.com", "password1", "admin", "user1", "", "" },
                    { new Guid("9f4ab7ce-9c51-42b2-86bd-701c9f61ddca"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 7, 3, 17, 37, 330, DateTimeKind.Utc).AddTicks(6279), "admin@email.com", "adminpassword", "admin", "admin", "", "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-9c51-42b2-86bd-701c9f61ddca"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9f4ab7ce-9c51-42b2-86bd-701c9f61ddca"));
        }
    }
}
