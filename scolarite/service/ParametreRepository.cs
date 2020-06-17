using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scolarite.service
{
    class ParametreRepository : IParametre
    {
        private BDContext db;

        public ParametreRepository()
        {
            db = new BDContext();
        }

        public List<classe> findAllClasse()
        {
            return db.classe.ToList();
        }

        public List<filiere> findAllFiliere()
        {
            return db.filiere.ToList();
        }

        public classe saveClasse(classe c)
        {
            try
            {
                db.classe.Add(c);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return c;
        }
    }
}
