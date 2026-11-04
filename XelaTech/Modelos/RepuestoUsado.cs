namespace XelaTechSoporte
{
    // Une una Solicitud con un Repuesto (relacion muchos a muchos)
    public class RepuestoUsado
    {
        private Repuesto repuesto;
        private int cantidad;

        public RepuestoUsado(Repuesto repuesto, int cantidad)
        {
            this.repuesto = repuesto;
            this.cantidad = cantidad;
        }

        public Repuesto Repuesto
        {
            get { return repuesto; }
        }

        public int Cantidad
        {
            get { return cantidad; }
        }

        public double CalcularSubtotal()
        {
            return repuesto.PrecioVenta * cantidad;
        }
    }
}
