using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationsApp.Migrations
{
    /// <inheritdoc />
    public partial class addData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Governorates ( Name ) VALUES ('القاهرة'),('الجيزة'),('الأسكندرية''),('الدقهلية'),('البحر الأحمر'),('البحيرة'),('الفيوم'),('الغربية'),('الإسماعلية'),('المنوفية'),('المنيا'),('القليوبية'),('الوادي الجديد'),('السويس'), ('اسوان'),('اسيوط'),('بني سويف'),('بورسعيد'),('دمياط'),('الشرقية'),( 'جنوب سيناء'),('كفر الشيخ'),('مطروح'),('الأقصر'),('قنا'),('شمال سيناء'),( 'سوهاج')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
