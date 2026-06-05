Create database PrimerAnio;
Go

use PrimerAnio;
Go

CREATE TABLE Estados
(
    Codigo VARCHAR(4) PRIMARY KEY
        CHECK (
            LEN(Codigo) = 4
            AND Codigo NOT LIKE '%[^A-Za-z]%'
        ),
    Nombre VARCHAR(100) NOT NULL,
    Pais VARCHAR(100) NOT NULL
);
GO

Create table Hospedajes (
    CodigoInterno Varchar(10) Primary key,
    Nombre Varchar(100) NOT NULL,
    Direccion Varchar(100) NOT NULL,
    TipoHospedaje Varchar(20) NOT NULL 
        check (TipoHospedaje IN ('Hotel STD', 'Posada', 'All Inclusive')),
    Precio Int NOT NULL check (Precio > 0),
    Estado Varchar(4) NOT NULL 
        FOREIGN KEY REFERENCES Estados(Codigo)
);
Go


Create table Servicios (
    NroOmnibus Int not null CHECK(NroOmnibus >= 1 AND NroOmnibus <= 100),
    FechaHoraP Datetime NOT NULL,
    FechaHoraLL Datetime NOT NULL,
	Precio Int NOT NULL check (Precio > 0),
	Primary key (nroOmnibus, FechaHoraP)
);
Go

CREATE TABLE Paquetes (
    CodigoPaquete INT IDENTITY(1,1) PRIMARY KEY,
    Titulo VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(300) NOT NULL,
    
    EstadoDestino VARCHAR(4) NOT NULL 
        FOREIGN KEY REFERENCES Estados(Codigo),

    CantidadDias INT NOT NULL,
    
    PrecioIndividual DECIMAL(10,2) NOT NULL,
    PrecioBaseDoble DECIMAL(10,2) NOT NULL,
    PrecioBaseTriple DECIMAL(10,2) NOT NULL,

    ServicioCodigo Int NOT NULL ,

	FechaPartida DATETIME not null,

	FOREIGN KEY (ServicioCodigo, FechaPartida)
	REFERENCES Servicios(NroOmnibus, FechaHoraP),

    HospedajeCodigo VARCHAR(10) NOT NULL 
        FOREIGN KEY REFERENCES Hospedajes(CodigoInterno),

    NochesHospedaje INT NOT NULL CHECK (NochesHospedaje > 0)
);
GO
--- Procedimiento almacenado (ESTADOS)---

CREATE PROCEDURE AltaEstado
    @Codigo VARCHAR(4),
    @Nombre VARCHAR(100),
    @Pais   VARCHAR(100)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Estados WHERE Codigo = @Codigo)
        RETURN -1;  

    BEGIN TRY
        INSERT INTO Estados (Codigo, Nombre, Pais)
        VALUES (@Codigo, @Nombre, @Pais);

        RETURN 1;    
    END TRY
    BEGIN CATCH
        RETURN -2;  
    END CATCH
END;
GO

CREATE PROCEDURE BajaEstado
    @Codigo VARCHAR(4)
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM Estados WHERE Codigo = @Codigo)
            RETURN -1;

 
        IF EXISTS (SELECT 1 FROM Paquetes WHERE EstadoDestino = @Codigo)
            RETURN -2;


        DELETE FROM Estados
        WHERE Codigo = @Codigo;

        RETURN 1;  
    END TRY
    BEGIN CATCH
        RETURN -3;
    END CATCH
END
GO

CREATE PROCEDURE ModificarEstado
    @Codigo VARCHAR(4),
    @Nombre VARCHAR(100),
    @Pais   VARCHAR(100)
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Estados WHERE Codigo = @Codigo)
            RETURN -1;

        UPDATE Estados
        SET Nombre = @Nombre,
            Pais   = @Pais
        WHERE Codigo = @Codigo;

        RETURN 1;  
    END TRY
    BEGIN CATCH
        RETURN -2; 
    END CATCH
END
GO

CREATE PROCEDURE BuscarEstados
AS
BEGIN
    SELECT Codigo, Nombre, Pais
    FROM Estados;
END
GO


