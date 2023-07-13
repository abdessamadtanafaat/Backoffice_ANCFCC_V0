using System;
using System.Collections.Generic;

namespace Backoffice_ANCFCC.Models;

public partial class Candidature
{
    public int Id { get; set; }

    public int? CandidatsId { get; set; }

    public int? ConcoursId { get; set; }

    public DateTime? DatePostulation { get; set; }

    public string? Reference { get; set; }

    public string? StatutDoc { get; set; }

    public string? CommentaireDoc { get; set; }

    public string? StatutExp { get; set; }

    public string? CommentaireExp { get; set; }

    public string? StatutInfoPerso { get; set; }

    public string? CommentaireInfoPerso { get; set; }

    public int? AdministrateurId { get; set; }

    public int? AgentId { get; set; }

    public string? CommentaireFormation { get; set; }

    public string? StatutFormation { get; set; }

    public DateTime? DateModification { get; set; }

    public virtual Candidat? Candidats { get; set; }

    public virtual Concour? Concours { get; set; }

    public virtual ICollection<Diplome> Diplomes { get; set; } = new List<Diplome>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Experience> Experiences { get; set; } = new List<Experience>();
}
