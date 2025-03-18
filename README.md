# Tutorial Paso a Paso para Construir una Aplicaci�n de Gestor de Tareas en .NET MAUI

## Introducci�n.
En este tutorial, aprender�s a construir una aplicaci�n de gestor de tareas en .NET MAUI desde cero. La aplicaci�n permitir� agregar, editar y borrar tareas, y utilizar� SQLite para el almacenamiento local. Este tutorial est� dise�ado para llevarte de la mano, paso a paso, con explicaciones te�ricas y consejos pr�cticos.

## Requisitos Previos.
- Visual Studio 2022 o superior  
- .NET 6.0 o superior  
- MAUI Installed
- Conocimientos b�sicos de C# y XAML  
- SQlite-Net-PCL comando: `dotnet add package sqlite-net-pcl

## Estructura del Proyecto

- **/Platforms**: Contiene el c�digo espec�fico de cada plataforma (Android, iOS, Windows, etc.).
- **/Resources**: Contiene recursos como im�genes, fuentes y archivos de dise�o.
- **/Views**: Contiene las vistas de la aplicaci�n.
- **/Models**: Contiene las clases de modelo de datos.

## Paso 1: Configuraci�n del Proyecto.
1. Crear un nuevo proyecto MAUI:
   - Abre Visual Studio y selecciona "Crear un nuevo proyecto".
   - Elige "Aplicaci�n .NET MAUI" y dale un nombre a tu proyecto, por ejemplo, `GestorDeTareas`.
   - Configura el entorno asegur�ndote de tener instaladas las herramientas necesarias para MAUI.


![Crear nuevo proyecto MAUI](Images/MauiTask1.png)

![Nombre proyect: GestorDeTareas](Images/MauiTask2.ong)

## Paso 2: Crear la Interfaz de Usuario (UI)
1. Dise�ar la pantalla principal:
   - Vamos a utilizar un `CollectionView` para mostrar la lista de tareas.
   - CollectionView: Es un control flexible y eficiente para mostrar listas de datos en MAUI.
   - Border: Permite agregar bordes, esquinas redondeadas a los elementos, y demas propiedades.
   - Abre el archivo `MainPage.xaml` y sigue los pasos listados.


### A�adir las etiquetas basicas.

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="gestorTareasaMaui.MainPage"
             BackgroundColor="{AppThemeBinding Light={StaticResource BackgroundColorLight}, Dark={StaticResource BackgroundColorDark}}">
```

### Establece el fondo de la pagina (Ya se encuentra dentro de ContentPage).

```xml
BackgroundColor="{AppThemeBinding Light={StaticResource BackgroundColorLight}, Dark={StaticResource BackgroundColorDark}}"
```

### Agregar SctollVioew y StackLayout.
- ScrollView: Permite desplazarse por la pantalla si el contenido es m�s grande que la pantalla.
- StackLayout: Permite apilar elementos verticalmentE o horizontalmente.
- Padding: Espacio alrededor del contenido. (Entre elementos y bordes)

```xml
<ScrollView>
    <StackLayout Padding="10" BackgroundColor="White">
```

### Agregar un Label con el t�tulo de la aplicaci�n (Podemos agregar algunas propiedades).

```xml
<Label Text="Mi Primer Gestor de Tareas" FontSize="22" HorizontalOptions="Center" TextColor="Purple" />
```

### Agregar un CollectionView para mostrar las tareas.
- ItemTemplate: Define la plantilla de cada elemento de la colecci�n.
- DataTemplate: Define la plantilla de datos para cada elemento.
- SwipeView: Permite agregar acciones deslizables a los elementos de la lista.
- SwipeItems: Define los elementos deslizables (Editar y Borrar).

```xml
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
                       
                               <!-- Aqui se agregan los elementos de la tarea a mostrar (Informaci�n) -->


                            
                        </SwipeView>
                    </DataTemplate>
                </CollectionView.ItemTemplate>
            </CollectionView>
                <!-- Aqui se agrega el boton para agregar tareas -->
```

### Mostrar Informacion de la Tarea.
- Border: Permite agregar bordes y esquinas redondeadas a los elementos.
- StackLayout: Permite apilar elementos verticalmente.
- Label: Muestra texto en la pantalla.
- Binding: Enlaza los datos de la tarea a los elementos de la interfaz de usuario.

```xml
<Border Stroke="LightGray" StrokeThickness="1" Padding="10" Margin="5">
    <StackLayout>
        <Label Text="{Binding nombre}" FontSize="18" FontAttributes="Bold" />
        <Label Text="{Binding descripcion}" FontSize="14" />
        <Label Text="{Binding estado}" FontSize="12" TextColor="Green" />
    </StackLayout>
</Border>
```

### Agregar un bot�n para agregar tareas.
- Button: Permite al usuario interactuar con la aplicaci�n.
- Clicked: Evento que se dispara cuando se hace clic en el bot�n.
- OnAgregarTareaClicked: M�todo que se ejecuta cuando se hace clic en el bot�n.
- Text: Texto que se muestra en el bot�n.

