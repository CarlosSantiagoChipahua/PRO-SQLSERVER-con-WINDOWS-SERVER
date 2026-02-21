
-- Crear la base de datos
--CREATE DATABASE TallerMecanico;
GO

--USE TallerMecanico;
GO

-- Tabla Clientes
CREATE TABLE Clientes (
    clave_interna VARCHAR(20) PRIMARY KEY,
    rfc VARCHAR(13) NOT NULL UNIQUE,
    nombre VARCHAR(50) NOT NULL,
    apellido_paterno VARCHAR(50) NOT NULL,
    apellido_materno VARCHAR(50),
    telefono1 VARCHAR(15) NOT NULL,
    telefono2 VARCHAR(15),
    telefono3 VARCHAR(15),
    calle VARCHAR(100) NOT NULL,
    numero VARCHAR(10) NOT NULL,
    colonia VARCHAR(50) NOT NULL,
    codigo_postal VARCHAR(5) NOT NULL,
    ciudad VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE,
    fecha_registro DATE NOT NULL DEFAULT GETDATE(),
    activo BIT NOT NULL DEFAULT 1  -- 1 = activo, 0 = inactivo (borrado lógico)
);

-- Tabla Vehiculos
CREATE TABLE Vehiculos (
    numero_serie VARCHAR(50) PRIMARY KEY,
    placas VARCHAR(15) NOT NULL UNIQUE,
    marca VARCHAR(50) NOT NULL,
    modelo VARCHAR(50) NOT NULL,
    anio INT NOT NULL,
    color VARCHAR(30) NOT NULL,
    kilometraje INT NOT NULL,
    tipo VARCHAR(30) NOT NULL CHECK (tipo IN ('Sedán', 'SUV', 'Pickup', 'Hatchback', 'Coupé', 'Convertible', 'Furgoneta', 'Otro')),
    clave_interna VARCHAR(20) NOT NULL,
    antigüedad AS (YEAR(GETDATE()) - anio),  -- Columna calculada
    activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Vehiculos_Cliente FOREIGN KEY (clave_interna) REFERENCES Clientes(clave_interna)
);

-- Tabla Mecanicos
CREATE TABLE Mecanicos (
    num_empleado INT IDENTITY(1,1) PRIMARY KEY,
    rfc VARCHAR(13) NOT NULL UNIQUE,
    nombre VARCHAR(50) NOT NULL,
    apellido_paterno VARCHAR(50) NOT NULL,
    apellido_materno VARCHAR(50),
    especialidades VARCHAR(200) NOT NULL,
    telefono VARCHAR(15) NOT NULL,
    salario DECIMAL(10,2) NOT NULL,
    anios_experiencia INT NOT NULL,
    activo BIT NOT NULL DEFAULT 1
);

-- Tabla Servicios
CREATE TABLE Servicios (
    clave_servicio INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(500),
    costo_base DECIMAL(10,2) NOT NULL,
    tiempo_estimado DECIMAL(4,2) NOT NULL,  -- Horas
    activo BIT NOT NULL DEFAULT 1
);

-- Tabla Refacciones 
CREATE TABLE Refacciones (
    codigo_refaccion INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    marca VARCHAR(50) NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    stock_actual INT NOT NULL,
    stock_minimo INT NOT NULL,
    proveedor VARCHAR(100) NOT NULL,
    activo BIT NOT NULL DEFAULT 1
);

-- Tabla Ordenes_Servicio
CREATE TABLE Ordenes_Servicio (
    folio_orden INT IDENTITY(1,1) PRIMARY KEY,
    numero_serie VARCHAR(50) NOT NULL,
    fecha_ingreso DATE NOT NULL DEFAULT GETDATE(),
    fecha_estimada_entrega DATE NOT NULL,
    fecha_real_entrega DATE,
    estado VARCHAR(20) NOT NULL CHECK (estado IN ('Abierta', 'En proceso', 'Finalizada', 'Cancelada')),
    costo_total DECIMAL(10,2),
    activo BIT NOT NULL DEFAULT 1,  -- Por consistencia, aunque se podría filtrar por estado
    CONSTRAINT FK_Ordenes_Vehiculo FOREIGN KEY (numero_serie) REFERENCES Vehiculos(numero_serie)
);

-- Tabla Orden_Servicio_Mecanicos
CREATE TABLE Orden_Servicio_Mecanicos (
    folio_orden INT NOT NULL,
    num_empleado INT NOT NULL,
    PRIMARY KEY (folio_orden, num_empleado),
    CONSTRAINT FK_OSM_Orden FOREIGN KEY (folio_orden) REFERENCES Ordenes_Servicio(folio_orden),
    CONSTRAINT FK_OSM_Mecanico FOREIGN KEY (num_empleado) REFERENCES Mecanicos(num_empleado)
);

