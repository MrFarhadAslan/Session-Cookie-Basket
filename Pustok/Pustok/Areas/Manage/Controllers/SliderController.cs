using Microsoft.AspNetCore.Mvc;
using Pustok.Helpers;
using Pustok.Models;
using System.Text.RegularExpressions;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private DataContext _dataContext { get; }

        private IWebHostEnvironment _env;

        public SliderController(DataContext dataContext,IWebHostEnvironment env)
        {
            _dataContext = dataContext;
            _env = env;
        }


        
        public IActionResult Index()
        {
            List<Slider> sliderList = _dataContext.Sliders.ToList();
            return View(sliderList);
        }

        
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {

            if (!ModelState.IsValid) return View();

            if (slider.ImageFile.ContentType != "image/png" && slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/jpg")
            {
                ModelState.AddModelError("ImageFile", "Ancaq png ve jpeg ola biler!");
                return View();
            }

            if (slider.ImageFile.Length > 3145728)
            {
                ModelState.AddModelError("ImageFile", "Fayl olcusu max 3 mb ola biler!");
                return View();
            }

            //string name = slider.ImageFile.FileName;

            //if(name.Length > 64)
            //{
            //    name = name.Substring(name.Length - 64 , 64);
            //}

            //name = Guid.NewGuid().ToString() + name;

            //string path = "C:\\Users\\Admin\\Desktop\\Depo\\Pustok\\Pustok\\wwwroot\\uploads\\sliders\\" + name;

            //using (FileStream filestream = new FileStream(path,FileMode.Create))
            //{
            //    slider.ImageFile.CopyTo(filestream);
            //}

            slider.Image = FileManager.SaveFile(_env.WebRootPath,"uploads/sliders",slider.ImageFile);

            _dataContext.Sliders.Add(slider);
            _dataContext.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Update(int id)
        {
            Slider slider = _dataContext.Sliders.Find(id);
            if(slider == null) return View("Error");
            return View(slider);
        }

        [HttpPost]
        public IActionResult Update(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            Slider existSlider = _dataContext.Sliders.Find(slider.Id);
            if (existSlider == null) return View("error");

            if(slider.Image != null)
            {
                if (slider.ImageFile.ContentType != "image/png" && slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/jpg")
                {
                    ModelState.AddModelError("ImageFile", "Ancaq png ve jpeg ola biler!");
                    return View();
                }

                if (slider.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "Fayl olcusu max 3 mb ola biler!");
                    return View();
                }

                //string deletePath = Path.Combine(_env.WebRootPath, "uploads/sliders", existSlider.Image);

                //if (System.IO.File.Exists(deletePath))
                //{
                //    System.IO.File.Delete(deletePath);
                //}

                string name = existSlider.ImageFile.FileName;

                FileManager.DeleteFile(_env.WebRootPath, "uploads/sliders", name);

                existSlider.Image = FileManager.SaveFile(_env.WebRootPath, "uploads/sliders", slider.ImageFile);

            }

            //string oldImgpath = "C:\\Users\\Admin\\Desktop\\CRUD\\Pustok\\Pustok\\wwwroot\\uploads\\sliders\\" + existSlider.Image;

            ////Delete Old Image
            //FileInfo fileInfo = new FileInfo(oldImgpath);

            //if (fileInfo.Exists) fileInfo.Delete();

            //Check Image

            //string name = slider.ImageFile.FileName;

            //if (name.Length > 64)
            //{
            //    name = name.Substring(name.Length - 64, 64);
            //}

            //name = Guid.NewGuid().ToString() + name;

            //string path = "C:\\Users\\Admin\\Desktop\\Depo\\Pustok\\Pustok\\wwwroot\\uploads\\sliders\\" + name;

            //using (FileStream filestream = new FileStream(path, FileMode.Create))
            //{
            //    slider.ImageFile.CopyTo(filestream);
            //}




            existSlider.Title1 = slider.Title1;
            existSlider.Title2 = slider.Title2;
            existSlider.RedirectUrlText = slider.RedirectUrlText;
            existSlider.RedirectUrl = slider.RedirectUrl;
            existSlider.Desc = slider.Desc;
            existSlider.Order = slider.Order;

            _dataContext.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Slider slider = _dataContext.Sliders.Find(id);
            if (slider == null) return NotFound();

            if (slider.Image != null)
            {
                FileManager.DeleteFile(_env.WebRootPath, "updates/sliders", slider.Image);
            }


            _dataContext.Sliders.Remove(slider);
            _dataContext.SaveChanges();

            return Ok();
        }
    }
}
