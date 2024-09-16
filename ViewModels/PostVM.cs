using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TQuanHome.ViewModels
{
    public class PostVM
    {
        [Required(ErrorMessage = "Tiêu đề tin đăng là bắt buộc")]
        public string PostTitle { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành phố")]
        public int ProvinceId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Quận/Huyện")]
        public int DistrictId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Phường/Xã")]
        public int WardId { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng nhập giá hợp lệ")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ảnh")]

        public IFormFile Image1 { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn ảnh")]
        public IFormFile Image2 { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn ảnh")]
        public IFormFile Image3 { get; set; }
    }
}
