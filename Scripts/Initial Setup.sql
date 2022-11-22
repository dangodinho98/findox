-- Database: FindoxDb

DROP DATABASE IF EXISTS "FindoxDb";

CREATE DATABASE "FindoxDb"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United States.1252'
    LC_CTYPE = 'English_United States.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;
	
CREATE TABLE accounts (
	user_id serial PRIMARY KEY,
	username VARCHAR ( 50 ) UNIQUE NOT NULL,
	password VARCHAR ( 100 ) NOT NULL,
	email VARCHAR ( 255 ),
	created_on TIMESTAMP NOT NULL,
    last_login TIMESTAMP 
);

CREATE TABLE roles(
   role_id serial PRIMARY KEY,
   role_name VARCHAR (255) UNIQUE NOT NULL
);

CREATE TABLE account_roles (
  user_id INT NOT NULL,
  role_id INT NOT NULL,
  grant_date TIMESTAMP,
  PRIMARY KEY (user_id, role_id),
  FOREIGN KEY (role_id)
      REFERENCES roles (role_id),
  FOREIGN KEY (user_id)
      REFERENCES accounts (user_id)
);

INSERT INTO roles VALUES (1, 'Admin');
INSERT INTO roles VALUES (2, 'Manager');
INSERT INTO roles VALUES (3, 'Regular');
	
CREATE TABLE groups(
   group_id serial PRIMARY KEY,
   group_name VARCHAR (255) UNIQUE NOT NULL
);

CREATE TABLE account_groups (
  user_id INT NOT NULL,
  group_id INT NOT NULL,
  grant_date TIMESTAMP,
  PRIMARY KEY (user_id, group_id),
  FOREIGN KEY (group_id)
      REFERENCES groups (group_id),
  FOREIGN KEY (user_id)
      REFERENCES accounts (user_id)
);
	
CREATE OR REPLACE PROCEDURE create_user(
   _username varchar,
   _pwd varchar(100), 
   _email varchar,
   _role_names varchar[]
)
language plpgsql AS $$
DECLARE 
		_new_account_id int;
		_role_id int;
		_role_name varchar;
BEGIN
    INSERT INTO accounts (username, password, email, created_on)
    VALUES (_username, _pwd, _email, NOW())
    RETURNING "user_id" INTO _new_account_id;
   
    FOREACH _role_name IN ARRAY _role_names
	LOOP
			
    	SELECT role_id
    	INTO _role_id
    	FROM roles
    	WHERE roles.role_name = _role_name;

    	INSERT INTO account_roles(user_id, role_id, grant_date)
    	VALUES (_new_account_id, _role_id, NOW());
	END LOOP;

    COMMIT;
END
$$

CREATE OR REPLACE PROCEDURE update_user  
(  
    _user_id INT,  
    _username VARCHAR,  
    _pwd VARCHAR,  
    _email VARCHAR,
   _role_names varchar[]  
)  
LANGUAGE plpgsql AS  
$$  
BEGIN

	DELETE FROM account_roles where user_id = _user_id
   
   	UPDATE account SET   
   	username = _username,  
   	password = _pwd,  
   	email = _email
   	where user_id = _user_id;
	
	FOREACH _role_name IN ARRAY _role_names
	LOOP
			
    	SELECT role_id
    	INTO _role_id
    	FROM roles
    	WHERE roles.role_name = _role_name;

    	INSERT INTO account_roles(user_id, role_id, grant_date)
    	VALUES (_user_id, _role_id, NOW());
	END LOOP;

    COMMIT;
END  
$$;  

CREATE OR REPLACE PROCEDURE delete_user(
   _user_id int
)
language plpgsql AS $$
BEGIN
	DELETE FROM account_roles WHERE user_id = _user_id;
    DELETE FROM accounts WHERE user_id = _user_id;
   
    COMMIT;
END
$$

CREATE OR REPLACE FUNCTION public.get_all_user_with_roles()
returns table(UserId INT,
			  Username VARCHAR,
			  PasswordHash VARCHAR,
			  Email VARCHAR,
			  CreatedOn TIMESTAMP,
			  LastLogin TIMESTAMP,
			  RoleId INT,
			  RoleName varchar)
AS $$
BEGIN
 RETURN QUERY
 SELECT accounts.user_id UserId,
	   accounts.username Username,
	   accounts.password PasswordHash,
	   accounts.email Email,
	   accounts.created_on CreatedOn,
	   accounts.last_login LastLogin,
	   roles.role_id RoleId,
	   roles.role_name RoleName
FROM accounts
INNER JOIN account_roles USING (user_id)
INNER JOIN roles USING (role_id);

END;
$$ LANGUAGE plpgsql;