using System.Text;
namespace ProyectoFinal_C.Utils
{
    public class Utils
    {
        // Ruta del archivo de registro
        private static readonly string logFilePath = "log.txt";
        // Método para escribir en el registro
        public static void Log(string message)
        {
            // Obtiene la marca de tiempo actual.
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Escribe el mensaje de registro en el archivo.
            using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
            {
                writer.WriteLine($"[{timeStamp}] {message}");
            }

            // Muestra el mensaje en la consola (opcional).
            Console.WriteLine($"[{timeStamp}] {message}");
        }
    }
}
