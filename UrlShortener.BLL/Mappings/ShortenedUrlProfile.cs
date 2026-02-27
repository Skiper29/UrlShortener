using AutoMapper;
using UrlShortener.BLL.DTOs.Urls;
using UrlShortener.DAL.Entities;

namespace UrlShortener.BLL.Mappings;

public class ShortenedUrlProfile : Profile
{
    public ShortenedUrlProfile()
    {
        CreateMap<ShortenedUrl, UrlResponse>()
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.UserName))
            .ForMember(dest => dest.ShortUrl, opt => opt.Ignore());

        CreateMap<ShortenedUrl, UrlDetailResponse>()
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedBy.UserName))
            .ForMember(dest => dest.CreatedByEmail, opt => opt.MapFrom(src => src.CreatedBy.Email))
            .ForMember(dest => dest.ShortUrl, opt => opt.Ignore());
    }
}
