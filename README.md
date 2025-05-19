Información del Equipo Integrante 1: Vicente Javier Figueroa Lazo, vicente.figueroa01@alumnos.ucn.cl, 21.536.417-8

Integrante 2: Vicente Arnoldo Castro Zepeda, vicente.castro02@alumnos.ucn.cl, 21.448.750-0

 --Descripción del Proyecto--

Este proyecto consiste en el desarrollo de un sistema de e-commerce para la empresa BLACKCAT, implementando un backend robusto con ASP.NET Core y una base de datos SQLite. El sistema incluye funcionalidades para:
- Usuarios no autenticados
- Clientes registrados
- Administradores
Todo siguiendo los requisitos especificados en el taller.

----Requisitos del Sistema----

- .NET Core SDK 9.0 o superior
- SQLite
- Git
- Visual Studio o Visual Studio Code (opcional)

----Instalación-----
-Clonar el repositorio:
    git clone https://github.com/VicenteCZ0/Taller-1.git
    cd Taller-1

-Restaurar dependencias:

    dotnet restore

-Configurar la base de datos:
    - La base de datos SQLite se genera automáticamente al iniciar la aplicación.
    - Las migraciones se ejecutan automáticamente para crear el esquema.

-Ejecutar la aplicación:
    dotnet run
    La API estará disponible en https://localhost:7283

----Estructura del Proyecto----
- Controllers: Manejan las solicitudes HTTP y las respuestas.
- Services: Contienen la lógica de negocio.
- Repositories: Gestionan el acceso a la base de datos.
- Models: Definiciones de las entidades.
- DTOs: Objetos de transferencia de datos.
- Data: Configuración de la base de datos y migraciones.
- Interfaces: Contratos de servicios y repositorios.

-----Configuración----
El archivo appsettings.json contiene la configuración de la aplicación.
Se proporciona una plantilla appsettings.example.json.

Importante: No incluyas información sensible. Agrega este archivo a .gitignore.

----Uso de Git----
Ramas:
- main: Versión estable del proyecto
- dev: Rama de integración
- features/(lugar a trabajar): Ramas de funcionalidades

Commits:
Usamos el estándar Conventional Commits.

Pull Requests:
Todos los cambios a main y development deben hacerse mediante Pull Requests con revisión.

---Población de Datos Iniciales---
La aplicación incluye seeders automáticos que generan datos de prueba usando Bogus. Esto incluye:

- Usuarios
- Direcciones
- Carritos con productos
- Productos con imágenes

----Endpoints de la API----
La API del sistema BLACKCAT proporciona los siguientes endpoints organizados por funcionalidad:
Autenticación

POST /api/auth/login - Inicio de sesión y obtención de token JWT
POST /api/auth/register - Registro de nuevos usuarios
POST /api/auth/logout - Cierre de sesión

Usuarios

GET /api/user - Listar todos los usuarios
GET /api/user/me - Obtener información del perfil del usuario autenticado
PUT /api/user/me - Actualizar información del perfil
PUT /api/user/me/password - Cambiar contraseña del usuario
PUT /api/user/me/address - Actualizar dirección de envío del usuario

Administración de Usuarios

GET /api/admin/users - (Admin) Listar todos los usuarios
POST /api/admin/users/detail - (Admin) Obtener detalle de un usuario específico por ID
PUT /api/admin/users/status - (Admin) Cambiar estado de cuenta de usuario (activar/desactivar)
DELETE /api/admin/users - (Admin) Desactivar usuario (soft delete)
POST /api/admin/users/filter - (Admin) Filtrar usuarios con paginación

Productos

GET /api/product/catalog - Explorar catálogo de productos con filtros y paginación
GET /api/product/{id} - Obtener detalle de un producto específico
GET /api/product/admin - (Admin) Listar productos para administración
POST /api/product - (Admin) Crear nuevo producto con imágenes
PUT /api/product/{id} - (Admin) Actualizar información de producto
DELETE /api/product/{id} - (Admin) Eliminar producto (soft delete si tiene órdenes)

Carrito de Compras

GET /api/cartitem - Obtener items del carrito del usuario
GET /api/cartitem/total - Obtener total del carrito
POST /api/cartitem - Añadir producto al carrito
PATCH /api/cartitem/{productId} - Actualizar cantidad de un producto en el carrito
DELETE /api/cartitem/{productId} - Eliminar producto del carrito

Órdenes y Pedidos

POST /api/order - Crear nueva orden (checkout)
GET /api/order - Obtener listado de órdenes del usuario
POST /api/order/filter - Filtrar órdenes del usuario
GET /api/order/{id}/pdf - Descargar PDF de una orden específica

Todos los endpoints protegidos requieren autenticación mediante token JWT que debe ser enviado en la cabecera HTTP Authorization con el formato Bearer {token}.
Códigos de Estado HTTP

200 OK - Petición exitosa
201 Created - Recurso creado exitosamente
400 Bad Request - Error en los datos enviados
401 Unauthorized - No autenticado
403 Forbidden - No autorizado para la acción
404 Not Found - Recurso no encontrado
500 Internal Server Error - Error interno del servidor

Formato de Respuesta
Las respuestas siguen un formato estandarizado utilizando la clase ApiResponse<T>:
json{
  "success": true,
  "message": "Operación exitosa",
  "data": { /* Datos de la respuesta */ },
  "errors": [] /* Lista de errores, si existen */
}
Paginación y Filtrado
Varios endpoints que retornan listas (como usuarios y productos) soportan paginación:

En usuarios: Página predeterminada de 20 registros
Los filtros varían según el endpoint y están documentados en los DTOs correspondientes

----Pruebas----
Flujos recomendados para probar:

Registrar nuevo usuario
Iniciar sesión y obtener JWT
Explorar catálogo de productos
Agregar productos al carrito
Realizar pedido

----Despliegue----
Para producción:
- Configurar appsettings.json con datos reales
- Habilitar HTTPS
- Configurar CORS

---Contribuciones----
¡Bienvenidas!
Sigue las pautas de Conventional Commits y usa Pull Requests para proponer cambios.

Este proyecto está bajo la licencia MIT.