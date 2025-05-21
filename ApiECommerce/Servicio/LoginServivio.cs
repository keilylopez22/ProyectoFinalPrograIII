/*using System.Net.Http.Json;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using ApiECommerce.Modelo;

--PENDIENTE DE RESOLVER---

namespace ECommerceWebAppFrontend.Servicio
{
    public class LoginServicio
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;

        public LoginServicio(IJSRuntime jsRuntime, HttpClient httpClient)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                // Llamar a la API de Firebase para autenticar
                var result = await _jsRuntime.InvokeAsync<string>(
                    "firebase.signInWithEmailAndPassword",
                    email,
                    password
                );

                if (!string.IsNullOrEmpty(result))
                {
                    // Guardar el token en localStorage
                    await _jsRuntime.InvokeVoidAsync(
                        "localStorage.setItem",
                        "firebase_token",
                        result
                    );

                    // Guardar también el email del usuario para mostrarlo en el sistema
                    await _jsRuntime.InvokeVoidAsync(
                        "localStorage.setItem",
                        "userEmail",
                        email
                    );

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en login: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsAuthenticated()
        {
            var token = await _jsRuntime.InvokeAsync<string>(
                "localStorage.getItem",
                "firebase_token"
            );
            return !string.IsNullOrEmpty(token);
        }

        public async Task<string> GetUserEmail()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userEmail");
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("firebase.signOut");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "firebase_token");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userEmail");
        }
    }
} */