```xml
<Button Text="Agregar Tarea" Clicked="OnAgregarTareaClicked" />
```

### Cerrar las etiquetas de StackLayout, ScrollView y ContentPage.

```xml
    </StackLayout>
</ScrollView>
</ContentPage>
```

### Codigo Completo de `MainPage.xaml`:

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

## Paso 3: Crear el Modelo de Datos
1. Definir la clase `Tarea`:
   - Crea una carpeta llamada `Models` en la ra�z del proyecto.
   - Dentro de la carpeta `Models`, crea una clase llamada `Tarea.cs`.
   - Esta clase representar� el modelo de datos para las tareas.

![Carpeta Models](Images/MauiTask3.png)

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

## Paso 4: Manejar la L�gica de Negocio
- Editar el archivo `MainPage.xaml.cs` para agregar la l�gica de negocio.
- Agregar m�todos para cargar, agregar, editar y borrar tareas.
- Utilizar `ObservableCollection` para notificar a la UI cuando hay cambios en la lista.
- Utilizar m�todos de la base de datos para guardar, actualizar y eliminar tareas.
- Recargar la lista de tareas despu�s de realizar cambios.
- Aseg�rate de que la clase `MainPage` herede de `ContentPage`.


### Inicializar la colecci�n de tareas.
- Crea una colecci�n observable de tareas llamada `tareasList`.
- En el constructor de la p�gina, inicializa la colecci�n y asigna la lista de tareas al `CollectionView`.
- Llama al m�todo `LoadTareas` para cargar las tareas al iniciar la aplicaci�n.
```csharp
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;


public MainPage()
{
   
    InitializeComponent();
    tareasList = new ObservableCollection<tarea> ();
    TareasCollectionView.ItemsSource = tareasList;
    LoadTareas();
}
```

### Crear el m�todo para cargar las tareas de la base de datos 'LoadTareas'.
- Utiliza el m�todo `GetTareasAsync` de la base de datos para obtener todas las tareas.
- Limpia la lista de tareas y agrega las tareas obtenidas a la colecci�n observable.
- Utiliza `await` y `async` para realizar operaciones asincr�nicas.
- Utiliza un bucle `foreach` para recorrer las tareas y agregarlas a la colecci�n.

```csharp
private async void LoadTareas()
{
    var tareas = await App.Database.GetTareasAsync();
    tareasList.Clear();
    foreach (var tarea in tareas)
    {
        tareasList.Add(tarea);
    }
}
```

### Crear el m�todo para agregar una nueva tarea 'OnAgregarTareaClicked'.
- DisplayActionSheet: Presenta opciones para elegir algo y DisplayPromptAsync: Muestra cuadros de entrada de texto.
- Utiliza `DisplayPromptAsync` para mostrar cuadros de entrada de texto.
- Crea una nueva tarea con los datos ingresados por el usuario.
- Guarda la tarea en la base de datos utilizando el m�todo `SaveTareaAsync`.
- Agrega la tarea a la colecci�n observable y recarga la lista de tareas.
- Utiliza `DisplayActionSheet` para mostrar opciones de estado.

```csharp
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
    }
}
```

### Crear el metodo para borrar una tarea de la lista y de la base de datos.
- Utiliza el evento `OnBorrarTareaCommand` para borrar una tarea.
- Utiliza el m�todo `DeleteTareaAsync` de la base de datos para eliminar la tarea.
- Elimina la tarea de la colecci�n observable.

```csharp
private async void OnBorrarTareaCommand(object sender, EventArgs e)
{
    var swipeItem = sender as SwipeItem;
    var tarea = swipeItem?.CommandParameter as tarea;
    if (tarea != null)
    {
        await App.Database.DeleteTareaAsync(tarea);
        tareasList.Remove(tarea); // Eliminar la tarea de la colecci�n observable
    }
}
```

### Crear el m�todo para editar una tarea de la lista y de la base de datos.
- Utiliza el evento `OnEditarTareaCommand` para editar una tarea.
- Utiliza el m�todo `SaveTareaAsync` de la base de datos para actualizar la tarea.
- Recarga la lista de tareas despu�s de editar una tarea.

```csharp
private async void OnEditarTareaCommand(object sender, EventArgs e)
{
    var swipeItem = sender as SwipeItem;
    var tarea = swipeItem?.CommandParameter as tarea;

    if (tarea != null)
    {
        string nuevoNombre = await DisplayPromptAsync("Editar Tarea", "Nombre de la tarea:", initialValue: tarea.nombre);
        string nuevaDescripcion = await DisplayPromptAsync("Editar Tarea", "Descripci�n de la tarea:", initialValue: tarea.descripcion);
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
```

### C�digo Completo de `MainPage.xaml.cs`:

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
                tareasList.Remove(tarea); // Eliminar la tarea de la colecci�n observable
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
                string nuevaDescripcion = await DisplayPromptAsync("Editar Tarea", "Descripci�n de la tarea:", initialValue: tarea.descripcion);
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

