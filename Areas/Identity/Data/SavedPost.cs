namespace TQuanHome.Areas.Identity.Data
{
    public class SavedPost
    {
        public int PostId { get; set; }
        public string UserName { get; set; } // Tài khoản của người dùng đăng nhập
        public DateTime SaveDate { get; set; } // Ngày lưu bài đăng

        // Khóa chính
        public virtual PostInfo PostInfo { get; set; }
    }
}
