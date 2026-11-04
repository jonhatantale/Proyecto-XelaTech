using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    // Reportes 1 a 10 y 12 (el 11, resultados de busquedas, esta en Menu)
    public class Reportes
    {
        private GestorTecnicos gestorTecnicos;
        private GestorRepuestos gestorRepuestos;
        private GestorSolicitudes gestorSolicitudes;
        private MatrizCarga matriz;

        public Reportes(GestorTecnicos gt, GestorRepuestos gr, GestorSolicitudes gs, MatrizCarga matriz)
        {
            gestorTecnicos = gt;
            gestorRepuestos = gr;
            gestorSolicitudes = gs;
            this.matriz = matriz;
        }

        private void Titulo(string titulo)
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', 70));
            Console.WriteLine("REPORTE: " + titulo);
            Console.WriteLine(new string('=', 70));
        }

        private bool PreguntarAscendente()
        {
            Console.WriteLine("1. Ascendente   2. Descendente");
            return Validador.LeerEntero("Orden: ", 1, 2) == 1;
        }

        private void EncabezadoOrdenado(string titulo, string algoritmo, string criterio, bool ascendente)
        {
            Titulo(titulo);
            string orden = ascendente ? "Ascendente" : "Descendente";
            Console.WriteLine("Algoritmo: " + algoritmo + " | Criterio: " + criterio + " | Orden: " + orden);
            Console.WriteLine(new string('-', 70));
        }

        // 1. Solicitudes en espera separadas por prioridad
        public void Reporte1()
        {
            Titulo("SOLICITUDES EN ESPERA POR PRIORIDAD");
            Console.WriteLine("Cola URGENTE (" + gestorSolicitudes.ColaUrgente.Count + "):");
            if (gestorSolicitudes.ColaUrgente.Count == 0) Console.WriteLine("   (vacía)");
            foreach (Solicitud s in gestorSolicitudes.ColaUrgente)
            {
                s.MostrarInformacion();
            }
            Console.WriteLine("Cola NORMAL (" + gestorSolicitudes.ColaNormal.Count + "):");
            if (gestorSolicitudes.ColaNormal.Count == 0) Console.WriteLine("   (vacía)");
            foreach (Solicitud s in gestorSolicitudes.ColaNormal)
            {
                s.MostrarInformacion();
            }
        }

        // 2. Solicitudes activas, finalizadas y canceladas
        public void Reporte2()
        {
            Titulo("SOLICITUDES ACTIVAS, FINALIZADAS Y CANCELADAS");
            int activas = 0;
            int finalizadas = 0;
            int canceladas = 0;

            Console.WriteLine("ACTIVAS:");
            foreach (Solicitud s in gestorSolicitudes.Solicitudes)
            {
                if (s.EstaActiva())
                {
                    s.MostrarInformacion();
                    activas++;
                }
            }
            Console.WriteLine("FINALIZADAS (entregadas):");
            foreach (Solicitud s in gestorSolicitudes.Solicitudes)
            {
                if (s.Estado == EstadoSolicitud.Entregada)
                {
                    s.MostrarInformacion();
                    finalizadas++;
                }
            }
            Console.WriteLine("CANCELADAS:");
            foreach (Solicitud s in gestorSolicitudes.Solicitudes)
            {
                if (s.Estado == EstadoSolicitud.Cancelada)
                {
                    s.MostrarInformacion();
                    canceladas++;
                }
            }
            Console.WriteLine("Resumen -> Activas: " + activas + " | Finalizadas: " + finalizadas + " | Canceladas: " + canceladas);
        }

        // 3. Tecnicos disponibles y tecnicos con carga completa
        public void Reporte3()
        {
            Titulo("TÉCNICOS DISPONIBLES Y CON CARGA COMPLETA");
            Console.WriteLine("DISPONIBLES (con cupo):");
            int disponibles = 0;
            foreach (Tecnico t in gestorTecnicos.Tecnicos)
            {
                if (t.PuedeRecibirTrabajo())
                {
                    t.MostrarInformacion();
                    disponibles++;
                }
            }
            if (disponibles == 0) Console.WriteLine("   (ninguno)");

            Console.WriteLine("CON CARGA COMPLETA:");
            int completos = 0;
            foreach (Tecnico t in gestorTecnicos.Tecnicos)
            {
                if (t.TrabajosActivos >= Tecnico.LIMITE_TRABAJOS)
                {
                    t.MostrarInformacion();
                    completos++;
                }
            }
            if (completos == 0) Console.WriteLine("   (ninguno)");
        }

        // 4. Tecnico con mas servicios finalizados (Selection Sort descendente)
        public void Reporte4()
        {
            List<Tecnico> tecnicos = gestorTecnicos.Tecnicos;
            if (tecnicos.Count == 0)
            {
                Console.WriteLine("No hay técnicos registrados.");
                return;
            }
            if (tecnicos.Count == 1)
            {
                Titulo("TÉCNICO CON MÁS SERVICIOS FINALIZADOS");
                tecnicos[0].MostrarInformacion();
                return;
            }

            List<Tecnico> ordenados = Ordenamiento.SelectionSortPorFinalizados(tecnicos, false);
            if (ordenados == null) return;

            EncabezadoOrdenado("TÉCNICO CON MÁS SERVICIOS FINALIZADOS", "Selection Sort", "servicios finalizados", false);
            foreach (Tecnico t in ordenados)
            {
                t.MostrarInformacion();
            }
            Console.WriteLine("Técnico con más servicios: " + ordenados[0].NombreCompleto + " (" + ordenados[0].ServiciosFinalizados + ")");
        }

        // 5. Equipos atendidos por tipo (arreglo unidimensional)
        public void Reporte5()
        {
            Titulo("EQUIPOS ATENDIDOS POR TIPO");
            int[] serviciosPorTipo = new int[3];   // 0 portatil, 1 escritorio, 2 impresora
            foreach (Solicitud s in gestorSolicitudes.Solicitudes)
            {
                if (s.Estado == EstadoSolicitud.Entregada)
                {
                    serviciosPorTipo[(int)s.Equipo.Tipo]++;
                }
            }
            Console.WriteLine("Computadoras portátiles:    " + serviciosPorTipo[0]);
            Console.WriteLine("Computadoras de escritorio: " + serviciosPorTipo[1]);
            Console.WriteLine("Impresoras:                 " + serviciosPorTipo[2]);
        }

        // 6. Solicitudes ordenadas por costo total (Bubble Sort)
        public void Reporte6()
        {
            foreach (Solicitud s in gestorSolicitudes.Solicitudes)
            {
                s.CalcularTotal();
            }
            if (gestorSolicitudes.Solicitudes.Count < 2)
            {
                Console.WriteLine("Datos insuficientes para ordenar (se necesitan al menos 2).");
                return;
            }
            bool ascendente = PreguntarAscendente();
            List<Solicitud> ordenadas = Ordenamiento.BubbleSortPorCosto(gestorSolicitudes.Solicitudes, ascendente);
            if (ordenadas == null) return;

            EncabezadoOrdenado("SOLICITUDES ORDENADAS POR COSTO TOTAL", "Bubble Sort", "costo total", ascendente);
            foreach (Solicitud s in ordenadas)
            {
                s.MostrarInformacion();
            }
            Console.WriteLine("Total de solicitudes mostradas: " + ordenadas.Count);
        }

        // 7. Solicitudes ordenadas por fecha de recepcion (Quick Sort)
        public void Reporte7()
        {
            if (gestorSolicitudes.Solicitudes.Count < 2)
            {
                Console.WriteLine("Datos insuficientes para ordenar (se necesitan al menos 2).");
                return;
            }
            bool ascendente = PreguntarAscendente();
            List<Solicitud> ordenadas = Ordenamiento.QuickSortPorFecha(gestorSolicitudes.Solicitudes, ascendente);
            if (ordenadas == null) return;

            EncabezadoOrdenado("SOLICITUDES ORDENADAS POR FECHA DE RECEPCIÓN", "Quick Sort", "fecha de recepción", ascendente);
            foreach (Solicitud s in ordenadas)
            {
                s.MostrarInformacion();
            }
            Console.WriteLine("Total de solicitudes mostradas: " + ordenadas.Count);
        }

        // 8. Repuestos ordenados por existencia (Shell Sort)
        public void Reporte8()
        {
            if (gestorRepuestos.Repuestos.Count < 2)
            {
                Console.WriteLine("Datos insuficientes para ordenar (se necesitan al menos 2).");
                return;
            }
            bool ascendente = PreguntarAscendente();
            List<Repuesto> ordenados = Ordenamiento.ShellSortPorExistencia(gestorRepuestos.Repuestos, ascendente);
            if (ordenados == null) return;

            EncabezadoOrdenado("REPUESTOS ORDENADOS POR EXISTENCIA", "Shell Sort", "existencia disponible", ascendente);
            foreach (Repuesto r in ordenados)
            {
                r.MostrarInformacion();
            }
            Console.WriteLine("Total de repuestos mostrados: " + ordenados.Count);
        }

        // 9. Repuestos con existencia minima o agotados
        public void Reporte9()
        {
            Titulo("REPUESTOS EN EXISTENCIA MÍNIMA O AGOTADOS");
            int cantidad = 0;
            foreach (Repuesto r in gestorRepuestos.Repuestos)
            {
                if (r.EstaBajoMinimo())
                {
                    r.MostrarInformacion();
                    cantidad++;
                }
            }
            if (cantidad == 0) Console.WriteLine("Ningún repuesto está en su mínimo ni agotado.");
        }

        // 10. Ingresos totales por servicios finalizados
        public void Reporte10()
        {
            Titulo("INGRESOS TOTALES POR SERVICIOS FINALIZADOS");
            double ingresos = 0;
            int cantidad = 0;
            foreach (Solicitud s in gestorSolicitudes.Solicitudes)
            {
                if (s.Estado == EstadoSolicitud.Entregada)
                {
                    s.MostrarInformacion();
                    ingresos += s.Total;
                    cantidad++;
                }
            }
            Console.WriteLine("Servicios finalizados: " + cantidad + " | Ingresos totales: Q " + ingresos.ToString("F2"));
        }

        // 12. Matriz semanal de carga de trabajo
        public void Reporte12()
        {
            Titulo("MATRIZ SEMANAL DE CARGA DE TRABAJO");
            if (gestorTecnicos.Tecnicos.Count == 0)
            {
                Console.WriteLine("No hay técnicos registrados.");
                return;
            }
            matriz.Mostrar(gestorTecnicos.Tecnicos);
            matriz.MostrarMayorCarga(gestorTecnicos.Tecnicos);
        }
    }
}
