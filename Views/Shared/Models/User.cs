using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KhumaloCraftKC.Models;

public class User : IdentityUser
{
    public int UserId { get; set; }

    public string Uname { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

  

    public string? UimageUrl { get; set; }

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
