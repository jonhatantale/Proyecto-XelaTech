using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    public class GestorClientes
    {
        private List<Cliente> clientes = new List<Cliente>();
        private int contadorCodigo = 1;

        public List<Cliente> Clientes
        {
            get { return clientes; }
        }

        public Cliente Registrar(string nombre, string telefono, string correo, TipoCliente tipo)
        {
            if (!Validador.TextoValido(nombre))
            {
                Console.WriteLine("El nombre no puede estar vacío.");
                return null;
            }
            if (!Cliente.TelefonoValido(telefono))
            {
                Console.WriteLine("Teléfono inválido: deben ser exactamente 8 dígitos.");
                return null;
            }

            Cliente nuevo;
            if (Validador.TextoValido(correo))
            {
                if (!Cliente.CorreoValido(correo))
                {
                    Console.WriteLine("Correo inválido: debe tener @ y un punto después de la arroba.");
                    return null;
                }
                nuevo = new Cliente(contadorCodigo, nombre.Trim(), telefono, correo.Trim(), tipo);
            }
            else
            {
                nuevo = new Cliente(contadorCodigo, nombre.Trim(), telefono, tipo);   // sobrecarga sin correo
            }

            clientes.Add(nuevo);
            contadorCodigo++;
            return nuevo;
        }

        public Cliente BuscarPorCodigo(int codigo)
        {
            foreach (Cliente c in clientes)
            {
                if (c.Codigo == codigo) return c;
            }
            return null;
        }

        public bool Actualizar(int codigo, string telefono, string correo)
        {
            Cliente c = BuscarPorCodigo(codigo);
            if (c == null)
            {
                Console.WriteLine("El cliente no existe.");
                return false;
            }
            return c.ActualizarContacto(telefono, correo);
        }

        public void Listar()
        {
            if (clientes.Count == 0)
            {
                Console.WriteLine("No hay clientes registrados.");
                return;
            }
            foreach (Cliente c in clientes)
            {
                c.MostrarInformacion();
            }
        }
    }
}
