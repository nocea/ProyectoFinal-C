using Newtonsoft.Json;

namespace ProyectoFinal_C.Models
{   //esto lo tuve que hacer asi porque si no al deserializar la respuesta de un objeto usuario de la api me daba error ya que me devilvia el usuario:atributos
    //y no los atributos directamente.
    public class UsuarioResponse
    {
        [JsonProperty("usuario")]
        public Usuario Usuario { get; set; }
    }
    public class Usuario
    {
        [JsonProperty("id_usuario")]
        public int? id_usuario { get; set; }
        [JsonProperty("nombreCompleto_usuario")]
        public string? nombreCompleto_usuario { get; set; }
        [JsonProperty("rol_usuario")]
        public string? rol_usuario { get; set; }
        [JsonProperty("movil_usuario")]
        public int? movil_usuario { get; set; }
        [JsonProperty("alias_usuario")]
        public string? alias_usuario { get; set; }
        [JsonProperty("email_usuario")]
        public string? email_usuario { get; set; }
        [JsonProperty("passwd_usuario")]
        public string? passwd_usuario { get; set; }
        public byte[]? imagen_usuario { get; set; }
       
    }
}
