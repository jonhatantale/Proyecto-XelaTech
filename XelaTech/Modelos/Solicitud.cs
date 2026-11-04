using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    public class Solicitud
    {
        private int codigo;
        private Cliente cliente;
        private Equipo equipo;
        private string descripcion = "";
        private DateTime fechaRecepcion;
        private Prioridad prioridad;
        private EstadoSolicitud estado;
        private Tecnico tecnicoAsignado;
        private Diagnostico diagnostico;
        private List<RepuestoUsado> repuestos = new List<RepuestoUsado>();
        private double costoManoObra;
        private double costoRepuestos;
        private double total;
        private int tiempoEstimado;
        private Stack<EstadoSolicitud> historial = new Stack<EstadoSolicitud>();

        // Constructor con la fecha de hoy
        public Solicitud(int codigo, Cliente cliente, Equipo equipo, string descripcion, Prioridad prioridad, int tiempoEstimado)
            : this(codigo, cliente, equipo, descripcion, prioridad, tiempoEstimado, DateTime.Now)
        {
        }

        // SOBRECARGA: constructor con fecha indicada
        public Solicitud(int codigo, Cliente cliente, Equipo equipo, string descripcion, Prioridad prioridad, int tiempoEstimado, DateTime fecha)
        {
            this.codigo = codigo;
            this.cliente = cliente;
            this.equipo = equipo;
            this.descripcion = descripcion;
            this.prioridad = prioridad;
            this.tiempoEstimado = tiempoEstimado;
            this.fechaRecepcion = fecha;
            estado = EstadoSolicitud.Recibida;
        }

        public int Codigo { get { return codigo; } }
        public Cliente Cliente { get { return cliente; } }
        public Equipo Equipo { get { return equipo; } }
        public string Descripcion { get { return descripcion; } }
        public Prioridad Prioridad { get { return prioridad; } }
        public EstadoSolicitud Estado { get { return estado; } }
        public Diagnostico Diagnostico { get { return diagnostico; } }
        public List<RepuestoUsado> Repuestos { get { return repuestos; } }
        public double CostoManoObra { get { return costoManoObra; } }
        public double CostoRepuestos { get { return costoRepuestos; } }
        public double Total { get { return total; } }
        public int TiempoEstimado { get { return tiempoEstimado; } }

        public DateTime FechaRecepcion
        {
            get { return fechaRecepcion; }
            set
            {
                if (value <= DateTime.Now) fechaRecepcion = value;
                else Console.WriteLine("La fecha de recepción no puede estar en el futuro.");
            }
        }

        public Tecnico TecnicoAsignado
        {
            get { return tecnicoAsignado; }
            set { tecnicoAsignado = value; }
        }

        public string CodigoMostrar
        {
            get { return "SOL-" + codigo.ToString("D4"); }
        }

        // Activa = no esta Entregada ni Cancelada
        public bool EstaActiva()
        {
            return estado != EstadoSolicitud.Entregada && estado != EstadoSolicitud.Cancelada;
        }

        private static bool EsTransicionValida(EstadoSolicitud actual, EstadoSolicitud nuevo)
        {
            if (nuevo == EstadoSolicitud.Cancelada)
            {
                return actual != EstadoSolicitud.Entregada && actual != EstadoSolicitud.Cancelada;
            }

            switch (actual)
            {
                case EstadoSolicitud.Recibida: return nuevo == EstadoSolicitud.EnEspera;
                case EstadoSolicitud.EnEspera: return nuevo == EstadoSolicitud.Asignada;
                case EstadoSolicitud.Asignada: return nuevo == EstadoSolicitud.EnDiagnostico;
                case EstadoSolicitud.EnDiagnostico: return nuevo == EstadoSolicitud.EnReparacion;
                case EstadoSolicitud.EnReparacion: return nuevo == EstadoSolicitud.ListaParaEntregar;
                case EstadoSolicitud.ListaParaEntregar: return nuevo == EstadoSolicitud.Entregada;
                default: return false;
            }
        }

        public bool CambiarEstado(EstadoSolicitud nuevo)
        {
            if (!EstaActiva())
            {
                Console.WriteLine("La solicitud ya está " + estado + ": no se puede modificar ni cancelar.");
                return false;
            }
            if (!EsTransicionValida(estado, nuevo))
            {
                Console.WriteLine("Cambio no permitido: de " + estado + " a " + nuevo + ".");
                return false;
            }
            if (nuevo == EstadoSolicitud.Asignada && tecnicoAsignado == null)
            {
                Console.WriteLine("No se puede asignar sin un técnico.");
                return false;
            }
            if (nuevo == EstadoSolicitud.EnReparacion && diagnostico == null)
            {
                Console.WriteLine("Primero debe registrar el diagnóstico.");
                return false;
            }
            if (nuevo == EstadoSolicitud.ListaParaEntregar)
            {
                CalcularTotal();
            }

            historial.Push(estado);   // se guarda el estado anterior en la PILA
            estado = nuevo;
            return true;
        }

        public bool DeshacerEstado()
        {
            if (!EstaActiva())
            {
                Console.WriteLine("La solicitud ya está " + estado + ": no se puede deshacer.");
                return false;
            }
            if (historial.Count == 0)   // validar que la pila tenga datos antes de desapilar
            {
                Console.WriteLine("No hay estados anteriores para deshacer.");
                return false;
            }
            EstadoSolicitud anterior = historial.Peek();
            if ((int)anterior < (int)EstadoSolicitud.Asignada)
            {
                Console.WriteLine("Solo se puede deshacer entre Asignada y Lista para entregar.");
                return false;
            }
            historial.Pop();
            estado = anterior;
            return true;
        }

        public void VerHistorial()
        {
            Console.WriteLine("Historial de " + CodigoMostrar + " (más reciente primero):");
            Console.WriteLine("   Estado actual: " + estado);
            if (historial.Count == 0)
            {
                Console.WriteLine("   Sin estados anteriores.");
                return;
            }
            foreach (EstadoSolicitud anterior in historial)
            {
                Console.WriteLine("   Anterior: " + anterior);
            }
        }

        public bool RegistrarDiagnostico(Diagnostico nuevoDiagnostico, double manoObra)
        {
            if (estado != EstadoSolicitud.EnDiagnostico)
            {
                Console.WriteLine("Solo se puede registrar el diagnóstico cuando la solicitud está En diagnóstico.");
                return false;
            }
            if (manoObra < 0)
            {
                Console.WriteLine("El costo de mano de obra no puede ser negativo.");
                return false;
            }
            diagnostico = nuevoDiagnostico;
            costoManoObra = manoObra;
            CalcularTotal();
            return true;
        }

        // Agregar repuesto por objeto
        public bool AgregarRepuesto(Repuesto repuesto, int cantidad)
        {
            if (estado != EstadoSolicitud.EnDiagnostico && estado != EstadoSolicitud.EnReparacion)
            {
                Console.WriteLine("Solo se agregan repuestos en diagnóstico o reparación.");
                return false;
            }
            if (repuesto == null)
            {
                Console.WriteLine("El repuesto no existe.");
                return false;
            }
            if (cantidad <= 0)
            {
                Console.WriteLine("La cantidad debe ser mayor a 0.");
                return false;
            }
            if (!repuesto.HayExistencia(cantidad))
            {
                Console.WriteLine("Existencia insuficiente de " + repuesto.Nombre + " (disponible: " + repuesto.Existencia + ").");
                return false;
            }

            repuesto.Descontar(cantidad);
            repuestos.Add(new RepuestoUsado(repuesto, cantidad));
            CalcularTotal();

            if (repuesto.EstaAgotado())
                Console.WriteLine("ALERTA: " + repuesto.Nombre + " quedó AGOTADO.");
            else if (repuesto.EstaBajoMinimo())
                Console.WriteLine("ALERTA: " + repuesto.Nombre + " llegó a su existencia mínima (" + repuesto.Existencia + ").");
            return true;
        }

        // SOBRECARGA: agregar repuesto por codigo
        public bool AgregarRepuesto(int codigoRepuesto, int cantidad, GestorRepuestos gestor)
        {
            Repuesto repuesto = gestor.BuscarPorCodigo(codigoRepuesto);
            if (repuesto == null)
            {
                Console.WriteLine("El repuesto no existe.");
                return false;
            }
            return AgregarRepuesto(repuesto, cantidad);
        }

        // Total = costo de revision (polimorfismo) + mano de obra + repuestos
        public double CalcularTotal()
        {
            costoRepuestos = 0;
            foreach (RepuestoUsado ru in repuestos)
            {
                costoRepuestos += ru.CalcularSubtotal();
            }
            total = equipo.CalcularCostoRevision() + costoManoObra + costoRepuestos;
            return total;
        }

        public void MostrarInformacion()
        {
            CalcularTotal();
            Console.WriteLine(CodigoMostrar + " | " + cliente.NombreCompleto + " | " + equipo.Marca + " " + equipo.Modelo
                + " | " + estado + " | " + prioridad + " | " + fechaRecepcion.ToString("dd/MM/yyyy HH:mm")
                + " | Q " + total.ToString("F2"));
        }

        public void MostrarDetalle()
        {
            CalcularTotal();
            string nombreTecnico = tecnicoAsignado == null ? "Sin asignar" : tecnicoAsignado.NombreCompleto;
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Solicitud:  " + CodigoMostrar);
            Console.WriteLine("Cliente:    " + cliente.NombreCompleto);
            Console.WriteLine("Equipo:     " + equipo.CodigoMostrar + " " + equipo.Marca + " " + equipo.Modelo + " (" + equipo.Tipo + ")");
            Console.WriteLine("Problema:   " + descripcion);
            Console.WriteLine("Recibida:   " + fechaRecepcion.ToString("dd/MM/yyyy HH:mm"));
            Console.WriteLine("Prioridad:  " + prioridad + " | Estado: " + estado + " | Tiempo estimado: " + tiempoEstimado + " h");
            Console.WriteLine("Técnico:    " + nombreTecnico);
            if (diagnostico != null)
            {
                diagnostico.MostrarInformacion();
            }
            foreach (RepuestoUsado ru in repuestos)
            {
                Console.WriteLine("   Repuesto: " + ru.Repuesto.Nombre + " x" + ru.Cantidad + " = Q " + ru.CalcularSubtotal().ToString("F2"));
            }
            Console.WriteLine("Revisión: Q " + equipo.CalcularCostoRevision().ToString("F2")
                + " | Mano de obra: Q " + costoManoObra.ToString("F2")
                + " | Repuestos: Q " + costoRepuestos.ToString("F2"));
            Console.WriteLine("TOTAL: Q " + total.ToString("F2"));
            Console.WriteLine("----------------------------------------");
        }
    }
}
