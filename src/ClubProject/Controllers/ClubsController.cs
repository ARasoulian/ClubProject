using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClubProject.Models;
using ClubProject.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.IO;
using ClubProject.Extention_Methods;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Routing;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ClubProject.Controllers
{
    public class ClubsController : Controller
    {
        private readonly ClubContext _context;
        private readonly ILogger<ClubContext> _logger;
        private readonly IHostingEnvironment _env;

        public ClubsController(ClubContext context, ILogger<ClubContext> logger,IHostingEnvironment env)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var clubs = _context.Clubs.Include(c => c.Pictures).ToList();
            return View(clubs);
        }

        [HttpGet]
        public async Task<IActionResult> Show(int? id)
        {
            if (id == null) return RedirectToAction("Error", "Home");

            var club = await _context.Clubs.Include(c => c.Pictures).SingleOrDefaultAsync(c => c.ClubId == id);

            if (club == null) return RedirectToAction("Error", "Home");

            return View(club);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClubViewModel clubViewModel, IFormFile coverPicture, IList<IFormFile> galleryPictures)
        {
            if (!ModelState.IsValid) return View();

            List<Picture> galleryPicList = new List<Picture>();

            if (galleryPictures != null)
            {
                foreach (var file in galleryPictures)
                {
                    if (!file.FileName.EndsWith(".jpg")) continue;

                    string pureFileName = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);
                    StringValues orderValue;
                    var getOrderSucceed = Request.Form.TryGetValue(pureFileName + "Order", out orderValue);
                    StringValues isActiveValue;
                    var getIsActiveSucceed = Request.Form.TryGetValue(pureFileName + "IsActive", out isActiveValue);

                    galleryPicList.Add(new Picture
                    {
                        IsActive = getIsActiveSucceed,
                        PictureName = pureFileName,
                        IsCoverPicture = false,
                        Order = getOrderSucceed ? int.Parse(orderValue) : 0
                    });
                }

                var orderGroup = galleryPicList.GroupBy(p => p.Order);
                if (orderGroup.Any(@group => @group.Count() > 1))
                {
                    ModelState.AddModelError("", "شما نمیتوانید چند عکس با اولویت یکسان وارد کنید");
                }
            }

            if (!ModelState.IsValid) return View();

            if(coverPicture != null && coverPicture.FileName.EndsWith(".jpg"))
            {
                galleryPicList.Add(new Picture()
                {
                    IsActive = true,
                    Order = 0,
                    IsCoverPicture = true,
                    PictureName = Guid.NewGuid()+".jpg"
                });
            }

            if (coverPicture == null)
            {
                galleryPicList.Add(new Picture()
                {
                    IsActive = true,
                    Order = 0,
                    IsCoverPicture = true,
                    PictureName = "no-photo.jpg"
                });
            }

            try
            {
                Club newClub = Mapper.Map<Club>(clubViewModel);
                newClub.Pictures = galleryPicList;

                if (galleryPictures != null)
                {
                    foreach (IFormFile picture in galleryPictures)
                    {
                        var fileName = picture.FileName.Substring(picture.FileName.LastIndexOf("\\") + 1);

                        if (fileName.EndsWith(".jpg"))
                        {
                            var name = fileName;
                            var galleryPic = galleryPicList.Find(p => p.PictureName == name);
                            galleryPic.PictureName = fileName = Guid.NewGuid() + ".jpg";
                            var filePath = _env.WebRootPath + "\\pictures\\" + fileName;
                            await picture.SaveAsAsync(filePath);
                        }
                    }
                }

                if (coverPicture != null)
                {
                    var coverFileName =
                        coverPicture.FileName.Substring(coverPicture.FileName.LastIndexOf("\\") + 1);

                    if (coverFileName.EndsWith(".jpg"))
                    {
                        coverFileName = galleryPicList.Find(p => p.IsCoverPicture == true).PictureName;
                        var filePath = _env.WebRootPath + "\\pictures\\" + coverFileName;
                        await coverPicture.SaveAsAsync(filePath);
                    }
                }

                _context.Clubs.Add(newClub);
                await _context.SaveChangesAsync();
                ViewBag.SaveSucceed = "yes";
                ModelState.Clear();
            }
            catch (Exception e)
            {
                ViewBag.SaveSucceed = "no";
                _logger.LogError($"Failed to save new Club : {e}");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var club = await _context.Clubs.Include(c=>c.Pictures).Where(c => c.ClubId == id).SingleOrDefaultAsync();

            if (club == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var clubViewModel = Mapper.Map<ClubViewModel>(club);

            return View(clubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClubViewModel clubViewModel, IFormFile coverPicture, IList<IFormFile> galleryPictures)
        {
            if (!ModelState.IsValid) return View(clubViewModel);

            try
            {
                var id = RouteData.Values["id"];
                int clubId;
                var result = int.TryParse(id.ToString(), out clubId);
                if(!result) throw new Exception("Cannot parse id in clubs editing. id= " + id.ToString());

                var club = await _context.Clubs.Include(c=>c.Pictures).SingleOrDefaultAsync(c => c.ClubId == clubId);
                if(club == null)
                    throw new Exception("cannot found club with id =" + id);

                club.Address = clubViewModel.Address;
                club.Description = clubViewModel.Description;
                club.PhoneNumber = clubViewModel.PhoneNumber;
                club.Name = clubViewModel.Name;

                var oldGalleryPictures = club.Pictures.Where(c => !c.IsCoverPicture);
                foreach (var oldPic in oldGalleryPictures)
                {
                    StringValues orderValue;
                    var pictureExist = Request.Form.TryGetValue(oldPic.PictureName + "Order", out orderValue);
                    if (!pictureExist) //اگر تصویر موجود نبود بیا حذفش کن و ادامه بده
                    {
                        _context.Pictures.Remove(oldPic); //حذف از دیتابیس
                        System.IO.File.Delete(_env.WebRootPath + "\\pictures\\" + oldPic.PictureName); //حذف فایل
                        continue;
                    }
                    StringValues isActiveValue;
                    var isActive = Request.Form.TryGetValue(oldPic.PictureName + "IsActive", out isActiveValue);
                    oldPic.IsActive = isActive;
                    oldPic.Order = int.Parse(orderValue);
                }

                //اضافه کردن عکس های جدید به گالری
                if (galleryPictures != null)
                {
                    foreach (IFormFile picture in galleryPictures)
                    {
                        var fileName = picture.FileName.Substring(picture.FileName.LastIndexOf("\\") + 1);

                        if (fileName.EndsWith(".jpg"))
                        {
                            StringValues isActiveValue;
                            StringValues orderValue;
                            Request.Form.TryGetValue(fileName + "Order", out orderValue);
                            var newPicture = new Picture
                            {
                                PictureName = Guid.NewGuid() + ".jpg",
                                IsCoverPicture = false,
                                IsActive = Request.Form.TryGetValue(fileName + "IsActive", out isActiveValue),
                                Order = int.Parse(orderValue)
                            };
                            club.Pictures.Add(newPicture);
                            var filePath = _env.WebRootPath + "\\pictures\\" + newPicture.PictureName;
                            await picture.SaveAsAsync(filePath);
                        }
                    }
                }

                if (coverPicture != null) //عکس کاور قبلی حذف جدید جایگذین آن میشود
                {

                    if (coverPicture.FileName.EndsWith(".jpg"))
                    {
                        var coverPic = club.Pictures.SingleOrDefault(c => c.IsCoverPicture);
                        if (coverPic != null)
                        {
                            System.IO.File.Delete(_env.WebRootPath + "\\pictures\\" + coverPic.PictureName);
                            coverPic.PictureName = Guid.NewGuid().ToString() + ".jpg";
                        }
                        else
                        {
                            coverPic = new Picture()
                            {
                                PictureName = Guid.NewGuid()+".jpg",
                                IsActive = true,
                                Order = 0,
                                IsCoverPicture = true
                            };
                            club.Pictures.Add(coverPic);
                        }
                        var filePath = _env.WebRootPath + "\\pictures\\" + coverPic.PictureName;
                        await coverPicture.SaveAsAsync(filePath);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Clubs");
            }
            catch (Exception e)
            {
                ViewBag.SaveSucceed = "no";
                _logger.LogError($"Failed to edit Club : {e}");
            }

            return View(clubViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return View();
        }
    }
}
