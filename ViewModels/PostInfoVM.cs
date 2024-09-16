namespace TQuanHome.ViewModels
{
    public class PostInfoVM
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public decimal Price { get; set; }
        public DateTime PostDate { get; set; }
        public string AddressDetail { get; set; }
        public int Status { get; set; }
        public string Img1 { get; set; }
        public string Img2 { get; set; }
        public string Img3 { get; set; }

        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
    }
}
