using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    public class GestorEquipos
    {
        private List<Equipo> equipos = new List<Equipo>();
        private int contadorCodigo = 1;

        public List<Equipo> Equipos
        {
            get { return equipos; }
        }

        public bool ExisteSerie(string serie)
        {
            foreach (Equipo e in equipos)
            {
                if (e.NumeroSerie.ToLower() == serie.Trim().ToLower()) return true;
            }
            return false;
        }

        private bool ValidarBase(Cliente propietario, string serie, string marca, string modelo, string estadoFisico, string accesorios)
        {
            if (propietario == null)
            {
                Console.WriteLine("El cliente propietario no existe.");
                return false;
            }
            if (!Validador.TextoValido(serie) || !Validador.TextoValido(marca) || !Validador.TextoValido(modelo)
                || !Validador.TextoValido(estadoFisico) || !Validador.TextoValido(accesorios))
            {
                Console.WriteLine("Hay campos obligatorios vacíos.");
                return false;
            }
            if (ExisteSerie(serie))
            {
                Console.WriteLine("Ya existe un equipo con ese número de serie.");
                return false;
            }
            return true;
        }

        public Equipo RegistrarPortatil(Cliente propietario, string serie, string marca, string modelo,
            string estadoFisico, string accesorios, string estadoBateria, double tamanoPantalla)
        {
            if (!ValidarBase(propietario, serie, marca, modelo, estadoFisico, accesorios)) return null;
            if (!Validador.TextoValido(estadoBateria) || tamanoPantalla <= 0)
            {
                Console.WriteLine("Datos de la portátil inválidos.");
                return null;
            }
            Equipo nuevo = new ComputadoraPortatil(contadorCodigo, serie, marca, modelo, propietario,
                estadoFisico, accesorios, estadoBateria, tamanoPantalla);
            equipos.Add(nuevo);
            contadorCodigo++;
            return nuevo;
        }

        public Equipo RegistrarEscritorio(Cliente propietario, string serie, string marca, string modelo,
            string estadoFisico, string accesorios, string tipoFuente, int cantidadDiscos)
        {
            if (!ValidarBase(propietario, serie, marca, modelo, estadoFisico, accesorios)) return null;
            if (!Validador.TextoValido(tipoFuente) || cantidadDiscos <= 0)
            {
                Console.WriteLine("Datos de la computadora de escritorio inválidos.");
                return null;
            }
            Equipo nuevo = new ComputadoraEscritorio(contadorCodigo, serie, marca, modelo, propietario,
                estadoFisico, accesorios, tipoFuente, cantidadDiscos);
            equipos.Add(nuevo);
            contadorCodigo++;
            return nuevo;
        }

        public Equipo RegistrarImpresora(Cliente propietario, string serie, string marca, string modelo,
            string estadoFisico, string accesorios, string tipoImpresion)
        {
            if (!ValidarBase(propietario, serie, marca, modelo, estadoFisico, accesorios)) return null;
            if (!Validador.TextoValido(tipoImpresion))
            {
                Console.WriteLine("Datos de la impresora inválidos.");
                return null;
            }
            Equipo nuevo = new Impresora(contadorCodigo, serie, marca, modelo, propietario,
                estadoFisico, accesorios, tipoImpresion);
            equipos.Add(nuevo);
            contadorCodigo++;
            return nuevo;
        }

        public Equipo BuscarPorCodigo(int codigo)
        {
            foreach (Equipo e in equipos)
            {
                if (e.CodigoInterno == codigo) return e;
            }
            return null;
        }

        public void Listar()
        {
            if (equipos.Count == 0)
            {
                Console.WriteLine("No hay equipos registrados.");
                return;
            }
            foreach (Equipo e in equipos)
            {
                e.MostrarInformacion();
            }
        }
    }
}
