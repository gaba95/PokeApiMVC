🧩 Proyecto ASP.NET MVC – Consumo de PokéAPI
📖 Descripción

Este proyecto es una aplicación web desarrollada en ASP.NET MVC (.NET Framework) que permite consultar información de Pokémon a través de la PokéAPI (https://pokeapi.co/
).

El sistema ofrece una interfaz sencilla en la que el usuario puede ingresar el nombre de un Pokémon y obtener datos como:

Nombre

Altura

Peso

Tipos

Imagen oficial del Pokémon

Este proyecto tiene como objetivo demostrar el consumo de APIs REST públicas utilizando C#, MVC clásico, y Newtonsoft.Json para el procesamiento de datos JSON.

⚙️ Tecnologías utilizadas

Lenguaje: C#

Framework: ASP.NET MVC (.NET Framework 4.x)

IDE: Visual Studio Community 2019

Librerías:

Newtonsoft.Json
 – para deserializar los datos JSON.

System.Net.Http – para realizar las peticiones HTTP a la PokéAPI.

🖥️ Vista principal (Views/Pokemon/Index.cshtml)

La vista contiene un formulario para ingresar el nombre del Pokémon y muestra los resultados obtenidos.

Características:

Campo de texto para búsqueda.

Botón de envío.

Visualización dinámica del nombre, imagen, altura, peso y tipos.

Mensajes de error cuando no se encuentra el Pokémon o hay problemas de conexión.

Ejemplo de pokemones: 
pikachu
charizard
chikorita
ditto

info de PokeAPI https://pokeapi.co/
