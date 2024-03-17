// Importa las bibliotecas de sistema necesarias para el funcionamiento del código.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Define el espacio de nombres 'Server' para agrupar las clases relacionadas con el servidor.
namespace Server
{
    // Declara una clase 'ClasseJugador' con acceso interno dentro del espacio de nombres 'Server'.
    internal class ClasseJugador
    {
        // Define una propiedad pública 'id_jugador' que permite obtener y establecer un valor entero.
        public int id_jugador { get; set; }

        // Define una propiedad pública 'color' que permite obtener y establecer una cadena de texto.
        public string color { get; set; }
    }
}
