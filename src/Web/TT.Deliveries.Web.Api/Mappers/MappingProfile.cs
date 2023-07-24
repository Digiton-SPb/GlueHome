using AutoMapper;
using TT.Deliveries.Domain.Dto;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.Web.Api.Mappers;

public class MappingProfile: Profile {
    public MappingProfile() {
        CreateMap<CreateDeliveryDto, Delivery>();
        CreateMap<UpdateDeliveryDto, Delivery>();
        CreateMap<UpdateDeliveryStateDto, Delivery>();

        CreateMap<RecipientDto, Recipient>();

        CreateMap<OrderDto, Order>();

        CreateMap<AccessWindowDto, AccessWindow>();
    }
}