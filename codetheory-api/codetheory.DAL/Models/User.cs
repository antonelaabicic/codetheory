using EntityFrameworkCore.EncryptColumn.Attribute;
using System;
using System.Collections.Generic;

namespace codetheory.DAL.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    [EncryptColumn]
    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    [EncryptColumn]
    public string? FirstName { get; set; }

    [EncryptColumn]
    public string? LastName { get; set; }

    [EncryptColumn]
    public string? ImagePath { get; set; }

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();

    public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
}
