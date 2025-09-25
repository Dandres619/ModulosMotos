using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModulosTaller.Models;

public partial class Agendamiento
{
    [Key]
    public int IdAgendamiento { get; set; }

    public DateTime Fecha { get; set; }

    public int? IdCliente { get; set; }
    public int? IdMoto { get; set; }
    public int? IdHorario { get; set; }

    [MaxLength(200)]
    public string? Descripcion { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }
    public virtual Horario? IdHorarioNavigation { get; set; }
    public virtual Motocicleta? IdMotoNavigation { get; set; }
}