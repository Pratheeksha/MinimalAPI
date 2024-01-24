namespace MagicVilla_Coupon.DTO
{
    public class CouponUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }
        public bool IsActive { get; set; }
    }
}
