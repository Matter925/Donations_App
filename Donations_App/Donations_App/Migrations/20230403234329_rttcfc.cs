using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationsApp.Migrations
{
    /// <inheritdoc />
    public partial class rttcfc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Governorates (Name ,EnName) VALUES ('القاهرة', 'Cairo'),('الجيزة', 'Giza'),( 'الأسكندرية', 'Alexandria'),('الدقهلية', 'Dakahlia'),( 'البحر الأحمر', 'Red Sea'),('البحيرة', 'Beheira'),('الفيوم', 'Fayoum'),('الغربية', 'Gharbiya'),('الإسماعلية', 'Ismailia'),('المنوفية', 'Menofia'),('المنيا', 'Minya'),( 'القليوبية', 'Qaliubiya'),('الوادي الجديد', 'New Valley'),('السويس', 'Suez'),('اسوان', 'Aswan'),('اسيوط', 'Assiut'),('بني سويف', 'Beni Suef'),('بورسعيد', 'Port Said'),( 'دمياط', 'Damietta'),( 'الشرقية', 'Sharkia'),('جنوب سيناء', 'South Sinai'),( 'كفر الشيخ', 'Kafr Al sheikh'),('مطروح', 'Matrouh'),('الأقصر', 'Luxor'),('قنا', 'Qena'),('شمال سيناء', 'North Sinai'),( 'سوهاج', 'Sohag'); ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
