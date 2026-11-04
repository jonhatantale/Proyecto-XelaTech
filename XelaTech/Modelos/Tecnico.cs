using System;

namespace XelaTechSoporte
{
    public class Tecnico
    {
        public const int LIMITE_TRABAJOS = 3;

        private int codigo;
        private string nombreCompleto = "";
        private TipoEquipo especialidad;
        private bool disponible;
        private int trabajosActivos;
        private int serviciosFinalizados;
        private int[] conteoCalificaciones = new int[5]; // posicion 0 = calificacion 1, ... posicion 4 = calificacion 5

        public Tecnico(int codigo, string nombreCompleto, TipoEquipo especialidad)
        {
            Codigo = codigo;
            NombreCompleto = nombreCompleto;
            this.especialidad = especialidad;
            disponible = true;
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

        public string NombreCompleto
        {
            get { return nombreCompleto; }
            set
            {
                if (Validador.TextoValido(value)) nombreCompleto = value.Trim();
                else Console.WriteLine("El nombre no puede estar vacío.");
            }
        }

        public TipoEquipo Especialidad
        {
            get { return especialidad; }
        }

        public bool Disponible
        {
            get { return disponible; }
            set { disponible = value; }
        }

        public int TrabajosActivos
        {
            get { return trabajosActivos; }
        }

        public int ServiciosFinalizados
        {
            get { return serviciosFinalizados; }
        }

        public string CodigoMostrar
        {
            get { return "TEC-" + codigo.ToString("D4"); }
        }

        public bool PuedeRecibirTrabajo()
        {
            return disponible && trabajosActivos < LIMITE_TRABAJOS;
        }

        public bool AsignarTrabajo()
        {
            if (!PuedeRecibirTrabajo()) return false;
            trabajosActivos++;
            return true;
        }

        // Se usa cuando se cancela una solicitud que ya tenia tecnico
        public void LiberarTrabajo()
        {
            if (trabajosActivos > 0) trabajosActivos--;
        }

        public bool FinalizarTrabajo(int calificacion)
        {
            if (calificacion < 1 || calificacion > 5)
            {
                Console.WriteLine("La calificación debe estar entre 1 y 5.");
                return false;
            }
            if (trabajosActivos > 0) trabajosActivos--;
            serviciosFinalizados++;
            conteoCalificaciones[calificacion - 1]++;
            return true;
        }

        public double CalcularPromedio()
        {
            int suma = 0;
            int total = 0;
            for (int i = 0; i < conteoCalificaciones.Length; i++)
            {
                suma += (i + 1) * conteoCalificaciones[i];
                total += conteoCalificaciones[i];
            }
            if (total == 0) return 0;
            return (double)suma / total;
        }

        public void MostrarInformacion()
        {
            string estado = disponible ? "Disponible" : "No disponible";
            Console.WriteLine(CodigoMostrar + " | " + nombreCompleto + " | " + especialidad + " | " + estado
                + " | Activos: " + trabajosActivos + "/" + LIMITE_TRABAJOS
                + " | Finalizados: " + serviciosFinalizados
                + " | Promedio: " + CalcularPromedio().ToString("F2"));
        }
    }
}
