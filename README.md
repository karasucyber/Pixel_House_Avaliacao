
```markdown
# Pixel House Avaliação

Bem-vindo ao projeto Pixel House Avaliação! Este projeto foi desenvolvido com .NET 7 e está disponível para você baixar e executar.
 Abaixo você encontrará instruções sobre como configurar o ambiente de desenvolvimento tanto no Linux quanto no Windows.

## Requisitos

- [.NET SDK 7.0](https://dotnet.microsoft.com/download/dotnet/7.0)
- Editor de código ou IDE (Visual Studio, Visual Studio Code, ou outro de sua preferência).

## Instalação

### No Windows

1. **Instalar o .NET SDK:**
   - Acesse [a página de download do .NET 7.0](https://dotnet.microsoft.com/download/dotnet/7.0) e baixe o SDK.
   - Execute o instalador e siga as instruções na tela para concluir a instalação.

2. **Clonar o repositório:**
   - Abra o Prompt de Comando ou PowerShell e execute:
     ```bash
     git clone https://github.com/karasucyber/Pixel_House_Avaliacao.git
     ```

3. **Navegar até a pasta do projeto:**
   ```bash
   cd Pixel_House_Avaliacao/PixelHouse
   ```

4. **Restaurar pacotes e construir o projeto:**
   ```bash
   dotnet restore
   dotnet build
   ```

5. **Executar o projeto:**
   ```bash
   dotnet watch
   ```

### No Linux

1. **Instalar o .NET SDK:**
   - Siga as instruções na [documentação oficial do .NET para Linux](https://docs.microsoft.com/en-us/dotnet/core/install/linux) para instalar a versão 7.0.
   - Exemplo para Ubuntu:
     ```bash
     wget https://download.visualstudio.microsoft.com/download/pr/xxxxxx/xxxxxx/dotnet-sdk-7.0.x-linux-x64.tar.gz
     sudo mkdir -p /usr/share/dotnet
     sudo tar -zxf dotnet-sdk-7.0.x-linux-x64.tar.gz -C /usr/share/dotnet
     ```

2. **Clonar o repositório:**
   - Abra o terminal e execute:
     ```bash
     git clone https://github.com/karasucyber/Pixel_House_Avaliacao.git
     ```

3. **Navegar até a pasta do projeto:**
   ```bash
   Pixel_House_Avaliacao/PixelHouse
   ```

4. **Restaurar pacotes e construir o projeto:**
   ```bash
   dotnet restore
   dotnet build
   ```

5. **Executar o projeto:**
   ```bash
   dotnet watch
   ```

## Dependências

Este projeto utiliza as seguintes bibliotecas e pacotes:

- **AutoMapper**: 13.0.1
- **EPPlus**: 7.3.1
- **Moq**: 4.20.71
- **OxyPlot.Core**: 2.2.0
- **OxyPlot.SkiaSharp**: 2.2.0
- **QuestPDF**: 2024.7.3
- **Serilog**: 4.0.1
- **Serilog.Sinks.Console**: 6.0.0
- **System.Drawing.Common**: 8.0.8
- **xunit**: 2.9.0
- **xunit.runner.visualstudio**: 2.8.2
- **Microsoft.EntityFrameworkCore**: 7.0.0
- **Microsoft.EntityFrameworkCore.Sqlite**: 7.0.0
- **Microsoft.EntityFrameworkCore.Design**: 7.0.0

```

Você pode copiar e colar este conteúdo diretamente no seu arquivo `README.md` no GitHub. Ajuste o link de download do SDK do .NET para refletir a versão correta e adicione qualquer outra informação relevante.
