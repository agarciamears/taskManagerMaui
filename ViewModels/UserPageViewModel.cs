using System.Windows.Input;
using gestorTareasaMaui.Models;

namespace gestorTareasaMaui.ViewModels
{
    public class UserPageViewModel : BaseViewModel
    {
        private string _nombre;
        private string _correo;
        private string _location;
        private string _username;
        public UserPageViewModel()
        {
            // Cargar los datos del usuario al iniciar
            CargarDatosUsuario();
        }

        // Nombre que se mostrarán en la UI View.
        public string Nombre
        {
            get => _nombre;
            set => SetProperty(ref _nombre, value);
        }

        // Username que se mostrará en la UI View.
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        // Correo que se mostrará en la UI View.
        public string Correo
        {
            get => _correo;
            set => SetProperty(ref _correo, value);
        }

        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }


        // Método para cargar los datos del usuario
        private void CargarDatosUsuario()
        {
            // Aquí deberíamos cargar los datos del usuario desde una fuente de datos,
            // por ejemplo, desde la base de datos o un servicio.
            // Para este ejemplo, usaremos valores predeterminados.
            Nombre = "Maui Microsoft Workshop 2025";
            Correo = "maui.the.best@microsoft.com";
            Username = "mauimicrosoft2025";
            Location = "Ciudad de México";
        }
    }
}
