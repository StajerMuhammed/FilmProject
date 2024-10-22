using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Film.Migrations
{
    public partial class UpdateYonetmenBirtDayToDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Geçici alan ekle
            migrationBuilder.AddColumn<DateTime>(
                name: "TempBirtDay",
                table: "Yonetmens",
                nullable: true);

            // Mevcut int değerleri datetime türüne dönüştür
            migrationBuilder.Sql(
                "UPDATE Yonetmens SET TempBirtDay = DATEADD(YEAR, BirtDay, '1900-01-01')");

            // Orijinal BirtDay alanını kaldır
            migrationBuilder.DropColumn(
                name: "BirtDay",
                table: "Yonetmens");

            // Yeni datetime türünde BirtDay alanını ekle
            migrationBuilder.AddColumn<DateTime>(
                name: "BirtDay",
                table: "Yonetmens",
                nullable: false,
                defaultValue: DateTime.Now);

            // Geçici alandaki veriyi yeni BirtDay alanına aktar
            migrationBuilder.Sql(
                "UPDATE Yonetmens SET BirtDay = TempBirtDay");

            // Geçici alanı kaldır
            migrationBuilder.DropColumn(
                name: "TempBirtDay",
                table: "Yonetmens");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri alma işlemi: Önce datetime alanını kaldır
            migrationBuilder.DropColumn(
                name: "BirtDay",
                table: "Yonetmens");

            // Ardından int türünde BirtDay alanını ekle
            migrationBuilder.AddColumn<int>(
                name: "BirtDay",
                table: "Yonetmens",
                nullable: false,
                defaultValue: 0); // Varsayılan değer

            // Geçici alanın geri alınmasını sağla
            migrationBuilder.AddColumn<DateTime>(
                name: "TempBirtDay",
                table: "Yonetmens",
                nullable: true);

            // Orijinal alanı datetime'dan int'e geri döndür
            migrationBuilder.Sql(
                "UPDATE Yonetmens SET BirtDay = DATEDIFF(YEAR, '1900-01-01', TempBirtDay)");

            // Geçici alanı kaldır
            migrationBuilder.DropColumn(
                name: "TempBirtDay",
                table: "Yonetmens");
        }
    }
}