--- Procedimiento almacenado (Servicios)---

CREATE PROCEDURE AltaServicio
    @NroOmnibus INT,
    @FechaHoraP DATETIME,
    @FechaHoraLL DATETIME,
    @Precio INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Servicios WHERE NroOmnibus = @NroOmnibus and FechaHoraP = @FechaHoraP)
        RETURN -1;  

    BEGIN TRY

        INSERT INTO Servicios (NroOmnibus, FechaHoraP, FechaHoraLL, Precio)
        VALUES (@NroOmnibus, @FechaHoraP, @FechaHoraLL, @Precio);

        RETURN 1;   
    END TRY
    BEGIN CATCH
        RETURN -2;  
    END CATCH
END;
GO

CREATE PROCEDURE BajaServicio
    @NroOmnibus INT,
	@FechaHoraP DATETIME

AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Servicios WHERE NroOmnibus = @NroOmnibus and FechaHoraP = @FechaHoraP)
            RETURN -1;  

        IF EXISTS (SELECT 1 FROM Paquetes WHERE ServicioCodigo = @NroOmnibus and FechaPartida = @FechaHoraP)
            RETURN -2;  

        DELETE FROM Servicios
        WHERE NroOmnibus = @NroOmnibus and FechaHoraP = @FechaHoraP;

        RETURN 1;
    END TRY
    BEGIN CATCH
        RETURN -3; 
    END CATCH
END
GO

CREATE PROCEDURE ModificarServicio
    @NroOmnibus INT,
    @FechaHoraP DATETIME,
    @FechaHoraLL DATETIME,
    @Precio INT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Servicios WHERE NroOmnibus = @NroOmnibus and FechaHoraP = @FechaHoraP)
            RETURN -1;

        UPDATE Servicios
        SET FechaHoraLL = @FechaHoraLL,
            Precio = @Precio
        WHERE NroOmnibus = @NroOmnibus and FechaHoraP = @FechaHoraP;

        RETURN 1; 
    END TRY
    BEGIN CATCH
        RETURN -2;
    END CATCH
END
GO

CREATE PROCEDURE ListarServiciosVigentes
AS
BEGIN
    SELECT 
        NroOmnibus,
        FechaHoraP,
        FechaHoraLL,
        Precio
    FROM Servicios
    WHERE FechaHoraP >= GETDATE()
    ORDER BY FechaHoraP;
END
GO

CREATE PROCEDURE ObtenerServiciosPorEstado
    @CodigoEstado VARCHAR(4)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Estados WHERE Codigo = @CodigoEstado)
        RETURN -1;

    SELECT DISTINCT
        ServicioCodigo,
        FechaPartida
    FROM Paquetes
    WHERE EstadoDestino = @CodigoEstado
    ORDER BY FechaPartida;

    RETURN 1;
END
GO

CREATE PROCEDURE BuscarServicioPorNro
    @NroOmnibus INT,
    @FechaHoraP DATETIME
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Servicios WHERE NroOmnibus = @NroOmnibus AND FechaHoraP = @FechaHoraP)
        RETURN -1;

    SELECT 
        NroOmnibus,
        FechaHoraP,
        FechaHoraLL,
        Precio
    FROM Servicios
    WHERE NroOmnibus = @NroOmnibus
      AND FechaHoraP = @FechaHoraP;

    RETURN 1;
END
GO

--- Procedimiento almacenado (Hospedajes)---

CREATE PROCEDURE AltaHospedaje
    @CodigoInterno VARCHAR(10),
    @Nombre VARCHAR(100),
    @Direccion VARCHAR(100),
    @TipoHospedaje VARCHAR(20),
    @Precio INT,
    @Estado VARCHAR(50)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Hospedajes WHERE CodigoInterno = @CodigoInterno)
        RETURN -1;

    BEGIN TRY
        INSERT INTO Hospedajes (CodigoInterno, Nombre, Direccion, TipoHospedaje, Precio, Estado)
        VALUES (@CodigoInterno, @Nombre, @Direccion, @TipoHospedaje, @Precio, @Estado);

        RETURN 1; 
    END TRY
    BEGIN CATCH
        RETURN -2; 
    END CATCH
