using System;
using System.Collections.Generic;

namespace final_softwareii_API.Models;

public partial class Candidato
{
    public int Dpi { get; set; }

    public string? NombreCompleto { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public string? PartidoPolitico { get; set; }

}
