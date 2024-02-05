using Newtonsoft.Json;

namespace ProyectoFinal_C.Models
{
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
        [JsonProperty("token_usuario")]
        public string? token_usuario {  get; set; }
        
    }
}
