using TQuanHome.Areas.Identity.Data;

namespace TQuanHome.ViewModels
{
    public class PostCompositeVM
    {
        /*public IEnumerable<TQuanHome.Areas.Identity.Data.Province> Provinces { get; set; }
        public TQuanHome.ViewModels.PostVM PostVM { get; set; }*/

        public List<Province> Provinces { get; set; }
        public PostVM PostVM { get; set; }

        public PostCompositeVM()
        {
            Provinces = new List<Province>();
            PostVM = new PostVM();
        }
    }
}
