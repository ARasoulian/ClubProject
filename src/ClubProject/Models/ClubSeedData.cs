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
                    Name = "سوارکاری عقیق",
                    Address = "یزد - اشکذر - انتهای جاده حسن آباد - حسین آباد باشگاه سوارکاری عقیق",
                    Description = "باشگاه سوارکاری عقیق یزد نشأط گرفته از عشق و علاقه مالک این مجموعه می باشد که در زمینی به مساحت 6 هکتار به منظور پرورش و تولید اسبهای تروبرد احداث و با هدف اصلاح نژاد و آماده سازی این نژاد با بهره گیری از کارشناسان داخلی و خارجی و دانش روز دنیا گام در این عرصه بی انتها گذاشت . در مورد سابقه مالک مجموعه ( علی شریف فرد ) در سال 1372 با یک رأس اسب عرب کرد که صرفاً به خاطر علاقه به این حیوان نجیب بود آغاز شد چندی نگذشت که به علت تحصیلات در خارج از کشور مدتی از اسب دور بود . زمان بازگشت در سال 1382 با خرید یک رأس مادیان دو خون به نام گنج آزمون آتش این علاقه داغ تر شد تا اینکه با گذشت پنج سال در سال 1387 تصمیم به احداث باشگاه سوارکاری عقیق گرفته شد . هرچند مالک این مجموعه در گذشته تجربه چندانی در این صنعت نداشت ولی به خواست خدا توانست فعالیت حرفه ای خود را آغاز نمود.\n" +
                                  "باشگاه سوار کاری عقیق در سال 1389 فصل بهاره به صورت رسمی و حرفه ای پای در میادین اسبدوانی ایران گذاشت و با لطف خدا و حمایت دوستان عزیز موفق به ثبت رکورد جدید در مسافت 1000 و 1700 متر ایران شد.\n" +
                                  "امید است به واسطه بار نهادن این تجربیات بتوان شاهد ارتقاء کمی و کیفی صنعت سوارکاری در ایران عزیزمان باشیم.\n",
                    PhoneNumber = "0352-3628180",
                    Pictures = new List<Picture>()
                    {
                        new Picture()
                        {
                            PictureName = "justjenny-banner.jpg",
                            IsActive = true,
                            Order = 0,
                            IsCoverPicture = true
                        },
                        new Picture()
                        {
                            PictureName = "primo-1.jpg",
                            IsActive = true,
                            Order = 1,
                            IsCoverPicture = false
                        },
                        new Picture()
                        {
                            PictureName = "primo-2.jpg",
                            IsActive = true,
                            Order = 2,
                            IsCoverPicture = false
                        },
                        new Picture()
                        {
                            PictureName = "primo-3.jpg",
                            IsActive = true,
                            Order = 3,
                            IsCoverPicture = false
                        },
                        new Picture()
                        {
                            PictureName = "primo-4.jpg",
                            IsActive = false,
                            Order = 4,
                            IsCoverPicture = false
                        },
                        new Picture()
                        {
                            PictureName = "primo-5.jpg",
                            IsActive = false,
                            Order = 5,
                            IsCoverPicture = false
                        },
                        new Picture()
                        {
                            PictureName = "justjenny-1.jpg",
                            IsActive = true,
                            Order = 6,
                            IsCoverPicture = false
                        },
                        new Picture()
                        {
                            PictureName = "justjenny-2.jpg",
                            IsActive = true,
                            Order = 7,
                            IsCoverPicture = false
                        },
                        new Picture()
                        {
                            PictureName = "justjenny-3.jpg",
                            IsActive = true,
                            Order = 8,
                            IsCoverPicture = false
                        }
                    }
                };

                _context.Clubs.Add(club);

                await _context.SaveChangesAsync();
            }
        }
    }
}
