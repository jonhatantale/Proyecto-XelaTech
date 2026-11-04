using System;

namespace XelaTechSoporte
{
    // Clase abstracta: no existe un "equipo generico", siempre es portatil, escritorio o impresora
    public abstract class Equipo
    {
        protected int codigoInterno;
        protected string numeroSerie = "";
        protected string marca = "";
        protected string modelo = "";
        protected Cliente propietario;
        protected TipoEquipo tipoEquipo;
        protected string estadoFisico = "";
        protected string accesorios = "";

        protected Equipo(int codigoInterno, string numeroSerie, string marca, string modelo,
            Cliente propietario, TipoEquipo tipoEquipo, string estadoFisico, string accesorios)
        {
            CodigoInterno = codigoInterno;
            NumeroSerie = numeroSerie;
            Marca = marca;
            Modelo = modelo;
            Propietario = propietario;
            this.tipoEquipo = tipoEquipo;
            EstadoFisico = estadoFisico;
            Accesorios = accesorios;
        }

        public int CodigoInterno
        {
            get { return codigoInterno; }
            set
            {
                if (value > 0) codigoInterno = value;
                else Console.WriteLine("El código debe ser mayor a 0.");
            }
        }

        public string NumeroSerie
        {
            get { return numeroSerie; }
            set
            {
                if (Validador.TextoValido(value)) numeroSerie = value.Trim();
                else Console.WriteLine("El número de serie no puede estar vacío.");
            }
        }

        public string Marca
        {
            get { return marca; }
            set
            {
                if (Validador.TextoValido(value)) marca = value.Trim();
                else Console.WriteLine("La marca no puede estar vacía.");
            }
        }

        public string Modelo
        {
            get { return modelo; }
            set
            {
                if (Validador.TextoValido(value)) modelo = value.Trim();
                else Console.WriteLine("El modelo no puede estar vacío.");
            }
        }

        public Cliente Propietario
        {
            get { return propietario; }
            set
            {
                if (value != null) propietario = value;
                else Console.WriteLine("El equipo debe tener propietario.");
            }
        }

        public TipoEquipo Tipo
        {
            get { return tipoEquipo; }
        }

        public string EstadoFisico
        {
            get { return estadoFisico; }
            set
            {
                if (Validador.TextoValido(value)) estadoFisico = value.Trim();
                else Console.WriteLine("El estado físico no puede estar vacío.");
            }
        }

        public string Accesorios
        {
            get { return accesorios; }
            set
            {
                if (Validador.TextoValido(value)) accesorios = value.Trim();
                else Console.WriteLine("Los accesorios no pueden estar vacíos (escriba Ninguno).");
            }
        }

        public string CodigoMostrar
        {
            get { return "EQU-" + codigoInterno.ToString("D4"); }
        }

        // Cada tipo de equipo calcula su costo distinto (POLIMORFISMO)
        public abstract double CalcularCostoRevision();

        public virtual string ObtenerReglaDiagnostico()
        {
            return "Revisión general del equipo.";
        }

        public virtual void MostrarInformacion()
        {
            Console.WriteLine(CodigoMostrar + " | " + tipoEquipo + " | Serie: " + numeroSerie + " | " + marca + " " + modelo
                + " | Dueño: " + propietario.NombreCompleto + " | Estado: " + estadoFisico + " | Accesorios: " + accesorios);
        }
    }
}
