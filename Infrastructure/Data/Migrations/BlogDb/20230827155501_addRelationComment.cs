using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.BlogDb
{
    /// <inheritdoc />
    public partial class addRelationComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_BlogPost_BlogPostId",
                table: "Comment");

            migrationBuilder.AlterColumn<Guid>(
                name: "BlogPostId",
                table: "Comment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_BlogPost_BlogPostId",
                table: "Comment",
                column: "BlogPostId",
                principalTable: "BlogPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_BlogPost_BlogPostId",
                table: "Comment");

            migrationBuilder.AlterColumn<Guid>(
                name: "BlogPostId",
                table: "Comment",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_BlogPost_BlogPostId",
                table: "Comment",
                column: "BlogPostId",
                principalTable: "BlogPost",
                principalColumn: "Id");
        }
    }
}
