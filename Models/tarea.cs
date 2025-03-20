using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gestorTareasaMaui.Models
{
    public class tarea
    {
        [PrimaryKey, AutoIncrement]
        //Modelo de datos de la tarea que necesitamos
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
      
    }
}

