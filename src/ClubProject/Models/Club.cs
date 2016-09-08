using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClubProject.Models
{
    public class Club
    {
        [Key]
        public int ClubId { get; set; }

        [Display(Name = "نام باشگاه")]
        [Required(ErrorMessage = "لطفا برای باشگاه نام انتخاب کنید.")]
        [StringLength(20,MinimumLength = 2,ErrorMessage = "نام باشگاه باید حداقل 2 کاراکتر و حداکثر 20 کاراکتر باشد.")]
        public string Name { get; set; }

        [Display(Name = "آدرس باشگاه")]
        [MaxLength(150,ErrorMessage = "آدرس باشگاه حداکثر می تواند 150 کاراکتر باشد.")]
        public string Address { get; set; }

        [Display(Name = "درباره باشگاه")]
        public string Description { get; set; }

        [Display(Name = "شماره تلفن")]
        public string PhoneNumber { get; set; }

        [Display(Name = "تصاویر")]
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
