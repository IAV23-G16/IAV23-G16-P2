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

El pseudocódigo del algoritmo de BFS (Breadth-First Search) utilizado es:
```
1  procedure BFS(G, root) is
 2      let Q be a queue
 3      label root as explored
 4      Q.enqueue(root)
 5      while Q is not empty do
 6          v := Q.dequeue()
 7          if v is the goal then
 8              return v
 9          for all edges from v to w in G.adjacentEdges(v) do
10              if w is not labeled as explored then
11                  label w as explored
12                  w.parent := v
13                  Q.enqueue(w)
```

El pseudocódigo del algoritmo de DFS (Depth-First Search) utilizado es:
```
procedure DFS(G, v) is
    label v as discovered
    for all directed edges from v to w that are in G.adjacentEdges(v) do
        if vertex w is not labeled as discovered then
            recursively call DFS(G, w)
```

El pseudocódigo del algoritmo de A* utilizado es:
```
def pathfindAStar(graph, start, end, heuristic):

  # This structure is used to keep track of the
  # information we need for each node
    struct NodeRecord:
    node
    connection
    costSoFar
    estimatedTotalCost

  # Initialize the record for the start node
  startRecord = new NodeRecord()
  startRecord.node = start
  startRecord.connection = None
  startRecord.costSoFar = 0
  startRecord.estimatedTotalCost =
    heuristic.estimate(start)

  # Initialize the open and closed lists
  open = PathfindingList()
  open += startRecord
  closed = PathfindingList()

  # Iterate through processing each node
  while length(open) > 0:

     # Find the smallest element in the open list
     # (using the estimatedTotalCost)
     current = open.smallestElement()

     # If it is the goal node, then terminate
     if current.node == goal: break

     # Otherwise get its outgoing connections
     connections = graph.getConnections(current)

     # Loop through each connection in turn
     for connection in connections:

       # Get the cost estimate for the end node
       endNode = connection.getToNode()
       endNodeCost = current.costSoFar +
       connection.getCost()

       # If the node is closed we may have to
       # skip, or remove it from the closed list.
       if closed.contains(endNode):

         # Here we find the record in the closed list
         # corresponding to the endNode.
         endNodeRecord = closed.find(endNode)

         # If we didn’t find a shorter route, skip
         if endNodeRecord.costSoFar <= endNodeCost:
         continue;

         # Otherwise remove it from the closed list
         closed -= endNodeRecord

         # We can use the node’s old cost values
         # to calculate its heuristic without calling
         # the possibly expensive heuristic function
         endNodeHeuristic = endNodeRecord.cost -
         endNodeRecord.costSoFar

     # Skip if the node is open and we’ve not
     # found a better route
     else if open.contains(endNode):

       # Here we find the record in the open list
       # corresponding to the endNode.
       endNodeRecord = open.find(endNode)

       # If our route is no better, then skip
       if endNodeRecord.costSoFar <= endNodeCost:
       continue;

       # We can use the node’s old cost values
       # to calculate its heuristic without calling
       # the possibly expensive heuristic function
       endNodeHeuristic = endNodeRecord.cost -
       endNodeRecord.costSoFar

     # Otherwise we know we’ve got an unvisited
     # node, so make a record for it
     else:
       endNodeRecord = new NodeRecord()
       endNodeRecord.node = endNode

       # We’ll need to calculate the heuristic value
       # using the function, since we don’t have an
       # existing record to use
       endNodeHeuristic = heuristic.estimate(endNode)

     # We’re here if we need to update the node
     # Update the cost, estimate and connection
     endNodeRecord.cost = endNodeCost
     endNodeRecord.connection = connection
     endNodeRecord.estimatedTotalCost =
     endNodeCost + endNodeHeuristic

     # And add it to the open list
     if not open.contains(endNode):
     open += endNodeRecord

   # We’ve finished looking at the connections for
   # the current node, so add it to the closed list
   # and remove it from the open list
   open -= current
   closed += current

 # We’re here if we’ve either found the goal, or
 # if we’ve no more nodes to search, find which.
 if current.node != goal:

   # We’ve run out of nodes without finding the
   # goal, so there’s no solution
   return None

 else:

   # Compile the list of connections in the path
   path = []

     # Work back along the path, accumulating
     # connections
     while current.node != start:
       path += current.connection
       current = current.connection.getFromNode()

   # Reverse the path, and return it
   return reverse(path)
```

El pseudocódigo del algoritmo de Smooth utilizado es:
```
def smoothPath(inputPath):

 # If the path is only two nodes long, then
 # we can’t smooth it, so return
 if len(inputPath) == 2: return inputPath

 # Compile an output path
 outputPath = [inputPath[0]]

 # Keep track of where we are in the input path
 # We start at 2, because we assume two adjacent
 # nodes will pass the ray cast
 inputIndex = 2

 # Loop until we find the last item in the input
 while inputIndex < len(inputPath)-1:

   # Do the ray cast
   if not rayClear(outputPath[len(outputPath)-1],
   inputPath[inputIndex]):

     # The ray text failed, add the last node that
     # passed to the output list
     outputPath += inputPath[inputIndex-1]

   # Consider the next node
   inputIndex ++

 # We’ve reached the end of the input path, add the
 # end node to the output and return it
 outputPath += inputPath[len(inputPath)-1]
 return outputPath
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
    - 4.3.3 "A* Pseudo-code", 220.
    - 4.4.7 "Path Smoothing", 251.
- [Breadth-first search](https://en.wikipedia.org/wiki/Breadth-first_search)
- [Depth-first search](https://en.wikipedia.org/wiki/Depth-first_search)
