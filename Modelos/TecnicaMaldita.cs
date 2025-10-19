using System.ComponentModel.DataAnnotations;

namespace GestionDeMisiones.Models;

public class TecnicaMaldita
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nombre { get; set; }
    [Required]
    public ETipoTecnica Tipo { get; set; }
    public int EfectividadProm { get; set; } = 0;
    public string CondicionesDeUso { get; set; } = "ninguna";


    public enum ETipoTecnica
    {
        amplificacion,
        dominio,
        restriccion,
        soporte
    }
}