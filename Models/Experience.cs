using System;
using System.Collections.Generic;

namespace Backoffice_ANCFCC.Models;

public partial class Experience
{
    public int Id { get; set; }

    public int? CandidatureId { get; set; }

    public string? DomaineActivite { get; set; }

    public int? NombreAnnee { get; set; }

    public string? Poste { get; set; }

    public virtual Candidature? Candidature { get; set; }
}
