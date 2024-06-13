using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Models
{
    public class KitapRepository : Repository<Kitap>, IKitapRepository
    {
        private  UygulamaDbContext _uygulamaDbContext;
        //
       // private readonly UygulamaDbContext _context;


        public KitapRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext) 
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        public void Guncelle(Kitap kitap)
        {
            _uygulamaDbContext.Update(kitap);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();  
        }
//
        public Kitap Get(Expression<Func<Kitap, bool>> filtre)
        {
            return _uygulamaDbContext.Kitaplar.FirstOrDefault(filtre);
        }


    }
}
