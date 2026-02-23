# Práctica 01 — Movimiento y Cámara (Unity 3D)

## Descripción

Este proyecto implementa un controlador de personaje en tercera dimensión utilizando Unity, incluyendo:

* Movimiento del jugador
* Sistema de salto y gravedad
* Cámara en primera persona (FPS)
* Cámara en tercera persona (TPS)
* Alternancia entre cámaras mediante la tecla TAB

---

## Características

### Movimiento

* W A S D → Movimiento
* Shift → Correr
* Espacio → Saltar

### Cámara FPS

* Control con mouse
* Vista en primera persona

### Cámara TPS

* Cámara orbital
* Sigue al jugador suavemente
* Control con mouse

### Cambio de cámara

* TAB → Alterna entre FPS y TPS

---

## Estructura del proyecto

Assets/

Scenes/

Practica01_3D.unity

Scripts/

Player/

PlayerMovement.cs

PlayerInputActions.cs

Camera/

CameraLook.cs

CameraSwitcher.cs

SmoothFollow.cs

---

## Requisitos

Unity 6 o superior

Input System Package instalado

---

## Instrucciones de uso

1. Clonar repositorio

2. Abrir proyecto en Unity Hub

3. Abrir escena:

Scenes/Practica01_3D.unity

4. Presionar Play

---

## Autor

Adrián Lima

---

## Estado

Proyecto funcional
