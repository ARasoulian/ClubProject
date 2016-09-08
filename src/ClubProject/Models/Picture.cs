using System.ComponentModel.DataAnnotations;

namespace ClubProject.Models
{
    public class Picture
    {
        [Key]
        public int PictureId { get; set; }

        [Required]
        public string PictureName { get; set; }

        [Required]
        public int Order { get; set; }

        public bool IsCoverPicture { get; set; }

        public bool IsActive { get; set; }

        public Club Club { get; set; }
    }
}
