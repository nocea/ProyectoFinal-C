using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using ProyectoFinal_C.Models;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace ProyectoFinal_C.Controllers
{   /// <summary>
/// Filtro para que cuadno se quieran acceder a controladores con este filtro el usuario que esta accediendo tenga de rol ADMIN
/// </summary>
    public class AdminAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            // Obtener el rol del usuario actual
            var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
            string nombreUsuario = claimsIdentity.Name;
            Usuario usuario;
            string rol_usuario;
            string apiUrl1 = "https://localhost:7289/api/Controlador_Admin/ObtenerUsuarioNombre/" + nombreUsuario;
            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage response = await client.GetAsync(apiUrl1);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserializar la respuesta JSON en una lista de usuarios
                    usuario = JsonConvert.DeserializeObject<Usuario>(responseBody);
                    rol_usuario = usuario.rol_usuario;
                    if (rol_usuario != "ADMIN")
                    {
                        // Si el usuario no tiene el rol "ADMIN", denegar el acceso
                        context.Result = new ForbidResult();
                        return;
                    }

                }
                else
                {
                    // Manejar errores al obtener el usuario de la base de datos
                    Console.WriteLine($"Error al obtener el usuario. Código de estado: {response.StatusCode}");
                    context.Result = new StatusCodeResult((int)response.StatusCode);
                    return;
                }
            }
        }
    }
}
