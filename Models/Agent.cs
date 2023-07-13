using System;
using System.Collections.Generic;

namespace Backoffice_ANCFCC.Models;

public partial class Agent
{
    public int AgentId { get; set; }

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string MotDePasse { get; set; } = null!;

    public virtual ICollection<Candidature> Candidatures { get; set; } = new List<Candidature>();
}
