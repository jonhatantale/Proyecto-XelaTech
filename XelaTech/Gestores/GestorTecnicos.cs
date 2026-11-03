using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    public class GestorTecnicos
    {
        public const int MAXIMO = 10;   // filas de la matriz de carga

        private List<Tecnico> tecnicos = new List<Tecnico>();
        private int contadorCodigo = 1;

        public List<Tecnico> Tecnicos
        {
            get { return tecnicos; }
        }

        public Tecnico Registrar(string nombre, TipoEquipo especialidad)
        {
            if (tecnicos.Count >= MAXIMO)
            {
                Console.WriteLine("No se pueden registrar más de " + MAXIMO + " técnicos (filas de la matriz de carga).");
                return null;
            }
            if (!Validador.TextoValido(nombre))
            {
                Console.WriteLine("El nombre no puede estar vacío.");
                return null;
            }
            Tecnico nuevo = new Tecnico(contadorCodigo, nombre.Trim(), especialidad);
            tecnicos.Add(nuevo);
            contadorCodigo++;
            return nuevo;
        }

        public Tecnico BuscarPorCodigo(int codigo)
        {
            foreach (Tecnico t in tecnicos)
            {
                if (t.Codigo == codigo) return t;
            }
            return null;
        }

        public void Listar()
        {
            if (tecnicos.Count == 0)
            {
                Console.WriteLine("No hay técnicos registrados.");
                return;
            }
            foreach (Tecnico t in tecnicos)
            {
                t.MostrarInformacion();
            }
        }
    }
}
