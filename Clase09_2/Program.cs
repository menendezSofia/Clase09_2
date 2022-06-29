//1)    Cartón de 3 filas por 9 columnas
//2)    El cartón debe tener 15 números y 12 espacios en blanco
//3)    Cada fila debe tener 5 números
//4)    Cada columna debe tener 1 o 2 números
//5)    Ningún número puede repetirse
//6)    La primer columna contiene los números del 1 al 9, la segunda del 10 al 19,
//      la tercera del 20 al 29, así sucesivamente hasta la última columna la cual contiene del 80 al 90
//7)    Mostrar el carton por pantalla


// INICIALIZACIÓN DE VARIABLES PRINCIPALES
int cantFilas = 3;
int cantColumnas = 9;
Random r = new Random();
int[,] carton = new int[cantFilas, cantColumnas];

// INICIALIZACIÓN DE VARIABLES AUXILIARES
int moneda;                                 // decide si inserta número en casilla o no
int[] contadorPorFila = new int[3];         // array con cantidad de números por fila
int[] contadorPorColumna = new int [9];     // array con cantidad de números por columna
int[] filasLlenas = new int[] { 5,5,5 };    // array con valores [5,5,5] para comparar con contador de filas
int numero = 0;                             // almacena temporalmente el número aleatorio a insertar en cartón
var numerosUsados = new List<int>();        // lista con números usados en cartón
int intento = 0;                            // cuenta los intentos de llenar el cartón


// intento llenar el cartón con números hasta que todas las filas tengan 5 números y las columnas 3 
while (!contadorPorFila.SequenceEqual(filasLlenas) || contadorPorColumna.Contains(0))
{
    for (int columna = 0; columna < cantColumnas; columna++)
    {
        for (int fila = 0; fila < cantFilas; fila++)
        {
            // intento instertar un número sólo si en la columna correspondiente hay 0 o 1 número,
            // menos de 5 números en esa fila, y si no existe un número en la casilla
            if ((contadorPorColumna[columna] < 2 && contadorPorFila[fila] < 5) && (carton[fila, columna] == 0))
            {
                moneda = r.Next(0, 2);

                if (moneda == 1)
                {
                    // genero un número aleatorio correspondiente a la ubicación
                    if (columna == 0)
                    {
                        numero = r.Next(1, 10);
                    }
                    else if (columna == 8)
                    {
                        numero = r.Next(80, 91);
                    }
                    else
                    {
                        numero = r.Next(0, 10) + (columna * 10);
                    }

                    // verifico que no sea un número repetido
                    while (numerosUsados.Contains(numero))
                    {
                        if (columna == 0)
                        {
                            numero = r.Next(1, 10);
                        }
                        else if (columna == 8)
                        {
                            numero = r.Next(80, 91);
                        }
                        else
                        {
                            numero = r.Next(0, 10) + (columna * 10);
                        }
                    }
                    
                    // inserto el número en la casilla, incremento los contadores y almaceno el 
                    // número usado en la lista de números usados
                    carton[fila, columna] = numero;
                    contadorPorColumna[columna]++;
                    contadorPorFila[fila]++;
                    numerosUsados.Add(numero);
                } 
            }
        }      
    }

    // incremento el contador de intentos de rellenar el cartón
    intento++;

    // relleno columnas vacías como prioridad
    for (int columna = 0; columna < cantColumnas; columna++)
    {
        for (int fila = 0; fila < cantFilas; fila++)
        {
            if ((contadorPorColumna[columna] == 0) && (contadorPorFila[fila] < 5))
            {
                if (columna == 0)
                {
                    numero = r.Next(1, 10);
                }
                else if (columna == 8)
                {
                    numero = r.Next(80, 91);
                }
                else
                {
                    numero = r.Next(0, 10) + (columna * 10);
                }
                carton[fila, columna] = numero;
                contadorPorColumna[columna]++;
                contadorPorFila[fila]++;              
                numerosUsados.Add(numero);
            }
        }
    }

    // este correctivo sólo se aplica en caso de que sea imposible insertar un número sin
    // romper las reglas de máximo de números por filas y columnas.
    // el primer FOR inserta un número donde haya una columna vacía
    // y el segundo FOR borra el sobrante. EJ:

    // N - N - N N N - -        N - N - N N N - X        - - N - N N N - X
    // N N N N N - - - -   ->   N N N N N - - - -   ->   N N N N N - - - -
    // - N - N - N N N -        - N - N - N N N -        - N - N - N N N -

    if (contadorPorFila.SequenceEqual(filasLlenas) && contadorPorColumna.Contains(0))
    {
        for (int columna = 0; columna < cantColumnas; columna++)
        {
            for (int fila = 0; fila < cantFilas; fila++)
            {
                if ((contadorPorColumna[columna] == 0) && (contadorPorFila[fila] == 5))
                {
                    if (columna == 0)
                    {
                        numero = r.Next(1, 10);
                    }
                    else if (columna == 8)
                    {
                        numero = r.Next(80, 91);
                    }
                    else
                    {
                        numero = r.Next(0, 10) + (columna * 10);
                    }
                    contadorPorColumna[columna]++;
                    contadorPorFila[fila]++;                    
                }
            }
        }
        for (int columna = 0; columna < cantColumnas; columna++)
        {
            for (int fila = 0; fila < cantFilas; fila++)
            {
                if ((contadorPorColumna[columna] > 1) && (contadorPorFila[fila] == 6))
                {
                    carton[fila, columna] = 0;
                    contadorPorColumna[columna]--;
                    contadorPorFila[fila]--;
                }
            }
        }
    }
   
    // en el muy extraño caso en que ocurra algo similar a lo siguiente:

    // N N N N N - - - -
    // N N N N N - - - -
    // - - - - - N N N N

    // se entraría en un bucle infinito intentando completar la última fila, 
    // por lo cual, el código siguiente determina que, si se ha intentado más de 10 veces
    // llenar el cartón, éste se resetea y el proceso comienza nuevamente.

    if (intento>10)
    {
        for (int columna = 0; columna < cantColumnas; columna++)
        {
            for (int fila = 0; fila < cantFilas; fila++)
            {
                carton[fila, columna] = 0;
                contadorPorColumna[columna]=0;
                contadorPorFila[fila]=0;                                   
            }
            intento=0; 
        }
    }
}


// muestro el cartón resultante en consola
Console.WriteLine("\n+--------------------------------------------+");
Console.Write("|                * B I N G O *               |");

for (int fila = 0; fila < cantFilas; fila++)
{
    Console.Write("\n+----+----+----+----+----+----+----+----+----+\n|");
    for (int columna = 0; columna < cantColumnas; columna++)
    {       
        if (carton[fila, columna] == 0)
        {
            Console.Write(" " + carton[fila, columna].ToString("D2").Replace('0', ' ') + " |");
        }
        else
        {
            Console.Write(" " + carton[fila, columna].ToString("D2") + " |");
        }
    }
}
Console.WriteLine("\n+----+----+----+----+----+----+----+----+----+\n");
