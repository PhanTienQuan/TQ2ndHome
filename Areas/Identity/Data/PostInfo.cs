using System;
using System.Collections.Generic;

namespace TQuanHome.Areas.Identity.Data;

public partial class PostInfo
{
    public int PostId { get; set; }

    public string UserName { get; set; } = null!;

    public int? ProvinceId { get; set; }

    public int? DistrictId { get; set; }

    public int? WardId { get; set; }

    public string AddressDetail { get; set; } = null!;

    public string PostTitle { get; set; } = null!;

    public string? Description { get; set; }

    public int Price { get; set; }

    public DateTime PostDate { get; set; }

    public int Status { get; set; }

    public int IsFull { get; set; }

    public string Img1 { get; set; } = null!;

    public string Img2 { get; set; } = null!;

    public string Img3 { get; set; } = null!;

    public virtual District? District { get; set; }

    public virtual Province? Province { get; set; }

    public virtual Ward? Ward { get; set; }


}
