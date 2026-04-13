# Release Pipeline Documentatie

## Auteur
| Versie | Auteur       | Datum      | Opmerking                      |
|--------|--------------|------------|--------------------------------|
| 1.0.0  | Osama Halabi | 2024-11-17 | Release Pipeline  documenteren |

Deze documentatie beschrijft een **release pipeline** in Azure DevOps, ontworpen om de versie van de applicatie uit te lezen en de GitOps repository bij te werken voor productieomgevingen.

## Trigger en PR Instellingen

De pipeline wordt getriggerd bij commits op de `main` branch:

```yaml
trigger:
  branches:
    include:
      - main

pr:
  branches:
    include:
      - main
```

## Variabelen
### Globale variabelen

```yaml
variables:
  GIT_OPS_BRANCH: "main"  # Branch in de GitOps repository
```

### Omgevingsvariabelen

```yaml
variables:
   $(GITOPS_USERNAME): # GitOps repository gebruikersnaam
   $(GITOPS_PASS): # GitOps repository wachtwoord
   $(GITOPS_URL): # GitOps repository URL
```

## Stage: Deploy_Production
De pipeline bestaat uit een enkele stage, **Deploy_Production**, die de versie van de applicatie leest en de `values.yaml` file bijwerkt in de GitOps repository.

#### Stappen

1. **Check out de code**:
   De repository wordt schoongemaakt en de code wordt opgehaald:
   ```yaml
   - checkout: self
     clean: true
   ```

2. **Lees de VERSION bestand**:
   Haalt de applicatieversie op en stelt deze in als een pipelinevariabele `AppVersion`:
   ```yaml
   - task: PowerShell@2
     displayName: Extract VERSION File
     inputs:
       targetType: inline
       script: |
         Write-Host "Reading VERSION file..."
         $versionFile = "$(Build.SourcesDirectory)\VERSION"
         if (!(Test-Path $versionFile)) {
             Write-Error "Error: VERSION file not found in $versionFile."
             exit 1
         }
         $version = Get-Content $versionFile -Raw
         $version = $version.Trim()
         Write-Host "Extracted version: $version"
         Write-Host "##vso[task.setvariable variable=AppVersion]$version"
   ```

3. **Clone de GitOps repository**:
   Clone de repository en checkt de juiste branch uit:
   ```yaml
   - script: |
       echo "Cloning GitOps repository..."
       git config --global user.name "CI/CD Pipeline"
       git config --global user.email "ci-cd@teamluna.han.nl"
       git clone https://$(GITOPS_USERNAME):$(GITOPS_PASS)@$(GITOPS_URL)
       cd mcps-gitops
       git checkout $(GIT_OPS_BRANCH)
     displayName: Clone GitOps Repository
     env:
       GITOPS_USERNAME: $(GITOPS_USERNAME)
       GITOPS_PASS: $(GITOPS_PASS)
       GITOPS_URL: $(GITOPS_URL)
   ```

4. **Installeer yq**:
   Installeert het hulpprogramma `yq` dat nodig is voor het bijwerken van YAML-bestanden:
   ```yaml
   - task: PowerShell@2
     displayName: Install yq
     inputs:
       targetType: filePath
       filePath: mcps-gitops/scripts/install-yq.ps1
       workingDirectory: mcps-gitops/scripts
   ```

5. **Update values.yaml**:
   Werkt de `values.yaml` file bij met de nieuwe versie:
   ```yaml
   - task: PowerShell@2
     displayName: Update values.yaml with Application Version
     inputs:
       targetType: filePath
       filePath: mcps-gitops/scripts/update-values.ps1
       arguments: "-Version $(AppVersion) -Env prod"
       workingDirectory: mcps-gitops/scripts
   ```

6. **Commit en push wijzigingen**:
   Voegt de wijzigingen toe, commit en pusht ze naar de GitOps repository:
   ```yaml
   - script: |
       cd mcps-gitops
       git add .
       git commit -m "chore: update values-prod.yaml to version $(AppVersion)"
       git push origin $(GIT_OPS_BRANCH)
     displayName: Commit and Push Changes
   ```

---

## Samenvatting
Deze release pipeline voert de volgende acties uit:

1. Leest de applicatieversie uit de `VERSION` file.
2. Clone de GitOps repository en checkt de juiste branch uit.
3. Werkt `values.yaml` bij met de nieuwe versie.
4. Commit en pusht de wijzigingen naar de `main` branch van de GitOps repository.

Deze configuratie zorgt ervoor dat de productieomgeving automatisch wordt bijgewerkt met de nieuwste applicatieversie.
