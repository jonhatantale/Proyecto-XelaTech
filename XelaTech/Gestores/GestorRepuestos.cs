using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    public class GestorRepuestos
    {
        private List<Repuesto> repuestos = new List<Repuesto>();
        private int contadorCodigo = 1;

        public List<Repuesto> Repuestos
        {
            get { return repuestos; }
        }

        public bool ExisteNombre(string nombre)
        {
            foreach (Repuesto r in repuestos)
            {
                if (r.Nombre.ToLower() == nombre.Trim().ToLower()) return true;
            }
            return false;
        }

        public Repuesto Registrar(string nombre, string categoria, int existencia, int existenciaMinima,
            double costoUnitario, double precioVenta)
        {
            if (!Validador.TextoValido(nombre) || !Validador.TextoValido(categoria))
            {
                Console.WriteLine("Nombre y categoría son obligatorios.");
                return null;
            }
            if (existencia < 0 || existenciaMinima < 0 || costoUnitario < 0)
            {
                Console.WriteLine("Existencias y costos no pueden ser negativos.");
                return null;
            }
            if (precioVenta < costoUnitario)
            {
                Console.WriteLine("El precio de venta no puede ser menor al costo unitario.");
                return null;
            }
            if (ExisteNombre(nombre))
            {
                Console.WriteLine("Ya existe un repuesto con ese nombre.");
                return null;
            }

            Repuesto nuevo = new Repuesto(contadorCodigo, nombre.Trim(), categoria.Trim(), existencia,
                existenciaMinima, costoUnitario, precioVenta);
            repuestos.Add(nuevo);
            contadorCodigo++;
            return nuevo;
        }

        public Repuesto BuscarPorCodigo(int codigo)
        {
            foreach (Repuesto r in repuestos)
            {
                if (r.Codigo == codigo) return r;
            }
            return null;
        }

        public void Listar()
        {
            if (repuestos.Count == 0)
            {
                Console.WriteLine("No hay repuestos registrados.");
                return;
            }
            foreach (Repuesto r in repuestos)
            {
                r.MostrarInformacion();
            }
        }
    }
}
