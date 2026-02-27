// Script para gerar e testar CNPJs válidos
// Cole este código no console do navegador (F12 > Console)

function calcularDigitosVerificadores(cnpj12digits) {
    // Primeira sequência multiplicadora
    let multiplicador1 = [5,4,3,2,9,8,7,6,5,4,3,2];
    let soma1 = 0;
    
    for (let i = 0; i < 12; i++) {
        soma1 += parseInt(cnpj12digits[i]) * multiplicador1[i];
    }
    
    let resto1 = soma1 % 11;
    let digito1 = resto1 < 2 ? 0 : 11 - resto1;
    
    // Segunda sequência multiplicadora
    let cnpj13 = cnpj12digits + digito1;
    let multiplicador2 = [6,5,4,3,2,9,8,7,6,5,4,3,2];
    let soma2 = 0;
    
    for (let i = 0; i < 13; i++) {
        soma2 += parseInt(cnpj13[i]) * multiplicador2[i];
    }
    
    let resto2 = soma2 % 11;
    let digito2 = resto2 < 2 ? 0 : 11 - resto2;
    
    return cnpj13 + digito2;
}

// Gerar alguns CNPJs válidos
console.log('=== CNPJs Válidos Gerados ===');
let cnjpBaseArray = [
    '112223330001',
    '114447770001',
    '118888880001',
    '119999990001',
    '112121210001',
    '115353530001'
];

cnjpBaseArray.forEach(base => {
    let cnpjCompleto = calcularDigitosVerificadores(base);
    console.log(cnpjCompleto + ' -> ' + 
        cnpjCompleto.substring(0,2) + '.' + 
        cnpjCompleto.substring(2,5) + '.' + 
        cnpjCompleto.substring(5,8) + '/' + 
        cnpjCompleto.substring(8,12) + '-' + 
        cnpjCompleto.substring(12,14));
});
