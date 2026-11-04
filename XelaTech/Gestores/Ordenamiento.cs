using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    // Los 4 algoritmos estan hechos a mano (sin Sort ni OrderBy).
    // Todos trabajan sobre una COPIA para no alterar la lista original.
    public static class Ordenamiento
    {
        private static bool HayDatosSuficientes(int cantidad)
        {
            if (cantidad < 2)
            {
                Console.WriteLine("Datos insuficientes para ordenar (se necesitan al menos 2).");
                return false;
            }
            return true;
        }

        // ---------- BUBBLE SORT: solicitudes por costo total ----------
        public static List<Solicitud> BubbleSortPorCosto(List<Solicitud> original, bool ascendente)
        {
            if (!HayDatosSuficientes(original.Count)) return null;
            List<Solicitud> lista = new List<Solicitud>(original);
            int n = lista.Count;

            for (int i = 0; i < n - 1; i++)
            {
                bool huboCambio = false;
                for (int j = 0; j < n - i - 1; j++)
                {
                    bool fueraDeOrden;
                    if (ascendente) fueraDeOrden = lista[j].Total > lista[j + 1].Total;
                    else fueraDeOrden = lista[j].Total < lista[j + 1].Total;

                    if (fueraDeOrden)
                    {
                        Solicitud temp = lista[j];
                        lista[j] = lista[j + 1];
                        lista[j + 1] = temp;
                        huboCambio = true;
                    }
                }
                if (!huboCambio) break;
            }
            return lista;
        }

        // ---------- SELECTION SORT: tecnicos por servicios finalizados ----------
        public static List<Tecnico> SelectionSortPorFinalizados(List<Tecnico> original, bool ascendente)
        {
            if (!HayDatosSuficientes(original.Count)) return null;
            List<Tecnico> lista = new List<Tecnico>(original);
            int n = lista.Count;

            for (int i = 0; i < n - 1; i++)
            {
                int pos = i;
                for (int j = i + 1; j < n; j++)
                {
                    bool mejor;
                    if (ascendente) mejor = lista[j].ServiciosFinalizados < lista[pos].ServiciosFinalizados;
                    else mejor = lista[j].ServiciosFinalizados > lista[pos].ServiciosFinalizados;

                    if (mejor) pos = j;
                }
                if (pos != i)
                {
                    Tecnico temp = lista[i];
                    lista[i] = lista[pos];
                    lista[pos] = temp;
                }
            }
            return lista;
        }

        // ---------- SHELL SORT: repuestos por existencia ----------
        public static List<Repuesto> ShellSortPorExistencia(List<Repuesto> original, bool ascendente)
        {
            if (!HayDatosSuficientes(original.Count)) return null;
            List<Repuesto> lista = new List<Repuesto>(original);
            int n = lista.Count;
            int salto = n / 2;

            while (salto > 0)
            {
                for (int i = salto; i < n; i++)
                {
                    Repuesto temp = lista[i];
                    int j = i;
                    while (j >= salto && FueraDeOrden(lista[j - salto].Existencia, temp.Existencia, ascendente))
                    {
                        lista[j] = lista[j - salto];
                        j = j - salto;
                    }
                    lista[j] = temp;
                }
                salto = salto / 2;
            }
            return lista;
        }

        private static bool FueraDeOrden(int a, int b, bool ascendente)
        {
            if (ascendente) return a > b;
            return a < b;
        }

        // ---------- QUICK SORT: solicitudes por fecha de recepcion ----------
        public static List<Solicitud> QuickSortPorFecha(List<Solicitud> original, bool ascendente)
        {
            if (!HayDatosSuficientes(original.Count)) return null;
            List<Solicitud> lista = new List<Solicitud>(original);
            QuickSort(lista, 0, lista.Count - 1, ascendente);
            return lista;
        }

        private static void QuickSort(List<Solicitud> lista, int inicio, int fin, bool ascendente)
        {
            if (inicio < fin)
            {
                int p = Particionar(lista, inicio, fin, ascendente);
                QuickSort(lista, inicio, p - 1, ascendente);
                QuickSort(lista, p + 1, fin, ascendente);
            }
        }

        private static int Particionar(List<Solicitud> lista, int inicio, int fin, bool ascendente)
        {
            DateTime pivote = lista[fin].FechaRecepcion;
            int i = inicio - 1;

            for (int j = inicio; j < fin; j++)
            {
                bool cumple;
                if (ascendente) cumple = lista[j].FechaRecepcion <= pivote;
                else cumple = lista[j].FechaRecepcion >= pivote;

                if (cumple)
                {
                    i++;
                    Intercambiar(lista, i, j);
                }
            }
            Intercambiar(lista, i + 1, fin);
            return i + 1;
        }

        private static void Intercambiar(List<Solicitud> lista, int a, int b)
        {
            Solicitud temp = lista[a];
            lista[a] = lista[b];
            lista[b] = temp;
        }
    }
}
