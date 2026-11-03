using System;
using System.Collections.Generic;

namespace XelaTechSoporte
{
    public class Menu
    {
        private GestorClientes gestorClientes = new GestorClientes();
        private GestorTecnicos gestorTecnicos = new GestorTecnicos();
        private GestorEquipos gestorEquipos = new GestorEquipos();
        private GestorRepuestos gestorRepuestos = new GestorRepuestos();
        private MatrizCarga matriz = new MatrizCarga();
        private GestorSolicitudes gestorSolicitudes;
        private Reportes reportes;
        private GeneradorDatos generador;

        public Menu()
        {
            gestorSolicitudes = new GestorSolicitudes(gestorClientes, gestorEquipos, gestorTecnicos, gestorRepuestos, matriz);
            reportes = new Reportes(gestorTecnicos, gestorRepuestos, gestorSolicitudes, matriz);
            generador = new GeneradorDatos(gestorClientes, gestorTecnicos, gestorEquipos, gestorRepuestos, gestorSolicitudes);
        }

        // ================= MENU PRINCIPAL =================
        public void Iniciar()
        {
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine();
                Console.WriteLine("============================================");
                Console.WriteLine("            XELATECH SOPORTE");
                Console.WriteLine("============================================");
                Console.WriteLine("1. Gestión de clientes");
                Console.WriteLine("2. Gestión de técnicos");
                Console.WriteLine("3. Gestión de equipos");
                Console.WriteLine("4. Gestión de solicitudes");
                Console.WriteLine("5. Gestión de repuestos");
                Console.WriteLine("6. Atención de colas");
                Console.WriteLine("7. Ordenamientos");
                Console.WriteLine("8. Búsquedas");
                Console.WriteLine("9. Reportes");
                Console.WriteLine("10. Generar datos de prueba");
                Console.WriteLine("11. Salir");
                Console.WriteLine("============================================");
                int opcion = Validador.LeerEntero("Seleccione una opción: ", 1, 11);

                switch (opcion)
                {
                    case 1: MenuClientes(); break;
                    case 2: MenuTecnicos(); break;
                    case 3: MenuEquipos(); break;
                    case 4: MenuSolicitudes(); break;
                    case 5: MenuRepuestos(); break;
                    case 6: MenuColas(); break;
                    case 7: MenuOrdenamientos(); break;
                    case 8: MenuBusquedas(); break;
                    case 9: MenuReportes(); break;
                    case 10: generador.GenerarTodo(); break;
                    case 11: salir = true; Console.WriteLine("¡Hasta luego!"); break;
                }
            }
        }

        // ================= AYUDAS DE LECTURA =================
        private int LeerCodigo(string mensaje)
        {
            return Validador.LeerEntero(mensaje, 1, 999999);
        }

        private string LeerTelefono()
        {
            while (true)
            {
                string t = Validador.LeerTexto("Teléfono (8 dígitos): ");
                if (Cliente.TelefonoValido(t)) return t;
                Console.WriteLine("Teléfono inválido: deben ser exactamente 8 dígitos.");
            }
        }

        // Devuelve "" si el usuario lo deja vacío
        private string LeerCorreo()
        {
            while (true)
            {
                Console.Write("Correo (Enter para omitir): ");
                string c = Console.ReadLine();
                if (!Validador.TextoValido(c)) return "";
                if (Cliente.CorreoValido(c)) return c.Trim();
                Console.WriteLine("Correo inválido: debe tener @ y un punto después de la arroba.");
            }
        }

        private TipoCliente LeerTipoCliente()
        {
            Console.WriteLine("Tipo: 1=Estudiante 2=Profesional 3=Negocio pequeño 4=Institución");
            int op = Validador.LeerEntero("Tipo: ", 1, 4);
            return (TipoCliente)(op - 1);
        }

        private TipoEquipo LeerTipoEquipo()
        {
            Console.WriteLine("Especialidad: 1=Portátil 2=Escritorio 3=Impresora");
            int op = Validador.LeerEntero("Especialidad: ", 1, 3);
            return (TipoEquipo)(op - 1);
        }

