using AutoMapper;
using MagicVilla_Coupon.DTO;
using MagicVilla_Coupon.Models;

namespace MagicVilla_Coupon
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon,CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon, CouponDTO>().ReverseMap();
        }
    }
}
