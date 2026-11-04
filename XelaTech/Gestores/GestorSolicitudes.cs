using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    public class GestorSolicitudes
    {
        private List<Solicitud> solicitudes = new List<Solicitud>();
        private Queue<Solicitud> colaUrgente = new Queue<Solicitud>();
        private Queue<Solicitud> colaNormal = new Queue<Solicitud>();
        private int contadorCodigo = 1;
        private int contadorDiagnostico = 1;

        private GestorClientes gestorClientes;
        private GestorEquipos gestorEquipos;
        private GestorTecnicos gestorTecnicos;
        private GestorRepuestos gestorRepuestos;
        private MatrizCarga matriz;

        public GestorSolicitudes(GestorClientes gc, GestorEquipos ge, GestorTecnicos gt, GestorRepuestos gr, MatrizCarga matriz)
        {
            gestorClientes = gc;
            gestorEquipos = ge;
            gestorTecnicos = gt;
            gestorRepuestos = gr;
            this.matriz = matriz;
        }

        public List<Solicitud> Solicitudes { get { return solicitudes; } }
        public Queue<Solicitud> ColaUrgente { get { return colaUrgente; } }
        public Queue<Solicitud> ColaNormal { get { return colaNormal; } }

        public Solicitud BuscarPorCodigo(int codigo)
        {
            foreach (Solicitud s in solicitudes)
            {
                if (s.Codigo == codigo) return s;
            }
            return null;
        }

        public bool EquipoTieneSolicitudActiva(Equipo equipo)
        {
            foreach (Solicitud s in solicitudes)
            {
                if (s.Equipo == equipo && s.EstaActiva()) return true;
            }
            return false;
        }

        public Solicitud CrearSolicitud(int codigoCliente, int codigoEquipo, string descripcion, Prioridad prioridad, int tiempoEstimado)
        {
            Cliente cliente = gestorClientes.BuscarPorCodigo(codigoCliente);
            if (cliente == null)
            {
                Console.WriteLine("El cliente no existe.");
                return null;
            }
            Equipo equipo = gestorEquipos.BuscarPorCodigo(codigoEquipo);
            if (equipo == null)
            {
                Console.WriteLine("El equipo no existe.");
                return null;
            }
            if (equipo.Propietario != cliente)
            {
                Console.WriteLine("Ese equipo no pertenece al cliente indicado.");
                return null;
            }
            if (!Validador.TextoValido(descripcion))
            {
                Console.WriteLine("La descripción del problema no puede estar vacía.");
                return null;
            }
            if (tiempoEstimado <= 0)
            {
                Console.WriteLine("El tiempo estimado debe ser mayor a 0.");
                return null;
            }
            if (EquipoTieneSolicitudActiva(equipo))
            {
                Console.WriteLine("Ese equipo ya tiene una solicitud activa.");
                return null;
            }

            Solicitud nueva = new Solicitud(contadorCodigo, cliente, equipo, descripcion.Trim(), prioridad, tiempoEstimado);
            contadorCodigo++;
            solicitudes.Add(nueva);
            cliente.IncrementarSolicitudes();

            if (prioridad == Prioridad.Urgente) colaUrgente.Enqueue(nueva);
            else colaNormal.Enqueue(nueva);

            nueva.CambiarEstado(EstadoSolicitud.EnEspera);
            return nueva;
        }

        // Atiende primero la cola urgente; la normal solo si la urgente esta vacia (FIFO en cada una)
        public bool AtenderSiguiente(bool mostrar = true)
        {
            Queue<Solicitud> cola;
            if (colaUrgente.Count > 0) cola = colaUrgente;
            else if (colaNormal.Count > 0) cola = colaNormal;
            else
            {
                if (mostrar) Console.WriteLine("No hay solicitudes en espera.");
                return false;
            }

            Solicitud siguiente = cola.Peek();
            if (!AsignarTecnico(siguiente))
            {
                if (mostrar)
                {
                    Console.WriteLine("No hay técnico disponible con especialidad en " + siguiente.Equipo.Tipo
                        + " y cupo. La solicitud " + siguiente.CodigoMostrar + " sigue en cola.");
                }
                return false;
            }

            cola.Dequeue();
            if (mostrar)
            {
                Console.WriteLine("Atendida " + siguiente.CodigoMostrar + " -> asignada a " + siguiente.TecnicoAsignado.NombreCompleto);
            }
            return true;
        }

        // Elige entre los tecnicos con la especialidad, disponibles y con cupo, el de menos trabajos activos
        private bool AsignarTecnico(Solicitud s)
        {
            Tecnico mejor = null;
            foreach (Tecnico t in gestorTecnicos.Tecnicos)
            {
                if (t.Especialidad == s.Equipo.Tipo && t.PuedeRecibirTrabajo())
                {
                    if (mejor == null || t.TrabajosActivos < mejor.TrabajosActivos)
                    {
                        mejor = t;
                    }
                }
            }
            if (mejor == null) return false;

            mejor.AsignarTrabajo();
            s.TecnicoAsignado = mejor;
            s.CambiarEstado(EstadoSolicitud.Asignada);

            int dia = ((int)DateTime.Now.DayOfWeek + 6) % 7;   // lunes = 0 ... domingo = 6
            matriz.Registrar(gestorTecnicos.Tecnicos.IndexOf(mejor), dia);
            return true;
        }

        public bool RegistrarDiagnostico(int codigoSolicitud, string descripcion, List<string> acciones, double manoObra)
        {
            Solicitud s = BuscarPorCodigo(codigoSolicitud);
            if (s == null)
            {
                Console.WriteLine("La solicitud no existe.");
                return false;
            }
            if (!Validador.TextoValido(descripcion))
            {
                Console.WriteLine("La descripción del diagnóstico no puede estar vacía.");
                return false;
            }
            Diagnostico d = new Diagnostico(contadorDiagnostico, descripcion.Trim(), s.TecnicoAsignado);
            foreach (string accion in acciones)
            {
                d.AgregarAccion(accion);
            }
            if (s.RegistrarDiagnostico(d, manoObra))
            {
                contadorDiagnostico++;
                return true;
            }
            return false;
        }

        public bool AgregarRepuesto(int codigoSolicitud, int codigoRepuesto, int cantidad)
        {
            Solicitud s = BuscarPorCodigo(codigoSolicitud);
            if (s == null)
            {
                Console.WriteLine("La solicitud no existe.");
                return false;
            }
            return s.AgregarRepuesto(codigoRepuesto, cantidad, gestorRepuestos);
        }

        public bool CambiarEstado(int codigo, EstadoSolicitud nuevo)
        {
            Solicitud s = BuscarPorCodigo(codigo);
            if (s == null)
            {
                Console.WriteLine("La solicitud no existe.");
                return false;
            }
            if (nuevo == EstadoSolicitud.Asignada || nuevo == EstadoSolicitud.EnEspera)
            {
                Console.WriteLine("Ese estado se asigna automáticamente desde 'Atención de colas'.");
                return false;
            }
            if (nuevo == EstadoSolicitud.Entregada)
            {
                Console.WriteLine("Para entregar use la opción de entrega (pide la calificación del técnico).");
                return false;
            }
            if (nuevo == EstadoSolicitud.Cancelada)
            {
                return CancelarSolicitud(s);
            }
            return s.CambiarEstado(nuevo);
        }

        private bool CancelarSolicitud(Solicitud s)
        {
            bool estabaEnEspera = s.Estado == EstadoSolicitud.EnEspera;
            if (!s.CambiarEstado(EstadoSolicitud.Cancelada)) return false;

            if (estabaEnEspera) RemoverDeCola(s);
            if (s.TecnicoAsignado != null) s.TecnicoAsignado.LiberarTrabajo();
            return true;
        }

        private void RemoverDeCola(Solicitud s)
        {
            Queue<Solicitud> origen = s.Prioridad == Prioridad.Urgente ? colaUrgente : colaNormal;
            Queue<Solicitud> resto = new Queue<Solicitud>();
            foreach (Solicitud x in origen)
            {
                if (x != s) resto.Enqueue(x);
            }
            origen.Clear();
            foreach (Solicitud x in resto)
            {
                origen.Enqueue(x);
            }
        }

        public bool EntregarSolicitud(int codigo, int calificacion)
        {
            Solicitud s = BuscarPorCodigo(codigo);
            if (s == null)
            {
                Console.WriteLine("La solicitud no existe.");
                return false;
            }
            if (calificacion < 1 || calificacion > 5)
            {
                Console.WriteLine("La calificación debe estar entre 1 y 5.");
                return false;
            }
            if (!s.CambiarEstado(EstadoSolicitud.Entregada)) return false;

            s.TecnicoAsignado.FinalizarTrabajo(calificacion);
            return true;
        }

        public bool DeshacerEstado(int codigo)
        {
            Solicitud s = BuscarPorCodigo(codigo);
            if (s == null)
            {
                Console.WriteLine("La solicitud no existe.");
                return false;
            }
            return s.DeshacerEstado();
        }

        public void Listar()
        {
            if (solicitudes.Count == 0)
            {
                Console.WriteLine("No hay solicitudes registradas.");
                return;
            }
            foreach (Solicitud s in solicitudes)
            {
                s.MostrarInformacion();
            }
        }

        public void MostrarHistorialEquipo(int codigoEquipo)
        {
            Equipo e = gestorEquipos.BuscarPorCodigo(codigoEquipo);
            if (e == null)
            {
                Console.WriteLine("El equipo no existe.");
                return;
            }
            Console.WriteLine("Historial de reparaciones de " + e.CodigoMostrar + " (" + e.Marca + " " + e.Modelo + "):");
            int cantidad = 0;
            foreach (Solicitud s in solicitudes)
            {
                if (s.Equipo == e)
                {
                    s.MostrarInformacion();
                    cantidad++;
                }
            }
            if (cantidad == 0) Console.WriteLine("Este equipo no tiene solicitudes.");
        }
    }
}
