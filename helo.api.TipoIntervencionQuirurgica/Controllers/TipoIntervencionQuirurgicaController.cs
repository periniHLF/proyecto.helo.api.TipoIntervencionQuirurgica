
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using helo.lib.helpers;
using helo.api.TipoIntervencionQuirurgica.service;
using helo.api.TipoIntervencionQuirurgica.dto;

namespace helo.api.Controllers;

/// <summary>
/// Controllador TipoIntervencionQuirurgica
/// </summary>
[ApiController]
[Route("api/parametricos")]
[Produces("application/json")]
[Authorize]
public class TipoIntervencionQuirurgicaController : ControllerBase
{

    private readonly ITipoIntervencionQuirurgicaService _TipoIntervencionQuirurgicaService;
    private readonly IConfiguracionHelper _configuraciones;
    /// constructor
    public TipoIntervencionQuirurgicaController(
        ITipoIntervencionQuirurgicaService TipoIntervencionQuirurgicaService,
        IConfiguracionHelper configuraciones
        )
    {
        this._TipoIntervencionQuirurgicaService = TipoIntervencionQuirurgicaService;
        this._configuraciones = configuraciones;
    }

    /// <summary>
    /// Metodo que permite obtener parametria de TipoIntervencionQuirurgica
    /// </summary>
    ///<remarks>
    /// Este metodo permite obtener parametria de TipoIntervencionQuirurgica
    ///
    ///</remarks>
    /// <param name="Peticion">Estructura de peticion</param>
    ///<response code="200">
    /// Objeto de respuesta
    ///
    ///     Estado             : Indicador de respuesta correcta o incorrecta
    ///     Mensaje            : Mensaje descriptivo de la respuesta
    ///     CantidadRegistros  : Indicador de cantidad de registros retornados en objeto datos
    ///     Error
    ///         Codigo         : Codigo del error
    ///         Capa           : Capa donde ocurrio el error
    ///         Mensaje        : Mensaje descriptivo del error
    ///     Datos
    ///         : 
    ///         : 
    ///         : 
    ///</response>
    [HttpGet("v1/TipoIntervencionQuirurgica")]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<IActionResult> TipoIntervencionQuirurgica([FromQuery] TipoIntervencionQuirurgicaRequest Peticion)
    {
        string nombreTabla = "TPIQ";
        int? codigo = Peticion.Codigo;
        string sMethod = MethodBase.GetCurrentMethod()!.DeclaringType!.FullName!;
        try
        {
            var oRespuesta = await this._TipoIntervencionQuirurgicaService.Lista(nombreTabla, codigo);
            return Ok(
                new JsonRespuesta()
                {
                    Estado = (oRespuesta != null),
                    Mensaje = (oRespuesta !.Count == 0 ? this._configuraciones.ObtieneError(100) : (this._configuraciones.ObtieneError(101))),
                    CantidadRegistros = (oRespuesta != null ? oRespuesta!.Count : 0),
                    Datos = oRespuesta
                });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new JsonRespuesta()
                {
                    Mensaje = this._configuraciones.ObtieneError(102),
                    Error = $"[{sMethod}]" + Environment.NewLine + ex.Message
                }
            );
        }
    }
}