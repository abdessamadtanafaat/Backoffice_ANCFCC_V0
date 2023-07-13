using System;
using System.Collections.Generic;

namespace Backoffice_ANCFCC.Models;

public partial class ConcoursAttachement
{
    public int Id { get; set; }

    public string? Chemin { get; set; }

    public int? ConcoursId { get; set; }

    public string? Nom { get; set; }

    public virtual Concour? Concours { get; set; }
}
