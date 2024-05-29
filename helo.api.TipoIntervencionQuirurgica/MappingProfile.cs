
using AutoMapper;
using helo.api.TipoIntervencionQuirurgica.dto;

namespace helo.api.TipoIntervencionQuirurgica;

/// <summary>
/// mappingProfile
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// mapping
    /// </summary>
    public MappingProfile()
    {
        CreateMap<helo.api.TipoIntervencionQuirurgica.dto.TipoIntervencionQuirurgica, TipoIntervencionQuirurgicaResponse>();
    }
}