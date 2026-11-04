using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    // Genera datos de prueba con Random. Los codigos salen de los contadores de cada gestor (nunca se repiten)
    public class GeneradorDatos
    {
        private Random rnd = new Random();
        private GestorClientes clientes;
        private GestorTecnicos tecnicos;
        private GestorEquipos equipos;
        private GestorRepuestos repuestos;
        private GestorSolicitudes solicitudes;

        private string[] nombres = { "Carlos", "María", "José", "Ana", "Luis", "Sofía", "Pedro", "Lucía" };
        private string[] apellidos = { "López", "Pérez", "García", "Tzoc", "Xicay", "Morales", "Ajanel", "Chávez" };
        private string[] marcasComputadora = { "HP", "Dell", "Lenovo", "Asus" };
        private string[] marcasImpresora = { "Epson", "Canon", "HP", "Brother" };
        private string[] problemas = { "No enciende", "Funciona muy lento", "Se sobrecalienta", "No imprime", "Pantalla dañada", "Ruido extraño" };
        private string[] nombresRepuesto = { "Batería", "Cargador", "Pantalla", "Fuente de poder", "Disco duro", "Memoria RAM", "Cabezal", "Cartucho" };
        private string[] categoriasRepuesto = { "Portátil", "Portátil", "Portátil", "Escritorio", "Escritorio", "Escritorio", "Impresora", "Impresora" };

        public GeneradorDatos(GestorClientes c, GestorTecnicos t, GestorEquipos e, GestorRepuestos r, GestorSolicitudes s)
        {
            clientes = c;
            tecnicos = t;
            equipos = e;
            repuestos = r;
            solicitudes = s;
        }

        private string NombreAleatorio()
        {
            return nombres[rnd.Next(nombres.Length)] + " " + apellidos[rnd.Next(apellidos.Length)];
        }

        public void GenerarTodo()
        {
            GenerarClientes(8);
            GenerarTecnicos(6);
            GenerarRepuestos(8);
            GenerarEquipos(12);
            GenerarSolicitudes(15);
            ProcesarSolicitudes();
            Console.WriteLine("Datos de prueba generados correctamente.");
        }

        public void GenerarClientes(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                string telefono = "5" + rnd.Next(1000000, 10000000);     // 8 digitos
                TipoCliente tipo = (TipoCliente)rnd.Next(4);
                Cliente nuevo = clientes.Registrar(NombreAleatorio(), telefono, "", tipo);
                if (nuevo != null)
                {
                    nuevo.Correo = "usuario" + nuevo.Codigo + "@correo.com";
                }
            }
        }

        public void GenerarTecnicos(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                if (tecnicos.Tecnicos.Count >= GestorTecnicos.MAXIMO) break;
                tecnicos.Registrar(NombreAleatorio(), (TipoEquipo)(i % 3));
            }
        }

        public void GenerarRepuestos(int cantidad)
        {
            for (int i = 0; i < cantidad && i < nombresRepuesto.Length; i++)
            {
                if (repuestos.ExisteNombre(nombresRepuesto[i])) continue;
                double costo = rnd.Next(20, 300);
                repuestos.Registrar(nombresRepuesto[i], categoriasRepuesto[i], rnd.Next(0, 16), rnd.Next(2, 5), costo, costo * 1.3);
            }
        }

        public void GenerarEquipos(int cantidad)
        {
            if (clientes.Clientes.Count == 0) return;

            for (int i = 0; i < cantidad; i++)
            {
                Cliente dueno = clientes.Clientes[rnd.Next(clientes.Clientes.Count)];
                int tipo = rnd.Next(3);

                string serie = "SN" + rnd.Next(100000, 1000000);
                while (equipos.ExisteSerie(serie))
                {
                    serie = "SN" + rnd.Next(100000, 1000000);
                }
                string modelo = "M-" + rnd.Next(100, 1000);

                if (tipo == 0)
                {
                    equipos.RegistrarPortatil(dueno, serie, marcasComputadora[rnd.Next(marcasComputadora.Length)], modelo,
                        "Buen estado", "Cargador", "Regular", 14 + rnd.Next(0, 4));
                }
                else if (tipo == 1)
                {
                    equipos.RegistrarEscritorio(dueno, serie, marcasComputadora[rnd.Next(marcasComputadora.Length)], modelo,
                        "Con rayones", "Cable de poder", "Fuente ATX", rnd.Next(1, 3));
                }
                else
                {
                    string tecnologia = rnd.Next(2) == 0 ? "Tinta" : "Láser";
                    equipos.RegistrarImpresora(dueno, serie, marcasImpresora[rnd.Next(marcasImpresora.Length)], modelo,
                        "Buen estado", "Cable USB", tecnologia);
                }
            }
        }

        public void GenerarSolicitudes(int cantidad)
        {
            if (equipos.Equipos.Count == 0) return;

            int creadas = 0;
            int intentos = 0;
            while (creadas < cantidad && intentos < cantidad * 10)
            {
                intentos++;
                Equipo equipo = equipos.Equipos[rnd.Next(equipos.Equipos.Count)];
                if (solicitudes.EquipoTieneSolicitudActiva(equipo)) continue;

                Prioridad prioridad = rnd.Next(100) < 30 ? Prioridad.Urgente : Prioridad.Normal;
                Solicitud s = solicitudes.CrearSolicitud(equipo.Propietario.Codigo, equipo.CodigoInterno,
                    problemas[rnd.Next(problemas.Length)], prioridad, rnd.Next(1, 25));
                if (s != null)
                {
                    s.FechaRecepcion = DateTime.Now.AddDays(-rnd.Next(0, 30)).AddMinutes(-rnd.Next(0, 1440));
                    creadas++;
                }
            }
        }

        // Atiende parte de las colas y avanza algunas solicitudes para que los reportes tengan datos reales
        private void ProcesarSolicitudes()
        {
            while (rnd.Next(100) < 80)
            {
                if (!solicitudes.AtenderSiguiente(false)) break;
            }

            foreach (Solicitud s in solicitudes.Solicitudes)
            {
                if (s.Estado != EstadoSolicitud.Asignada) continue;

                int avance = rnd.Next(1, 5);   // 1 a 4
                s.CambiarEstado(EstadoSolicitud.EnDiagnostico);

                List<string> acciones = new List<string>();
                acciones.Add("Revisión inicial");
                acciones.Add("Limpieza interna");
                solicitudes.RegistrarDiagnostico(s.Codigo, "Falla detectada tras la revisión", acciones, rnd.Next(50, 301));

                if (avance >= 2)
                {
                    if (repuestos.Repuestos.Count > 0)
                    {
                        Repuesto r = repuestos.Repuestos[rnd.Next(repuestos.Repuestos.Count)];
                        s.AgregarRepuesto(r, rnd.Next(1, 3));
                    }
                    s.CambiarEstado(EstadoSolicitud.EnReparacion);
                }
                if (avance >= 3)
                {
                    s.CambiarEstado(EstadoSolicitud.ListaParaEntregar);
                }
                if (avance == 4)
                {
                    solicitudes.EntregarSolicitud(s.Codigo, rnd.Next(3, 6));
                }
            }
        }
    }
}
