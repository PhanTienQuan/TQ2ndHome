using System;
using System.Collections.Generic;

namespace TQuanHome.Areas.Identity.Data;

public partial class Ward
{
    public int WardId { get; set; }

    public string? WardName { get; set; }

    public int? DistrictId { get; set; }

    public virtual District? District { get; set; }

    public virtual ICollection<PostInfo> PostInfos { get; set; } = new List<PostInfo>();
}
