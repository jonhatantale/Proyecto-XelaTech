using System;

namespace XelaTechSoporte
{
    public class Impresora : Equipo
    {
        private const double COSTO_BASE = 75;
        private string tipoImpresion = "";
        private bool paginaPruebaImpresa;

        public Impresora(int codigoInterno, string numeroSerie, string marca, string modelo,
            Cliente propietario, string estadoFisico, string accesorios, string tipoImpresion)
            : base(codigoInterno, numeroSerie, marca, modelo, propietario, TipoEquipo.Impresora, estadoFisico, accesorios)
        {
            TipoImpresion = tipoImpresion;
        }

        public string TipoImpresion
        {
            get { return tipoImpresion; }
            set
            {
                if (Validador.TextoValido(value)) tipoImpresion = value.Trim();
                else Console.WriteLine("El tipo de impresión no puede estar vacío.");
            }
        }

        public bool PaginaPruebaImpresa
        {
            get { return paginaPruebaImpresa; }
            set { paginaPruebaImpresa = value; }
        }

        public override double CalcularCostoRevision()
        {
            return COSTO_BASE;
        }

        public override string ObtenerReglaDiagnostico()
        {
            return "Revisar cabezal, cartuchos y rodillos.";
        }

        public override void MostrarInformacion()
        {
            base.MostrarInformacion();
            Console.WriteLine("      Impresión: " + tipoImpresion);
        }
    }
}
