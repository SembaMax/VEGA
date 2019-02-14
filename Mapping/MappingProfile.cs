using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEFA.Models;
using VEFA.Models.Resources;

namespace VEFA.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Domain To Resource
            CreateMap<Make, MakeResource>();
            CreateMap<Model, ModelResource>();
            CreateMap<Feature,FeatureResource>();
            CreateMap<Vehicle, VehicleResource>()
                /*.ForMember(vr => vr.Contact.Name, opt => opt.MapFrom(v => v.Contact.ContactName))
                .ForMember(vr => vr.Contact.Phone, opt => opt.MapFrom(v => v.Contact.ContactPhone))
                .ForMember(vr => vr.Contact.Email, opt => opt.MapFrom(v => v.Contact.ContactEmail))*/
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));

            // Resource To Domain
            CreateMap<VehicleResource, Vehicle>()
            /*.ForMember(v => v.Contact.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
            .ForMember(v => v.Contact.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
            .ForMember(v => v.Contact.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))*/
            .ForMember(v => v.Features, opt => opt.MapFrom(vr => vr.Features.Select(id => new VehicleFeature { FeatureId = id })));
        }

    }
}
