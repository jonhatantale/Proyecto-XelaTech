using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    // Matriz: filas = tecnicos (max 10), columnas = dias (lunes a domingo)
    public class MatrizCarga
    {
        private int[,] carga = new int[10, 7];
        private string[] dias = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

        public void Registrar(int tecnico, int dia)
        {
            if (tecnico >= 0 && tecnico < 10 && dia >= 0 && dia < 7)
            {
                carga[tecnico, dia]++;
            }
        }

        public void Mostrar(List<Tecnico> tecnicos)
        {
            Console.WriteLine("MATRIZ DE CARGA SEMANAL (solicitudes asignadas por técnico y día)");
            Console.WriteLine("Técnico    Lun  Mar  Mié  Jue  Vie  Sáb  Dom");
            for (int i = 0; i < tecnicos.Count; i++)
            {
                string fila = tecnicos[i].CodigoMostrar.PadRight(10);
                for (int j = 0; j < 7; j++)
                {
                    fila += carga[i, j].ToString().PadLeft(4) + " ";
                }
                Console.WriteLine(fila);
            }
        }

        public void MostrarMayorCarga(List<Tecnico> tecnicos)
        {
            int mayor = 0;
            int filaMayor = -1;
            int diaMayor = -1;
            for (int i = 0; i < tecnicos.Count; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (carga[i, j] > mayor)
                    {
                        mayor = carga[i, j];
                        filaMayor = i;
                        diaMayor = j;
                    }
                }
            }
            if (filaMayor == -1)
            {
                Console.WriteLine("Aún no hay asignaciones registradas.");
            }
            else
            {
                Console.WriteLine("Mayor carga: " + tecnicos[filaMayor].NombreCompleto + " (" + tecnicos[filaMayor].CodigoMostrar
                    + ") el día " + dias[diaMayor] + " con " + mayor + " solicitud(es).");
            }
        }
    }
}