END;
GO

CREATE PROCEDURE BajaHospedaje
    @CodigoInterno VARCHAR(10)
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Hospedajes WHERE CodigoInterno = @CodigoInterno)
            RETURN -1;

        IF EXISTS (SELECT 1 FROM Paquetes WHERE HospedajeCodigo = @CodigoInterno)
            RETURN -2;  

        DELETE FROM Hospedajes
        WHERE CodigoInterno = @CodigoInterno;

        RETURN 1; 
    END TRY
    BEGIN CATCH
        RETURN -3; 
    END CATCH
END;
GO

CREATE PROCEDURE ModificarHospedaje
    @CodigoInterno VARCHAR(10),
    @Nombre VARCHAR(100),
    @Direccion VARCHAR(100),
    @TipoHospedaje VARCHAR(20),
    @Precio INT,
    @Estado VARCHAR(50)
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Hospedajes WHERE CodigoInterno = @CodigoInterno)
            RETURN -1;

        UPDATE Hospedajes
        SET Nombre = @Nombre,
            Direccion = @Direccion,
            TipoHospedaje = @TipoHospedaje,
            Precio = @Precio,
            Estado = @Estado
        WHERE CodigoInterno = @CodigoInterno;

        RETURN 1;
    END TRY
    BEGIN CATCH
        RETURN -2; 
    END CATCH
END;
GO

CREATE PROCEDURE BuscarHospedajePorCodigo
    @CodigoInterno VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CodigoInterno,
        Nombre,
        Direccion,
        TipoHospedaje,
        Precio,
        Estado
    FROM Hospedajes
    WHERE CodigoInterno = @CodigoInterno;
END
GO

CREATE PROCEDURE ObtenerHospedajesPorEstado
    @CodigoEstado VARCHAR(4)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CodigoInterno,
        Nombre,
        Direccion,
        TipoHospedaje,
        Precio,
        Estado
    FROM Hospedajes
    WHERE Estado = @CodigoEstado;
END
GO

CREATE PROCEDURE ListarHospedajes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CodigoInterno,
        TipoHospedaje,
        Nombre,
        Direccion,
        Precio,
        Estado
    FROM Hospedajes
    ORDER BY CodigoInterno;
END
GO

