PARA PORDER USAR EL PROGRAMA CREEN ESTA BASE DE DATOS 


Puedes cambiar el nombre de la base de datos a la que se conecta en el archivo appsettings.json

CAMBIEN

"ConnectionStrings": {
  "DefaultConnection": "Server=MAINPC\\REALDB;Database=InveDB2;User Id=sa;Password=FilianEnjoyer;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;"
},

POR    

"ConnectionStrings": {
  "DefaultConnection": "Server=SU SERVIDOR;Database=NOMBRE DE LA BASE DE DATOS;User Id=USUARIO;Password=CONTRASEÑA;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=True;"
},
------------------------------------------------------------
-- 1️⃣ CREACIÓN DE BASE DE DATOS
------------------------------------------------------------
CREATE DATABASE InveDB;
GO
USE InveDB;
GO

------------------------------------------------------------
-- 2️⃣ TABLAS PRINCIPALES
------------------------------------------------------------
CREATE TABLE Categoria (
    id_categoria INT PRIMARY KEY IDENTITY,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255)
);
GO

CREATE TABLE Proveedor (
    id_proveedor INT PRIMARY KEY IDENTITY,
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(20),
    direccion VARCHAR(255)
);
GO

CREATE TABLE Sucursal (
    id_sucursal INT PRIMARY KEY IDENTITY,
    nombre VARCHAR(100) NOT NULL,
    direccion VARCHAR(255),
    telefono VARCHAR(20)
);
GO

CREATE TABLE Producto (
    id_producto INT PRIMARY KEY IDENTITY,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255),
    unidad_medida VARCHAR(20),
    id_categoria INT,
    id_proveedor INT NULL,
    precio_unitario DECIMAL(10,2),
    FOREIGN KEY (id_categoria) REFERENCES Categoria(id_categoria),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedor(id_proveedor)
);
GO

CREATE TABLE Inventario (
    id_inventario INT PRIMARY KEY IDENTITY,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL DEFAULT 0,
    FOREIGN KEY (id_producto) REFERENCES Producto(id_producto)
);
GO

CREATE TABLE Movimiento (
    id_movimiento INT PRIMARY KEY IDENTITY,
    id_producto INT NOT NULL,
    tipo CHAR(1) CHECK (tipo IN ('E','S')),
    cantidad INT NOT NULL,
    fecha DATETIME DEFAULT GETDATE(),
    id_proveedor INT NULL,
    id_sucursal INT NULL,
    FOREIGN KEY (id_producto) REFERENCES Producto(id_producto),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedor(id_proveedor),
    FOREIGN KEY (id_sucursal) REFERENCES Sucursal(id_sucursal)
);
GO

------------------------------------------------------------
-- 3️⃣ DATOS INICIALES (categorías, productos ejemplo)
------------------------------------------------------------
INSERT INTO Categoria (nombre, descripcion) VALUES
('Bebidas', 'Refrescos, jugos y agua'),
('Ingredientes', 'Materia prima para pizzas'),
('Postres', 'Dulces y complementos'),
('Salsas', 'Aderezos y salsas preparadas');
GO

INSERT INTO Producto (nombre, descripcion, unidad_medida, precio_unitario, id_categoria)
VALUES
('Coca-Cola 600ml', 'Bebida gaseosa', 'Unidad', 18.50, 1),
('Agua Bonafont 1L', 'Agua natural', 'Unidad', 12.00, 1),
('Harina de trigo 1kg', 'Harina para base de pizza', 'Kg', 28.90, 2),
('Queso mozzarella', 'Queso rallado fresco', 'Kg', 75.00, 2),
('Salsa BBQ', 'Salsa para pizza o alitas', 'Botella', 45.50, 4),
('Brownie', 'Postre individual de chocolate', 'Unidad', 22.00, 3);
GO

-- (Opcional) insertar proveedores y sucursales de ejemplo si no existen
INSERT INTO Proveedor (nombre, telefono, direccion) VALUES
('Bebidas Mundiales', '555-1000', 'Calle A #1'),
('Lácteos del Norte', '555-1001', 'Calle B #2'),
('Distribuidora El Buen Sabor', '555-1002', 'Calle C #3');
GO

INSERT INTO Sucursal (nombre, direccion, telefono) VALUES
('Sucursal Central', 'Av. Principal 123', '555-2000'),
('Sucursal Norte', 'Av. Norte 45', '555-2001');
GO

------------------------------------------------------------
-- 4️⃣ TRIGGERS (mantienen Inventario sincronizado con Movimiento)
------------------------------------------------------------
CREATE OR ALTER TRIGGER trg_Movimiento_Entrada
ON Movimiento
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insertar en Inventario si no existe el producto (cuando es entrada)
    INSERT INTO Inventario (id_producto, cantidad)
    SELECT i.id_producto, i.cantidad
    FROM inserted i
    WHERE i.tipo = 'E'
      AND NOT EXISTS (SELECT 1 FROM Inventario inv WHERE inv.id_producto = i.id_producto);

    -- Si ya existe, sumar la cantidad entrante
    UPDATE inv
    SET inv.cantidad = inv.cantidad + i.cantidad
    FROM Inventario inv
    INNER JOIN inserted i ON inv.id_producto = i.id_producto
    WHERE i.tipo = 'E';
