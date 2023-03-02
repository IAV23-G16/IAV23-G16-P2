# IAV - Base para la Práctica 2

## Autores
- Pablo Arredondo Nowak (PabloArrNowak)
- Mario Miguel Cuartero (mamigu05)

## Propuesta
A partir de la base proporcionada, se deben diseñar e implementar soluciones para los siguientes comportamientos, indicados en https://narratech.com/es/inteligencia-artificial-para-videojuegos/navegacion/el-secreto-del-laberinto/ :

Avatar del Jugador (**Teseo**): Inicialmente se encontrará en la baldosa de entrada, pudiendo moverse por el mapa controlado por el click izquierdo del ratón. Con el click derecho, el jugador activará el hilo de Ariadna que le guiará y lo moverá automaticamente hasta la baldosa de salida. Además, si se encuentra en proximidad de un minotauro (enemigo), éste empezará a perseguirlo.

**Minotauros**: Si el avatar no se encuentra próximo a ellos o dentro de su campo de vista, deambularán por su cuenta de forma aleatoria y errática a una velocidad determinada y cambiando de dirección cada cierto tiempo, en el caso contrario, perseguirán al avatar del jugador.

**Hilo de Ariadna**: Se activa con el click derecho del ratón, y mide el camino más corto a la baldosa de salida calculado por el algoritmo A*, dejando pintado una línea blanca en el suelo y destacando las baldosas con bolitas blancas. Además el jugador podrá elegir la heurística y suavizar el camino generado por el algoritmo.
El hilo irá desapareciendo a medida que el avatar avanza por el camino.

## Punto de partida
Se parte de un proyecto base de Unity proporcionado por el profesor aquí:
https://github.com/Narratech/IAV-Navegacion/

El proyecto se compone de dos escenas: Labyrinth y Menu

**Labyrinth**

En esta escena se encuentran:

- **Cámara**, que contiene el script "CameraFollow" que hace seguir al jugador y te ofrece la posibilidad de hacer zoom con la ruleta del ratón.
- **Avatar**, al que puedes controlar por teclado haciendo que pueda rotar y moverse por el laberinto.
- **GraphGrid**, que genera un mapa con 3 dimensiones diferentes a elección del jugador leyendo un archivo .map. Este mapa instancia paredes, intersecciones y pilares en forma de prefabs que construyen el mapa. Es controlado por el script "GraphGrid".
- **TesterGraph**, que contiene el script "TheseusGraph" que permite dibujar el hilo de Ariadna blanco y cambiar la heurística.
- **MinoManager**, que contiene el script "MinoManager" que genera 0, 1, 2 o 5 minotauros a elección del jugador en una baldosa aleatoria del mapa.
- **Canvas**, que una vez ha comenzado el juego, muestra por pantalla información sobre el tipo de heurística; FPS actuales; e información del input de teclado como el reinicio de la escena, el cambio de heurística...
- **ExitSlab** y **StartSlab**, baldosas que indican la entrada y salida del laberinto.

**Menu**

En esta escena se encuentran:

- **Cámara**, que contiene el script "CameraFollow".
- **Canvas**, que muestra por pantalla el título del juego, y le ofrece al jugador la posibilidad de elegir la dimensión del mapa y el número de minotauros antes de darle al botón comenzar que llevará a la escena "Labyrinth" en base al input recogido.

Se incluyen los siguientes scripts/clases:

- 

## Diseño de la solución

Lo que vamos a realizar para resolver esta práctica es...

El pseudocódigo del algoritmo de llegada utilizado es:
```
a
```

## Pruebas y métricas

- [Vídeo con la batería de pruebas]

## Ampliaciones

Se han realizado las siguientes ampliaciones


## Producción

Las tareas se han realizado y el esfuerzo ha sido repartido entre los autores.


## Referencias

Los recursos de terceros utilizados son de uso público.

- *AI for Games*, Ian Millington.
- [Kaykit Medieval Builder Pack](https://kaylousberg.itch.io/kaykit-medieval-builder-pack)
- [Kaykit Dungeon](https://kaylousberg.itch.io/kaykit-dungeon)
- [Kaykit Animations](https://kaylousberg.itch.io/kaykit-animations)
