using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TQuanHome.Areas.Identity.Data;

// Add profile data for application users by adding properties to the TQuanHomeUser class
public class TQuanHomeUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string Avatar { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FullName { get; set; }

    [PersonalData]
    public DateTime CreateAt { get; set; }


}