--- Procedimiento almacenado (Paquetes)---
CREATE PROCEDURE AltaPaquete
    @Titulo VARCHAR(100),
    @Descripcion VARCHAR(300),
    @EstadoDestino VARCHAR(4),
    @CantidadDias INT, 
    @PrecioIndividual DECIMAL(10,2), 
    @PrecioBaseDoble DECIMAL(10,2),
    @PrecioBaseTriple DECIMAL(10,2),
    @ServicioCodigo INT,
    @FechaPartida DATETIME,
    @HospedajeCodigo VARCHAR(10),
    @NochesHospedaje INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF LTRIM(RTRIM(@Titulo)) = '' OR @Titulo IS NULL
            RETURN -10;

        IF LTRIM(RTRIM(@Descripcion)) = '' OR @Descripcion IS NULL
            RETURN -11;

        IF @NochesHospedaje IS NULL OR @NochesHospedaje <= 0
            RETURN -12;

        IF @ServicioCodigo IS NULL OR @ServicioCodigo <= 0
            RETURN -13;

        IF NOT EXISTS (SELECT 1 FROM Estados WHERE Codigo = @EstadoDestino)
            RETURN -1;

        IF NOT EXISTS (
            SELECT 1
            FROM Servicios
            WHERE NroOmnibus = @ServicioCodigo
              AND FechaHoraP = @FechaPartida
        )
            RETURN -2;

        IF NOT EXISTS (SELECT 1 FROM Hospedajes WHERE CodigoInterno = @HospedajeCodigo)
            RETURN -3;

        IF NOT EXISTS (
            SELECT 1
            FROM Hospedajes
            WHERE CodigoInterno = @HospedajeCodigo
              AND Estado = @EstadoDestino
        )
            RETURN -5;

        DECLARE @FechaLlegada DATETIME;

        SELECT @FechaLlegada = FechaHoraLL
        FROM Servicios
        WHERE NroOmnibus = @ServicioCodigo
          AND FechaHoraP = @FechaPartida;

        IF @FechaLlegada IS NULL
            RETURN -2; 

        DECLARE @CantidadDiasCalc INT;
        SET @CantidadDiasCalc = DATEDIFF(DAY, @FechaPartida, @FechaLlegada);

        IF @CantidadDiasCalc <= 0
            RETURN -6;

        IF @NochesHospedaje >= @CantidadDiasCalc
            RETURN -7;

        DECLARE @PrecioServicio INT;
        DECLARE @PrecioHospedaje INT;

        SELECT @PrecioServicio = Precio
        FROM Servicios
        WHERE NroOmnibus = @ServicioCodigo
          AND FechaHoraP = @FechaPartida;

        SELECT @PrecioHospedaje = Precio
        FROM Hospedajes
        WHERE CodigoInterno = @HospedajeCodigo;

        IF @PrecioServicio IS NULL OR @PrecioHospedaje IS NULL
            RETURN -8;

        DECLARE @PrecioBase DECIMAL(10,2);
        SET @PrecioBase = CAST(@PrecioServicio AS DECIMAL(10,2))
                        + (CAST(@NochesHospedaje AS DECIMAL(10,2)) * CAST(@PrecioHospedaje AS DECIMAL(10,2)));

        DECLARE @PrecioIndividualCalc DECIMAL(10,2);
        DECLARE @PrecioBaseDobleCalc  DECIMAL(10,2);
        DECLARE @PrecioBaseTripleCalc DECIMAL(10,2);

        SET @PrecioIndividualCalc = @PrecioBase * 1.35;

        SET @PrecioBaseDobleCalc  = (@PrecioBase * 2) * 1.10;
        SET @PrecioBaseTripleCalc = (@PrecioBase * 3) * 1.10;

        INSERT INTO Paquetes
        (Titulo, Descripcion, EstadoDestino, CantidadDias,
         PrecioIndividual, PrecioBaseDoble, PrecioBaseTriple,
         ServicioCodigo, FechaPartida,
         HospedajeCodigo, NochesHospedaje)
        VALUES
        (@Titulo, @Descripcion, @EstadoDestino, @CantidadDiasCalc,
         @PrecioIndividualCalc, @PrecioBaseDobleCalc, @PrecioBaseTripleCalc,
         @ServicioCodigo, @FechaPartida,
         @HospedajeCodigo, @NochesHospedaje);

        RETURN 1;
    END TRY
    BEGIN CATCH
        RETURN -4;
    END CATCH
END;
GO

CREATE PROCEDURE ListarPaquetes
AS
BEGIN
    SELECT 
        CodigoPaquete,
        Titulo,
        Descripcion,
        EstadoDestino,
        CantidadDias,
        PrecioIndividual,
        PrecioBaseDoble,
        PrecioBaseTriple,
        ServicioCodigo,
        FechaPartida,     
        HospedajeCodigo,
        NochesHospedaje
    FROM Paquetes
    ORDER BY CodigoPaquete;
END
GO

CREATE PROCEDURE ListarPaquetesPorEstado
    @CodigoEstado VARCHAR(4)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CodigoPaquete,
        Titulo,
        Descripcion,
        EstadoDestino,
        CantidadDias,
        PrecioIndividual,
        PrecioBaseDoble,
        PrecioBaseTriple,
        ServicioCodigo,
        FechaPartida,
        HospedajeCodigo,
        NochesHospedaje
    FROM Paquetes
    WHERE EstadoDestino = @CodigoEstado
    ORDER BY CodigoPaquete;
END
GO

CREATE PROCEDURE ListarPaquetesPorServicio
    @ServicioCodigo INT,
    @FechaPartida DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CodigoPaquete,
        Titulo,
        Descripcion,
        EstadoDestino,
        CantidadDias,
        PrecioIndividual,
        PrecioBaseDoble,
        PrecioBaseTriple,
        ServicioCodigo,
        FechaPartida,
        HospedajeCodigo,
        NochesHospedaje
    FROM Paquetes
    WHERE ServicioCodigo = @ServicioCodigo
      AND FechaPartida = @FechaPartida
    ORDER BY CodigoPaquete;
