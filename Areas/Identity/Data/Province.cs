using System;
using System.Collections.Generic;

namespace TQuanHome.Areas.Identity.Data;

public partial class Province
{
    public int ProvinceId { get; set; }

    public string? ProvinceName { get; set; }

    public virtual ICollection<District> Districts { get; set; } = new List<District>();

    public virtual ICollection<PostInfo> PostInfos { get; set; } = new List<PostInfo>();
}
