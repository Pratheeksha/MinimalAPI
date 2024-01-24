using MagicVilla_Coupon.Models;

namespace MagicVilla_Coupon.Data
{
    public static class CouponStore
    {
        public static List<Coupon> Coupons = new List<Coupon>()
        {
            new Coupon() { Id = 1, Name = "10OFF", Percentage = 10, IsActive = true },
            new Coupon { Id = 2, Name = "20OFF", Percentage = 20, IsActive = true }
        };
    }
}
