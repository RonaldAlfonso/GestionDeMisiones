using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GestionDeMisiones.Models;

public class Hechicero
{
    [Key]
    public int Id{ get; set; }
    [Required]
    public string Name{ get; set; }

    public EGrados Grado{ get; set; }

    public int Experiencia{ get; set; }

    public EEstado Estado { get; set; }
    [AllowNull]
    public TecnicaMaldita TecnicaPrincipal{ get; set; }



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