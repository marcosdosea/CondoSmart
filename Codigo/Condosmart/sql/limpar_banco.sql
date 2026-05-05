use condosmart;
SET FOREIGN_KEY_CHECKS = 0;

-- Tabelas do Identity (Usuários, Roles, etc)
TRUNCATE TABLE AspNetRoleClaims;
TRUNCATE TABLE AspNetUserClaims;
TRUNCATE TABLE AspNetUserLogins;
TRUNCATE TABLE AspNetUserRoles;
TRUNCATE TABLE AspNetUserTokens;
TRUNCATE TABLE AspNetUsers;
TRUNCATE TABLE AspNetRoles;

-- Tabelas de Transação e Histórico
TRUNCATE TABLE mensalidades;
TRUNCATE TABLE pagamentos;
TRUNCATE TABLE chamados;
TRUNCATE TABLE reservas;
TRUNCATE TABLE visitantes;
TRUNCATE TABLE atas;
TRUNCATE TABLE notificacoes_sistema;

-- Tabelas de Entidades Básicas
TRUNCATE TABLE area_de_lazer;
TRUNCATE TABLE porteiros;
TRUNCATE TABLE unidades_residenciais;
TRUNCATE TABLE moradores;
TRUNCATE TABLE configuracao_mensalidades;
TRUNCATE TABLE condominios;
TRUNCATE TABLE sindicos;

SET FOREIGN_KEY_CHECKS = 1;
