namespace helo.api.TipoIntervencionQuirurgica.dto;

public class JsonRespuesta
{
    public bool Estado { get; set; } = false;
    public string Mensaje { get; set; } = string.Empty;
    public int CantidadRegistros { get; set; } = 0;
    public string? Error { get; set; } = null;
    public dynamic? Datos { get; set; } = null;
}