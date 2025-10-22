using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace GestionDeMisiones.Models;

public class Traslado
{
    [Key]
    public int Id{ get; set; }
    [DataType(DataType.DateTime)]
    public DateTime Fecha { get; set; }
    public EEstadoTraslado Estado{ get; set; }
    
    [AllowNull]
    public string Motivo { get; set; }
   
   [Required][DeleteBehavior(DeleteBehavior.Restrict)] 
    public Ubicacion Origen { get; set; }
   [Required][DeleteBehavior(DeleteBehavior.Restrict)]
    public Ubicacion Destino{ get; set; }    

    [Required]
    public int MisionId { get; set; }
    public Mision? Mision { get; set; }






    public enum EEstadoTraslado
    {
        programado,
        en_curso,
        finalizado
    }
} 