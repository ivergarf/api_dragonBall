using AutoMapper;
using Servicios.Core.DTOs;
using Servicios.Core.Entities;


namespace Servicios.Infrastructure.Mappings
{
    internal class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            #region Pokemon
            CreateMap<PersonajeDragonBallresponseDto, PersonajesDragonBallResponseEntity>().ReverseMap();
            #endregion
        }
    }
}