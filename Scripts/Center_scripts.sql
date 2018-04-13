CREATE TABLE  COUNTRY (
    CODE int NOT NULL, 
	NAME_RUS VARCHAR(128) NOT NULL, 
	NAME_ENG VARCHAR(128) NOT NULL, 
	SHORT_NAME_RUS VARCHAR(8), 
	SHORT_NAME_ENG VARCHAR(8), 
	 CONSTRAINT "COUNTRY_PK" PRIMARY KEY ("CODE"),
	 CONSTRAINT "COUNTRY_UK1" UNIQUE ("NAME_RUS"),
	 CONSTRAINT "COUNTRY_UK2" UNIQUE ("NAME_ENG"),
	 CONSTRAINT "COUNTRY_UK3" UNIQUE ("SHORT_NAME_RUS"),
	 CONSTRAINT "COUNTRY_UK4" UNIQUE ("SHORT_NAME_ENG")
)

CREATE TABLE  "CITY" (
	CODE int NOT NULL, 
	NAME_RUS VARCHAR(64) NOT NULL, 
	NAME_ENG VARCHAR(64) NOT NULL, 
	COUNTRY_CODE int NOT NULL, 
	CONSTRAINT "CITY_PK" PRIMARY KEY ("CODE")
)

CREATE TABLE  PRACTICE(	
    ID int Identity(1, 1) NOT NULL, 
	NAME_RUS VARCHAR(256) NOT NULL, 
	NAME_ENG VARCHAR(256) NOT NULL, 
	CONSTRAINT "PRACTICE_PK" PRIMARY KEY ("ID"), 
	 CONSTRAINT "PRACTICE_UK1" UNIQUE ("NAME_RUS"), 
	 CONSTRAINT "PRACTICE_UK2" UNIQUE ("NAME_ENG")
)

CREATE TABLE PRACTICE_SCHEDULE (
	ID int Identity(1,1) NOT NULL, 
	ID_PRACTICE int NOT NULL, 
	CITY_CODE int NOT NULL, 
	LOCAL_ADDRESS VARCHAR(128), 
	CONTACT_PHONE VARCHAR(32), 
	CONTACT_E_MAIL VARCHAR(64), 
	DATE_FROM DATE, 
	DATE_TO DATE, 
	CONSTRAINT "PRACTICE_SCHEDULE_PK" PRIMARY KEY ("ID"), 
	 CONSTRAINT "DATE_CHECK" CHECK (Date_from <= Date_to)
)

CREATE TABLE  MIGRATION_CENTER (	
    ID int Identity(1,1) NOT NULL, 
	NAME_RUS VARCHAR(256) NOT NULL, 
	NAME_ENG VARCHAR(256) NOT NULL, 
	ID_CITY int NOT NULL, 
	LOCAL_ADDRESS VARCHAR(256), 
	CONTACT_E_MAIL VARCHAR(64), 
	CONTACT_PHONE VARCHAR(32), 
	WEB_SITE VARCHAR(128), 
	CONSTRAINT "MIGRATION_CENTER_PK" PRIMARY KEY ("ID")
)

ALTER TABLE  "CITY" ADD CONSTRAINT "CITY_FK" FOREIGN KEY ("COUNTRY_CODE")
	  REFERENCES  "COUNTRY" ("CODE");

ALTER TABLE  "MIGRATION_CENTER" ADD CONSTRAINT "MIGRATION_CENTER_FK" FOREIGN KEY ("ID_CITY")
	  REFERENCES  "CITY" ("CODE");

ALTER TABLE  "PRACTICE_SCHEDULE" ADD CONSTRAINT "PRACTICE_SCHEDULE_FK1" FOREIGN KEY ("ID_PRACTICE")
	  REFERENCES  "PRACTICE" ("ID");

ALTER TABLE  "PRACTICE_SCHEDULE" ADD CONSTRAINT "PRACTICE_SCHEDULE_FK2" FOREIGN KEY ("CITY_CODE")
	  REFERENCES  "CITY" ("CODE");
