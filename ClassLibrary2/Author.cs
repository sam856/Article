using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaqaletyCore
{
    public  class Author
    {
        [Display(Name="المعرف")]
        public int Id { get; set; }

        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; }

        [Display(Name = "معرف المستخدم")]
        public string UserId { get; set; }

        [Display(Name = "الصوره")]
        public  string? PictureImageUrl { get; set; }

        [Display(Name = "السيره الذاتيه")]
        public string? Bio { get; set;}

        [Display(Name = "فيسبوك")]
        public string? FaceBook { get; set; }
        [Display(Name = "انستجرام")]

        

        public string? Instgram { get; set; }
        [Display(Name = "تويتر")]

        public string? Twiter { get; set; }

    }
}
