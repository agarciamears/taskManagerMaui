namespace gestorTareasaMaui
{
    public partial class App : Application
    {

        //Inicializar la base de datos en App.xaml.cs
        static database database;
        
        public static database Database
        {
            get
            {
                if (database == null)
                {
                    //Creando la base de datos en la carpeta local de la aplicacion con el nombre Tareas.db3
                    database = new database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tareas.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}