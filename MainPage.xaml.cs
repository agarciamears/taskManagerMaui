﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using gestorTareasaMaui.Models;

namespace gestorTareasaMaui
{
    public partial class MainPage : ContentPage
    {
        
        
        private ObservableCollection<tarea> tareasList;


        public MainPage()
        {
            InitializeComponent();
            tareasList = new ObservableCollection<tarea> ();
            TareasCollectionView.ItemsSource = tareasList;
            LoadTareas();
        }


        // Método para cargar las tareas desde la base de datos con picker agregado

        private async void LoadTareas(string estadoFiltro = null) //Agregamos Variable dentro del metodo
        {
            var tareas = await App.Database.GetTareasAsync();
            tareasList.Clear();

            //------------  Filtrar tareas si se especifica un estado dentro --------------------------
            if (!string.IsNullOrEmpty(estadoFiltro) && estadoFiltro != "Mostrar Todo")
            {
                tareas = tareas.Where(t => t.estado == estadoFiltro).ToList();
            }

            // Agregar las tareas a la colección
            foreach (var tarea in tareas)
            {
                tareasList.Add(tarea);
            }
        }

        // Evento cuando el usuario cambia el filtro de estado
        private void OnEstadoChanged(object sender, EventArgs e)
        {
            string estadoSeleccionado = EstadoPicker.SelectedItem as string;
            LoadTareas(estadoSeleccionado);  // Cargar las tareas filtradas por estado
        }

        private async void OnAgregarTareaClicked(object sender, EventArgs e)
        {

            string nombre = await DisplayPromptAsync("Nombre", "Ingrese el nombre de la tarea");    
            string descripcion = await DisplayPromptAsync("Descripcion", "Ingrese la descripcion de la tarea");
            string estado = await DisplayActionSheet("Estado", "Cancelar", null, "Pendiente", "Proceso", "Completada");

            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(descripcion))
            {
                tarea nuevaTarea = new tarea()
                {
                    nombre = nombre,
                    descripcion = descripcion,
                    estado = estado
                };

                //Guardar la tarea en la base de datos
                await App.Database.SaveTareaAsync(nuevaTarea);

                tareasList.Add(nuevaTarea);
                await DisplayAlert("Tarea Agregada", "La tarea se ha agregado correctamente", "OK");
                //TareasListView.ItemsSource = null;
                //TareasListView.ItemsSource = tareasList;
                // LoadTareas();
            }

        }

        //Borrando tarea de la Lista y de la base de datos
        private async void OnBorrarTareaCommand(object sender, EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            var tarea = swipeItem?.CommandParameter as tarea;

            if (tarea != null)
            {
                await App.Database.DeleteTareaAsync(tarea);
                tareasList.Remove(tarea); // Eliminar la tarea de la colección observable
                await DisplayAlert("Tarea Eliminada", "La tarea se ha eliminado correctamente", "OK");
            }
        }

        // Editando tarea de la Lista y de la base de datos
        private async void OnEditarTareaCommand(object sender, EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            var tarea = swipeItem?.CommandParameter as tarea;

            if (tarea != null)
            {
                string nuevoNombre = await DisplayPromptAsync("Editar Tarea", "Nombre de la tarea:", initialValue: tarea.nombre);
                string nuevaDescripcion = await DisplayPromptAsync("Editar Tarea", "Descripción de la tarea:", initialValue: tarea.descripcion);
                string nuevoEstado = await DisplayActionSheet("Estado", "Cancelar", null, "Pendiente", "Proceso", "Completada");

                if (!string.IsNullOrEmpty(nuevoNombre) && !string.IsNullOrEmpty(nuevaDescripcion))
                {
                    tarea.nombre = nuevoNombre;
                    tarea.descripcion = nuevaDescripcion;
                    tarea.estado = nuevoEstado;

                    // Actualizar la tarea en la base de datos
                    await App.Database.SaveTareaAsync(tarea);
                   
                    // Recargar la lista de tareas
                    LoadTareas();
                }
            }
        }

    }

}
