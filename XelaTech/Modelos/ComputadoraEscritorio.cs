using System;

namespace XelaTechSoporte
{
    public class ComputadoraEscritorio : Equipo
    {
        private const double COSTO_BASE = 100;
        private string tipoFuente = "";
        private int cantidadDiscos;

        public ComputadoraEscritorio(int codigoInterno, string numeroSerie, string marca, string modelo,
            Cliente propietario, string estadoFisico, string accesorios, string tipoFuente, int cantidadDiscos)
            : base(codigoInterno, numeroSerie, marca, modelo, propietario, TipoEquipo.ComputadoraEscritorio, estadoFisico, accesorios)
        {
            TipoFuente = tipoFuente;
            CantidadDiscos = cantidadDiscos;
        }

        public string TipoFuente
        {
            get { return tipoFuente; }
            set
            {
                if (Validador.TextoValido(value)) tipoFuente = value.Trim();
                else Console.WriteLine("El tipo de fuente no puede estar vacío.");
            }
        }

        public int CantidadDiscos
        {
            get { return cantidadDiscos; }
            set
            {
                if (value > 0) cantidadDiscos = value;
                else Console.WriteLine("La cantidad de discos debe ser mayor a 0.");
            }
        }

        public override double CalcularCostoRevision()
        {
            return COSTO_BASE;
        }

        public override string ObtenerReglaDiagnostico()
        {
            return "Revisar fuente de poder, memoria y disco.";
        }

        public override void MostrarInformacion()
        {
            base.MostrarInformacion();
            Console.WriteLine("      Fuente: " + tipoFuente + " | Discos: " + cantidadDiscos);
        }
    }
}
