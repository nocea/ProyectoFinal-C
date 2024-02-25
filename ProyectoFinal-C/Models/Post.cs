using Newtonsoft.Json;

namespace ProyectoFinal_C.Models
{
    public class PostResponse
    {
        [JsonProperty("post")]
        public Post Post { get; set; }
    }
    public class Post
    {
        [JsonProperty("id_post")]
        public int? id_post { get; set; }
        [JsonProperty("titulo_post")]
        public string? titulo_post { get; set; }
        [JsonProperty("pie_post")]
        public string? pie_post { get; set; }
        [JsonProperty("imagen_post")]
        public byte[]? imagen_post { get; set; }
        [JsonIgnore]
        public IFormFile ImagenFile { get; set; }
        [JsonProperty("UsuarioId")]
        public int? UsuarioId { get; set; }
        [JsonProperty("Usuario")]
        // Propiedad de navegación al usuario que publicó el post
        public Usuario? Usuario { get; set; }
        [JsonProperty("Comentarios")]
        public ICollection<Comentario>? Comentarios { get; set; }

    }
}