## Paso 5: Implementar Almacenamiento Local con SQLite
1. Instalar el paquete NuGet `sqlite-net-pcl`:

```sh
dotnet add package sqlite-net-pcl
```

- Crear la clase `database.cs` en la ra�z del proyecto.
- Esta clase manejar� la conexi�n a la base de datos y las operaciones CRUD.
- SQL lite es una base de datos relacional que se utiliza para almacenar datos localmente en aplicaciones m�viles.
- Utiliza m�todos asincr�nicos para interactuar con la base de datos.
- Utiliza `CreateTableAsync` para crear la tabla de tareas en la base de datos.
- Utiliza `ToListAsync`, `InsertAsync`, `UpdateAsync` y `DeleteAsync` para realizar operaciones en la base de datos.


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

## Paso 6: Probar la Aplicaci�n
1. Ejecutar la aplicaci�n:
   - Ejecuta la aplicaci�n en un emulador Android o dispositivo f�sico.
   - Verifica que las tareas se guardan y persisten en la base de datos.
   - Prueba las funcionalidades de agregar, editar y borrar tareas.


## Paso 7: Agregar Picker para seleccionar el estado de la tarea.
- Agregar un Picker para seleccionar el estado de la tarea.
- Utilizar `Picker` para mostrar una lista de opciones y permitir al usuario seleccionar una.
- Agregar opciones de estado a la lista del Picker.

### Agregar Picker al formulario de agregar tarea MainPage.xaml.
- Agregar entre el Titulo del label y el CollectionView.
- Agregar Picker con opciones de estado: Pendiente, En Proceso, Completada o Mostrar Todo.
```xml
<!-- Picker para seleccionar el estado -->
<Picker x:Name="EstadoPicker" Title="Filtrar por Estado" SelectedIndexChanged="OnEstadoChanged">
    <Picker.Items>
        <x:String>Mostrar Todo</x:String>
        <x:String>Pendiente</x:String>
        <x:String>En Proceso</x:String>
        <x:String>Completada</x:String>
    </Picker.Items>
</Picker>
```

### Manejar el evento SelectedIndexChanged del Picker en MainPage.xaml.cs.
- Crear el m�todo `OnEstadoChanged` para filtrar las tareas por estado.
- Utilizar el m�todo `LoadTareas` para cargar las tareas seg�n el estado seleccionado.
- Utilizar un switch para filtrar las tareas seg�n el estado seleccionado.

```csharp
 // M�todo para cargar las tareas desde la base de datos con picker agregado

    private async void LoadTareas(string estadoFiltro = null) //Agregamos Variable dentro del metodo
    {
        var tareas = await App.Database.GetTareasAsync();
        tareasList.Clear();

        //------------  Filtrar tareas si se especifica un estado dentro --------------------------
        if (!string.IsNullOrEmpty(estadoFiltro) && estadoFiltro != "Mostrar Todo")
        {
            tareas = tareas.Where(t => t.estado == estadoFiltro).ToList();
        }

        // Agregar las tareas a la colecci�n
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

```

---

## Paso 8 (Opcional): Agregar una P�gina de Perfil al Men� de Navegaci�n

En este paso, agregaremos una nueva p�gina **Perfil** al men� de la aplicaci�n utilizando `Shell`.

### **1. Agregar el �tem de navegaci�n en `AppShell.xaml`**

Abre el archivo `AppShell.xaml` y modif�calo para incluir la nueva p�gina de perfil:

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
        ContentTemplate="{DataTemplate local:UserPage}"
        Route="UserPage" />
</Shell>
```

### **2. Crear la P�gina `UserPage.xaml`**

Agrega un nuevo archivo **UserPage.xaml** y copia el siguiente c�digo:

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

        <!-- Tarjeta de Informaci�n -->
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
                <Label Text="Ciudad de M�xico, MX" FontSize="18" TextColor="Black" Grid.Row="3" Grid.Column="1"/>
            </Grid>
        </Border>

        <!-- Bot�n de edici�n -->
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

### **3. Crear el C�digo `UserPage.xaml.cs`**

Agrega un archivo `UserPage.xaml.cs` y define la clase para la nueva p�gina:

```csharp
namespace gestorTareasaMaui;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();
	}
}
```

### **4. Ejecutar la Aplicaci�n**
Prueba la aplicaci�n y verifica que el nuevo �tem de perfil se muestra en el men� de navegaci�n. Puedes personalizar la p�gina de perfil y agregar m�s informaci�n seg�n tus necesidades.

---

## Contribuci�n

Si deseas contribuir a este proyecto, por favor sigue los siguientes pasos:

1. Haz un fork del repositorio.
2. Crea una nueva rama (`git checkout -b feature/nueva-funcionalidad`).
3. Realiza tus cambios y haz commit (`git commit -am 'A�adir nueva funcionalidad'`).
4. Sube tus cambios a tu fork (`git push origin feature/nueva-funcionalidad`).
5. Abre un Pull Request.

## Licencia

Este proyecto est� licenciado bajo la Licencia MIT. Consulta el archivo LICENSE para m�s detalles.