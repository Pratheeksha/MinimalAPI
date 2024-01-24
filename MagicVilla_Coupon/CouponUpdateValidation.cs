using FluentValidation;
using MagicVilla_Coupon.DTO;

namespace MagicVilla_Coupon
{
    public class CouponUpdateValidation : AbstractValidator<CouponUpdateDTO>
    {
        public CouponUpdateValidation()
        {
            RuleFor(model=>model.Id).GreaterThan(0);
            RuleFor(model=>model.Name).NotEmpty();
            RuleFor(model=>model.Percentage).InclusiveBetween(0, 100);
        }
    }
}
