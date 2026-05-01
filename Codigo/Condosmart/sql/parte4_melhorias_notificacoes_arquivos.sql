START TRANSACTION;

ALTER TABLE `atas` ADD `arquivo_caminho` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL;

ALTER TABLE `atas` ADD `arquivo_nome_original` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL;

ALTER TABLE `area_de_lazer` ADD `imagem_caminho` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL;

ALTER TABLE `area_de_lazer` ADD `imagem_nome_original` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL;

CREATE TABLE `notificacoes_sistema` (
    `id` int NOT NULL AUTO_INCREMENT,
    `condominio_id` int NULL,
    `usuario_email` varchar(120) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `usuario_nome` varchar(120) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `titulo` varchar(120) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `mensagem` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `tipo` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `url_destino` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
    `created_at` datetime NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PRIMARY` PRIMARY KEY (`id`),
    CONSTRAINT `fk_notificacao_condominio` FOREIGN KEY (`condominio_id`) REFERENCES `condominios` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX `ix_notificacoes_condominio` ON `notificacoes_sistema` (`condominio_id`);

CREATE INDEX `ix_notificacoes_created_at` ON `notificacoes_sistema` (`created_at`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260501034106_AddNotificacoesEAquivos', '8.0.23');

COMMIT;

