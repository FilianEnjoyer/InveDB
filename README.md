PARA PORDER USAR EL PROGRAMA CREEN ESTA BASE DE DATOS 

[LINK PARA DESCARGAR LOS COMANDOS SQL](https://ensenadatecnm-my.sharepoint.com/:u:/g/personal/l23760349_ensenada_tecnm_mx/EUrrbJEKtP5JgzAF6QrlMcUB6AZvs0qO5XMS6ifzuFD6Vw?e=I0ypjg)



Puedes cambiar el nombre de la base de datos a la que se conecta en el archivo appsettings.json

CAMBIEN

"ConnectionStrings": {
  "DefaultConnection": "Server=MAINPC\\REALDB;Database=InveDB2;User Id=sa;Password=FilianEnjoyer;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;"
},

POR    

"ConnectionStrings": {
  "DefaultConnection": "Server=SU SERVIDOR;Database=NOMBRE DE LA BASE DE DATOS;User Id=USUARIO;Password=CONTRASEÑA;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;"
},

