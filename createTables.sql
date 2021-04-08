CREATE TABLE Vehicle(
vID int,
vmake varchar(30) ,
vmodel varchar(30) ,
vyear int ,
vcondition varchar(10) ,
CONSTRAINT "PK_vehicle" PRIMARY KEY  ("vID")
);


CREATE TABLE Inventory(
iID int,
vID int ,
stock int ,
price float ,
cost float ,
CONSTRAINT "PK_inventory" PRIMARY KEY  ("iID"),
CONSTRAINT "FK_Vehicle_Vehicle" FOREIGN KEY ("vID") REFERENCES "dbo"."Vehicle" ("vID") ON DELETE CASCADE
);


CREATE TABLE Repair(
rID int,
iID int ,
whatToRepair varchar(30) ,
CONSTRAINT "PK_repair" PRIMARY KEY  ("rID"),
CONSTRAINT "FK_Inventory_Inventory" FOREIGN KEY ("iID") REFERENCES "dbo"."Inventory" ("iID") ON DELETE CASCADE
);


INSERT INTO vehicle(vID,vmake,vmodel,vyear,vcondition) VALUES(1, 'FORD', 'ESCAPE', 2000, 'USED');
INSERT INTO vehicle(vID,vmake,vmodel,vyear,vcondition) VALUES(2, 'HONDA', 'CIVIC', 2021, 'NEW');
INSERT INTO vehicle(vID,vmake,vmodel,vyear,vcondition) VALUES(3, 'NISSAN', 'ARMAAD', 1995, 'USED');
INSERT INTO vehicle(vID,vmake,vmodel,vyear,vcondition) VALUES(4, 'HONDA', 'CR-V', 1993, 'USED');
INSERT INTO vehicle(vID,vmake,vmodel,vyear,vcondition) VALUES(5, 'TOYOTA', 'CAMMRY', 2012, 'NEW');

INSERT INTO Inventory(iID, vID,stock,price,cost) VALUES(1, 1, 1, 1, 1);
INSERT INTO Inventory(iID, vID,stock,price,cost) VALUES(2, 2, 2, 2, 2);
INSERT INTO Inventory(iID, vID,stock,price,cost) VALUES(3, 3, 3, 3, 3);
INSERT INTO Inventory(iID, vID,stock,price,cost) VALUES(4, 4, 4, 4, 4);
INSERT INTO Inventory(iID, vID,stock,price,cost) VALUES(5, 4, 5, 5, 5);

INSERT INTO Repair(rID, iID, whatToRepair) VALUES(1, 1, 'TIRE');
INSERT INTO Repair(rID, iID, whatToRepair) VALUES(2, 2, 'SIDE MIRROR');
INSERT INTO Repair(rID, iID, whatToRepair) VALUES(3, 3, 'DOOR HANDLE');
INSERT INTO Repair(rID, iID, whatToRepair) VALUES(4, 4, 'ENGINE');
INSERT INTO Repair(rID, iID, whatToRepair) VALUES(5, 5, 'BACKLIGHT');


