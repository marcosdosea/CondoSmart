# API de Consulta de CNPJ - Documentação

## Endpoints Disponíveis

### 1. Consultar CNPJ (GET)
**Endpoint:** `GET /api/cnpj/consultar/{cnpj}`

**Descrição:** Consulta os dados de um CNPJ através de uma API externa (ReceitaWS).

**Parâmetros:**
- `cnpj` (string, obrigatório): CNPJ com ou sem formatação (ex: "11222333000181" ou "11.222.333/0001-81")

**Exemplo de Requisição:**
```
GET /api/cnpj/consultar/11222333000181
```

**Resposta de Sucesso (200 OK):**
```json
{
    "cnpj": "11.222.333/0001-81",
    "nome": "EMPRESA LTDA",
    "rua": "Rua Exemplo",
    "numero": "123",
    "bairro": "Centro",
    "cidade": "São Paulo",
    "uf": "SP",
    "cep": "01310-100",
    "complemento": "Apto 10",
    "valorValido": true
}
```

**Respostas de Erro:**
- `400 Bad Request`: CNPJ não fornecido
- `404 Not Found`: CNPJ não encontrado ou inválido
- `500 Internal Server Error`: Erro ao consultar CNPJ

---

### 2. Consultar CNPJ (POST)
**Endpoint:** `POST /api/cnpj/consultar`

**Descrição:** Consulta os dados de um CNPJ através de uma API externa (ReceitaWS).

**Body da Requisição:**
```json
{
    "cnpj": "11222333000181"
}
```

**Exemplo de Requisição cURL:**
```bash
curl -X POST https://seu-dominio.com/api/cnpj/consultar \
  -H "Content-Type: application/json" \
  -d '{"cnpj":"11222333000181"}'
```

**Resposta de Sucesso (200 OK):**
```json
{
    "cnpj": "11.222.333/0001-81",
    "nome": "EMPRESA LTDA",
    "rua": "Rua Exemplo",
    "numero": "123",
    "bairro": "Centro",
    "cidade": "São Paulo",
    "uf": "SP",
    "cep": "01310-100",
    "complemento": "Apto 10",
    "valorValido": true
}
```

---

## Validações Implementadas no CondominioService

### 1. Nome
- ✅ Obrigatório
- ✅ Máximo 150 caracteres

### 2. CNPJ
- ✅ Obrigatório
- ✅ 14 dígitos numéricos (remove caracteres especiais automaticamente)
- ✅ Valida algoritmo oficial de check digit
- ✅ Rejeita sequências repetidas (ex: 00000000000000)
- ✅ Verifica unicidade no banco de dados

### 3. Rua
- ✅ Obrigatória
- ✅ Máximo 150 caracteres

### 4. Número
- ✅ Obrigatório
- ✅ Máximo 20 caracteres

### 5. Bairro
- ✅ Obrigatório
- ✅ Máximo 100 caracteres

### 6. Cidade
- ✅ Obrigatória
- ✅ Máximo 100 caracteres

### 7. UF (Estado)
- ✅ Obrigatório
- ✅ Exatamente 2 caracteres

### 8. CEP
- ✅ Obrigatório
- ✅ 8 dígitos numéricos (remove caracteres especiais automaticamente)

### 9. Email
- ✅ Validação de formato (quando preenchido)
- ✅ Opcional (se não preenchido, não valida)

### 10. Telefone
- ✅ Validação de quantidade de dígitos (10 a 11)
- ✅ Opcional (se não preenchido, não valida)

### 11. Unidades
- ✅ Obrigatório
- ✅ Deve ser maior que zero

---

## Exemplo de Uso em JavaScript

```javascript
// Consultar CNPJ via GET
async function consultarCnpj(cnpj) {
    try {
        const response = await fetch(`/api/cnpj/consultar/${cnpj}`);
        
        if (!response.ok) {
            throw new Error(`Erro: ${response.status}`);
        }
        
        const dados = await response.json();
        console.log('Dados do CNPJ:', dados);
        
        // Preencher formulário com os dados
        document.getElementById('nome').value = dados.nome;
        document.getElementById('rua').value = dados.rua;
        document.getElementById('numero').value = dados.numero;
        document.getElementById('bairro').value = dados.bairro;
        document.getElementById('cidade').value = dados.cidade;
        document.getElementById('uf').value = dados.uf;
        document.getElementById('cep').value = dados.cep;
        document.getElementById('complemento').value = dados.complemento || '';
        document.getElementById('cnpj').value = dados.cnpj;
        
    } catch (error) {
        console.error('Erro ao consultar CNPJ:', error);
        alert('CNPJ não encontrado ou inválido');
    }
}

// Consultar CNPJ via POST
async function consultarCnpjPost(cnpj) {
    try {
        const response = await fetch('/api/cnpj/consultar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ cnpj: cnpj })
        });
        
        if (!response.ok) {
            throw new Error(`Erro: ${response.status}`);
        }
        
        const dados = await response.json();
        console.log('Dados do CNPJ:', dados);
        
        // Preencher formulário com os dados
        preencherFormulario(dados);
        
    } catch (error) {
        console.error('Erro ao consultar CNPJ:', error);
        alert('CNPJ não encontrado ou inválido');
    }
}

// Usar em um botão de busca
document.getElementById('btnBuscarCnpj').addEventListener('click', function() {
    const cnpj = document.getElementById('cnpjInput').value;
    consultarCnpj(cnpj);
});
```

---

## Notas Importantes

1. **API Externa**: O serviço utiliza a API pública ReceitaWS para consultar dados de CNPJ. Esta API tem limitações de rate limiting, então use com cuidado em produção.

2. **Tratamento de Erros**: Se o CNPJ não existir na API externa, a resposta será 404. Trate este cenário na sua aplicação frontend.

3. **Segurança**: O endpoint `/api/cnpj/consultar` permite acesso anônimo (AllowAnonymous) para facilitar consultas durante o cadastro. Considere adicionar rate limiting em produção.

4. **Formatação**: Os dados retornados pela API já vêm formatados (CNPJ com pontos e barra, CEP com hífen). Você pode usar diretamente ou remover a formatação conforme necessário.
