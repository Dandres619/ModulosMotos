using System;
using System.Collections.Generic;

namespace ModulosTaller.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public string? DiaSemana { get; set; }

    public TimeOnly? HoraInicio { get; set; }

    public TimeOnly? HoraFin { get; set; }

    public virtual ICollection<Agendamiento> Agendamientos { get; set; } = new List<Agendamiento>();
}
