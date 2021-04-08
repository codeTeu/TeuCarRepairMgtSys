CREATE TABLE Vehicle(
id int IDENTITY(1,1) ,
vmake varchar(30) ,
vmodel varchar(30) ,
vyear int ,
vcondition varchar(10) ,
CONSTRAINT "PK_vehicle" PRIMARY KEY  CLUSTERED ("id")
);


CREATE TABLE Inventory(
id int IDENTITY(1,1) ,
vID int ,
stock int ,
price float ,
cost float ,
CONSTRAINT "PK_inventory" PRIMARY KEY  CLUSTERED ("id"),
CONSTRAINT "FK_Vehicle_Vehicle" FOREIGN KEY ("vID") REFERENCES "dbo"."Vehicle" ("id") ON DELETE CASCADE
);


CREATE TABLE Repair(
id int IDENTITY(1,1) ,
iID int ,
whatToRepair varchar(30) ,
CONSTRAINT "PK_repair" PRIMARY KEY  CLUSTERED ("iD"),
CONSTRAINT "FK_Inventory_Inventory" FOREIGN KEY ("iID") REFERENCES "dbo"."Inventory" ("id") ON DELETE CASCADE
);


INSERT INTO vehicle(vmake,vmodel,vyear,vcondition) VALUES('FORD', 'ESCAPE', 2000, 'USED');
INSERT INTO vehicle(vmake,vmodel,vyear,vcondition) VALUES('HONDA', 'CIVIC', 2021, 'NEW');
INSERT INTO vehicle(vmake,vmodel,vyear,vcondition) VALUES('NISSAN', 'ARMAAD', 1995, 'USED');
INSERT INTO vehicle(vmake,vmodel,vyear,vcondition) VALUES('HONDA', 'CR-V', 1993, 'USED');
INSERT INTO vehicle(vmake,vmodel,vyear,vcondition) VALUES('TOYOTA', 'CAMMRY', 2012, 'NEW');

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


