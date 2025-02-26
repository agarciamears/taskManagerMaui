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
             x:Class="gestorTareasaMaui.MainPage"
              BackgroundColor="{AppThemeBinding Light={StaticResource BackgroundColorLight}, Dark={StaticResource BackgroundColorDark}}">
    <ScrollView>
        <StackLayout Padding="10" BackgroundColor="White">
            <Label Text="Mi Primer Gestor de Tareas" FontSize="22" HorizontalOptions="Center" TextColor="Purple" />
            <CollectionView x:Name="TareasCollectionView" BackgroundColor="White">
                <CollectionView.ItemTemplate>
                    <DataTemplate>
                        <SwipeView>
                            <SwipeView.LeftItems>
                                <SwipeItems>
                                    <SwipeItem Text="Editar" BackgroundColor="Blue" Invoked="OnEditarTareaCommand" CommandParameter="{Binding .}" />
                                </SwipeItems>
                            </SwipeView.LeftItems>
                            <SwipeView.RightItems>
                                <SwipeItems>
                                    <SwipeItem Text="Borrar" BackgroundColor="Red"  Invoked="OnBorrarTareaCommand"  CommandParameter="{Binding .}" />
                                </SwipeItems>
                            </SwipeView.RightItems>
                            <Border Stroke="LightGray" StrokeThickness="1" Padding="10" Margin="5">
                                <StackLayout>
                                    <Label Text="{Binding nombre}" FontSize="18" FontAttributes="Bold" />
                                    <Label Text="{Binding descripcion}" FontSize="14" />
                                    <Label Text="{Binding estado}" FontSize="12" TextColor="Green" />
                                </StackLayout>
                            </Border>
                        </SwipeView>
                    </DataTemplate>
                </CollectionView.ItemTemplate>
            </CollectionView>
            <Button Text="Agregar Tarea" Clicked="OnAgregarTareaClicked" />
        </StackLayout>
    </ScrollView>
</ContentPage>
```

### Explicación Teórica
- **CollectionView**: Es un control flexible y eficiente para mostrar listas de datos en MAUI.
- **SwipeView**: Permite agregar acciones deslizables a los elementos de la lista, como editar y borrar.
- **Border**: Es un contenedor que permite agregar bordes y esquinas redondeadas a los elementos.

## Paso 3: Crear el Modelo de Datos
1. Definir la clase `Tarea`:
   - Añade un nuevo archivo llamado  `tarea.cs` en raiz y define la clase de la siguiente manera:

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
using System.Windows.Input;

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

        
        //Metodo para cargar las tareas de la base de datos que ya persisten.
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

                //Guardar la tarea en la base de datos
                await App.Database.SaveTareaAsync(nuevaTarea);

                tareasList.Add(nuevaTarea);
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
                string nuevoEstado = await DisplayActionSheet("Estado", "Cancelar", null, "Pendiente", "En Proceso", "Completada");

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

2. Crear la base de datos y la tabla en `database.cs` paea ello tenemos que agregar la clase a nuestros archivos raiz:

```csharp
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Creando clase de base de datos:
namespace gestorTareasaMaui
{
     public class database
    {
        //Creando conexion a la base de datos:
        private readonly SQLiteAsyncConnection _database;

 
        public database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<tarea>().Wait();
        }

        //Metodo para obtener todas las tareas de la base de datos:
        public Task<List<tarea>> GetTareasAsync()
        {
            return _database.Table<tarea>().ToListAsync();
        }

        //Metdo para guardar tarea en la base de datos:
        public Task<int> SaveTareaAsync(tarea tarea)
        {
            if (tarea.id != 0)
            {
                return _database.UpdateAsync(tarea);
            }
            else
            {
                return _database.InsertAsync(tarea);
            }
        }

