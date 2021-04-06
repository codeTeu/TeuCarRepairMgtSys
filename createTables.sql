CREATE TABLE Vehicle(
vID int IDENTITY(1,1) ,
vmake varchar(30) ,
vmodel varchar(30) ,
vyear int ,
vcondition varchar(10) ,
CONSTRAINT "PK_vehicle" PRIMARY KEY  CLUSTERED ("vID")
);


CREATE TABLE Inventory(
iID int IDENTITY(1,1) ,
vID int ,
stock varchar(30) ,
price int ,
cost varchar(5) ,
CONSTRAINT "PK_inventory" PRIMARY KEY  CLUSTERED ("iID"),
CONSTRAINT "FK_Vehicle_Vehicle" FOREIGN KEY ("vID") REFERENCES "dbo"."Vehicle" ("vID")
);


CREATE TABLE Repair(
rID int IDENTITY(1,1) ,
iID int ,
whatToRepair varchar(30) ,
CONSTRAINT "PK_repair" PRIMARY KEY  CLUSTERED ("rID"),
CONSTRAINT "FK_Inventory_Inventory" FOREIGN KEY ("iID") REFERENCES "dbo"."Inventory" ("iID")
);