END;
GO

CREATE OR ALTER TRIGGER trg_Movimiento_Salida
ON Movimiento
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Restar la cantidad en Inventario cuando se registra una salida
    UPDATE inv
    SET inv.cantidad = inv.cantidad - i.cantidad
    FROM Inventario inv
    INNER JOIN inserted i ON inv.id_producto = i.id_producto
    WHERE i.tipo = 'S';
END;
GO

------------------------------------------------------------
-- 5️⃣ PROCEDIMIENTOS ALMACENADOS (CRUDs básicos)
-- (Categoría, Proveedor, Sucursal, Producto y búsqueda de inventario)
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_Categoria_GetAll AS
BEGIN
    SELECT id_categoria, nombre, descripcion FROM Categoria ORDER BY nombre;
END;
GO

CREATE OR ALTER PROCEDURE sp_Proveedor_GetAll AS
BEGIN
    SELECT id_proveedor, nombre, telefono, direccion FROM Proveedor ORDER BY nombre;
END;
GO

CREATE OR ALTER PROCEDURE sp_Sucursal_GetAll AS
BEGIN
    SELECT id_sucursal, nombre, direccion, telefono FROM Sucursal ORDER BY nombre;
END;
GO

CREATE OR ALTER PROCEDURE sp_Producto_GetAll AS
BEGIN
    SELECT p.id_producto, p.nombre, p.descripcion, p.unidad_medida,
           p.id_categoria, p.precio_unitario, c.nombre AS nombre_categoria
    FROM Producto p
    LEFT JOIN Categoria c ON p.id_categoria = c.id_categoria
    ORDER BY p.nombre;
END;
GO

CREATE OR ALTER PROCEDURE sp_ObtenerInventarioFiltrado
    @fecha DATE = NULL,
    @nombreProveedor VARCHAR(100) = NULL,
    @nombreCategoria VARCHAR(100) = NULL,
    @busquedaProducto VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH UltimoMovimiento AS (
        SELECT 
            m.id_producto,
            pr.nombre AS Proveedor,
            CAST(MAX(m.fecha) AS DATE) AS Ultima_Fecha
        FROM Movimiento m
        LEFT JOIN Proveedor pr ON m.id_proveedor = pr.id_proveedor
        WHERE m.tipo = 'E'
        GROUP BY m.id_producto, pr.nombre
    )

    SELECT DISTINCT
        p.id_producto,
        p.nombre AS Producto,
        c.nombre AS Categoria,
        i.cantidad AS Cantidad_Actual,
        p.unidad_medida,
        p.precio_unitario AS Precio_Unitario,
        p.descripcion,
        u.Proveedor,
        u.Ultima_Fecha
    FROM Inventario i
    INNER JOIN Producto p ON i.id_producto = p.id_producto
    INNER JOIN Categoria c ON p.id_categoria = c.id_categoria
    LEFT JOIN UltimoMovimiento u ON u.id_producto = p.id_producto
    WHERE 
        (@busquedaProducto IS NULL OR p.nombre LIKE '%' + @busquedaProducto + '%')
        AND (@nombreCategoria IS NULL OR c.nombre = @nombreCategoria)
        AND (@nombreProveedor IS NULL OR u.Proveedor = @nombreProveedor)
        AND (@fecha IS NULL OR u.Ultima_Fecha = @fecha)
    ORDER BY c.nombre, p.nombre;
END;
GO

------------------------------------------------------------
-- 6️⃣ INSERCIONES DE MOVIMIENTOS (ENTRADAS Y SALIDAS)
-- Colocar después de crear los triggers para que estos actualicen Inventario.
------------------------------------------------------------
-- Movimientos de entrada (registros que suman inventario)
INSERT INTO Movimiento (id_producto, tipo, cantidad, id_proveedor, id_sucursal)
VALUES
(1, 'E', 60, 3, 1),
(2, 'E', 80, 3, 1),
(3, 'E', 25, 1, 1),
(4, 'E', 15, 2, 1),
(5, 'E', 10, 1, 1),
(6, 'E', 20, 1, 1);
GO

-- Movimientos de salida (registros que restan inventario)
INSERT INTO Movimiento (id_producto, tipo, cantidad, id_sucursal)
VALUES
(1, 'S', 10, 1),
(2, 'S', 5, 1),
(3, 'S', 2, 1),
(4, 'S', 1, 1);
GO

------------------------------------------------------------
-- 7️⃣ CONSULTA FINAL DE VERIFICACIÓN
-- Muestra el inventario calculado tras aplicar los movimientos
------------------------------------------------------------
SELECT 
    p.id_producto AS [ID Producto],
    p.nombre AS [Nombre del Producto],
    p.descripcion AS [Descripción],
    ISNULL(i.cantidad, 0) AS [Cantidad],
    p.precio_unitario AS [Precio],
    c.nombre AS [Categoría]
FROM Producto p
INNER JOIN Categoria c ON p.id_categoria = c.id_categoria
LEFT JOIN Inventario i ON p.id_producto = i.id_producto
ORDER BY c.nombre, p.nombre;
GO