END
GO

CREATE PROCEDURE BuscarPaquetes
    @CodigoPaquete INT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Paquetes WHERE CodigoPaquete = @CodigoPaquete)
            RETURN -1;

        SELECT 
            CodigoPaquete,
            Titulo,
            Descripcion,
            EstadoDestino,
            CantidadDias,
            PrecioIndividual,
            PrecioBaseDoble,
            PrecioBaseTriple,
            ServicioCodigo,
            FechaPartida,      
            HospedajeCodigo,
            NochesHospedaje
        FROM Paquetes
        WHERE CodigoPaquete = @CodigoPaquete;

        RETURN 1;
    END TRY
    BEGIN CATCH
        RETURN -2;
    END CATCH
END;
GO

INSERT INTO Estados (Codigo, Nombre, Pais)
SELECT v.Codigo, v.Nombre, v.Pais
FROM (VALUES
('MTVD','Montevideo','Uruguay'),
('MLDN','Maldonado','Uruguay'),
('COLN','Colonia','Uruguay'),
('BAIR','Buenos Aires','Argentina'),
('CRDB','Córdoba','Argentina'),
('MNDZ','Mendoza','Argentina'),
('STGO','Santiago','Chile'),
('VPRZ','Valparaíso','Chile'),
('POAL','Porto Alegre','Brasil'),
('ASUN','Asunción','Paraguay')
) v(Codigo,Nombre,Pais)
WHERE NOT EXISTS (SELECT 1 FROM Estados e WHERE e.Codigo = v.Codigo);
GO

INSERT INTO Servicios (NroOmnibus, FechaHoraP, FechaHoraLL, Precio)
SELECT v.NroOmnibus, v.FechaHoraP, v.FechaHoraLL, v.Precio
FROM (VALUES
(1,  CAST('20260210 08:00:00' AS datetime), CAST('20260212 14:30:00' AS datetime), 4500),
(2,  CAST('20260212 07:15:00' AS datetime), CAST('20260215 13:20:00' AS datetime), 5200),
(3,  CAST('20260301 09:00:00' AS datetime), CAST('20260304 16:10:00' AS datetime), 6100),
(4,  CAST('20260315 06:30:00' AS datetime), CAST('20260318 12:00:00' AS datetime), 4900),
(5,  CAST('20260405 10:00:00' AS datetime), CAST('20260410 18:30:00' AS datetime), 7300),
(6,  CAST('20260520 05:45:00' AS datetime), CAST('20260523 11:55:00' AS datetime), 5600),
(7,  CAST('20260601 13:00:00' AS datetime), CAST('20260605 20:10:00' AS datetime), 6800),
(8,  CAST('20260618 04:20:00' AS datetime), CAST('20260620 09:50:00' AS datetime), 4300),
(9,  CAST('20260703 08:40:00' AS datetime), CAST('20260707 15:15:00' AS datetime), 5900),
(10, CAST('20260722 12:10:00' AS datetime), CAST('20260726 19:05:00' AS datetime), 7100),
(11, CAST('20260810 06:00:00' AS datetime), CAST('20260813 12:40:00' AS datetime), 5100),
(12, CAST('20260901 09:25:00' AS datetime), CAST('20260906 17:30:00' AS datetime), 7600)
) v(NroOmnibus, FechaHoraP, FechaHoraLL, Precio)
WHERE NOT EXISTS (SELECT 1 FROM Servicios s WHERE s.NroOmnibus = v.NroOmnibus  AND s.FechaHoraP = v.FechaHoraP
);
GO

