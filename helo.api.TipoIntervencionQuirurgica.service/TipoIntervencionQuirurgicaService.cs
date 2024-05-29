using helo.api.TipoIntervencionQuirurgica.dto;
using AutoMapper;
using System.Reflection;
using System.Diagnostics;
using helo.api.TipoIntervencionQuirurgica.repository;

namespace helo.api.TipoIntervencionQuirurgica.service
{
    public interface ITipoIntervencionQuirurgicaService
    {
        Task<List<TipoIntervencionQuirurgicaResponse>> Lista(string TblCdg,int? Codigo);
    }

    public class TipoIntervencionQuirurgicaService : ITipoIntervencionQuirurgicaService
    {
        private readonly IMapper mapper;
        private readonly ITipoIntervencionQuirurgicaRepository _TipoIntervencionQuirurgicaRepository;
        public TipoIntervencionQuirurgicaService(IMapper mapper, ITipoIntervencionQuirurgicaRepository TipoIntervencionQuirurgicaRepository)
        {
            this.mapper = mapper;
            this._TipoIntervencionQuirurgicaRepository = TipoIntervencionQuirurgicaRepository;
        }

        public async Task<List<TipoIntervencionQuirurgicaResponse>> Lista(string TblCdg, int? Codigo)
        {
            List<TipoIntervencionQuirurgicaResponse>? lDatos = null;
            string sMethod = MethodBase.GetCurrentMethod()!.DeclaringType!.FullName!;
            try
            {
                List<TipoIntervencionQuirurgicaResponse> oJson = await this._TipoIntervencionQuirurgicaRepository.Lista(TblCdg, Codigo)!;

                lDatos = mapper.Map<List<TipoIntervencionQuirurgicaResponse>>(oJson);

                return lDatos;
            }
            catch (Exception ex)
            {
                throw new Exception($"[{sMethod}]" + Environment.NewLine + ex.Message);
            }
        }
    }
}