using System;

namespace XelaTechSoporte
{
    public class Repuesto
    {
        private int codigo;
        private string nombre = "";
        private string categoria = "";
        private int existencia;
        private int existenciaMinima;
        private double costoUnitario;
        private double precioVenta;
        private int cantidadUtilizada;

        public Repuesto(int codigo, string nombre, string categoria, int existencia, int existenciaMinima,
            double costoUnitario, double precioVenta)
        {
            Codigo = codigo;
            Nombre = nombre;
            Categoria = categoria;
            Existencia = existencia;
            ExistenciaMinima = existenciaMinima;
            CostoUnitario = costoUnitario;   // primero el costo, porque el precio se compara contra él
            PrecioVenta = precioVenta;
        }

        public int Codigo
        {
            get { return codigo; }
            set
            {
                if (value > 0) codigo = value;
                else Console.WriteLine("El código debe ser mayor a 0.");
            }
        }

        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (Validador.TextoValido(value)) nombre = value.Trim();
                else Console.WriteLine("El nombre no puede estar vacío.");
            }
        }

        public string Categoria
        {
            get { return categoria; }
            set
            {
                if (Validador.TextoValido(value)) categoria = value.Trim();
                else Console.WriteLine("La categoría no puede estar vacía.");
            }
        }

        public int Existencia
        {
            get { return existencia; }
            set
            {
                if (value >= 0) existencia = value;
                else Console.WriteLine("La existencia no puede ser negativa.");
            }
        }

        public int ExistenciaMinima
        {
            get { return existenciaMinima; }
            set
            {
                if (value >= 0) existenciaMinima = value;
                else Console.WriteLine("La existencia mínima no puede ser negativa.");
            }
        }

        public double CostoUnitario
        {
            get { return costoUnitario; }
            set
            {
                if (value >= 0) costoUnitario = value;
                else Console.WriteLine("El costo unitario no puede ser negativo.");
            }
        }

        public double PrecioVenta
        {
            get { return precioVenta; }
            set
            {
                if (value >= costoUnitario) precioVenta = value;
                else Console.WriteLine("El precio de venta no puede ser menor al costo unitario.");
            }
        }

        public int CantidadUtilizada
        {
            get { return cantidadUtilizada; }
        }

        public string CodigoMostrar
        {
            get { return "REP-" + codigo.ToString("D4"); }
        }

        public bool HayExistencia(int cantidad)
        {
            return cantidad > 0 && cantidad <= existencia;
        }

        public bool Descontar(int cantidad)
        {
            if (!HayExistencia(cantidad)) return false;
            existencia -= cantidad;
            cantidadUtilizada += cantidad;
            return true;
        }

        public bool EstaBajoMinimo()
        {
            return existencia <= existenciaMinima;
        }

        public bool EstaAgotado()
        {
            return existencia == 0;
        }

        public void MostrarInformacion()
        {
            string estado = "OK";
            if (EstaAgotado()) estado = "AGOTADO";
            else if (EstaBajoMinimo()) estado = "EN MÍNIMO";
            Console.WriteLine(CodigoMostrar + " | " + nombre + " | " + categoria + " | Existencia: " + existencia
                + " (mín " + existenciaMinima + ") | Costo: Q " + costoUnitario.ToString("F2")
                + " | Venta: Q " + precioVenta.ToString("F2") + " | Usados: " + cantidadUtilizada + " | " + estado);
        }
    }
}
