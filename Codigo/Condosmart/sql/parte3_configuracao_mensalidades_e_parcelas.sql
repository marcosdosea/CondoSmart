CREATE TABLE IF NOT EXISTS `configuracao_mensalidades` (
    `id` INT NOT NULL AUTO_INCREMENT,
    `condominio_id` INT NOT NULL,
    `valor_mensalidade` DECIMAL(10,2) NOT NULL,
    `dia_vencimento` INT NOT NULL,
    `quantidade_parcelas_padrao` INT NOT NULL DEFAULT 12,
    `ativa` BIT NOT NULL DEFAULT b'1',
    `created_at` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    UNIQUE KEY `ux_config_mensalidade_condominio` (`condominio_id`),
    CONSTRAINT `fk_config_mensalidade_condominio`
        FOREIGN KEY (`condominio_id`) REFERENCES `condominios` (`id`)
        ON DELETE CASCADE
);

ALTER TABLE `mensalidades`
    ADD COLUMN `valor_original` DECIMAL(10,2) NOT NULL DEFAULT 0.00 AFTER `valor`,
    ADD COLUMN `valor_final` DECIMAL(10,2) NOT NULL DEFAULT 0.00 AFTER `valor_original`,
    ADD COLUMN `data_pagamento` DATE NULL AFTER `created_at`;

UPDATE `mensalidades`
SET `valor_original` = `valor`,
    `valor_final` = `valor`
WHERE `valor_original` = 0
  AND `valor_final` = 0;

UPDATE `mensalidades`
SET `data_pagamento` = DATE(`created_at`)
WHERE `status` = 'pago'
  AND `data_pagamento` IS NULL
  AND `created_at` IS NOT NULL;

UPDATE `mensalidades`
SET `status` = 'atrasado'
WHERE `status` = 'vencida';

ALTER TABLE `mensalidades`
    MODIFY COLUMN `status` ENUM('pendente','pago','atrasado','cancelada') NOT NULL DEFAULT 'pendente';
