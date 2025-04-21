Información del Equipo
Integrante 1: Vicente Javier Figueroa Lazo, vicente.figueroa01@alumnos.ucn.cl, 21.536.417-8 

Integrante 2: Vicente Arnoldo Castro Zepeda, vicente.castro02@alumnos.ucn.cl, 21.448.750-0


----Descripción del Proyecto---

Este proyecto consiste en el desarrollo de un sistema de e-commerce para la empresa BLACKCAT, implementando un backend robusto con ASP.NET Core y una base de datos SQLite. El sistema incluye funcionalidades para usuarios no autenticados, clientes registrados y administradores, siguiendo los requisitos especificados en el taller.


----Requisitos del Sistema----

.NET Core SDK 9.0 o superior

SQLite

Git

Visual Studio o Visual Studio Code (opcional)


----Instalación-----

Clonar el repositorio:

bash
git clone https://github.com/VicenteCZ0/Taller-1.git
cd tu-repositorio
Restaurar dependencias:

bash
dotnet restore
Configurar la base de datos:

La base de datos SQLite se generará automáticamente al iniciar la aplicación.

Las migraciones se ejecutarán automáticamente para crear el esquema de la base de datos.

Ejecutar la aplicación:

bash
dotnet run
Acceder a la API:

La API estará disponible en https://localhost:5001.


----Estructura del Proyecto----

Controllers: Manejan las solicitudes HTTP y las respuestas.

Services: Contienen la lógica de negocio.

Repositories: Gestionan el acceso a la base de datos.

Models: Definiciones de las entidades de la base de datos.

DTOs: Objetos de transferencia de datos para las interacciones con la API.

Data: Configuración de la base de datos y migraciones.

Interfaces: Definiciones de contratos para servicios y repositorios.

-----Configuración----

El archivo appsettings.json contiene la configuración necesaria para la aplicación. Se proporciona un archivo appsettings.example.json como plantilla.

Importante: Asegúrese de no incluir información sensible en appsettings.json y agregue este archivo al .gitignore.


----Uso de Git----

Ramas:

main: Versión estable del proyecto.

development: Rama de integración para características en desarrollo.

[Apellidos]: Ramas para nuevas funcionalidades.

Commits: Seguimos el estándar Conventional Commits.

Pull Requests: Todos los cambios a main y development deben realizarse mediante Pull Requests con revisión de otro desarrollador.

---Población de Datos Iniciales---

La aplicación incluye Seeders que generan datos ficticios utilizando la biblioteca Bogus. Estos datos se cargan automáticamente al iniciar la aplicación por primera vez.


----Endpoints de la API----

Los endpoints siguen una estructura RESTful y están documentados en la colección de Postman adjunta. Algunos ejemplos incluyen:

Autenticación: /api/auth/register, /api/auth/login

Productos: /api/products (GET, POST, PUT, DELETE)

Carrito: /api/cart (GET, POST, PUT, DELETE)

Pedidos: /api/orders (GET, POST)

----Pruebas----

Para probar los flujos principales, siga estos pasos:

Registre un nuevo usuario.

Inicie sesión y obtenga un token JWT.

Explore el catálogo de productos.

Agregue productos al carrito.

Realice un pedido.



----Despliegue----

Para desplegar la aplicación en un entorno de producción, asegúrese de:

Configurar adecuadamente appsettings.json con las credenciales reales.

Habilitar HTTPS.

Configurar CORS para los orígenes permitidos.


---Contribuciones----

Las contribuciones son bienvenidas. Por favor, siga las pautas de Conventional Commits y utilice Pull Requests para proponer cambios.

Licencia
Este proyecto está bajo la licencia MIT. 
