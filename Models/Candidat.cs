using System;
using System.Collections.Generic;

namespace Backoffice_ANCFCC.Models;

public partial class Candidat
{
    public int Id { get; set; }

    public string? Adresse { get; set; }

    public string? AdresseArabe { get; set; }

    public bool? AvoirQualiteResistant { get; set; }

    public string? Cin { get; set; }

    public string? CodePostal { get; set; }

    public DateTime? DateNaissance { get; set; }

    public string? Email { get; set; }

    public bool? EstHandicape { get; set; }

    public string? Genre { get; set; }

    public string? Matrimoniale { get; set; }

    public string? Nom { get; set; }

    public string? NomArabe { get; set; }

    public string? Prenom { get; set; }

    public string? PrenomArabe { get; set; }

    public string? Telephone1 { get; set; }

    public string? Telephone2 { get; set; }

    public string? Ville { get; set; }

    public bool? EstFonctionnaire { get; set; }

    public bool? AvoirQualiteMilitaire { get; set; }

    public bool? AvoirQualitePupille { get; set; }

    public bool? AvoirQualiteComBattant { get; set; }

    public virtual ICollection<Candidature> Candidatures { get; set; } = new List<Candidature>();
}
