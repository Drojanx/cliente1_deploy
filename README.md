# ClienteAPI
***
## _Backend_

Para ejecutar la Base de Datos en Docker (Aunque actualmente está configurada para funcionar con la Base de Datos desplegada en Azure):
```
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=MyPassword-1234" -p 3012:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

Seguidamente, para crear los esquemas en la Base de Datos almacenados en la carpeta de Migrations:
```
dotnet ef database update
```


Para iniciar la API:
```
dotnet run
```
La URL a la que habrá mandar las peticiones CRUD es: https://localhost:3022

Contiene también un fichero **ClienteAA.postman_collection.json** con una coleccción Postman con la que probar las peticiones.

Además, al lanzarse la API, se genera un Openapi 3 en la url http://localhost:3022/swagger

Por otro lado, indicar que se ha configurado la API para conectar con la base de datos desplegada en azure en este servidor: alanzexamenserver.database.windows.net

Además, la API está desplegada en Azure con la siguiente URL (está conectada a la misma Base de Datos desplegada en Azure): https://alanzcliente1examen.azurewebsites.net
