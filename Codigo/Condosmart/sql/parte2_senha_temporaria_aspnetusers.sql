ALTER TABLE AspNetUsers
    ADD COLUMN SenhaTemporaria TINYINT(1) NOT NULL DEFAULT 1;

UPDATE AspNetUsers
SET SenhaTemporaria = 0;
