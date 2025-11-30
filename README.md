PARA PORDER USAR EL PROGRAMA CREEN ESTA BASE DE DATOS 

[LINK PARA DESCARGAR LOS COMANDOS SQL](https://ensenadatecnm-my.sharepoint.com/:u:/g/personal/l23760349_ensenada_tecnm_mx/EdMnjXpd09JGl0ulnj45d8IBOotjINjonUMXv6pBrfqIbQ?e=9wvuAq)


Puedes cambiar el nombre de la base de datos a la que se conecta en el archivo appsettings.json  OJO HAY 3 SECCIONES DONDE SE TIENE QUE CAMBIAR EL NOMBRE DE LA BASE DE DATOS EN EL SQL LAS 2 PRIMERA EN EL INICIO Y LA ULTIMA DESPUES DE CREAR LOS USUARIOS CON MASTER

CAMBIEN

"ConnectionStrings": {
  "DefaultConnection": "Server=MainPC\\REALDB;Database=InveDB5;User Id=sa;Password=FilianEnjoyer;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;",
  "Conexion_Admin": "Server=MainPC\\REALDB;Database=InveDB5;User Id=admin_login;Password=1234;Encrypt=True;TrustServerCertificate=True;",
  "Conexion_Encargado": "Server=MainPC\\REALDB;Database=InveDB5;User Id=encargado_login;Password=1234;Encrypt=True;TrustServerCertificate=True;",
  "Conexion_Capturista": "Server=MainPC\\REALDB;Database=InveDB5;User Id=capturista_login;Password=1234;Encrypt=True;TrustServerCertificate=True;"
},

POR    

"ConnectionStrings": {
  "DefaultConnection": "Server=SU SERVIDOR;Database=NOMBRE DE LA BASE DE DATOS;User Id=USUARIO;Password=CONTRASEÑA;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  "Conexion_Admin": "Server=SU SERVIDOR;Database=NOMBRE DE LA BASE DE DATOS;User Id=admin_login;Password=1234;Encrypt=True;TrustServerCertificate=True;",
  "Conexion_Encargado": "Server=SU SERVIDOR;Database=NOMBRE DE LA BASE DE DATOS;User Id=encargado_login;Password=1234;Encrypt=True;TrustServerCertificate=True;",
  "Conexion_Capturista": "Server=SU SERVIDOR;Database=NOMBRE DE LA BASE DE DATOS;User Id=capturista_login;Password=1234;Encrypt=True;TrustServerCertificate=True;"
},

LOS LOGINS PARA INICIAR SESION POR ROL SON

ADMINISTRADOR

Usuario: admin_login
Contraseña: 1234

ENCARGADO DE ALMACEN

Usuario: encargado_login
Contraseña: 1234

CAPTURISTA

Usuario: capturista_login
Contraseña: 1234
