using AutoMapper;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Models;
using Route = FastxWebApi.Models.Route;

namespace FastxWebApi.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
           
            
            CreateMap<RegisterDTO, User>()
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));

            
            CreateMap<User, UpdateUserDTO>().ReverseMap();


           
            CreateMap<Bus, BusDTO>().ReverseMap();

           
            CreateMap<Route, RouteDTO>()
                    .ForMember(dest => dest.BusName, opt => opt.MapFrom(src => src.Bus.BusName))
                    .ForMember(dest => dest.BusType, opt => opt.MapFrom(src => src.Bus.BusType))
                    .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.Bus.Amenities));

           
            CreateMap<RouteDTO, Route>()
                .ForMember(dest => dest.Bus, opt => opt.Ignore());


            CreateMap<BookingRequestDTO, Booking>()
                    .ForMember(dest => dest.BookingSeats, opt => opt.Ignore())
                    .ForMember(dest => dest.Route, opt => opt.Ignore())
                    .ForMember(dest => dest.User, opt => opt.Ignore());

            
            CreateMap<Booking, BookingResponseDTO>()
                    .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Route.Origin))
                    .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Route.Destination))
                    .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.Route.DepartureTime));

           
            CreateMap<Refund, RefundDTO>().ReverseMap();

            
            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<Payment, PaymentDisplayDTO>();

           
            CreateMap<Seat, SeatDTO>().ReverseMap();


        }







    }
}
