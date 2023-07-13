using System;
using System.Collections.Generic;

namespace Backoffice_ANCFCC.Models;

public partial class Document
{
    public int Id { get; set; }

    public int? CandidatureId { get; set; }

    public string? Chemin { get; set; }

    public string? Nom { get; set; }

    public string? Statut { get; set; }

    public string? Commentaire { get; set; }

    public virtual Candidature? Candidature { get; set; }
}
