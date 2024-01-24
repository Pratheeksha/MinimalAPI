using FluentValidation;
using MagicVilla_Coupon.DTO;

namespace MagicVilla_Coupon
{
    public class CouponCreateValidation:AbstractValidator<CouponCreateDTO>
    {
        public CouponCreateValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percentage).InclusiveBetween(1, 100);
        }
    }
}
