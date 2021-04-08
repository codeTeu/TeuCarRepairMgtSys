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
stock int ,
price float ,
cost float ,
CONSTRAINT "PK_inventory" PRIMARY KEY  CLUSTERED ("iID"),
CONSTRAINT "FK_Vehicle_Vehicle" FOREIGN KEY ("vID") REFERENCES "dbo"."Vehicle" ("vID") ON DELETE CASCADE
);


CREATE TABLE Repair(
rID int IDENTITY(1,1) ,
iID int ,
whatToRepair varchar(30) ,
CONSTRAINT "PK_repair" PRIMARY KEY  CLUSTERED ("rID"),
CONSTRAINT "FK_Inventory_Inventory" FOREIGN KEY ("iID") REFERENCES "dbo"."Inventory" ("iID") ON DELETE CASCADE
);


INSERT INTO vehicle(vmake,vmodel,vyear,vcondition) VALUES('FORD', 'ESCAPE', 2000, 'used');
INSERT INTO vehicle(vmake,vmodel,vyear,vcondition) VALUES('HONDA', 'CIVIC', 2021, 'new');
INSERT INTO vehicle(vmake,vmodel,vyear,vcondition) VALUES('NISSAN', 'ARMAAD', 1995, 'used');
INSERT INTO vehicle(vmake,vmodel,vyear,vcondition) VALUES('HONDA', 'CR-V', 1993, 'used');
INSERT INTO vehicle(vmake,vmodel,vyear,vcondition) VALUES('TOYOTA', 'CAMMMRY', 2012, 'new');

INSERT INTO Inventory(vID,stock,price,cost) VALUES(1, 1, 1, 1);
INSERT INTO Inventory(vID,stock,price,cost) VALUES(2, 2, 2, 2);
INSERT INTO Inventory(vID,stock,price,cost) VALUES(3, 3, 3, 3);
INSERT INTO Inventory(vID,stock,price,cost) VALUES(4, 4, 4, 4);
INSERT INTO Inventory(vID,stock,price,cost) VALUES(5, 5, 5, 5);

INSERT INTO Repair(iID, whatToRepair) VALUES(1, 'TIRE');
INSERT INTO Repair(iID, whatToRepair) VALUES(2, 'SIDE MIRROR');
INSERT INTO Repair(iID, whatToRepair) VALUES(3, 'DOOR HANDLE');
INSERT INTO Repair(iID, whatToRepair) VALUES(4, 'ENGINE');
INSERT INTO Repair(iID, whatToRepair) VALUES(5, 'BACKLIGHT');


