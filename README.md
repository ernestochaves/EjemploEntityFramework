# EjemploEntityFramework
Ejemplo de uso y configuración básico de código de acceso a datos usando entity framework. 

**ADVERTENCIA**
Existe muchisima documentación de como usar EF. Éste es un ejemplo basico con lo minimo necesario para conectarse a la base de datos, pero las diferentes opciones están bien documentadas. 
- https://docs.microsoft.com/en-us/ef/ef6/index
- https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/existing-database
- https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application

El proyecto de ejemplo consta de un proyecto con entidades, que en este caso además representan las tablas con las que se va a trabajar en la base de datos. 
**TIP**: Recomiendo investigar un poco sobre ORMs en general, pero es una práctica común que las entidades de alguna forma hagan una similituda a la estructura relacional. En este caso, una base de datos MYSQL. 

Requisitos: En este caso, van a necesitar tener algun entendimiento de SQL y tener configurado el MYSQL con su base de datos al menos localmente. 

# MYSQL
Para el ejemplo, solamente instalar un servidor y algun editor para correr los scripts. Yo instalé usando ésta descarga https://dev.mysql.com/downloads/file/?id=476476 
Y para los scripts use workbench. Se instala como cualquier producto windows, el servidor gratuito y el workbench. 
Por supuesto, cualquier base de datos relacional la pueden usar, la diferencia va a ser el connection string. 

**INSTALAR CONNECTOR/NET**
Es necesario instalar este componente. Ver requisitos para conectar EF6 con MySQL
https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework60.html#connector-net-ef6-config 


## Creación de la base de datos
La base de datos de ejemplo usada en éste proyecto es muuuy simple. Solamente son "items" con un Id, Nombre y Descripcion.

El script está en el repo pero es sencillo:

`CREATE Database Demo;`

Crea una base de datos llamada demo. 

`CREATE TABLE Items 
(
	Id int NOT NULL Primary Key auto_increment, 
    Name VARCHAR(100),
	Description VARCHAR(200)	
);`

Crea una table con las propiedades. Id es la llave primaria, un entero auto incrementado. 

# El proyecto Demo.Entities

Es un proyecto donde agrego una clase para representar las tablas de la base de datos. En éste caso, la tabla items corresponde a la clase Item. Para agregar mas tablas se agregan las clases correspondientes. 

