using AutoMapper;
using UrlShortener.BLL.DTOs.About;
using UrlShortener.DAL.Entities;

namespace UrlShortener.BLL.Mappings;

public class AboutContentProfile : Profile
{
    public AboutContentProfile()
    {
        CreateMap<AboutContent, AboutContentResponse>();
    }
}
