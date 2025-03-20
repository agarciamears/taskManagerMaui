using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using gestorTareasaMaui.Models;
using gestorTareasaMaui;

namespace gestorTareasaMaui.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private ObservableCollection<tarea> _tareasList;
        private string _estadoFiltro;
        private ICommand _onAgregarTareaCommand;
        private ICommand _onEditarTareaCommand;
        private ICommand _onBorrarTareaCommand;

        public MainPageViewModel()
        {
            _tareasList = new ObservableCollection<tarea>();
            LoadTareas();
        }

        public ObservableCollection<tarea> TareasList
        {
            get => _tareasList;
            set => SetProperty(ref _tareasList, value);
        }

      public string EstadoFiltro
        {
            get => _estadoFiltro;
            set
            {
                SetProperty(ref _estadoFiltro, value);
                LoadTareas(value);
            }
        }

        public ICommand OnAgregarTareaCommand => _onAgregarTareaCommand ??= new Command(OnAgregarTarea);
        public ICommand OnEditarTareaCommand => _onEditarTareaCommand ??= new Command<tarea>(OnEditarTarea);
        public ICommand OnBorrarTareaCommand => _onBorrarTareaCommand ??= new Command<tarea>(OnBorrarTarea);

        private async void LoadTareas(string estadoFiltro = null)
        {
            var tareas = await App.Database.GetTareasAsync();

            if (!string.IsNullOrEmpty(estadoFiltro) && estadoFiltro != "Mostrar Todo")
            {
                tareas = tareas.Where(t => t.estado == estadoFiltro).ToList();
            }

            TareasList.Clear();
            foreach (var tarea in tareas)
            {
                TareasList.Add(tarea);
                Console.WriteLine($"Tarea agregada: {tarea.nombre}, {tarea.descripcion}, {tarea.estado}");

            }
        }

        private async void OnAgregarTarea()
        {
            string nombre = await App.Current.MainPage.DisplayPromptAsync("Nombre", "Ingrese el nombre de la tarea");
            string descripcion = await App.Current.MainPage.DisplayPromptAsync("Descripcion", "Ingrese la descripcion de la tarea");
            string estado = await App.Current.MainPage.DisplayActionSheet("Estado", "Cancelar", null, "Pendiente", "Proceso", "Completada");

            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(descripcion))
            {
                var nuevaTarea = new tarea
                {
                    nombre = nombre,
                    descripcion = descripcion,
                    estado = estado
                };

                await App.Database.SaveTareaAsync(nuevaTarea);
                TareasList.Add(nuevaTarea);
                await App.Current.MainPage.DisplayAlert("Tarea Agregada", "La tarea se ha agregado correctamente", "OK");
            }
        }

        private async void OnEditarTarea(tarea tarea)
        {
            string nuevoNombre = await App.Current.MainPage.DisplayPromptAsync("Editar Tarea", "Nombre de la tarea:", initialValue: tarea.nombre);
            string nuevaDescripcion = await App.Current.MainPage.DisplayPromptAsync("Editar Tarea", "Descripción de la tarea:", initialValue: tarea.descripcion);
            string nuevoEstado = await App.Current.MainPage.DisplayActionSheet("Estado", "Cancelar", null, "Pendiente", "Proceso", "Completada");

            if (!string.IsNullOrEmpty(nuevoNombre) && !string.IsNullOrEmpty(nuevaDescripcion))
            {
                tarea.nombre = nuevoNombre;
                tarea.descripcion = nuevaDescripcion;
                tarea.estado = nuevoEstado;

                await App.Database.SaveTareaAsync(tarea);
                LoadTareas();  // Recargar la lista de tareas
            }
        }

        private async void OnBorrarTarea(tarea tarea)
        {
            await App.Database.DeleteTareaAsync(tarea);
            TareasList.Remove(tarea);  // Eliminar la tarea de la lista
            await App.Current.MainPage.DisplayAlert("Tarea Eliminada", "La tarea se ha eliminado correctamente", "OK");
        }
    }
}