## Entity Framework
[Entity Framework](https://docs.microsoft.com/en-us/ef/) es un [ORM](https://en.wikipedia.org/wiki/Object-relational_mapping) hecho por Microsoft. Hay muchas configuraciones posibles, algunas donde se hace una generación de código, a partir de la base de datos (database first) o se trabaja con un modelo o diagrama, o se genera la base de datos a partir de el código (las clases). 
Para éste ejemplo, intenté usar una configuración donde se use la menor cantidad de "magia" o generación automática de código, solamente para ejemplificar como se asocian las clases y las tablas. 
Yo usé **EF6** para el ejemplo. 

## NUGET

Para "instalar" y mantener las librerías en .net es muy común usar [NUGET](https://docs.microsoft.com/en-us/nuget/what-is-nuget), En el enlace se explican los detalles pero basicamente, es el mecanismo mas comun en .net para descargar, actualizar y mantener librerias de codigo de terceros en sus proyectos.  

La forma sencilla de usar NUGET en uno de sus proyectos, es dar click derecho al proyecto desde el explorador de soluciones, y buscar la opción "Manage Nuget Packages". 
Eso abre una ventana con tres pestañas, "Browse", "Installed" y "Updates". 

Para este caso, agregué la librería de Entity Framework 6 al proyecto demo.entities. La forma de hacerlo es escribir entityframework, seleccionarlo e instalarlo. La versión utilizada es 6.2.0. 

Una vez hecho eso por primera vez, NUGET agrega un archivo packages.config al proyecto, que contiene la configuración de packetes instalados. 

Tambien es necesario instalar el conector para MySQL que se llama  MySql.Data.Entity de la misma forma que con EntityFramework. 


## Configuración del DataContext

El término datacontext es común en el uso de entity framework. "El datacontext" es el repositorio genérico que se usa para interactuar con las clases, y por tanto, con las tablas en la base de datos. 
Para poder conectarse con la base de datos es necesario crear o tener una clase data context. 

1. Como mínimo, la clase **DemoDBContext** es muy importante. Notar como la clase puede tener cualquier nombre, pero debe heredar de DbContext. 
2. Por cada tabla en la base de datos, hay que agregar una propiedad pública de tipo genérico DBSet, y el parametro de tipo debe ser la clase que representa a la tabla. Por ejemplo, nosotros tenemos una clase Item que representa la tabla items, por lo que en el DBContext debemos agregar una propiedad  
`public DBSet<Item> Items{get;set;}`. 
Notar que los nombres de las propiedades, las mayúsculas, minúsculas, etc, son importantes para que el ORM funciones por defecto. De forma que el sepa qué colección refleja cual tabla. La convención de nombres es muy importante aquí (si se revisa la documentación, hay formas de cambiar la convención si se desea).
3. La configuración básica se hace en el constructor de la clase dbcontext. En éste ejemplo básico, por ejemplo, no estamos haciendo ninguna generación automática de cambios en la base de datos así que el constructor es muy simple. 
`public DemoDBContext() : base("DemoDBContextConnection")
        {
            Database.SetInitializer<DemoDBContext>(null);
        }`

La parte que dice base('DemoDBContextConnection') lo que hace es pasarle al constructor el nombre de la cadena de conección. La configuración final se hace en el archivo de configuración (web.config o app.config dependiendo si es web o windows) desde donde se usa el proyecto. 


# Demo.Console

En este proyecto, usamos la base de datos al agregar una referencia al proyecto que creamos inicialmente. 

Hay alguna configuración necesaria:

1. Agregar referencia al proyecto inicial. 
2. Agregar los mismos packetes nuget que en el proyecto inicial.
3. Modificar el archivo App.Config para configurar la información que necesita la librería para conectarse mediante entity framework a MYSQL. 
Para eso, en el archivo app.config, dentro del elemento "configuration", se agrega la configuración de Entity Framework y el connectionString (la información del servidor mysql y el nombre de la base de datos).


  ```xml
 <connectionStrings>
    <!-- El name debe coincidir con lo que se escribió en el DBContext y se le pasó al constructor. El server, port y database depende de cada proyecto y de como se configuró mysql -->
    <add name="DemoDBContextConnection" providerName="MySql.Data.MySqlClient" connectionString="server=127.0.0.1;port=3306;database=demo;uid=root;password=root" />
  </connectionStrings>
  <!-- Esta parte de requerida para que EF6 funcione.  -->
  <entityFramework codeConfigurationType="MySql.Data.Entity.MySqlEFConfiguration, MySql.Data.Entity.EF6">
    <defaultConnectionFactory type="System.Data.Entity.Infrastructure.SqlConnectionFactory, EntityFramework" />
    <providers>

      <provider invariantName="MySql.Data.MySqlClient" type="MySql.Data.MySqlClient.MySqlProviderServices, MySql.Data.Entity.EF6, Version=6.8.8.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d"></provider>
    </providers>
  </entityFramework>
```

**IMPORTANTE**
Puede que al agregar los paquetes NUGET configure los proyectos, esto puede generar problemas algunas secciones en el app.config estan repetidas. Asi que revisar que la seccion de providers no repita los elementos. 

4. La forma más directa de usar el Data Context que ya configuramos, es declarar una nueva instancia dentro de un bloque "[using](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement)". De forma que dentro del bloque al terminar de usarlo los recursos usados (la conección) se destruya adecuadamente. 

En el ejemplo, la parte comentada crea unos datos, y luego, para asegurarse que fueron creados, los lee y los muestra en pantalla. 

**Importante**
El ejemplo no tiene proyecto de logica de negocios, etc. Generalmente, el acceso a los datos no se hace desde la consola o desde el controlador, sino que aparecen todas esas capas que normalmente están involucradas en un proyecto. 




**Errores encontrados**

Al parecer, el conector de oracle para mysql da un error extraño de seguridad que solo se resuelve usando una version mas antigua en nuget. Revisar lo que está en packages.config en cada proyecto. Yo termine installando mysql connector 6.9.12 (https://dev.mysql.com/downloads/file/?id=478117). Y las versiones de los packetes son 6.9.11. 
Tomar en cuenta que pueden solo modificar el proyecto existente.
El conector parece no ser gran cosa porque si las versiones no calzan no funciona bien con EF. 



