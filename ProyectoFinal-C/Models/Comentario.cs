namespace ProyectoFinal_C.Models
{
    public class Comentario
    {
        public int? id_comentario { get; set; }
        public string? contenido_comentario { get; set; }
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // ID del post en el que se hizo el comentario
        public int? PostId { get; set; }
        public Post? Post { get; set; }
    }
}