        //Metodo para eliminar tarea de la base de datos:
        public Task<int> DeleteTareaAsync(tarea tarea)
        {
            return _database.DeleteAsync(tarea);
        }
    }
}
```

## Paso 6: Probar la Aplicación
1. Ejecutar la aplicación:
   - Ejecuta la aplicación en un emulador o dispositivo físico.
   - Verifica que las tareas se guardan y persisten en la base de datos.
   - Prueba las funcionalidades de agregar, editar y borrar tareas.

## Paso 7 (Opcional): Agregar una Página de Perfil al Menú de Navegación

En este paso, agregaremos una nueva página **Perfil** al menú de la aplicación utilizando `Shell`.

### **1. Agregar el ítem de navegación en `AppShell.xaml`**

Abre el archivo `AppShell.xaml` y modifícalo para incluir la nueva página de perfil:

```xml
<Shell
    x:Class="gestorTareasaMaui.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:gestorTareasaMaui"
    Shell.FlyoutBehavior="Flyout">

    <ShellContent
        Title="Home"
        ContentTemplate="{DataTemplate local:MainPage}"
        Route="MainPage" />

    <ShellContent
        Title="Perfil"
        ContentTemplate="{DataTemplate local:PerfilPage}"
        Route="UserPage" />
</Shell>
```

### **2. Crear la Página `UserPage.xaml`**

Agrega un nuevo archivo **UserPage.xaml** y copia el siguiente código:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="gestorTareasaMaui.UserPage"
             Title="My Profile">

    <ContentPage.Background>
        <LinearGradientBrush>
            <GradientStop Color="#6A0DAD" Offset="0.1"/>
            <GradientStop Color="#9B30FF" Offset="1.0"/>
        </LinearGradientBrush>
    </ContentPage.Background>

    <VerticalStackLayout Padding="30" Spacing="25" VerticalOptions="Center">

        <!-- Avatar Redondeado -->
        <Border Stroke="White" StrokeThickness="3" Background="White"
                WidthRequest="120" HeightRequest="120" StrokeShape="Ellipse"
                HorizontalOptions="Center">
            <Image Source="dotnet_bot.png" WidthRequest="100" HeightRequest="100"/>
        </Border>

        <!-- Tarjeta de Información -->
        <Border BackgroundColor="White" StrokeThickness="0" StrokeShape="RoundRectangle 15"
                Padding="20" Margin="5">
            <Grid RowSpacing="10">
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto"/>
                    <RowDefinition Height="Auto"/>
                    <RowDefinition Height="Auto"/>
                    <RowDefinition Height="Auto"/>
                </Grid.RowDefinitions>

                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="Auto"/>
                    <ColumnDefinition Width="*"/>
                </Grid.ColumnDefinitions>

                <Label Text="Name:" FontAttributes="Bold" TextColor="#6A0DAD" Grid.Row="0" Grid.Column="0"/>
                <Label Text="Alex Maui" FontSize="18" TextColor="Black" Grid.Row="0" Grid.Column="1"/>

                <Label Text="Username:" FontAttributes="Bold" TextColor="#6A0DAD" Grid.Row="1" Grid.Column="0"/>
                <Label Text="@agmmauideveloper" FontSize="18" TextColor="Black" Grid.Row="1" Grid.Column="1"/>

                <Label Text="Email:" FontAttributes="Bold" TextColor="#6A0DAD" Grid.Row="2" Grid.Column="0"/>
                <Label Text="alexmaui@email.com" FontSize="18" TextColor="Black" Grid.Row="2" Grid.Column="1"/>

                <Label Text="Location:" FontAttributes="Bold" TextColor="#6A0DAD" Grid.Row="3" Grid.Column="0"/>
                <Label Text="Ciudad de México, MX" FontSize="18" TextColor="Black" Grid.Row="3" Grid.Column="1"/>
            </Grid>
        </Border>

        <!-- Botón de edición -->
        <Button Text="Editar Perfil"
                BackgroundColor="#6A0DAD"
                TextColor="White"
                CornerRadius="20"
                FontAttributes="Bold"
                WidthRequest="200"
                HorizontalOptions="Center"/>
    </VerticalStackLayout>
    
</ContentPage>
```

### **3. Crear el Código `UserPage.xaml.cs`**

Agrega un archivo `UserPage.xaml.cs` y define la clase para la nueva página:

```csharp
namespace gestorTareasaMaui
{
    public partial class PerfilPage : ContentPage
    {
        public PerfilPage()
        {
            InitializeComponent();
        }
    }
}
```

### **4. Ejecutar la Aplicación**
Prueba la aplicación y verifica que el nuevo ítem de perfil se muestra en el menú de navegación. Puedes personalizar la página de perfil y agregar más información según tus necesidades.

