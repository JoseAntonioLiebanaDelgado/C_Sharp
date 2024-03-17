// Incorpora las librerías necesarias para usar las funcionalidades básicas de C#.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Declara el espacio de nombres para la organización del código.
namespace Client
{
    // Define una clase interna llamada ClasseJugador.
    internal class ClasseJugador
    {
        // Propiedades autoimplementadas para obtener y establecer el ID del jugador.
        public int id_jugador { get; set; }
        // Propiedades autoimplementadas para obtener y establecer el color del jugador.
        public string color { get; set; }
    }
}
