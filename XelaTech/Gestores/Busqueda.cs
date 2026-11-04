using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    public static class Busqueda
    {
        // ---------- SECUENCIAL (devuelven TODAS las coincidencias) ----------
        public static List<Cliente> BuscarClientesPorNombre(List<Cliente> clientes, string texto)
        {
            List<Cliente> resultados = new List<Cliente>();
            if (!Validador.TextoValido(texto)) return resultados;

            foreach (Cliente c in clientes)
            {
                if (c.NombreCompleto.ToLower().Contains(texto.Trim().ToLower()))
                {
                    resultados.Add(c);
                }
            }
            return resultados;
        }

        public static List<Solicitud> BuscarSolicitudesPorEstado(List<Solicitud> solicitudes, EstadoSolicitud estado)
        {
            List<Solicitud> resultados = new List<Solicitud>();
            foreach (Solicitud s in solicitudes)
            {
                if (s.Estado == estado) resultados.Add(s);
            }
            return resultados;
        }

        public static List<Equipo> BuscarEquiposPorMarca(List<Equipo> equipos, string marca)
        {
            List<Equipo> resultados = new List<Equipo>();
            if (!Validador.TextoValido(marca)) return resultados;

            foreach (Equipo e in equipos)
            {
                if (e.Marca.ToLower().Contains(marca.Trim().ToLower()))
                {
                    resultados.Add(e);
                }
            }
            return resultados;
        }

        // ---------- BINARIA (por codigo de solicitud) ----------
        public static bool EstaOrdenadaPorCodigo(List<Solicitud> lista)
        {
            for (int i = 0; i < lista.Count - 1; i++)
            {
                if (lista[i].Codigo > lista[i + 1].Codigo) return false;
            }
            return true;
        }

        // Devuelve la posicion encontrada o -1
        public static int BusquedaBinaria(List<Solicitud> solicitudes, int codigoBuscado)
        {
            if (!EstaOrdenadaPorCodigo(solicitudes))
            {
                Console.WriteLine("Ordene la lista por código antes de buscar.");
                return -1;
            }

            int inicio = 0;
            int fin = solicitudes.Count - 1;
            while (inicio <= fin)
            {
                int medio = (inicio + fin) / 2;
                if (solicitudes[medio].Codigo == codigoBuscado) return medio;

                if (solicitudes[medio].Codigo < codigoBuscado) inicio = medio + 1;
                else fin = medio - 1;
            }
            return -1;
        }
    }
}
