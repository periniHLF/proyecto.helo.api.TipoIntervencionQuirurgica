
using helo.api.TipoIntervencionQuirurgica.dto;
using helo.lib.context.json;
using helo.lib.helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace helo.api.TipoIntervencionQuirurgica.repository
{
    public interface ITipoIntervencionQuirurgicaRepository
    {

        Task<List<TipoIntervencionQuirurgicaResponse>> Lista(string TblCdg, int? Codigo);
    }

    public class TipoIntervencionQuirurgicaRepository : ITipoIntervencionQuirurgicaRepository
    {
        private readonly IConexionOracleContextJson _conexion;
        private readonly IConfiguracionHelper _configuracion;
        public TipoIntervencionQuirurgicaRepository(
            IConexionOracleContextJson oConn,
            IConfiguracionHelper configuraciones
            )
        {
            this._conexion = oConn;
            this._configuracion = configuraciones;
        }

        public async Task<List<TipoIntervencionQuirurgicaResponse>> Lista(string TblCdg, int? Codigo)
        {
            List<TipoIntervencionQuirurgicaResponse>? oDatos = null;
            string sMethod = MethodBase.GetCurrentMethod()!.DeclaringType!.FullName!;
            try
            {
                string sProcedimiento = this._configuracion.ObtieneProcedimientoAlmacenado("Procedimientos:Listar");
                List<paramSQL> lParametros = new List<paramSQL>(){
                new paramSQL(){ Nombre = "i_TblCdg", Tipo = (int)TipoOracle.Varchar2 , Direccion = (int)DireccionParametro.input, Valor = TblCdg.ToString() },
                  new paramSQL(){ Nombre = "i_ParCdg", Tipo = (int)TipoOracle.Int32 , Direccion = (int)DireccionParametro.input, Valor = Codigo.ToString() },
                new paramSQL(){ Nombre = "o_Cursor", Tipo = (int)TipoOracle.RefCursor , Direccion = (int)DireccionParametro.output }
            };

                List<registroSQL>? rDatos = await this._conexion.EjecutarProcedimiento(sProcedimiento, lParametros)!;
                oDatos = new List<TipoIntervencionQuirurgicaResponse>();
                foreach (registroSQL record in rDatos)
                {
                    oDatos.Add(
                         new TipoIntervencionQuirurgicaResponse()
                         {
                             Codigo = record!.Campos!.Find(x => x.Nombre == TblCdg.ToUpper() + "_CDG")!.Valor,
                             Descripcion = record!.Campos!.Find(x => x.Nombre == TblCdg.ToUpper() + "_DSC")!.Valor
                         }
                     );
                }

                return oDatos!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{sMethod}]" + Environment.NewLine + ex.Message);
                throw new Exception($"[{sMethod}]" + Environment.NewLine + ex.Message);
            }
        }
    }
}