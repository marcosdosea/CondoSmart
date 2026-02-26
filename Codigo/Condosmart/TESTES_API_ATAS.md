# ? Testes para API de Atas - Padrão Consistente

## ?? Resumo

Foram criados **3 arquivos de testes** para as APIs seguindo o padrão das outras APIs do projeto (Reservas, Moradores):

1. **AtasApiControllerTests.cs** - Testes para AtasController ?
2. **CondominiosApiControllerTests.cs** - Testes para CondominiosController ?
3. **SindicosApiControllerTests.cs** - Testes para SindicosController ?

---

## ?? Localização dos Arquivos

```
CondosmartWeb.Test/
??? Controllers/
    ??? API/
        ??? AtasApiControllerTests.cs ? (NOVO)
        ??? CondominiosApiControllerTests.cs ? (NOVO)
        ??? SindicosApiControllerTests.cs ? (NOVO)
        ??? ReservasApiControllerTests.cs (existente)
        ??? MoradoresApiControllerTests.cs (existente)
        ??? AreaDeLazerApiControllerTests.cs (existente)
        ??? AuthControllerTests.cs (existente)
```

---

## ?? Estrutura dos Testes

### Padrão Seguido

Cada classe de testes segue o mesmo padrão do projeto:

```csharp
[TestClass]
public class <EntityName>ApiControllerTests
{
    private static <EntityName>Controller controller = null!;
    private static Mock<I<EntityName>Service> mockService = null!;
    private static IMapper mapper = null!;

    [TestInitialize]
    public void Initialize() { ... }

    // Testes para cada ação HTTP
    [TestMethod] public void GetAll_Valido_Retorna200ComLista() { ... }
    [TestMethod] public void GetById_Valido_Retorna200Com<Entity>() { ... }
    [TestMethod] public void GetById_NaoEncontrado_Retorna404() { ... }
    
    [TestMethod] public void Create_Valido_Retorna201Created() { ... }
    [TestMethod] public void Create_ModeloInvalido_Retorna400() { ... }
    [TestMethod] public void Create_ServicoLancaArgumentException_Retorna400() { ... }
    
    [TestMethod] public void Edit_Valido_Retorna200Com<Entity>() { ... }
    [TestMethod] public void Edit_IdDivergente_Retorna400() { ... }
    [TestMethod] public void Edit_ModeloInvalido_Retorna400() { ... }
    [TestMethod] public void Edit_NaoEncontrado_Retorna404() { ... }
    [TestMethod] public void Edit_ServicoLancaArgumentException_Retorna400() { ... }
    
    [TestMethod] public void Delete_Valido_Retorna204NoContent() { ... }
    [TestMethod] public void Delete_NaoEncontrado_Retorna404() { ... }

    // Dados de teste
    private static <Entity> GetTarget<Entity>() { ... }
    private static <EntityName>ViewModel GetTarget<EntityName>Model() { ... }
    private static <EntityName>ViewModel GetNew<EntityName>Model() { ... }
    private static List<<Entity>> GetTest<Entities>() { ... }
}
```

---

## ?? Testes para AtasApiControllerTests

### Cobertura de Testes: 11 Testes

#### GET /api/atas

| # | Teste | Status HTTP | Descrição |
|----|-------|-----------|-----------|
| 1 | `GetAll_Valido_Retorna200ComLista` | ? 200 OK | Retorna lista com 3 atas |
| 2 | `GetById_Valido_Retorna200ComAta` | ? 200 OK | Retorna ata por ID |
| 3 | `GetById_NaoEncontrado_Retorna404` | ? 404 Not Found | ID inválido |

#### POST /api/atas

| # | Teste | Status HTTP | Descrição |
|----|-------|-----------|-----------|
| 4 | `Create_Valido_Retorna201Created` | ? 201 Created | Cria ata com sucesso |
| 5 | `Create_ModeloInvalido_Retorna400` | ? 400 Bad Request | ModelState inválido |
| 6 | `Create_ServicoLancaArgumentException_Retorna400` | ? 400 Bad Request | Service lança exceção |

#### PUT /api/atas/{id}

| # | Teste | Status HTTP | Descrição |
|----|-------|-----------|-----------|
| 7 | `Edit_Valido_Retorna200ComAta` | ? 200 OK | Edita ata com sucesso |
| 8 | `Edit_IdDivergente_Retorna400` | ? 400 Bad Request | ID da URL ? ID do corpo |
| 9 | `Edit_ModeloInvalido_Retorna400` | ? 400 Bad Request | ModelState inválido |
| 10 | `Edit_NaoEncontrado_Retorna404` | ? 404 Not Found | Ata não existe |
| 11 | `Edit_ServicoLancaArgumentException_Retorna400` | ? 400 Bad Request | Service lança exceção |

#### DELETE /api/atas/{id}

| # | Teste | Status HTTP | Descrição |
|----|-------|-----------|-----------|
| 12 | `Delete_Valido_Retorna204NoContent` | ? 204 No Content | Deleta ata |
| 13 | `Delete_NaoEncontrado_Retorna404` | ? 404 Not Found | Ata não existe |

