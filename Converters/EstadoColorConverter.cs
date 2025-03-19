using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;


namespace gestorTareasaMaui.Converters
{   public class EstadoColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string estado = value as string;

            switch (estado)
            {
                case "Pendiente":
                    return Color.FromArgb("#FF0000"); // Rojo para tareas Pendientes
                case "Proceso":
                    return Color.FromArgb("#000080");  // Amarillo para tareas en Proceso
                case "Completada":
                    return Color.FromArgb("#008000");  // Verde para tareas Completadas
                default:
                    return Color.FromArgb("#808080");  // Gris para cualquier otro caso
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
