using System.Net.Http.Json;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using System.Security.Claims;


namespace ECommerceWebAppFrontend.Services
{
   

   public class LoginService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly CustomAuthStateProvider _authProvider;

    public LoginService(IJSRuntime jsRuntime, CustomAuthStateProvider authProvider)
    {
        _jsRuntime = jsRuntime;
        _authProvider = authProvider;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var token = await _jsRuntime.InvokeAsync<string>(
                "firebase.signInWithEmailAndPassword", email, password);

            if (!string.IsNullOrEmpty(token))
            {
                await GuardarCredenciales(token, email);

                // Crear identidad y ClaimsPrincipal
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, email)
                }, "firebase");

                var user = new ClaimsPrincipal(identity);

                // Actualizar el estado de autenticación
                _authProvider.SetUser(user);

                return true;
            }
        }
        catch (JSException jsEx)
        {
            Console.WriteLine($"[LoginService] JS Error al iniciar sesión: {jsEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LoginService] Error general al iniciar sesión: {ex.Message}");
        }

        return false;
    }

    private Task GuardarCredenciales(string token, string email)
    {
        // Aquí puedes guardar el token y correo en localStorage si deseas
        Console.WriteLine($"Token guardado: {token} para {email}");
        return Task.CompletedTask;
    }
    
    




        public async Task<bool> RegistrarAsync(string email, string password)
            {
                try
                {
                    var token = await _jsRuntime.InvokeAsync<string>(
                        "firebase.createUserWithEmailAndPassword", email, password);

                    if (!string.IsNullOrEmpty(token))
                    {
                        await GuardarCredenciales(token, email);
                        return true;
                    }
                }
                catch (JSException jsEx)
                {
                    Console.WriteLine($"[LoginService] JS Error al registrar usuario: {jsEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[LoginService] Error general al registrar: {ex.Message}");
                }

                return false;
            }

        public async Task<bool> IsAuthenticated()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "firebase_token");
            return !string.IsNullOrWhiteSpace(token);
        }

        public async Task<string> GetUserEmail()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userEmail") ?? "";
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("firebase.signOut");
            }
            catch (JSException jsEx)
            {
                Console.WriteLine($"[LoginService] JS Error al cerrar sesión: {jsEx.Message}");
            }

            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "firebase_token");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userEmail");
        }

        
    }
}
