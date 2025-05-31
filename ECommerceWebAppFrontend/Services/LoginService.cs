using System.Net.Http.Json;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace ECommerceWebAppFrontend.Services
{
    public class LoginService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;

        public LoginService(IJSRuntime jsRuntime, HttpClient httpClient)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
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

        private async Task GuardarCredenciales(string token, string email)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "firebase_token", token);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userEmail", email);
        }
    }
}
