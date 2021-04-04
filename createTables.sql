CREATE TABLE Vehicle(
vID int IDENTITY(1,1) NOT NULL,
vmake varchar(30) NOT NULL,
vmodel varchar(30) NOT NULL,
vyear int NOT NULL,
vcondition varchar(10) NOT NULL,
CONSTRAINT "PK_vehicle" PRIMARY KEY  CLUSTERED ("vID")
);


CREATE TABLE Inventory(
iID int IDENTITY(1,1) NOT NULL,
vID int NOT NULL,
vmodel varchar(30) NOT NULL,
vyear int NOT NULL,
vcondition varchar(5) NOT NULL,
CONSTRAINT "PK_inventory" PRIMARY KEY  CLUSTERED ("iID"),
CONSTRAINT "FK_Vehicle_Vehicle" FOREIGN KEY ("vID") REFERENCES "dbo"."Vehicle" ("vID")
);


CREATE TABLE Repair(
rID int IDENTITY(1,1) NOT NULL,
iID int NOT NULL,
whatToRepair varchar(30) NOT NULL,
CONSTRAINT "PK_repair" PRIMARY KEY  CLUSTERED ("rID"),
CONSTRAINT "FK_Inventory_Inventory" FOREIGN KEY ("iID") REFERENCES "dbo"."Inventory" ("iID")
);
