﻿using System;
using System.Collections.Generic;

namespace Backoffice_ANCFCC.Models;

public partial class Administrateur
{
    public int AdministrateurId { get; set; }

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public string? Username { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public string? VerificationToken { get; set; }

    public string? PasswordResetToken { get; set; }

    public DateTime? ResetTokenExpires { get; set; }

    public int? AccessFailedCount { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public bool? IsLockedOut { get; set; }

    public DateTime? LockoutTimestamp { get; set; }

    public DateTime? LastLoginAttemptDate { get; set; }

    public virtual ICollection<Candidature> Candidatures { get; set; } = new List<Candidature>();
}
