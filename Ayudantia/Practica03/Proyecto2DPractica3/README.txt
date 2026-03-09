# Practica 03 – Movimiento Avanzado Unity 2D

## Descripción

Esta práctica implementa un sistema de movimiento avanzado para un personaje en un juego de plataformas 2D usando física manual.

Se agregaron mejoras sobre la práctica anterior como desaceleración automática, control avanzado de salto y variables para animaciones.

## Características implementadas

* Movimiento horizontal con aceleración
* Desaceleración automática al soltar el input
* Salto con gravedad manual
* Coyote Time
* Jump Buffering
* Gravedad mejorada para caídas
* Variables de estado para animaciones

## Scripts principales

* `Jugador.cs`
* `ControlJugador.cs`

## Cómo ejecutar

1. Abrir el proyecto en Unity.
2. Cargar la escena principal.
3. Presionar Play.
4. Usar:

   * **A / D** o **flechas** para moverse
   * **Espacio** para saltar

## Notas

La física del personaje está implementada manualmente utilizando velocidad, aceleración y cálculo manual de deltaTime.
