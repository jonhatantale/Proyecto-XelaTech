namespace XelaTechSoporte
{
    public enum TipoEquipo { ComputadoraPortatil, ComputadoraEscritorio, Impresora }

    public enum TipoCliente { Estudiante, Profesional, NegocioPequeno, Institucion }

    public enum Prioridad { Normal, Urgente }

    // El orden importa: DeshacerEstado compara estos valores como numeros
    public enum EstadoSolicitud
    {
        Recibida,
        EnEspera,
        Asignada,
        EnDiagnostico,
        EnReparacion,
        ListaParaEntregar,
        Entregada,
        Cancelada
    }
}