-- Tabla Orden_Servicio_Servicios
CREATE TABLE Orden_Servicio_Servicios (
    folio_orden INT NOT NULL,
    clave_servicio INT NOT NULL,
    cantidad INT NOT NULL DEFAULT 1,
    precio_aplicado DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (folio_orden, clave_servicio),
    CONSTRAINT FK_OSS_Orden FOREIGN KEY (folio_orden) REFERENCES Ordenes_Servicio(folio_orden),
    CONSTRAINT FK_OSS_Servicio FOREIGN KEY (clave_servicio) REFERENCES Servicios(clave_servicio)
);

-- Tabla Orden_Servicio_Refacciones
CREATE TABLE Orden_Servicio_Refacciones (
    folio_orden INT NOT NULL,
    codigo_refaccion INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario_aplicado DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (folio_orden, codigo_refaccion),
    CONSTRAINT FK_OSR_Orden FOREIGN KEY (folio_orden) REFERENCES Ordenes_Servicio(folio_orden),
    CONSTRAINT FK_OSR_Refaccion FOREIGN KEY (codigo_refaccion) REFERENCES Refacciones(codigo_refaccion)
);
GO

-- =====================================================
-- INSERCIÓN DE DATOS
-- =====================================================

-- Clientes (5 registros)
INSERT INTO Clientes (clave_interna, rfc, nombre, apellido_paterno, apellido_materno, telefono1, telefono2, telefono3, calle, numero, colonia, codigo_postal, ciudad, email, fecha_registro)
VALUES
('CLI001', 'GOMJ800101HDF', 'Juan', 'González', 'Martínez', '5551234567', NULL, NULL, 'Av. Reforma', '123', 'Centro', '06000', 'Ciudad de México', 'juan.gonzalez@email.com', '2023-01-15'),
('CLI002', 'LOPA850512MDF', 'Ana', 'López', 'Pérez', '5559876543', '5552345678', NULL, 'Calle Roble', '45', 'Del Valle', '03100', 'Ciudad de México', 'ana.lopez@email.com', '2023-02-20'),
('CLI003', 'MART900101HDF', 'Carlos', 'Martínez', 'Sánchez', '5558765432', NULL, NULL, 'Av. Insurgentes', '789', 'Roma', '06700', 'Ciudad de México', 'carlos.martinez@email.com', '2023-03-10'),
('CLI004', 'SALG920202MDF', 'María', 'Salgado', 'García', '5553456789', '5554567890', '5555678901', 'Calle Durango', '234', 'Condesa', '06100', 'Ciudad de México', 'maria.salgado@email.com', '2023-04-05'),
('CLI005', 'HERJ880303HDF', 'José', 'Hernández', 'Jiménez', '5556781234', NULL, NULL, 'Av. Universidad', '567', 'Narvarte', '03020', 'Ciudad de México', 'jose.hernandez@email.com', '2023-05-12');
GO

-- Vehículos (5 registros)
INSERT INTO Vehiculos (numero_serie, placas, marca, modelo, anio, color, kilometraje, tipo, clave_interna)
VALUES
('1HGCM82633A123456', 'ABC123', 'Honda', 'Civic', 2018, 'Gris', 45000, 'Sedán', 'CLI001'),
('2G1FP22G5Y2171845', 'DEF456', 'Chevrolet', 'Camaro', 2019, 'Amarillo', 30000, 'Coupé', 'CLI002'),
('3VWDP7AJ3DM412345', 'GHI789', 'Volkswagen', 'Jetta', 2020, 'Blanco', 25000, 'Sedán', 'CLI003'),
('4S3BMHB68B3287654', 'JKL012', 'Subaru', 'Forester', 2017, 'Verde', 60000, 'SUV', 'CLI004'),
('5N1AR2MM9JC654321', 'MNO345', 'Nissan', 'Frontier', 2021, 'Rojo', 15000, 'Pickup', 'CLI005');
GO

-- Mecánicos (5 registros)
INSERT INTO Mecanicos (rfc, nombre, apellido_paterno, apellido_materno, especialidades, telefono, salario, anios_experiencia)
VALUES
('RAMJ800101', 'Javier', 'Ramírez', 'López', 'Motor, Transmisión', '5551112233', 15000.00, 8),
('GONA850202', 'Ana', 'González', 'Mora', 'Frenos, Suspensión', '5552223344', 14000.00, 6),
('PERE900303', 'Luis', 'Pérez', 'Castro', 'Electricidad, Diagnóstico', '5553334455', 16000.00, 10),
('DIAZ950404', 'Mario', 'Díaz', 'Ruiz', 'Aire acondicionado, Refrigeración', '5554445566', 13000.00, 4),
('TORR880505', 'Laura', 'Torres', 'Vega', 'Hojalatería, Pintura', '5555556677', 15500.00, 7);
GO

