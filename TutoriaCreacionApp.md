# Tutorial Paso a Paso para Construir una Aplicación de Gestor de Tareas en .NET MAUI

## Introducción
En este tutorial, aprenderás a construir una aplicación de gestor de tareas en .NET MAUI desde cero. La aplicación permitirá agregar, editar y borrar tareas, y utilizará SQLite para el almacenamiento local. Este tutorial está diseñado para llevarte de la mano, paso a paso, con explicaciones teóricas y consejos prácticos.

## Requisitos Previos
- Visual Studio 2022 o superior  
- .NET 6.0 o superior  
- Conocimientos básicos de C# y XAML  

## Paso 1: Configuración del Proyecto
1. Crear un nuevo proyecto MAUI:
   - Abre Visual Studio y selecciona "Crear un nuevo proyecto".
   - Elige "Aplicación .NET MAUI" y dale un nombre a tu proyecto, por ejemplo, `GestorDeTareas`.
   - Configura el entorno asegurándote de tener instaladas las herramientas necesarias para MAUI.

## Paso 2: Crear la Interfaz de Usuario (UI)
1. Diseñar la pantalla principal:
   - Abre el archivo `MainPage.xaml` y añade el siguiente código para definir una lista simple utilizando `CollectionView` y `Border`:

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="gestorTareasaMaui.MainPage">

    <StackLayout Padding="10">
        <Label Text="Gestor de Tareas" FontSize="24" HorizontalOptions="Center" />
        <ScrollView>
            <CollectionView x:Name="TareasCollectionView">
                <CollectionView.ItemTemplate>
                    <DataTemplate>
                        <SwipeView>
                            <SwipeView.RightItems>
                                <SwipeItems>
                                    <SwipeItem Text="Editar" BackgroundColor="Blue" Invoked="OnEditarTareaCommand" CommandParameter="{Binding .}" />
                                    <SwipeItem Text="Borrar" BackgroundColor="Red" Invoked="OnBorrarTareaCommand" CommandParameter="{Binding .}" />
                                </SwipeItems>
                            </SwipeView.RightItems>
                            <Border Stroke="LightGray" StrokeThickness="1" CornerRadius="10" Padding="10" Margin="5">
                                <StackLayout>
                                    <Label Text="{Binding nombre}" FontSize="18" FontAttributes="Bold" />
                                    <Label Text="{Binding descripcion}" FontSize="14" />
                                </StackLayout>
                            </Border>
                        </SwipeView>
                    </DataTemplate>
                </CollectionView.ItemTemplate>
            </CollectionView>
        </ScrollView>
        <Button Text="Agregar Tarea" Clicked="OnAgregarTareaClicked" />
    </StackLayout>
</ContentPage>
```

### Explicación Teórica
- **CollectionView**: Es un control flexible y eficiente para mostrar listas de datos en MAUI.
- **SwipeView**: Permite agregar acciones deslizables a los elementos de la lista, como editar y borrar.
- **Border**: Es un contenedor que permite agregar bordes y esquinas redondeadas a los elementos.

## Paso 3: Crear el Modelo de Datos
1. Definir la clase `Tarea`:
   - Añade un nuevo archivo llamado `tarea.cs` y define la clase de la siguiente manera:

```csharp
using SQLite;

public class tarea
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string nombre { get; set; }
    public string descripcion { get; set; }
    public string estado { get; set; }
}
```

### Explicación Teórica
- **SQLite**: Base de datos ligera y autosuficiente.
- **PrimaryKey y AutoIncrement**: Definen la clave primaria autoincremental.

## Paso 4: Manejar la Lógica de Negocio
1. Agregar tareas a la lista en `MainPage.xaml.cs`:

```csharp
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace gestorTareasaMaui
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<tarea> tareasList;

        public MainPage()
        {
            InitializeComponent();
            tareasList = new ObservableCollection<tarea>();
            TareasCollectionView.ItemsSource = tareasList;
            LoadTareas();
        }

        private async void LoadTareas()
        {
            var tareas = await App.Database.GetTareasAsync();
            tareasList.Clear();
            foreach (var tarea in tareas)
            {
                tareasList.Add(tarea);
            }
        }

        private async void OnAgregarTareaClicked(object sender, EventArgs e)
        {
            string nombre = await DisplayPromptAsync("Nombre", "Ingrese el nombre de la tarea");
            string descripcion = await DisplayPromptAsync("Descripcion", "Ingrese la descripcion de la tarea");
            string estado = await DisplayActionSheet("Estado", "Cancelar", null, "Pendiente", "En Proceso", "Completada");

            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(descripcion))
            {
                tarea nuevaTarea = new tarea()
                {
                    nombre = nombre,
                    descripcion = descripcion,
                    estado = estado
                };
                await App.Database.SaveTareaAsync(nuevaTarea);
                tareasList.Add(nuevaTarea);
            }
        }
    }
}
```

### Explicación Teórica
- **ObservableCollection**: Notifica automáticamente a la UI cuando hay cambios en la lista.
- **DisplayPromptAsync**: Muestra cuadros de entrada de texto.
- **DisplayActionSheet**: Presenta opciones para elegir un estado.

## Paso 5: Implementar Almacenamiento Local con SQLite
1. Instalar el paquete NuGet `sqlite-net-pcl`:

```sh
dotnet add package sqlite-net-pcl
```

2. Crear la base de datos y la tabla en `Database.cs`:

```csharp
public class Database
{
    private readonly SQLiteAsyncConnection _database;

    public Database(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<tarea>().Wait();
    }

    public Task<List<tarea>> GetTareasAsync()
    {
        return _database.Table<tarea>().ToListAsync();
    }

    public Task<int> SaveTareaAsync(tarea tarea)
    {
        if (tarea.Id != 0)
        {
            return _database.UpdateAsync(tarea);
        }
        else
        {
            return _database.InsertAsync(tarea);
        }
    }
}
```

## Paso 6: Probar la Aplicación
1. Ejecutar la aplicación:
   - Ejecuta la aplicación en un emulador o dispositivo físico.
   - Verifica que las tareas se guardan y persisten en la base de datos.
   - Prueba las funcionalidades de agregar, editar y borrar tareas.
