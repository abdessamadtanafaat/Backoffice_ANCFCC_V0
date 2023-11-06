using System;
using System.Collections.Generic;

namespace Backoffice_ANCFCC.Models;

public partial class Candidature
{
    public int Id { get; set; }

    public string? Reference { get; set; }

    public int? CandidatsId { get; set; }

    public int? ConcoursId { get; set; }

    public DateTime? DatePostulation { get; set; }

    public int? StatutDoc { get; set; }

    public string? CommentaireDoc { get; set; }

    public int? StatutExp { get; set; }

    public string? CommentaireExp { get; set; }

    public int? StatutInfoPerso { get; set; }

    public string? CommentaireInfoPerso { get; set; }

    public string? CommentaireFormation { get; set; }

    public int? StatutFormation { get; set; }

    public DateTime? DateModification { get; set; }

    public int? StatusGlobal { get; set; }

    public int? StatusAgent { get; set; }

    public string? Message { get; set; }

    public bool Verrouille { get; set; }

    public int? UserId { get; set; }

    public string? TreatedByAgent { get; set; }

    public string? TreatedByAdmin { get; set; }

    public string? AgentId { get; set; }

    public string? AdminId { get; set; }

    public string? AssignedTo { get; set; }

    public byte[]? RowVersion { get; set; }

    public string? AssignedToAdmin { get; set; }

    public bool? IsTraitedByAgent { get; set; }

    public bool? IsTraitedByAdmin { get; set; }

    public virtual Candidat? Candidats { get; set; }

    public virtual Concour? Concours { get; set; }

    public virtual ICollection<Diplome> Diplomes { get; set; } = new List<Diplome>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Experience> Experiences { get; set; } = new List<Experience>();

    public virtual User? User { get; set; }
}