        // ================= CLIENTES =================
        private void MenuClientes()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("\n--- Gestión de clientes ---");
                Console.WriteLine("1. Registrar cliente");
                Console.WriteLine("2. Consultar cliente");
                Console.WriteLine("3. Listar clientes");
                Console.WriteLine("4. Actualizar contacto");
                Console.WriteLine("0. Volver");
                int op = Validador.LeerEntero("Opción: ", 0, 4);

                switch (op)
                {
                    case 1: RegistrarCliente(); break;
                    case 2: ConsultarCliente(); break;
                    case 3: gestorClientes.Listar(); break;
                    case 4: ActualizarCliente(); break;
                    case 0: volver = true; break;
                }
            }
        }

        private void RegistrarCliente()
        {
            string nombre = Validador.LeerTexto("Nombre completo: ");
            string telefono = LeerTelefono();
            string correo = LeerCorreo();
            TipoCliente tipo = LeerTipoCliente();
            Cliente nuevo = gestorClientes.Registrar(nombre, telefono, correo, tipo);
            if (nuevo != null) Console.WriteLine("Cliente registrado con código " + nuevo.CodigoMostrar + " (número: " + nuevo.Codigo + ")");
        }

        private void ConsultarCliente()
        {
            Cliente c = gestorClientes.BuscarPorCodigo(LeerCodigo("Código del cliente (número): "));
            if (c == null) Console.WriteLine("El cliente no existe.");
            else c.MostrarInformacion();
        }

        private void ActualizarCliente()
        {
            int codigo = LeerCodigo("Código del cliente (número): ");
            if (gestorClientes.BuscarPorCodigo(codigo) == null)
            {
                Console.WriteLine("El cliente no existe.");
                return;
            }
            string telefono = LeerTelefono();
            string correo = LeerCorreo();
            if (gestorClientes.Actualizar(codigo, telefono, correo)) Console.WriteLine("Contacto actualizado.");
        }

        // ================= TECNICOS =================
        private void MenuTecnicos()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("\n--- Gestión de técnicos ---");
                Console.WriteLine("1. Registrar técnico");
                Console.WriteLine("2. Consultar técnico");
                Console.WriteLine("3. Listar técnicos");
                Console.WriteLine("4. Cambiar disponibilidad");
                Console.WriteLine("0. Volver");
                int op = Validador.LeerEntero("Opción: ", 0, 4);

                switch (op)
                {
                    case 1:
                        string nombre = Validador.LeerTexto("Nombre completo: ");
                        TipoEquipo esp = LeerTipoEquipo();
                        Tecnico nuevo = gestorTecnicos.Registrar(nombre, esp);
                        if (nuevo != null) Console.WriteLine("Técnico registrado con código " + nuevo.CodigoMostrar);
                        break;
                    case 2:
                        Tecnico t = gestorTecnicos.BuscarPorCodigo(LeerCodigo("Código del técnico (número): "));
                        if (t == null) Console.WriteLine("El técnico no existe.");
                        else t.MostrarInformacion();
                        break;
                    case 3:
                        gestorTecnicos.Listar();
                        break;
                    case 4:
                        Tecnico t2 = gestorTecnicos.BuscarPorCodigo(LeerCodigo("Código del técnico (número): "));
                        if (t2 == null)
                        {
                            Console.WriteLine("El técnico no existe.");
                        }
                        else
                        {
                            t2.Disponible = !t2.Disponible;
                            Console.WriteLine(t2.NombreCompleto + " ahora está " + (t2.Disponible ? "disponible." : "no disponible."));
                        }
                        break;
                    case 0:
                        volver = true;
                        break;
                }
            }
        }

        // ================= EQUIPOS =================
        private void MenuEquipos()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("\n--- Gestión de equipos ---");
                Console.WriteLine("1. Registrar computadora portátil");
                Console.WriteLine("2. Registrar computadora de escritorio");
                Console.WriteLine("3. Registrar impresora");
                Console.WriteLine("4. Consultar equipo");
                Console.WriteLine("5. Listar equipos");
                Console.WriteLine("0. Volver");
                int op = Validador.LeerEntero("Opción: ", 0, 5);

                switch (op)
                {
                    case 1: RegistrarEquipo(TipoEquipo.ComputadoraPortatil); break;
                    case 2: RegistrarEquipo(TipoEquipo.ComputadoraEscritorio); break;
                    case 3: RegistrarEquipo(TipoEquipo.Impresora); break;
                    case 4:
                        Equipo e = gestorEquipos.BuscarPorCodigo(LeerCodigo("Código del equipo (número): "));
                        if (e == null) Console.WriteLine("El equipo no existe.");
                        else e.MostrarInformacion();
                        break;
                    case 5: gestorEquipos.Listar(); break;
                    case 0: volver = true; break;
                }
            }
        }

        private void RegistrarEquipo(TipoEquipo tipo)
        {
            Cliente propietario = gestorClientes.BuscarPorCodigo(LeerCodigo("Código del cliente propietario (número): "));
            if (propietario == null)
            {
                Console.WriteLine("El cliente no existe. Regístrelo primero.");
                return;
            }
            string serie = Validador.LeerTexto("Número de serie: ");
            string marca = Validador.LeerTexto("Marca: ");
            string modelo = Validador.LeerTexto("Modelo: ");
            string estadoFisico = Validador.LeerTexto("Estado físico recibido: ");
            Console.Write("Accesorios entregados (Enter si ninguno): ");
            string accesorios = Console.ReadLine();
            if (!Validador.TextoValido(accesorios)) accesorios = "Ninguno";

            Equipo nuevo = null;
            if (tipo == TipoEquipo.ComputadoraPortatil)
            {
                string bateria = Validador.LeerTexto("Estado de la batería: ");
                double pantalla = Validador.LeerDecimal("Tamaño de pantalla (pulgadas): ", 1);
                nuevo = gestorEquipos.RegistrarPortatil(propietario, serie, marca, modelo, estadoFisico, accesorios, bateria, pantalla);
            }
            else if (tipo == TipoEquipo.ComputadoraEscritorio)
            {
                string fuente = Validador.LeerTexto("Tipo de fuente: ");
                int discos = Validador.LeerEntero("Cantidad de discos: ", 1, 20);
                nuevo = gestorEquipos.RegistrarEscritorio(propietario, serie, marca, modelo, estadoFisico, accesorios, fuente, discos);
            }
            else
            {
                string tecnologia = Validador.LeerTexto("Tipo de impresión (tinta o láser): ");
                nuevo = gestorEquipos.RegistrarImpresora(propietario, serie, marca, modelo, estadoFisico, accesorios, tecnologia);
            }

            if (nuevo != null)
            {
                Console.WriteLine("Equipo registrado: " + nuevo.CodigoMostrar + " | Costo de revisión: Q " + nuevo.CalcularCostoRevision().ToString("F2"));
            }
        }

        // ================= SOLICITUDES =================
        private void MenuSolicitudes()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("\n--- Gestión de solicitudes ---");
                Console.WriteLine("1. Crear solicitud");
                Console.WriteLine("2. Consultar solicitud");
                Console.WriteLine("3. Listar solicitudes");
                Console.WriteLine("4. Registrar diagnóstico y mano de obra");
                Console.WriteLine("5. Agregar repuesto");
                Console.WriteLine("6. Cambiar estado / entregar / cancelar");
                Console.WriteLine("7. Ver historial de estados");
                Console.WriteLine("8. Deshacer último cambio de estado");
                Console.WriteLine("9. Historial de reparaciones de un equipo");
                Console.WriteLine("0. Volver");
                int op = Validador.LeerEntero("Opción: ", 0, 9);

                switch (op)
                {
                    case 1: CrearSolicitud(); break;
                    case 2:
                        Solicitud s = gestorSolicitudes.BuscarPorCodigo(LeerCodigo("Código de la solicitud (número): "));
                        if (s == null) Console.WriteLine("La solicitud no existe.");
                        else s.MostrarDetalle();
                        break;
                    case 3: gestorSolicitudes.Listar(); break;
                    case 4: RegistrarDiagnostico(); break;
                    case 5: AgregarRepuesto(); break;
                    case 6: CambiarEstado(); break;
                    case 7:
                        Solicitud sh = gestorSolicitudes.BuscarPorCodigo(LeerCodigo("Código de la solicitud (número): "));
                        if (sh == null) Console.WriteLine("La solicitud no existe.");
                        else sh.VerHistorial();
                        break;
                    case 8:
                        if (gestorSolicitudes.DeshacerEstado(LeerCodigo("Código de la solicitud (número): ")))
                            Console.WriteLine("Último cambio de estado deshecho.");
                        break;
                    case 9:
                        gestorSolicitudes.MostrarHistorialEquipo(LeerCodigo("Código del equipo (número): "));
                        break;
                    case 0: volver = true; break;
                }
            }
        }

        private void CrearSolicitud()
        {
            int codCliente = LeerCodigo("Código del cliente (número): ");
            int codEquipo = LeerCodigo("Código del equipo (número): ");
            string descripcion = Validador.LeerTexto("Descripción del problema: ");
            Console.WriteLine("Prioridad: 1=Normal 2=Urgente");
            int p = Validador.LeerEntero("Prioridad: ", 1, 2);
            Prioridad prioridad = p == 2 ? Prioridad.Urgente : Prioridad.Normal;
            int tiempo = Validador.LeerEntero("Tiempo estimado de atención (horas): ", 1, 720);

            Solicitud nueva = gestorSolicitudes.CrearSolicitud(codCliente, codEquipo, descripcion, prioridad, tiempo);
            if (nueva != null)
            {
                Console.WriteLine("Solicitud " + nueva.CodigoMostrar + " creada y colocada en la cola " + prioridad + ".");
            }
        }

        private void RegistrarDiagnostico()
        {
            int codigo = LeerCodigo("Código de la solicitud (número): ");
            string descripcion = Validador.LeerTexto("Diagnóstico: ");
            Console.Write("Acciones realizadas (separadas por coma): ");
            string linea = Console.ReadLine();
            List<string> acciones = new List<string>();
            if (linea != null)
            {
                foreach (string parte in linea.Split(','))
                {
                    if (Validador.TextoValido(parte)) acciones.Add(parte.Trim());
                }
            }
            double manoObra = Validador.LeerDecimal("Costo de mano de obra (Q): ", 0);
            if (gestorSolicitudes.RegistrarDiagnostico(codigo, descripcion, acciones, manoObra))
                Console.WriteLine("Diagnóstico registrado.");
        }

        private void AgregarRepuesto()
        {
            int codSolicitud = LeerCodigo("Código de la solicitud (número): ");
            int codRepuesto = LeerCodigo("Código del repuesto (número): ");
            int cantidad = Validador.LeerEntero("Cantidad: ", 1, 1000);
            if (gestorSolicitudes.AgregarRepuesto(codSolicitud, codRepuesto, cantidad))
                Console.WriteLine("Repuesto agregado a la solicitud.");
        }

        private void CambiarEstado()
        {
            int codigo = LeerCodigo("Código de la solicitud (número): ");
            Console.WriteLine("Nuevo estado:");
            Console.WriteLine("1=Recibida 2=En espera 3=Asignada 4=En diagnóstico 5=En reparación");
            Console.WriteLine("6=Lista para entregar 7=Entregada 8=Cancelada");
            int op = Validador.LeerEntero("Estado: ", 1, 8);
            EstadoSolicitud nuevo = (EstadoSolicitud)(op - 1);

            if (nuevo == EstadoSolicitud.Entregada)
            {
                int calificacion = Validador.LeerEntero("Calificación del servicio (1 a 5): ", 1, 5);
                if (gestorSolicitudes.EntregarSolicitud(codigo, calificacion)) Console.WriteLine("Equipo entregado.");
            }
            else
            {
                if (gestorSolicitudes.CambiarEstado(codigo, nuevo)) Console.WriteLine("Estado actualizado.");
            }
        }

        // ================= REPUESTOS =================
        private void MenuRepuestos()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("\n--- Gestión de repuestos ---");
                Console.WriteLine("1. Registrar repuesto");
                Console.WriteLine("2. Consultar repuesto");
                Console.WriteLine("3. Listar repuestos");
                Console.WriteLine("4. Alertas de existencia mínima / agotados");
                Console.WriteLine("0. Volver");
                int op = Validador.LeerEntero("Opción: ", 0, 4);

                switch (op)
                {
                    case 1:
                        string nombre = Validador.LeerTexto("Nombre: ");
                        string categoria = Validador.LeerTexto("Categoría: ");
                        int existencia = Validador.LeerEntero("Existencia: ", 0, 100000);
                        int minima = Validador.LeerEntero("Existencia mínima: ", 0, 100000);
                        double costo = Validador.LeerDecimal("Costo unitario (Q): ", 0);
                        double precio = Validador.LeerDecimal("Precio de venta (Q): ", 0);
                        Repuesto nuevo = gestorRepuestos.Registrar(nombre, categoria, existencia, minima, costo, precio);
                        if (nuevo != null) Console.WriteLine("Repuesto registrado con código " + nuevo.CodigoMostrar + " (número: " + nuevo.Codigo + ")");
                        break;
                    case 2:
                        Repuesto r = gestorRepuestos.BuscarPorCodigo(LeerCodigo("Código del repuesto (número): "));
                        if (r == null) Console.WriteLine("El repuesto no existe.");
                        else r.MostrarInformacion();
                        break;
                    case 3: gestorRepuestos.Listar(); break;
                    case 4: reportes.Reporte9(); break;
                    case 0: volver = true; break;
                }
            }
        }

        // ================= ATENCION DE COLAS =================
        private void MenuColas()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("\n--- Atención de colas ---");
                Console.WriteLine("1. Atender siguiente solicitud (asigna técnico)");
                Console.WriteLine("2. Ver solicitudes en espera");
                Console.WriteLine("3. Ver matriz de carga semanal");
                Console.WriteLine("0. Volver");
                int op = Validador.LeerEntero("Opción: ", 0, 3);

                switch (op)
                {
                    case 1: gestorSolicitudes.AtenderSiguiente(); break;
                    case 2: reportes.Reporte1(); break;
                    case 3: reportes.Reporte12(); break;
                    case 0: volver = true; break;
                }
            }
        }

        // ================= ORDENAMIENTOS =================
        private void MenuOrdenamientos()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("\n--- Ordenamientos ---");
                Console.WriteLine("1. Bubble Sort: solicitudes por costo total");
                Console.WriteLine("2. Selection Sort: técnicos por servicios finalizados");
                Console.WriteLine("3. Shell Sort: repuestos por existencia");
                Console.WriteLine("4. Quick Sort: solicitudes por fecha de recepción");
                Console.WriteLine("0. Volver");
                int op = Validador.LeerEntero("Opción: ", 0, 4);

                switch (op)
                {
                    case 1: reportes.Reporte6(); break;
                    case 2: reportes.Reporte4(); break;
                    case 3: reportes.Reporte8(); break;
                    case 4: reportes.Reporte7(); break;
                    case 0: volver = true; break;
                }
            }
        }

        // ================= BUSQUEDAS =================
        private void MenuBusquedas()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("\n--- Búsquedas ---");
                Console.WriteLine("1. Secuencial: clientes por nombre parcial");
                Console.WriteLine("2. Secuencial: solicitudes por estado");
                Console.WriteLine("3. Secuencial: equipos por marca");
                Console.WriteLine("4. Binaria: solicitud por código");
                Console.WriteLine("0. Volver");
                int op = Validador.LeerEntero("Opción: ", 0, 4);

                switch (op)
                {
                    case 1: BuscarClientes(); break;
                    case 2: BuscarSolicitudes(); break;
                    case 3: BuscarEquipos(); break;
                    case 4: BuscarBinaria(); break;
                    case 0: volver = true; break;
                }
            }
        }

        private void BuscarClientes()
        {
            string texto = Validador.LeerTexto("Nombre (o parte): ");
            List<Cliente> res = Busqueda.BuscarClientesPorNombre(gestorClientes.Clientes, texto);
            Console.WriteLine("Búsqueda SECUENCIAL de clientes por nombre: " + res.Count + " coincidencia(s).");
            foreach (Cliente c in res) c.MostrarInformacion();
        }

        private void BuscarSolicitudes()
        {
            Console.WriteLine("1=Recibida 2=En espera 3=Asignada 4=En diagnóstico 5=En reparación");
            Console.WriteLine("6=Lista para entregar 7=Entregada 8=Cancelada");
            int op = Validador.LeerEntero("Estado: ", 1, 8);
            List<Solicitud> res = Busqueda.BuscarSolicitudesPorEstado(gestorSolicitudes.Solicitudes, (EstadoSolicitud)(op - 1));
            Console.WriteLine("Búsqueda SECUENCIAL de solicitudes por estado: " + res.Count + " coincidencia(s).");
            foreach (Solicitud s in res) s.MostrarInformacion();
        }

        private void BuscarEquipos()
        {
            string marca = Validador.LeerTexto("Marca: ");
            List<Equipo> res = Busqueda.BuscarEquiposPorMarca(gestorEquipos.Equipos, marca);
            Console.WriteLine("Búsqueda SECUENCIAL de equipos por marca: " + res.Count + " coincidencia(s).");
            foreach (Equipo e in res) e.MostrarInformacion();
        }

        private void BuscarBinaria()
        {
            int codigo = LeerCodigo("Código de la solicitud (número): ");
            int pos = Busqueda.BusquedaBinaria(gestorSolicitudes.Solicitudes, codigo);
            if (pos >= 0)
            {
                Console.WriteLine("Búsqueda BINARIA: encontrada en la posición " + pos + ".");
                gestorSolicitudes.Solicitudes[pos].MostrarDetalle();
            }
            else
            {
                Console.WriteLine("Búsqueda BINARIA: no se encontró la solicitud.");
            }
        }

        // ================= REPORTES =================
        private void MenuReportes()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("\n--- Reportes ---");
                Console.WriteLine(" 1. Solicitudes en espera por prioridad");
                Console.WriteLine(" 2. Solicitudes activas, finalizadas y canceladas");
                Console.WriteLine(" 3. Técnicos disponibles y con carga completa");
                Console.WriteLine(" 4. Técnico con más servicios finalizados");
                Console.WriteLine(" 5. Equipos atendidos por tipo");
                Console.WriteLine(" 6. Solicitudes ordenadas por costo total");
                Console.WriteLine(" 7. Solicitudes ordenadas por fecha");
                Console.WriteLine(" 8. Repuestos ordenados por existencia");
                Console.WriteLine(" 9. Repuestos en mínimo o agotados");
                Console.WriteLine("10. Ingresos totales por servicios finalizados");
                Console.WriteLine("11. Resultados de las formas de búsqueda");
                Console.WriteLine("12. Matriz semanal de carga de trabajo");
                Console.WriteLine(" 0. Volver");
                int op = Validador.LeerEntero("Opción: ", 0, 12);

                switch (op)
                {
                    case 1: reportes.Reporte1(); break;
                    case 2: reportes.Reporte2(); break;
                    case 3: reportes.Reporte3(); break;
                    case 4: reportes.Reporte4(); break;
                    case 5: reportes.Reporte5(); break;
                    case 6: reportes.Reporte6(); break;
                    case 7: reportes.Reporte7(); break;
                    case 8: reportes.Reporte8(); break;
                    case 9: reportes.Reporte9(); break;
                    case 10: reportes.Reporte10(); break;
                    case 11:
                        Console.WriteLine("\n=== REPORTE: RESULTADOS DE LAS BÚSQUEDAS ===");
                        BuscarClientes();
                        BuscarSolicitudes();
                        BuscarEquipos();
                        BuscarBinaria();
                        break;
                    case 12: reportes.Reporte12(); break;
                    case 0: volver = true; break;
                }
            }
        }
    }
}
