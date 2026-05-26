using SemilleroGR3.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;

namespace SemilleroGR3.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string mensajeError;

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        public async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MensajeError = "Por favor, ingresa correo y contraseña.";
                return;
            }

            IsBusy = true;
            MensajeError = string.Empty;

            var usuario = await _authService.LoginAsync(Email, Password);

            IsBusy = false;

            if (usuario != null)
            {
                // Navegamos a la pantalla principal (AppShell)
                await Shell.Current.GoToAsync("//Dashboard");
            }
            else
            {
                MensajeError = "Credenciales incorrectas o error de conexión.";
            }
        }
    }
}