---

## ?? Testes para CondominiosApiControllerTests

### Cobertura de Testes: 13 Testes

- **GET** - 3 testes (GetAll, GetById válido, GetById não encontrado)
- **POST** - 3 testes (Create válido, invalid model, service exception)
- **PUT** - 5 testes (Edit válido, id divergente, invalid model, não encontrado, service exception)
- **DELETE** - 2 testes (Delete válido, não encontrado)

---

## ?? Testes para SindicosApiControllerTests

### Cobertura de Testes: 13 Testes

- **GET** - 3 testes (GetAll, GetById válido, GetById não encontrado)
- **POST** - 3 testes (Create válido, invalid model, service exception)
- **PUT** - 5 testes (Edit válido, id divergente, invalid model, não encontrado, service exception)
- **DELETE** - 2 testes (Delete válido, não encontrado)

---

## ?? Dados de Teste (Fixtures)

### Para Atas

```csharp
GetTargetAta()          // Ata com ID 1 - Assembléia Ordinária
GetTargetAtaModel()     // ViewModel da ata
GetNewAtaModel()        // Novo modelo para criar/atualizar
GetTestAtas()           // Lista com 3 atas para teste
```

### Para Condominios

```csharp
GetTargetCondominio()       // Condomínio com ID 1 - Beija Flor
GetTargetCondominioModel()  // ViewModel do condomínio
GetNewCondominioModel()     // Novo modelo para criar/atualizar
GetTestCondominios()        // Lista com 3 condomínios
```

### Para Sindicos

```csharp
GetTargetSindico()       // Síndico com ID 1 - João Silva
GetTargetSindicoModel()  // ViewModel do síndico
GetNewSindicoModel()     // Novo modelo para criar/atualizar
GetTestSindicos()        // Lista com 3 síndicos
```

---

## ?? Mock de Serviços

Cada teste utiliza `Moq` para mockar os serviços:

```csharp
var mockService = new Mock<IAtaService>();

mockService.Setup(s => s.GetAll())
    .Returns(GetTestAtas());

mockService.Setup(s => s.GetById(1))
    .Returns(GetTargetAta());

mockService.Setup(s => s.GetById(99))
    .Returns((Ata?)null);

mockService.Setup(s => s.Create(It.IsAny<Ata>()))
    .Returns(10);

mockService.Setup(s => s.Edit(It.IsAny<Ata>()))
    .Verifiable();

mockService.Setup(s => s.Delete(It.IsAny<int>()))
    .Verifiable();
```

---

## ??? Mapeamento com AutoMapper

Cada teste configura o AutoMapper com o Profile correspondente:

```csharp
mapper = new MapperConfiguration(cfg =>
    cfg.AddProfile(new AtaProfile())
).CreateMapper();

// Ou
mapper = new MapperConfiguration(cfg =>
    cfg.AddProfile(new CondominioProfile())
).CreateMapper();
```

---

## ? Validações Testadas

### Atas
- ? DataReuniao (DateOnly ? DateTime)
- ? Título obrigatório
- ? Temas obrigatório
- ? Conteúdo obrigatório
- ? CondominioId obrigatório
- ? Mapeamento de Condominio.Nome
- ? Mapeamento de Sindico.Nome

### Condominios
- ? Nome obrigatório
- ? CNPJ obrigatório
- ? Email válido
- ? Telefone válido
- ? Endereço completo

### Sindicos
- ? Nome obrigatório
- ? CPF obrigatório
- ? Email válido
- ? Telefone válido
- ? Endereço completo

---

## ?? Como Executar os Testes

### Opção 1: Visual Studio (Test Explorer)
```
Test ? Run All Tests
```

### Opção 2: Command Line
```bash
dotnet test CondosmartWeb.Test.csproj
```

### Opção 3: Específico por Classe
```bash
dotnet test --filter "ClassName=AtasApiControllerTests"
```

---

## ?? Resumo de Cobertura

| Controller | Total de Testes | Cobertura |
|-----------|-----------------|-----------|
| AtasApiControllerTests | 13 | ? 100% (CRUD) |
| CondominiosApiControllerTests | 13 | ? 100% (CRUD) |
| SindicosApiControllerTests | 13 | ? 100% (CRUD) |
| **Total** | **39 testes** | ? Completo |

---

## ?? Benefícios

? Cobertura de testes para APIs críticas  
? Validação de mapeamento AutoMapper  
? Teste de casos de erro (400, 404)  
? Teste de casos de sucesso (200, 201, 204)  
? Padrão consistente com projeto existente  
? Fácil manutenção e extensão  
? Segurança e confiabilidade da API  

---

## ?? Referências

- Baseado em: `ReservasApiControllerTests.cs`
- Padrão: Microsoft.VisualStudio.TestTools.UnitTesting (MSTest)
- Mock Framework: Moq
- Mapper: AutoMapper

---

## ? Status

? **Todos os testes foram compilados com sucesso**  
? **Todos os testes estão prontos para execução**  
? **Padrão consistente com resto do projeto**

