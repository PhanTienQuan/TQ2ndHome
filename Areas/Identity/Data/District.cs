using System;
using System.Collections.Generic;

namespace TQuanHome.Areas.Identity.Data;

public partial class District
{
    public int DistrictId { get; set; }

    public string? DistrictName { get; set; }

    public int? ProvinceId { get; set; }

    public virtual ICollection<PostInfo> PostInfos { get; set; } = new List<PostInfo>();

    public virtual Province? Province { get; set; }

    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
}
