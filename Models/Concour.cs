using System;
using System.Collections.Generic;

namespace Backoffice_ANCFCC.Models;

public partial class Concour
{
    public int Id { get; set; }

    public DateTime? DateExpiration { get; set; }

    public DateTime? DatePublication { get; set; }

    public string? Description { get; set; }

    public string? Nom { get; set; }

    public string? Diplome { get; set; }

    public string? Domaine { get; set; }

    public virtual ICollection<Candidature> Candidatures { get; set; } = new List<Candidature>();

    public virtual ICollection<ConcoursAttachement> ConcoursAttachements { get; set; } = new List<ConcoursAttachement>();
}
