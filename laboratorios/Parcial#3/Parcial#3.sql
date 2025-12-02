

CREATE DATABASE Ismael_Martinez;
GO

USE Ismael_Martinez;
GO

CREATE TABLE IM_Abogados (
    id_abogado INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(150) NOT NULL,
    telefono NVARCHAR(20),
    direccion NVARCHAR(250)
);
GO

CREATE TABLE IM_Usuario (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100) NOT NULL,
    correo NVARCHAR(100) NOT NULL,
    contrasena NVARCHAR(255) NOT NULL,
    rol NVARCHAR(50) NOT NULL,
    tipo_acceso NVARCHAR(50),
    descripcion NVARCHAR(500)
);
GO

CREATE TABLE IM_Clientes (
    id_cliente INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(150) NOT NULL,
    telefono NVARCHAR(20),
    correo NVARCHAR(100),
    direccion NVARCHAR(250)
);
GO

CREATE TABLE IM_Casos (
    id_caso INT IDENTITY(1,1) PRIMARY KEY,
    fecha_inicio DATE NOT NULL,
    fecha_vencimiento DATE,
    abogado_asignado INT NOT NULL,
    estado_caso NVARCHAR(50) NOT NULL,
    id_cliente INT NOT NULL,
    descripcion NVARCHAR(MAX),
    
    CONSTRAINT FK_Casos_Cliente FOREIGN KEY (id_cliente) 
        REFERENCES IM_Clientes(id_cliente),
    CONSTRAINT FK_Casos_Abogado FOREIGN KEY (abogado_asignado) 
        REFERENCES IM_Abogados(id_abogado)
);
GO


CREATE TABLE IM_Audiencia (
    id_audiencia INT IDENTITY(1,1) PRIMARY KEY,
    id_caso INT NOT NULL,
    fecha_inicio DATETIME NOT NULL,
    fecha_vencimiento DATETIME,
    descripcion NVARCHAR(MAX),
    
    CONSTRAINT FK_Audiencia_Caso FOREIGN KEY (id_caso) 
        REFERENCES IM_Casos(id_caso)
);
GO

CREATE TABLE IM_Reuniones (
    id_reunion INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente INT NOT NULL,
    fecha DATETIME NOT NULL,
    descripcion NVARCHAR(MAX),
    
    CONSTRAINT FK_Reuniones_Cliente FOREIGN KEY (id_cliente) 
        REFERENCES IM_Clientes(id_cliente)
);
GO


CREATE TABLE IM_Documentos (
    id_documento INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente INT,
    id_caso INT,
    id_abogado INT,
    nombre_documento NVARCHAR(250) NOT NULL,
    tipo_documento NVARCHAR(100), -- Demanda,Evidencias, Escrito, Sentencia, Contrato, etc.
    descripcion NVARCHAR(MAX),
    archivo VARBINARY(MAX), -- almacena documentos pero tiene que ser convertido los archivos en bytes.
    extension NVARCHAR(10),
    fecha_documento DATE,
    fecha_creacion DATETIME DEFAULT GETDATE(),
    fecha_modificacion DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT FK_Documentos_Cliente FOREIGN KEY (id_cliente) 
        REFERENCES IM_Clientes(id_cliente),
    CONSTRAINT FK_Documentos_Caso FOREIGN KEY (id_caso) 
        REFERENCES IM_Casos(id_caso),
    CONSTRAINT FK_Documentos_Abogado FOREIGN KEY (id_abogado) 
        REFERENCES IM_Abogados(id_abogado)
);
GO

CREATE TABLE IM_Estados_Caso (
    id_estado INT IDENTITY(1,1) PRIMARY KEY,
    nombre_estado NVARCHAR(50) NOT NULL UNIQUE,
    descripcion NVARCHAR(250),
    orden INT,
    estado BIT DEFAULT 1
);
GO

CREATE TABLE IM_Tipos_Caso (
    id_tipo INT IDENTITY(1,1) PRIMARY KEY,
    nombre_tipo NVARCHAR(100) NOT NULL UNIQUE,
    descripcion NVARCHAR(250),
    estado BIT DEFAULT 1
);
GO


-- Estados de Caso
INSERT INTO IM_Estados_Caso (nombre_estado, descripcion, orden) VALUES
('Nuevo', 'Caso recién ingresado', 1),
('En Proceso', 'Caso en trámite', 2),
('En Audiencia', 'Caso con audiencias programadas', 3),
('Suspendido', 'Caso temporalmente suspendido', 4),
('Finalizado', 'Caso concluido', 5),
('Archivado', 'Caso archivado', 6);
GO

-- Tipos de Caso
INSERT INTO IM_Tipos_Caso (nombre_tipo, descripcion) VALUES
('Civil', 'Casos de derecho civil'),
('Penal', 'Casos de derecho penal'),
('Laboral', 'Casos de derecho laboral'),
('Mercantil', 'Casos de derecho mercantil'),
('Familia', 'Casos de derecho de familia'),
('Administrativo', 'Casos de derecho administrativo'),
('Constitucional', 'Casos de derecho constitucional'),
('Tributario', 'Casos de derecho tributario');
GO


CREATE VIEW VW_Casos_Completo AS
SELECT 
    c.id_caso,
    c.estado_caso,
    c.fecha_inicio,
    c.fecha_vencimiento,
    cl.nombre AS nombre_cliente,
    cl.telefono AS telefono_cliente,
    cl.correo AS correo_cliente,
    a.nombre AS nombre_abogado,
    a.telefono AS telefono_abogado,
    c.descripcion
FROM IM_Casos c
INNER JOIN IM_Clientes cl ON c.id_cliente = cl.id_cliente
INNER JOIN IM_Abogados a ON c.abogado_asignado = a.id_abogado;
GO


CREATE VIEW VW_Proximas_Audiencias AS
SELECT 
    au.id_audiencia,
    au.fecha_inicio,
    cl.nombre AS nombre_cliente,
    a.nombre AS nombre_abogado,
    au.descripcion
FROM IM_Audiencia au
INNER JOIN IM_Casos c ON au.id_caso = c.id_caso
INNER JOIN IM_Clientes cl ON c.id_cliente = cl.id_cliente
INNER JOIN IM_Abogados a ON c.abogado_asignado = a.id_abogado
WHERE au.fecha_inicio >= GETDATE();
GO

CREATE VIEW VW_Carga_Abogados AS
SELECT 
    a.id_abogado,
    a.nombre AS nombre_abogado,
    COUNT(c.id_caso) AS casos_activos
FROM IM_Abogados a
LEFT JOIN IM_Casos c ON a.id_abogado = c.abogado_asignado 
    AND c.estado_caso IN ('Nuevo', 'En Proceso', 'En Audiencia')
GROUP BY a.id_abogado, a.nombre;
GO


CREATE PROCEDURE SP_CrearCaso
    @id_cliente INT,
    @id_abogado INT,
    @descripcion NVARCHAR(MAX),
    @fecha_inicio DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO IM_Casos (
        id_cliente, 
        abogado_asignado, 
        fecha_inicio, 
        estado_caso, 
        descripcion
    )
    VALUES (
        @id_cliente, 
        @id_abogado, 
        @fecha_inicio, 
        'Nuevo', 
        @descripcion
    );
    
    SELECT SCOPE_IDENTITY() AS id_caso;
END;
GO