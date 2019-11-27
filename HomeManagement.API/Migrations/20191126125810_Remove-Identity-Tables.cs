using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HomeManagement.API.Migrations
{
    public partial class RemoveIdentityTables : Migration
    {
        #region custom

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_UsersSet_UserId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_UsersSet_UserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyExpenses_UsersSet_UserId",
                table: "MonthlyExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_UsersSet_UserId",
                table: "Preferences");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_UsersSet_UserId",
                table: "Reminders");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            //migrate data
            migrationBuilder.Sql("INSERT INTO \"Users\" (\"Email\") SELECT \"Email\" FROM \"UsersSet\"");

            migrationBuilder.DropTable(
                 name: "UsersSet");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Users_UserId",
                table: "Accounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyExpenses_Users_UserId",
                table: "MonthlyExpenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_Users_UserId",
                table: "Preferences",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Users_UserId",
                table: "Reminders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Users_UserId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_UserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyExpenses_Users_UserId",
                table: "MonthlyExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_Users_UserId",
                table: "Preferences");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Users_UserId",
                table: "Reminders");

            migrationBuilder.CreateTable(
                name: "UsersSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersSet", x => x.Id);
                });

            //migrate data
            migrationBuilder.Sql("INSERT INTO \"UsersSet\" (\"Email\") SELECT \"Email\" FROM \"Users\"");

            migrationBuilder.DropTable(
                 name: "Users");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_UsersSet_UserId",
                table: "Accounts",
                column: "UserId",
                principalTable: "UsersSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_UsersSet_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "UsersSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyExpenses_UsersSet_UserId",
                table: "MonthlyExpenses",
                column: "UserId",
                principalTable: "UsersSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_UsersSet_UserId",
                table: "Preferences",
                column: "UserId",
                principalTable: "UsersSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_UsersSet_UserId",
                table: "Reminders",
                column: "UserId",
                principalTable: "UsersSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
        #endregion

        #region auto generated

        //protected override void Up(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Accounts_UsersSet_UserId",
        //        table: "Accounts");

        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Categories_UsersSet_UserId",
        //        table: "Categories");

        //    migrationBuilder.DropForeignKey(
        //        name: "FK_MonthlyExpenses_UsersSet_UserId",
        //        table: "MonthlyExpenses");

        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Preferences_UsersSet_UserId",
        //        table: "Preferences");

        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Reminders_UsersSet_UserId",
        //        table: "Reminders");

        //    migrationBuilder.DropTable(
        //        name: "AspNetRoleClaims");

        //    migrationBuilder.DropTable(
        //        name: "AspNetUserClaims");

        //    migrationBuilder.DropTable(
        //        name: "AspNetUserLogins");

        //    migrationBuilder.DropTable(
        //        name: "AspNetUserRoles");

        //    migrationBuilder.DropTable(
        //        name: "AspNetUserTokens");

        //    migrationBuilder.DropTable(
        //        name: "AspNetRoles");

        //    migrationBuilder.DropTable(
        //        name: "AspNetUsers");

        //    migrationBuilder.DropPrimaryKey(
        //        name: "PK_UsersSet",
        //        table: "UsersSet");

        //    migrationBuilder.RenameTable(
        //        name: "UsersSet",
        //        newName: "Users");

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "WebClients",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Transactions",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "StorageItems",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Reminders",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Preferences",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Notifications",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "MonthlyExpenses",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Currencies",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "ConfigurationSettings",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Categories",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Accounts",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Users",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

        //    migrationBuilder.AddPrimaryKey(
        //        name: "PK_Users",
        //        table: "Users",
        //        column: "Id");

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Accounts_Users_UserId",
        //        table: "Accounts",
        //        column: "UserId",
        //        principalTable: "Users",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Categories_Users_UserId",
        //        table: "Categories",
        //        column: "UserId",
        //        principalTable: "Users",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_MonthlyExpenses_Users_UserId",
        //        table: "MonthlyExpenses",
        //        column: "UserId",
        //        principalTable: "Users",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Preferences_Users_UserId",
        //        table: "Preferences",
        //        column: "UserId",
        //        principalTable: "Users",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Reminders_Users_UserId",
        //        table: "Reminders",
        //        column: "UserId",
        //        principalTable: "Users",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);
        //}

        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Accounts_Users_UserId",
        //        table: "Accounts");

        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Categories_Users_UserId",
        //        table: "Categories");

        //    migrationBuilder.DropForeignKey(
        //        name: "FK_MonthlyExpenses_Users_UserId",
        //        table: "MonthlyExpenses");

        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Preferences_Users_UserId",
        //        table: "Preferences");

        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Reminders_Users_UserId",
        //        table: "Reminders");

        //    migrationBuilder.DropPrimaryKey(
        //        name: "PK_Users",
        //        table: "Users");

        //    migrationBuilder.RenameTable(
        //        name: "Users",
        //        newName: "UsersSet");

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "WebClients",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Transactions",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "StorageItems",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Reminders",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Preferences",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Notifications",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "MonthlyExpenses",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Currencies",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "ConfigurationSettings",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Categories",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "Accounts",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AlterColumn<int>(
        //        name: "Id",
        //        table: "UsersSet",
        //        nullable: false,
        //        oldClrType: typeof(int))
        //        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
        //        .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        //    migrationBuilder.AddPrimaryKey(
        //        name: "PK_UsersSet",
        //        table: "UsersSet",
        //        column: "Id");

        //    migrationBuilder.CreateTable(
        //        name: "AspNetRoles",
        //        columns: table => new
        //        {
        //            Id = table.Column<string>(nullable: false),
        //            ConcurrencyStamp = table.Column<string>(nullable: true),
        //            Name = table.Column<string>(maxLength: 256, nullable: true),
        //            NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_AspNetRoles", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "AspNetUsers",
        //        columns: table => new
        //        {
        //            Id = table.Column<string>(nullable: false),
        //            AccessFailedCount = table.Column<int>(nullable: false),
        //            ConcurrencyStamp = table.Column<string>(nullable: true),
        //            Email = table.Column<string>(maxLength: 256, nullable: true),
        //            EmailConfirmed = table.Column<bool>(nullable: false),
        //            LockoutEnabled = table.Column<bool>(nullable: false),
        //            LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
        //            NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
        //            NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
        //            PasswordHash = table.Column<string>(nullable: true),
        //            PhoneNumber = table.Column<string>(nullable: true),
        //            PhoneNumberConfirmed = table.Column<bool>(nullable: false),
        //            SecurityStamp = table.Column<string>(nullable: true),
        //            TwoFactorEnabled = table.Column<bool>(nullable: false),
        //            UserName = table.Column<string>(maxLength: 256, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_AspNetUsers", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "AspNetRoleClaims",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
        //            ClaimType = table.Column<string>(nullable: true),
        //            ClaimValue = table.Column<string>(nullable: true),
        //            RoleId = table.Column<string>(nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
        //                column: x => x.RoleId,
        //                principalTable: "AspNetRoles",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "AspNetUserClaims",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
        //            ClaimType = table.Column<string>(nullable: true),
        //            ClaimValue = table.Column<string>(nullable: true),
        //            UserId = table.Column<string>(nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
        //                column: x => x.UserId,
        //                principalTable: "AspNetUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "AspNetUserLogins",
        //        columns: table => new
        //        {
        //            LoginProvider = table.Column<string>(nullable: false),
        //            ProviderKey = table.Column<string>(nullable: false),
        //            ProviderDisplayName = table.Column<string>(nullable: true),
        //            UserId = table.Column<string>(nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
        //            table.ForeignKey(
        //                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
        //                column: x => x.UserId,
        //                principalTable: "AspNetUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "AspNetUserRoles",
        //        columns: table => new
        //        {
        //            UserId = table.Column<string>(nullable: false),
        //            RoleId = table.Column<string>(nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
        //            table.ForeignKey(
        //                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
        //                column: x => x.RoleId,
        //                principalTable: "AspNetRoles",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
        //                column: x => x.UserId,
        //                principalTable: "AspNetUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "AspNetUserTokens",
        //        columns: table => new
        //        {
        //            UserId = table.Column<string>(nullable: false),
        //            LoginProvider = table.Column<string>(nullable: false),
        //            Name = table.Column<string>(nullable: false),
        //            Value = table.Column<string>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
        //            table.ForeignKey(
        //                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
        //                column: x => x.UserId,
        //                principalTable: "AspNetUsers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateIndex(
        //        name: "IX_AspNetRoleClaims_RoleId",
        //        table: "AspNetRoleClaims",
        //        column: "RoleId");

        //    migrationBuilder.CreateIndex(
        //        name: "RoleNameIndex",
        //        table: "AspNetRoles",
        //        column: "NormalizedName",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_AspNetUserClaims_UserId",
        //        table: "AspNetUserClaims",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_AspNetUserLogins_UserId",
        //        table: "AspNetUserLogins",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_AspNetUserRoles_RoleId",
        //        table: "AspNetUserRoles",
        //        column: "RoleId");

        //    migrationBuilder.CreateIndex(
        //        name: "EmailIndex",
        //        table: "AspNetUsers",
        //        column: "NormalizedEmail");

        //    migrationBuilder.CreateIndex(
        //        name: "UserNameIndex",
        //        table: "AspNetUsers",
        //        column: "NormalizedUserName",
        //        unique: true);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Accounts_UsersSet_UserId",
        //        table: "Accounts",
        //        column: "UserId",
        //        principalTable: "UsersSet",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Categories_UsersSet_UserId",
        //        table: "Categories",
        //        column: "UserId",
        //        principalTable: "UsersSet",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_MonthlyExpenses_UsersSet_UserId",
        //        table: "MonthlyExpenses",
        //        column: "UserId",
        //        principalTable: "UsersSet",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Preferences_UsersSet_UserId",
        //        table: "Preferences",
        //        column: "UserId",
        //        principalTable: "UsersSet",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Reminders_UsersSet_UserId",
        //        table: "Reminders",
        //        column: "UserId",
        //        principalTable: "UsersSet",
        //        principalColumn: "Id",
        //        onDelete: ReferentialAction.Cascade);
        //}
        #endregion
    }
}
