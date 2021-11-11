namespace LeonardoSuarezCasas.Model
{
    /// <summary>
    /// Estado de las casas tras la actualizacion de celdas
    /// </summary>
    public class EstadoCasasActualizado
    {
        public int Dias { get; set; }
        public int[] Entrada { get; set; }
        public int[] Salida { get; set; }
    }
}
