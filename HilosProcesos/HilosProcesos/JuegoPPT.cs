namespace HilosProcesos;
using System;
using System.Threading;

class JuegoPPT
{
    // Resultados posibles
    enum Move { Piedra, Papel, Tijera }

    static Random random = new Random();

    // Variables para almacenar los movimientos y puntuaciones
    static Move jugador1Move;
    static Move jugador2Move;
    static int jugador1Score = 0;
    static int jugador2Score = 0;

    static readonly object lockObject = new object();

    static void Main(string[] args)
    {
        Console.WriteLine("Partida de Piedra, Papel o Tijera al mejor de 3\n");

        while (jugador1Score < 2 && jugador2Score < 2)
        {
            // Crear hilos para los jugadores
            Thread jugador1 = new Thread(() => jugadorHilo(1));
            Thread jugador2 = new Thread(() => jugadorHilo(2));

            // Ejecutar los hilos
            jugador1.Start();
            jugador2.Start();

            // Esperar a que ambos terminen
            jugador1.Join();
            jugador2.Join();

            // Ganador
            determinarGanadorRonda();

            Console.WriteLine($"Marcador: Jugador 1: {jugador1Score}, Jugador 2: {jugador2Score}\n");
        }

        Console.WriteLine(jugador1Score == 2 ? "\n\tJugador 1 gana la partida!" : "\n\tJugador 2 gana la partida!");
    }

    static void jugadorHilo(int jugador)
    {
        // Generar un movimiento aleatorio
        Move movimiento = (Move)random.Next(0, 3);

        lock (lockObject)
        {
            if (jugador == 1)
                jugador1Move = movimiento;
            else
                jugador2Move = movimiento;
        }
    }

    static void determinarGanadorRonda()
    {
        Console.WriteLine($"Jugador 1 juega: {jugador1Move}, Jugador 2 juega: {jugador2Move}");

        if (jugador1Move == jugador2Move)
        {
            Console.WriteLine("Empate en esta ronda!");
        }
        else if ((jugador1Move == Move.Piedra && jugador2Move == Move.Tijera) ||
                 (jugador1Move == Move.Papel && jugador2Move == Move.Piedra) ||
                 (jugador1Move == Move.Tijera && jugador2Move == Move.Papel))
        {
            Console.WriteLine("Jugador 1 gana esta ronda!");
            jugador1Score++;
        }
        else
        {
            Console.WriteLine("Jugador 2 gana esta ronda!");
            jugador2Score++;
        }
    }
}
