using System;
using System.Collections.Generic;

namespace final_softwareii_API.Models;

public partial class Voto
{
    public int DpiVotante { get; set; }

    public int? DpiCandidato { get; set; }

    public DateTime HoraVoto { get; set; }

    public string IpVoto { get; set; } = null!;

}