INSERT INTO Hospedajes (CodigoInterno, Nombre, Direccion, TipoHospedaje, Precio, Estado) VALUES
('HOTMON','Hotel Centro','18 de Julio 1234','Hotel STD',3200,'MTVD'),
('POSBRA','Posada Brava','Ruta 10 Km 142','Posada',4100,'MLDN'),
('AIBRAV','All Inclusive Brava','Av. Playa 55','All Inclusive',8900,'MLDN'),
('HOTBSU','Hotel Barrio Sur','San Salvador 850','Hotel STD',3600,'MTVD'),
('POSCOL','Posada Colonial','Av. Gral Flores 22','Posada',2900,'COLN'),
('HOTMIC','Hotel Microcentro','Florida 222','Hotel STD',3500,'BAIR'),
('POSSER','Posada Serrana','Camino Alto 88','Posada',2800,'CRDB'),
('AIAND','All Inclusive Andes','Cordillera 900','All Inclusive',9200,'STGO'),
('HOTPLZ','Hotel Plaza BA','Plaza Mayor 1','Hotel STD',3700,'BAIR'),
('AIPAC','AI Pacífico','Costanera 1010','All Inclusive',9500,'VPRZ'),
('HOTMEN','Hotel Mendoza Centro','San Martín 440','Hotel STD',3400,'MNDZ'),
('POSGUA','Posada Guaraní','Centro 77','Posada',2600,'ASUN'),
('HOTGUA','Hotel Guaíba','Av. Ipiranga 500','Hotel STD',3800,'POAL'),
('AILAG','AI Laguna','Playa Laguna 300','All Inclusive',9900,'MLDN'),
('POSRAM','Posada Rambla','Rambla 777','Posada',3100,'MTVD');
GO

INSERT INTO Paquetes
(Titulo, Descripcion, EstadoDestino, CantidadDias,
 PrecioIndividual, PrecioBaseDoble, PrecioBaseTriple,
 ServicioCodigo, FechaPartida, HospedajeCodigo, NochesHospedaje)
VALUES
('Escapada Montevideo','City tour + costa','MTVD',3,12000,21000,29000,1, CAST('20260210 08:00:00' AS datetime),'HOTMON',2),
('Montevideo Cultural','Museos y barrios','MTVD',2,9000,16000,22000,2, CAST('20260212 07:15:00' AS datetime),'HOTBSU',1),
('Punta del Este Relax','Playa y noche','MLDN',4,18000,32000,45000,3, CAST('20260301 09:00:00' AS datetime),'POSBRA',3),
('All Inclusive Brava','Resort completo','MLDN',5,26000,48000,65000,4, CAST('20260315 06:30:00' AS datetime),'AIBRAV',4),
('Laguna Premium','Spa y relax','MLDN',6,31000,57000,78000,5, CAST('20260405 10:00:00' AS datetime),'AILAG',5),
('Colonia Histórica','Casco antiguo','COLN',2,11000,20000,27000,6, CAST('20260520 05:45:00' AS datetime),'POSCOL',1),
('Buenos Aires Clásico','Teatro y museos','BAIR',4,20000,36000,51000,7, CAST('20260601 13:00:00' AS datetime),'HOTMIC',3),
('BA Compras','Outlets y shoppings','BAIR',2,13000,24000,33000,8, CAST('20260618 04:20:00' AS datetime),'HOTPLZ',1),
('Córdoba Sierras','Naturaleza','CRDB',5,22000,40000,56000,9, CAST('20260703 08:40:00' AS datetime),'POSSER',4),
('Mendoza Vinos','Bodegas','MNDZ',4,24000,44000,61000,10, CAST('20260722 12:10:00' AS datetime),'HOTMEN',3),
('Santiago Premium','City y viñedos','STGO',4,25000,46000,64000,11, CAST('20260810 06:00:00' AS datetime),'AIAND',3),
('Valparaíso Costa','Puerto y cerros','VPRZ',3,19000,35000,48000,12, CAST('20260901 09:25:00' AS datetime),'AIPAC',2),
('Asunción Urbana','Circuito histórico','ASUN',3,16000,30000,41000,6, CAST('20260520 05:45:00' AS datetime),'POSGUA',2),
('Porto Alegre Weekend','Cultura y parques','POAL',3,17000,31000,43000,7, CAST('20260601 13:00:00' AS datetime),'HOTGUA',2),
('Montevideo Rambla','Paseo costero','MTVD',2,10000,18000,25000,9, CAST('20260703 08:40:00' AS datetime),'POSRAM',1);
GO

