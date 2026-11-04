using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    public class Diagnostico
    {
        private int codigo;
        private string descripcion = "";
        private DateTime fecha;
        private Tecnico tecnico;
        private List<string> accionesRealizadas = new List<string>();

        public Diagnostico(int codigo, string descripcion, Tecnico tecnico)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
            this.tecnico = tecnico;
            fecha = DateTime.Now;
        }

        public int Codigo
        {
            get { return codigo; }
        }

        public string Descripcion
        {
            get { return descripcion; }
        }

        public List<string> AccionesRealizadas
        {
            get { return accionesRealizadas; }
        }

        public void AgregarAccion(string accion)
        {
            if (Validador.TextoValido(accion))
            {
                accionesRealizadas.Add(accion.Trim());
            }
        }

        public void MostrarInformacion()
        {
            string nombreTecnico = tecnico == null ? "Sin técnico" : tecnico.NombreCompleto;
            Console.WriteLine("Diagnóstico DIA-" + codigo.ToString("D4") + " (" + fecha.ToString("dd/MM/yyyy HH:mm") + ", " + nombreTecnico + "): " + descripcion);
            foreach (string accion in accionesRealizadas)
            {
                Console.WriteLine("   - Acción: " + accion);
            }
        }
    }
}
