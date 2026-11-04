using System;

namespace XelaTechSoporte
{
    public class ComputadoraPortatil : Equipo
    {
        private const double COSTO_BASE = 150;
        private string estadoBateria = "";
        private double tamanoPantalla;

        public ComputadoraPortatil(int codigoInterno, string numeroSerie, string marca, string modelo,
            Cliente propietario, string estadoFisico, string accesorios, string estadoBateria, double tamanoPantalla)
            : base(codigoInterno, numeroSerie, marca, modelo, propietario, TipoEquipo.ComputadoraPortatil, estadoFisico, accesorios)
        {
            EstadoBateria = estadoBateria;
            TamanoPantalla = tamanoPantalla;
        }

        public string EstadoBateria
        {
            get { return estadoBateria; }
            set
            {
                if (Validador.TextoValido(value)) estadoBateria = value.Trim();
                else Console.WriteLine("El estado de la batería no puede estar vacío.");
            }
        }

        public double TamanoPantalla
        {
            get { return tamanoPantalla; }
            set
            {
                if (value > 0) tamanoPantalla = value;
                else Console.WriteLine("El tamaño de pantalla debe ser mayor a 0.");
            }
        }

        public override double CalcularCostoRevision()
        {
            return COSTO_BASE;
        }

        public override string ObtenerReglaDiagnostico()
        {
            return "Revisar batería, cargador y pantalla.";
        }

        public override void MostrarInformacion()
        {
            base.MostrarInformacion();
            Console.WriteLine("      Batería: " + estadoBateria + " | Pantalla: " + tamanoPantalla + " pulgadas");
        }
    }
}
