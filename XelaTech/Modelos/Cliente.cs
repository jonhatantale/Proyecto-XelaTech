using System;

namespace XelaTechSoporte
{
    public class Cliente
    {
        private int codigo;
        private string nombreCompleto = "";
        private string telefono = "";
        private string correo = "";
        private TipoCliente tipo;
        private int cantidadSolicitudes;

        // Constructor completo
        public Cliente(int codigo, string nombreCompleto, string telefono, string correo, TipoCliente tipo)
        {
            Codigo = codigo;
            NombreCompleto = nombreCompleto;
            Telefono = telefono;
            Correo = correo;
            Tipo = tipo;
        }

        // SOBRECARGA: constructor sin correo
        public Cliente(int codigo, string nombreCompleto, string telefono, TipoCliente tipo)
        {
            Codigo = codigo;
            NombreCompleto = nombreCompleto;
            Telefono = telefono;
            correo = "No registrado";
            Tipo = tipo;
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

        public string Telefono
        {
            get { return telefono; }
            set
            {
                if (TelefonoValido(value)) telefono = value;
                else Console.WriteLine("Teléfono inválido: deben ser exactamente 8 dígitos.");
            }
        }

        public string Correo
        {
            get { return correo; }
            set
            {
                if (CorreoValido(value)) correo = value.Trim();
                else Console.WriteLine("Correo inválido: debe tener @ y un punto después de la arroba.");
            }
        }

        public TipoCliente Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public int CantidadSolicitudes
        {
            get { return cantidadSolicitudes; }
        }

        public string CodigoMostrar
        {
            get { return "CLI-" + codigo.ToString("D4"); }
        }

        public static bool TelefonoValido(string t)
        {
            if (t == null || t.Length != 8) return false;
            foreach (char c in t)
            {
                if (c < '0' || c > '9') return false;
            }
            return true;
        }

        public static bool CorreoValido(string c)
        {
            if (string.IsNullOrWhiteSpace(c)) return false;
            int arroba = c.IndexOf('@');
            if (arroba <= 0) return false;
            int punto = c.IndexOf('.', arroba);
            return punto > arroba + 1 && punto < c.Length - 1;
        }

        public void IncrementarSolicitudes()
        {
            cantidadSolicitudes++;
        }

        // Si el correo viene vacío se conserva el actual
        public bool ActualizarContacto(string nuevoTelefono, string nuevoCorreo)
        {
            if (!TelefonoValido(nuevoTelefono))
            {
                Console.WriteLine("Teléfono inválido: deben ser exactamente 8 dígitos.");
                return false;
            }
            if (Validador.TextoValido(nuevoCorreo))
            {
                if (!CorreoValido(nuevoCorreo))
                {
                    Console.WriteLine("Correo inválido: debe tener @ y un punto después de la arroba.");
                    return false;
                }
                correo = nuevoCorreo.Trim();
            }
            telefono = nuevoTelefono;
            return true;
        }

        public void MostrarInformacion()
        {
            Console.WriteLine(CodigoMostrar + " | " + nombreCompleto + " | Tel: " + telefono + " | " + correo
                + " | " + tipo + " | Solicitudes: " + cantidadSolicitudes);
        }
    }
}
