using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    public class KiralamaController : Controller
    {
        private readonly IKiralamaRepository _kiralamaRepository;
        private readonly IKitapRepository _kitapRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;
        // singletone olarak bir tane obje oluşturup sürekli onu kullanır (Dependency injection).


        public KiralamaController(IKiralamaRepository kiralamaRepository, IKitapRepository kitapRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kiralamaRepository = kiralamaRepository;
            _kitapRepository = kitapRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
           List<Kiralama> objKiralamaList = _kiralamaRepository.GetAll(includeProps:"Kitap").ToList();
           

            //Veritabanına gidip kitapları çekip bu listeye alır 
            return View(objKiralamaList);

        }
        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll()
                .Select(k => new SelectListItem { Text = k.KitapAdi, Value = k.Id.ToString() });
            ViewBag.KitapList = KitapList;

            if(id == null || id ==0)
            {
                return View(); // ekleme
            }
            else
            {
                //güncelleme
                Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre
                if (kiralamaVt == null)
                {
                    return NotFound();
                }

                return View(kiralamaVt);
            }
        }


        [HttpPost]
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {
           
            if(ModelState.IsValid) // Kitap.cs dosyası içerisindeki requirementlearın sağlanmasını kontrol eder. 
            {
                
                if(kiralama.Id == 0)
                {
                    _kiralamaRepository.Ekle(kiralama);
                    TempData["basarili"] = "Yeni Kiralama işlemi Oluşturuldu!";

                }
                else
                {
                    _kiralamaRepository.Guncelle(kiralama);
                    TempData["basarili"] = "Kiralama Kayıt güncelleme Yapıldı!";

                }
                _kiralamaRepository.Kaydet();  // save changes yapılmaz ise veriler veritabanına eklenmez 
            return RedirectToAction("Index","Kiralama");// veritabanına kaydettikten sonra Indexi çağırır
            }
            return View();
        }

       
        // Get ACTION
        public IActionResult Sil(int? id) // koyulan soru işareti konulan parametrenin null olabilme ihtimalinde sistemin çökmemesi içindir.
        {

            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll()
               .Select(k => new SelectListItem { Text = k.KitapAdi, Value = k.Id.ToString() });
            ViewBag.KitapList = KitapList;


            if (id == null || id == 0)
            { 
                return NotFound();
            }
            Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id); 
            if (kiralamaVt == null)
            {
                return NotFound();
            }

            return View(kiralamaVt);
        }

        [HttpPost , ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            Kiralama? kiralama = _kiralamaRepository.Get(u => u.Id == id);
            if (kiralama == null)
            { 
                return NotFound();
            }
            _kiralamaRepository.Sil(kiralama);
            _kiralamaRepository.Kaydet();
            TempData["basarili"] = "Kayıt Silindi!";
            return RedirectToAction("Index" ,"Kiralama");
        }


    }
}
