using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MaqaletyCore
{
    public class Category
    {
        [Required]
        [Display(Name = "المعرف")]
        
        public int Id { get; set; }
        [Required(ErrorMessage ="هذا الحقل مطلوب")]
        [MaxLength(50,ErrorMessage ="اعلي قيمه للادخال هي 50 حرف")]
        [MinLength(2,ErrorMessage ="اقل قيمه للادخال هي حرفان")]
        [DataType(DataType.Text)]
        [Display(Name = "اسم الصنف")]
        public string Name { get; set; }
         
        public List< AuthorPost> AuthorPosts { get; set; }
    }
}
