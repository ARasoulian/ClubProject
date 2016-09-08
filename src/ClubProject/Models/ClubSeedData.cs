using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubProject.Models
{
    public class ClubSeedData
    {
        private readonly ClubContext _context;

        public ClubSeedData(ClubContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedData()
        {
            if (!_context.Clubs.Any())
            {
                var club = new Club()
                {
                    Name = "باشگاه مهران",
                    Address = "کرج،عظیمیه،میدان مهران،جنب بانک تجارت",
                    Pictures = new List<Picture>()
                    {
                        new Picture()
                        {
                            PictureName ="club1.jpg",
                            IsActive = true,
                            Order = 0,
                            IsCoverPicture = false
                        },
                        new Picture()
                        {
                            PictureName ="club2.jpg",
                            IsActive = true,
                            Order = 0,
                            IsCoverPicture = true
                        }
                    }
                };

                _context.Clubs.Add(club);

                await _context.SaveChangesAsync();
            }
        }
    }
}
