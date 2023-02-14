using Comun.ViewModels;
using Modelo.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DAL
{
    public class EgresoDAL
    {
        public static ListadoPaginadoVMR<EgresoVMR> LeerTodo(int cantidad, int pagina, string textoBusqueda)
        {
            ListadoPaginadoVMR<EgresoVMR> resultado = new ListadoPaginadoVMR<EgresoVMR>();

            using (var db = DbConexion.Create())
            {
                var query = db.Egreso.Where(x => !x.borrado).Select(x => new EgresoVMR
                {
                   id = x.id,
                   fecha = x.fecha,
                   tratamiento = x.tratamiento,
                   monto = x.monto,
                   medicoId = x.medicoId,
                   ingresoId = x.ingresoId,
          
                });

                if (!string.IsNullOrEmpty(textoBusqueda))
                {
                    query = query.Where(x => x.tratamiento.Contains(textoBusqueda));
                }

                resultado.cantidadTotal = query.Count();

                resultado.elemento = query
                    .OrderBy(x => x.id)
                    .Skip(pagina * cantidad)
                    .Take(cantidad)
                    .ToList();
            }
            return resultado;

        }

        public static EgresoVMR LeerUno(long id)
        {
            EgresoVMR item = null;

            using (var db = DbConexion.Create())
            {
                item = db.Egreso.Where(x => !x.borrado && x.id == id).Select(x => new EgresoVMR
                {
                    id = x.id,
                    fecha = x.fecha,
                    tratamiento = x.tratamiento,
                    monto = x.monto,
                    medicoId = x.medicoId,
                    ingresoId = x.ingresoId
                }).FirstOrDefault();

            }

            return item;
        }

        public static long Crear(Egreso item)
        {
            long id = 0;

            using (var db = DbConexion.Create())
            {
                item.borrado = false;
                db.Egreso.Add(item);
                db.SaveChanges();

            }

            return id;

        }

        public static void Actualizar(EgresoVMR item)
        {
            using (var db = DbConexion.Create())
            {
                var itemUpdate = db.Egreso.Find(item.id);

                itemUpdate.fecha = item.fecha;
                itemUpdate.tratamiento = item.tratamiento;
                itemUpdate.monto = item.monto;
                //itemUpdate.medicoId = item.medicoId;
                //itemUpdate.ingresoId = item.ingresoId;
            
                db.Entry(itemUpdate).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

        }

        public static void Eliminar(List<long> ids)
        {
            using (var db = DbConexion.Create())
            {
                var items = db.Egreso.Where(x => ids.Contains(x.id));

                foreach (var item in items)
                {
                    item.borrado = true;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }

        }
    }
}
