namespace HilosProcesos;
using System;
using System.Threading;

class Hilos
{
    // Resultados posibles
    enum Move { Piedra, Papel, Tijera }

    static Random random = new Random();

    // Variables para almacenar los movimientos y puntuaciones
    static Move[] movimientos;
    static int[] puntuaciones;

    static readonly object lockObject = new object();

    static void Main(string[] args)
    {
        Console.WriteLine("Competencia de Piedra, Papel o Tijera entre 16 jugadores\n");

        int numJugadores = 16;
        movimientos = new Move[numJugadores];
        puntuaciones = new int[numJugadores];

        while (numJugadores > 1)
        {
            Console.WriteLine($"\nComienza una nueva ronda con {numJugadores} jugadores\n");
            Thread[] hilos = new Thread[numJugadores];

            // Crear y ejecutar los hilos para cada jugador
            for (int i = 0; i < numJugadores; i++)
            {
                int jugador = i; // Necesario para capturar correctamente el índice en el hilo
                hilos[i] = new Thread(() => jugadorHilo(jugador));
                hilos[i].Start();
            }

            // Esperar a que todos los hilos terminen
            foreach (var hilo in hilos)
            {
                hilo.Join();
            }

            // Ganador
            numJugadores = determinarGanadores(numJugadores);
        }

        Console.WriteLine($"\n\tEl ganador del torneo es el Jugador {Array.IndexOf(puntuaciones, 1) + 1}!\n");
    }

    static void jugadorHilo(int jugador)
    {
        // Generar un movimiento aleatorio
        Move movimiento = (Move)random.Next(0, 3);

        lock (lockObject)
        {
            movimientos[jugador] = movimiento;
        }
    }

    static int determinarGanadores(int numJugadores)
    {
        for (int i = 0; i < numJugadores; i += 2)
        {
            int jugador1 = i;
            int jugador2 = i + 1;

            Console.WriteLine($"Jugador {jugador1 + 1} juega: {movimientos[jugador1]}, Jugador {jugador2 + 1} juega: {movimientos[jugador2]}");

            if (movimientos[jugador1] == movimientos[jugador2])
            {
                Console.WriteLine($"Empate entre Jugador {jugador1 + 1} y Jugador {jugador2 + 1}. Ambos eliminados!\n");
                puntuaciones[jugador1] = 0;
                puntuaciones[jugador2] = 0;
            }
            else if ((movimientos[jugador1] == Move.Piedra && movimientos[jugador2] == Move.Tijera) ||
                     (movimientos[jugador1] == Move.Papel && movimientos[jugador2] == Move.Piedra) ||
                     (movimientos[jugador1] == Move.Tijera && movimientos[jugador2] == Move.Papel))
            {
                Console.WriteLine($"Jugador {jugador1 + 1} gana contra Jugador {jugador2 + 1}!\n");
                puntuaciones[jugador1] = 1;
                puntuaciones[jugador2] = 0;
            }
            else
            {
                Console.WriteLine($"Jugador {jugador2 + 1} gana contra Jugador {jugador1 + 1}!\n");
                puntuaciones[jugador1] = 0;
                puntuaciones[jugador2] = 1;
            }
        }

        // Filtrar los jugadores que avanzan a la siguiente ronda
        int nuevoNumJugadores = 0;
        for (int i = 0; i < puntuaciones.Length; i++)
        {
            if (puntuaciones[i] == 1)
            {
                movimientos[nuevoNumJugadores] = movimientos[i];
                nuevoNumJugadores++;
            }
        }

        return nuevoNumJugadores;
    }
}
