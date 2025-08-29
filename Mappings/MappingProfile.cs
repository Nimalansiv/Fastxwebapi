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
           
            CreateMap<PagenationResponseDTO<Bus>, PagenationResponseDTO<BusDTO>>();
            CreateMap<PagenationResponseDTO<Route>, PagenationResponseDTO<RouteDTO>>();

            CreateMap<PagenationResponseDTO<Payment>, PagenationResponseDTO<PaymentDisplayDTO>>();
            CreateMap<PagenationResponseDTO<User>, PagenationResponseDTO<UserDTO>>();


            CreateMap<PagenationResponseDTO<Seat>, PagenationResponseDTO<SeatDTO>>();
            CreateMap<PagenationResponseDTO<Booking>, PagenationResponseDTO<BookingResponseDTO>>();
            CreateMap<PagenationResponseDTO<Refund>, PagenationResponseDTO<RefundDTO>>();
            CreateMap<User, RegisterUserResponseDTO>();

            
            CreateMap<RegisterUserRequestDTO, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.ContactNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            
            CreateMap<Bus, BusDTO>().ReverseMap();

            CreateMap<Seat, SeatDTO>().ReverseMap();
            CreateMap<Refund, RefundDTO>().ReverseMap();

            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<Payment, PaymentDisplayDTO>();
            CreateMap<BookingRequestDTO, Booking>();


            
            CreateMap<Route, RouteDTO>()
                .ForMember(dest => dest.BusName, opt => opt.MapFrom(src => src.Bus.BusName))
                .ForMember(dest => dest.BusType, opt => opt.MapFrom(src => src.Bus.BusType))
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.Bus.Amenities));

            CreateMap<RouteDTO, Route>()
                .ForMember(dest => dest.Bus, opt => opt.Ignore());

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));

            CreateMap<Booking, BookingResponseDTO>()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Route.Origin))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Route.Destination))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.Route.DepartureTime));

            CreateMap<User, LoginResponseDTO>();

            CreateMap<UpdateUserDTO, User>()
                .ForMember(dest => dest.BusId, opt => opt.Ignore());

            CreateMap<BookingRequestDTO, Booking>()
                .ForMember(dest => dest.BookingSeats, opt => opt.Ignore())
                .ForMember(dest => dest.Route, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }

}
