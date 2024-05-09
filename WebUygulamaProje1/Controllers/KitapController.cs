﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{

    public class KitapController : Controller
    {
        private readonly IKitapRepository _kitapRepository;
        // singletone olarak bir tane obje oluşturup sürekli onu kullanır (Dependency injection).
        private readonly IKitapTuruRepository _kitapTuruRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;


        public KitapController(IKitapRepository kitapRepository, IKitapTuruRepository kitapTuruRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kitapRepository = kitapRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Admin,Ogrenci")]
        public IActionResult Index()
        {
           List<Kitap> objKitapList = _kitapRepository.GetAll(includeProps:"KitapTuru").ToList();
           

            //Veritabanına gidip kitapları çekip bu listeye alır 
            return View(objKitapList);

        }

        [Authorize(Roles = UserRoles.Role_Admin)]

        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.GetAll()
                .Select(k => new SelectListItem { Text = k.Ad, Value = k.Id.ToString() });
            ViewBag.KitapTuruList = KitapTuruList;

            if(id == null || id ==0)
            {
                return View(); // ekleme
            }
            else
            {
                //güncelleme
                Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre
                if (kitapVt == null)
                {
                    return NotFound();
                }

                return View(kitapVt);
            }
        }


        [HttpPost]
        [Authorize(Roles = UserRoles.Role_Admin)]

        public IActionResult EkleGuncelle(Kitap kitap, IFormFile? file)
        {
           
            if(ModelState.IsValid) // Kitap.cs dosyası içerisindeki requirementlearın sağlanmasını kontrol eder. 
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string kitapPath = Path.Combine(wwwRootPath, @"img");
                if (file != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    kitap.ResimUrl = @"\img\" + file.FileName;
                }
                
                if(kitap.Id == 0)
                {
                    _kitapRepository.Ekle(kitap);
                    TempData["basarili"] = "Yeni Kitap Oluşturuldu!";

                }
                else
                {
                    _kitapRepository.Guncelle(kitap);
                    TempData["basarili"] = "Yeni güncelleme Yapıldı!";

                }
                _kitapRepository.Kaydet();  // save changes yapılmaz ise veriler veritabanına eklenmez 
            return RedirectToAction("Index","Kitap");// veritabanına kaydettikten sonra Indexi çağırır
            }
            return View();
        }

        /*public IActionResult Guncelle(int? id)  koyulan soru işareti konulan parametrenin null olabilme ihtimalinde sistemin çökmemesi içindir.
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kitap? kitapVt = _kitapRepository.Get(u=>u.Id==id); //Expression<Func<T, bool>> filtre
            if (kitapVt == null) 
            {
                return NotFound(); 
            }

            return View(kitapVt);
        }*/

        /* [HttpPost]
         public IActionResult Guncelle(Kitap kitap)
         {
             if (ModelState.IsValid) // Kitap.cs dosyası içerisindeki requirementlearın sağlanmasını kontrol eder. 
             {
                 _kitapRepository.Guncelle(kitap);
                 _kitapRepository.Kaydet();  // save changes yapılmaz ise veriler veritabanına eklenmez 
                 TempData["basarili"] = "Yeni Kitap Güncellendi!";
                 return RedirectToAction("Index", "Kitap");// veritabanına kaydettikten sonra Indexi çağırır
             }
             return View();
         }
        */
        // Get ACTION
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(int? id) // koyulan soru işareti konulan parametrenin null olabilme ihtimalinde sistemin çökmemesi içindir.
        {
            if (id == null || id == 0)
            { 
                return NotFound();
            }
            Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id); 
            if (kitapVt == null)
            {
                return NotFound();
            }

            return View(kitapVt);
        }

        [HttpPost , ActionName("Sil")]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult SilPOST(int? id)
        {
            Kitap? kitap = _kitapRepository.Get(u => u.Id == id);
            if (kitap == null)
            { 
                return NotFound();
            }
            _kitapRepository.Sil(kitap);
            _kitapRepository.Kaydet();
            TempData["basarili"] = "Kitap Silindi!";
            return RedirectToAction("Index" ,"Kitap");
        }


    }
}
