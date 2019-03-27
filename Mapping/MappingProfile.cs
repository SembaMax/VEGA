using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEFA.Core.Models;
using VEFA.REST.Resources;

namespace VEFA.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Domain To Resource
            CreateMap<Make, MakeResource>();
            CreateMap<Model, KeyValuePairResource>();
            CreateMap<Feature,KeyValuePairResource>();
            CreateMap<Vehicle, SaveVehicleResource>()
                /*.ForMember(vr => vr.Contact.Name, opt => opt.MapFrom(v => v.Contact.ContactName))
                .ForMember(vr => vr.Contact.Phone, opt => opt.MapFrom(v => v.Contact.ContactPhone))
                .ForMember(vr => vr.Contact.Email, opt => opt.MapFrom(v => v.Contact.ContactEmail))*/
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));
            CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr => vr.Make, opt => opt.MapFrom(v => v.Model.Make))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(f => new KeyValuePairResource { Id = f.FeatureId, Name = f.Feature.Name })));

            // Resource To Domain
            CreateMap<SaveVehicleResource, Vehicle>()
            /*.ForMember(v => v.Contact.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
            .ForMember(v => v.Contact.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
            .ForMember(v => v.Contact.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))*/
            .ForMember(v => v.Features, opt => opt.Ignore())
            .AfterMap((vr, v) => {
                //Remove Unselected Features
                var removedFeatures = v.Features.Where(f => !vr.Features.Any(id => id == f.FeatureId));
                foreach (VehicleFeature f in removedFeatures)
                    v.Features.Remove(f);

                //Add New Features
                var addedFeatures = vr.Features.Where(id => !v.Features.Any(f => f.FeatureId == id)).Select(id => new VehicleFeature { FeatureId = id });
                foreach (VehicleFeature f in addedFeatures)
                    v.Features.Add(f);

            })
            ;
        }

    }
}
