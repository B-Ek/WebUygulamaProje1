using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]

    public class KitapTuruController : Controller
    {
        private readonly IKitapTuruRepository _kitapTuruRepository;
        // singletone olarak bir tane obje oluşturup sürekli onu kullanır (Dependency injection).
         
        public KitapTuruController(IKitapTuruRepository context)
        {
            _kitapTuruRepository = context;
        }
        public IActionResult Index()
        {
           List<KitapTuru> objKitapTuruList = _kitapTuruRepository.GetAll().ToList();
            //Veritabanına gidip kitap türlerini çekip bu listeye alır 
            return View(objKitapTuruList);

        }
        public IActionResult Ekle()
        { 
            return View(); 
        }

        [HttpPost]
        public IActionResult Ekle(KitapTuru kitapTuru)
        {
            if(ModelState.IsValid) // KitapTuru.cs dosyası içerisindeki requirementlearın sağlanmasını kontrol eder. 
            { 
            _kitapTuruRepository.Ekle(kitapTuru);
            _kitapTuruRepository.Kaydet();  // save changes yapılmaz ise veriler veritabanına eklenmez 
                TempData["basarili"] = "Yeni Kitap Türü Oluşturuldu!";
            return RedirectToAction("Index","KitapTuru");// veritabanına kaydettikten sonra Indexi çağırır
            }
            return View();
        }

        public IActionResult Guncelle(int? id) // koyulan soru işareti konulan parametrenin null olabilme ihtimalinde sistemin çökmemesi içindir.
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u=>u.Id==id); //Expression<Func<T, bool>> filtre
            if (kitapTuruVt == null) 
            {
                return NotFound(); 
            }

            return View(kitapTuruVt);
        }

        [HttpPost]
        public IActionResult Guncelle(KitapTuru kitapTuru)
        {
            if (ModelState.IsValid) // KitapTuru.cs dosyası içerisindeki requirementlearın sağlanmasını kontrol eder. 
            {
                _kitapTuruRepository.Guncelle(kitapTuru);
                _kitapTuruRepository.Kaydet();  // save changes yapılmaz ise veriler veritabanına eklenmez 
                TempData["basarili"] = "Yeni Kitap Türü Güncellendi!";
                return RedirectToAction("Index", "KitapTuru");// veritabanına kaydettikten sonra Indexi çağırır
            }
            return View();
        }

        // Get ACTION
        public IActionResult Sil(int? id) // koyulan soru işareti konulan parametrenin null olabilme ihtimalinde sistemin çökmemesi içindir.
        {
            if (id == null || id == 0)
            { 
                return NotFound();
            }
            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u => u.Id == id); 
            if (kitapTuruVt == null)
            {
                return NotFound();
            }

            return View(kitapTuruVt);
        }

        [HttpPost , ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            KitapTuru? kitapTuru = _kitapTuruRepository.Get(u => u.Id == id);
            if (kitapTuru == null)
            { 
                return NotFound();
            }
            _kitapTuruRepository.Sil(kitapTuru);
            _kitapTuruRepository.Kaydet();
            TempData["basarili"] = "Kitap Türü Silindi!";
            return RedirectToAction("Index" ,"KitapTuru");
        }


    }
}
