using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLevent.Migrations
{
    public partial class SeedRoles : Migration
    {
        private string ManagerRoleId = Guid.NewGuid().ToString();
        private string AdminRoleId = Guid.NewGuid().ToString();
        private string UserRoleId = Guid.NewGuid().ToString();

        private string User1Id = Guid.NewGuid().ToString();
        private string User2Id = Guid.NewGuid().ToString();
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SeedRolesSQL(migrationBuilder);
            SeedUserSQL(migrationBuilder);
            SeedUserRolesSQL(migrationBuilder);
        }
        private void SeedRolesSQL(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"INSERT INTO [dbo].[AspNetRoles] ([Id],[Name],[NormalizedName],[ConcurrencyStamp])
            VALUES ('{AdminRoleId}','Administrator','ADMINISTRATOR',null);");
            migrationBuilder.Sql($@"INSERT INTO [dbo].[AspNetRoles] ([Id],[Name],[NormalizedName],[ConcurrencyStamp])
            VALUES ('{ManagerRoleId}','Manager','MANAGER',null);");
            migrationBuilder.Sql($@"INSERT INTO [dbo].[AspNetRoles] ([Id],[Name],[NormalizedName],[ConcurrencyStamp])
            VALUES ('{UserRoleId}','User','USER',null);");
        }
        private void SeedUserSQL(MigrationBuilder migrationBuilder)
        {
        }
        private void SeedUserRolesSQL(MigrationBuilder migrationBuilder) { }
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
