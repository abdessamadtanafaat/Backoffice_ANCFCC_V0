using System;
using System.Collections.Generic;

namespace Backoffice_ANCFCC.Models;

public partial class Diplome
{
    public int Id { get; set; }

    public int? Anne { get; set; }

    public bool? AuMaroc { get; set; }

    public string? AutreDiplome { get; set; }

    public int? CandidatureId { get; set; }

    public bool? EstSimilaire { get; set; }

    public string? Etablissement { get; set; }

    public string? Nom { get; set; }

    public double? Note { get; set; }

    public string? OptionDiplome { get; set; }

    public virtual Candidature? Candidature { get; set; }
}
