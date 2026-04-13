# Deployment en Build pipeline Documentatie

## Auteur
| Versie | Auteur       | Datum      | Opmerking             |
|--------|--------------|------------|-----------------------|
| 1.0.0  | Osama Halabi | 2024-11-17 | Pipeline documenteren |

Deze documentatie beschrijft een **Azure Pipeline** met meerdere stages. Elke stage wordt uitgevoerd op basis van het resultaat van de vorige stage. De pipeline bouwt, test, voert auto-versioning uit, bouwt Docker-images en werkt een GitOps repository bij.

## Trigger en PR Instellingen

De pipeline wordt getriggerd bij commits op de `development` branch, behalve bij berichten die `ci:skip` bevatten:

```yaml
trigger:
  branches:
    include:
      - development
    exclude:
      - 'ci:skip'

pr:
  branches:
    include:
      - development
```

## Variabelen
Globale variabelen:

```yaml
variables:
  solution: 'src/groep5-rantsoenwijzer.sln'  # Pad naar .NET oplossing
  angularAppPath: 'src/WebApp'              # Pad naar Angular applicatie
  GIT_OPS_BRANCH: "main"                  # GitOps branch naam
```

## Stages
De pipeline bestaat uit **vier stages** die sequentieel worden uitgevoerd, afhankelijk van het resultaat van de voorgaande stage.

### 1. BuildAndTest Stage
**Doel**: Bouwt en test de .NET en Angular applicatie.

#### Voorwaarden
De stage wordt alleen uitgevoerd als:
- Het geen `main` of `development` branch betreft.
- De commit geen `[ci:skip]` of `doc` bevat.

#### Stappen
1. **.NET build en test**
2. **Angular dependencies installeren**
3. **Angular applicatie bouwen**

```yaml
- task: UseDotNet@2
  inputs:
    packageType: 'sdk'
    version: '8.x'

- script: dotnet restore $(solution)
  displayName: Restore .NET Dependencies

- script: dotnet build --no-restore $(solution)
  displayName: Build .NET Solution

- script: dotnet test --no-build --verbosity normal $(solution)
  displayName: Run .NET Tests

- task: NodeTool@0
  inputs:
    versionSpec: '18.x'
  displayName: Install Node.js

- script: |
    cd $(angularAppPath)
    npm install
  displayName: Restore Angular Dependencies

- script: |
    cd $(angularAppPath)
    npm run build
  displayName: Build Angular Application
```

---

### 2. AutoVersion Stage

**Doel**: Voert automatische versiebeheer uit.

#### Afhankelijk van
Deze stage wordt alleen uitgevoerd als de **BuildAndTest** stage succesvol is.

#### Stappen
1. Voert een PowerShell-script uit voor versiebeheer. Wat Auto-Versioning Script doet is beschreven in [auto-versioning-script](./auto-versioning-script-doc.md)
2. Haalt de versie uit de `VERSION` file en stelt deze in als variabele `AppVersion`.

```yaml
- task: PowerShell@2
  displayName: Run Auto-Versioning Script
  inputs:
    targetType: filePath
    filePath: $(Build.SourcesDirectory)/src/AutoVersion.ps1

- task: PowerShell@2
  displayName: Extract and Set Application Version
  inputs:
    targetType: inline
    script: |
      $version = Get-Content "$(Build.SourcesDirectory)/src/VERSION"
      Write-Host "##vso[task.setvariable variable=AppVersion]$version"
```

---

### 3. BuildAndPush Stage
**Doel**: Bouwt Docker images en pusht ze naar een azure container registry.

#### Afhankelijk van
Deze stage wordt alleen uitgevoerd als de **AutoVersion** stage succesvol is en op basis van de commit message van de [auto-versioning-script](./auto-versioning-script-doc.md).

#### Stappen
1. Logt in bij de Azure container registry.
2. Voert een PowerShell-script uit om Docker-images te bouwen en pushen.

```yaml
- task: Docker@2
  inputs:
    command: login
    containerRegistry: minorRegistry

- task: PowerShell@2
  displayName: Rebuild and Push Docker Images
  inputs:
    targetType: filePath
    filePath: $(Build.SourcesDirectory)/src/RebuildAndPush.ps1
```

---

### 4. Deploy Stage
**Doel**: Update de GitOps repository met de nieuwe versie.

#### Afhankelijk van

Deze stage wordt alleen uitgevoerd als de **BuildAndPush** stage succesvol is.

#### Stappen
1. Lees de versie uit de `VERSION` file.
2. Clone de GitOps repository.
3. Update de `values.yaml` met de nieuwe versie.
4. Commit en push de wijzigingen.

```yaml
- task: PowerShell@2
  displayName: Extract VERSION File
  inputs:
    targetType: inline
    script: |
      Write-Host "Reading VERSION file..."
      $versionFile = "$(Build.SourcesDirectory)\VERSION"
      $version = Get-Content $versionFile -Raw
      Write-Host "##vso[task.setvariable variable=AppVersion]$version"

- script: |
    git clone https://$(GITOPS_USERNAME):$(GITOPS_PASS)@$(GITOPS_URL)
    cd mcps-gitops
    git checkout $(GIT_OPS_BRANCH)
  displayName: Clone GitOps Repository

- task: PowerShell@2
  filePath: mcps-gitops/scripts/update-values.ps1
  arguments: "-Version $(AppVersion) -Env dev"

- script: |
    cd mcps-gitops
    git add .
    git commit -m "chore: update values.yaml to version $(AppVersion) [ci:skip]"
    git push origin $(GIT_OPS_BRANCH)
  displayName: Commit and Push Changes
```

---

## Samenvatting
Deze Azure Pipeline voert de volgende acties uit:

1. **BuildAndTest**: Bouwt en test de applicatie.
2. **AutoVersion**: Voert automatisch versiebeheer uit.
3. **BuildAndPush**: Bouwt Docker images en pusht ze naar een container registry.
4. **Deploy**: Update de GitOps repository met de nieuwste versie.

Elke stage wordt uitgevoerd op basis van het resultaat van de voorgaande stage, wat zorgt voor een sequentiële en betrouwbare CI/CD-workflow.
