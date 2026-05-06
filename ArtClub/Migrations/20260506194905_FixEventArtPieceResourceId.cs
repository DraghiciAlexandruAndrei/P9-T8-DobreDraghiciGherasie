using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtClub.Migrations
{
    /// <inheritdoc />
    public partial class FixEventArtPieceResourceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventArtPieces_ArtPieces_ArtPieceId",
                table: "EventArtPieces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventArtPieces",
                table: "EventArtPieces");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ArtPieceId",
                table: "EventArtPieces",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ResourceId",
                table: "EventArtPieces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventArtPieces",
                table: "EventArtPieces",
                columns: new[] { "EventId", "ResourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_EventArtPieces_ResourceId",
                table: "EventArtPieces",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventArtPieces_ArtPieces_ArtPieceId",
                table: "EventArtPieces",
                column: "ArtPieceId",
                principalTable: "ArtPieces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventArtPieces_Resources_ResourceId",
                table: "EventArtPieces",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventArtPieces_ArtPieces_ArtPieceId",
                table: "EventArtPieces");

            migrationBuilder.DropForeignKey(
                name: "FK_EventArtPieces_Resources_ResourceId",
                table: "EventArtPieces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventArtPieces",
                table: "EventArtPieces");

            migrationBuilder.DropIndex(
                name: "IX_EventArtPieces_ResourceId",
                table: "EventArtPieces");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "EventArtPieces");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ArtPieceId",
                table: "EventArtPieces",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventArtPieces",
                table: "EventArtPieces",
                columns: new[] { "EventId", "ArtPieceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventArtPieces_ArtPieces_ArtPieceId",
                table: "EventArtPieces",
                column: "ArtPieceId",
                principalTable: "ArtPieces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