-- Servicios (5 registros)
INSERT INTO Servicios (nombre, descripcion, costo_base, tiempo_estimado)
VALUES
('Cambio de aceite', 'Cambio de aceite y filtro', 500.00, 1.0),
('Alineación y balanceo', 'Alineación de dirección y balanceo de llantas', 800.00, 1.5),
('Revisión de frenos', 'Inspección y ajuste de frenos', 400.00, 1.0),
('Diagnóstico computarizado', 'Escaneo electrónico del vehículo', 600.00, 0.5),
('Cambio de bujías', 'Sustitución de bujías y cables', 700.00, 1.5);
GO

-- Refacciones (15 registros)
INSERT INTO Refacciones (nombre, marca, precio_unitario, stock_actual, stock_minimo, proveedor)
VALUES
('Filtro de aceite', 'Fram', 80.00, 50, 10, 'Autopartes SA'),
('Aceite 5W30 (1L)', 'Mobil', 120.00, 200, 30, 'Lubricantes MX'),
('Pastillas de freno delanteras', 'Bosch', 450.00, 30, 5, 'Frenos y Más'),
('Bujías de iridio (juego)', 'NGK', 350.00, 25, 5, 'Autopartes SA'),
('Batería 12V', 'LTH', 1200.00, 15, 3, 'Acumuladores Azteca'),
('Filtro de aire', 'Bosch', 150.00, 40, 8, 'Autopartes SA'),
('Amortiguador delantero', 'Monroe', 850.00, 12, 2, 'Suspensiones SA'),
('Rótula de suspensión', 'Moog', 320.00, 18, 4, 'Suspensiones SA'),
('Correa de distribución', 'Gates', 580.00, 10, 2, 'Refacciones Universal'),
('Bomba de agua', 'ACDelco', 780.00, 8, 1, 'Refacciones Universal'),
('Alternador', 'Bosch', 2500.00, 5, 1, 'Electropartes'),
('Foco delantero', 'Philips', 200.00, 30, 5, 'Iluminación Auto'),
('Líquido de frenos (1L)', 'Castrol', 90.00, 60, 10, 'Lubricantes MX'),
('Anticongelante (4L)', 'Prestone', 220.00, 40, 8, 'Lubricantes MX'),
('Filtro de combustible', 'Bosch', 180.00, 25, 5, 'Autopartes SA');
GO

-- Órdenes de Servicio (5 registros)
INSERT INTO Ordenes_Servicio (numero_serie, fecha_ingreso, fecha_estimada_entrega, fecha_real_entrega, estado, costo_total)
VALUES
('1HGCM82633A123456', '2024-02-01', '2024-02-02', '2024-02-02', 'Finalizada', 1020.00),
('2G1FP22G5Y2171845', '2024-02-05', '2024-02-06', '2024-02-06', 'Finalizada', 1300.00),
('3VWDP7AJ3DM412345', '2024-02-10', '2024-02-12', '2024-02-12', 'Finalizada', 750.00),
('4S3BMHB68B3287654', '2024-02-15', '2024-02-16', NULL, 'En proceso', NULL),
('5N1AR2MM9JC654321', '2024-02-18', '2024-02-20', NULL, 'Abierta', NULL);
GO

-- Relación Orden_Servicio_Mecanicos
INSERT INTO Orden_Servicio_Mecanicos (folio_orden, num_empleado)
VALUES
(1, 1), (1, 3),
(2, 2), (2, 4),
(3, 1), (3, 5),
(4, 3), (4, 2),
(5, 4), (5, 5);
GO

-- Relación Orden_Servicio_Servicios
INSERT INTO Orden_Servicio_Servicios (folio_orden, clave_servicio, cantidad, precio_aplicado)
VALUES
(1, 1, 1, 500.00),
(1, 2, 1, 800.00),
(2, 3, 1, 400.00),
(2, 4, 1, 600.00),
(2, 5, 1, 700.00),
(3, 1, 1, 500.00),
(3, 3, 1, 400.00),
(4, 4, 1, 600.00),
(5, 2, 1, 800.00);
GO

-- Relación Orden_Servicio_Refacciones
INSERT INTO Orden_Servicio_Refacciones (folio_orden, codigo_refaccion, cantidad, precio_unitario_aplicado)
VALUES
(1, 1, 1, 80.00),
(1, 2, 4, 120.00),
(2, 3, 1, 450.00),
(2, 4, 1, 350.00),
(2, 5, 1, 1200.00),
(3, 1, 1, 80.00),
(3, 2, 4, 120.00),
(3, 6, 1, 150.00),
(4, 7, 2, 850.00),
(4, 8, 2, 320.00),
(5, 9, 1, 580.00);
GO