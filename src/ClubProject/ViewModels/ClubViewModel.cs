using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubProject.Models;

namespace ClubProject.ViewModels
{
    public class ClubViewModel
    {
        [Required(ErrorMessage = "لطفا برای باشگاه نام انتخاب کنید.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "نام باشگاه باید حداقل 2 کاراکتر و حداکثر 20 کاراکتر باشد.")]
        public string Name { get; set; }
        
        [MaxLength(150, ErrorMessage = "آدرس باشگاه حداکثر می تواند 150 کاراکتر باشد.")]
        public string Address { get; set; }

        //public string DefaultPicture { get; set; }

        public string Description { get; set; }
        
        public string PhoneNumber { get; set; }

        public ICollection<Picture> Pictures { get; set; }
    }
}
