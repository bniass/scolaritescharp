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

        public classe updateClasse(classe c)
        {
            try
            {
                classe classe = db.classe.Find(c.Id);
                classe.libelle = c.libelle;
                classe.code = c.code;
                classe.fraisinscription = c.fraisinscription;
                classe.mensualite = c.mensualite;
                classe.filiere = c.filiere;
                classe.photo = c.photo;
                db.SaveChanges();
                return classe;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
