using System.ComponentModel.DataAnnotations;

namespace GestionDeMisiones.Models;

public class Hechicero
{
    [Key]
    public int idH{ get; set; }
    [Required]
    public string name{ get; set; }

    public EGrados grado{ get; set; }

    public int experiencia{ get; set; }

    public EEstado estado{ get; set; }



    public enum EEstado
    {
        activo,
        lesionado,
        recuperandose,
        baja,
        inactivo
    }


    public enum EGrados
    {
        estudiante,
        aprendiz,
        medio,
        alto,
        especial

    }
}