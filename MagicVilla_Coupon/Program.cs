using AutoMapper;
using FluentValidation;
using MagicVilla_Coupon;
using MagicVilla_Coupon.Data;
using MagicVilla_Coupon.DTO;
using MagicVilla_Coupon.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", (ILogger<Program> logger) => 
{
    APIResponse response= new APIResponse();
    logger.Log(LogLevel.Information,"Retreiving coupons");
    response.IsSuccess = true;
    response.Result = CouponStore.Coupons;
    response.StatusCode = System.Net.HttpStatusCode.OK;
    return Results.Ok(response); 
}).WithName("GetCoupon").Produces<APIResponse>(200);

app.MapGet("/api/coupon/{id:int}", (int id) =>
{
    APIResponse response = new APIResponse();
    response.IsSuccess = true;
    response.Result = CouponStore.Coupons.FirstOrDefault(c => c.Id == id);
    response.StatusCode = System.Net.HttpStatusCode.OK;

    return Results.Ok(response);
}).WithName("GetCouponById").Produces<APIResponse>(200).Produces(400);

app.MapPost("/api/coupon", async (IMapper _mapper,IValidator<CouponCreateDTO> _validator, [FromBody]CouponCreateDTO couponDTO) => {


    APIResponse response = new APIResponse { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
    var validationResult = await _validator.ValidateAsync(couponDTO);
    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }
    if(CouponStore.Coupons.Any(c=>c.Name.ToLower() == couponDTO.Name.ToLower()))
    {
        response.ErrorMessages.Add("coupon already exists!!");
        return Results.BadRequest(response);
    }
    Coupon coupon = _mapper.Map<Coupon>(couponDTO);
    coupon.Id = CouponStore.Coupons.OrderByDescending(c => c.Id)
    .FirstOrDefault().Id + 1;
    CouponStore.Coupons.Add(coupon);
    CouponDTO coupon1 = _mapper.Map<CouponDTO>(coupon);

    response.IsSuccess = true;
    response.Result = coupon1;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);
    //return Results.CreatedAtRoute("GetCoupon",new { Id = coupon1.Id}, coupon1);

}).WithName("CreateCoupon").Produces<APIResponse>(200).Produces(400); 

app.MapPut("/api/coupon",async (IMapper mapper, IValidator<CouponUpdateDTO> validator ,[FromBody]CouponUpdateDTO couponUpdateDTO) => 
{
    APIResponse response = new APIResponse { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    var validatorResult = await validator.ValidateAsync(couponUpdateDTO);
    if (!validatorResult.IsValid)
    {
        response.ErrorMessages.Add(validatorResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }

    Coupon coupon = CouponStore.Coupons.FirstOrDefault(c=>c.Id== couponUpdateDTO.Id);
    coupon.Name = couponUpdateDTO.Name;
    coupon.Percentage = couponUpdateDTO.Percentage;
    coupon.LastUpdated = DateTime.Now;
    coupon.IsActive = couponUpdateDTO.IsActive;

    response.IsSuccess = true;
    response.Result = mapper.Map<CouponDTO>(coupon); ;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);
}).WithName("UpdateCoupon").Accepts<CouponUpdateDTO>("application/json").Produces<APIResponse>(200).Produces(400); ;
app.MapDelete("/api/coupon/{id:int}", (int id) => 
{
    APIResponse response = new APIResponse { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
    Coupon coupon = CouponStore.Coupons.FirstOrDefault(c => c.Id == id);
    if(coupon == null)
    {
        response.ErrorMessages.Add($"Coupon with {id} doesn't exist");
        return Results.BadRequest(response);
    }
    CouponStore.Coupons.Remove(coupon);
    response.IsSuccess = true;
    response.StatusCode= HttpStatusCode.OK;
    return Results.Ok(response);
});

app.UseHttpsRedirection();
app.Run();

