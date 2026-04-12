# AutoVersion Script Documentatie

## Auteur
| Versie | Auteur       | Datum      | Opmerking                                 |
|--------|--------------|------------|-------------------------------------------|
| 1.0.0  | Osama Halabi | 2024-11-17 | Eerste versie van document                |
| 2.0.0  | Osama Halabi | 2024-12-17 | AutoVersion Script Documentatie verbeterd |

Dit document beschrijft het **AutoVersion** script dat wordt uitgevoerd in de AutoVersion Stage van de Azure Pipeline. Het script automatiseert het versiebeheer op basis van commitberichten en bijgewerkte PR's.

## Doel van het Script
Het script heeft als doel:
- **Versiebeheer**: Automatisch verhogen van de versie in het `VERSION` bestand en de Angular `package.json`.
- **Committen en Taggen**: Nieuwe versies worden gecommit en als Git-tags gepusht naar de repository.

De versie wordt verhoogd op basis van trefwoorden in commitberichten of PR-titels:
- `add`: Verhoogt het **minor**-nummer.
- `fix`: Verhoogt het **patch**-nummer.

---

## Voorwaarden voor Uitvoering
1. **Git geïnstalleerd**: Het script valideert dat Git beschikbaar is.
2. **Valid Branch**: Het script voert alleen uit op de `development` branch.
3. **VERSION Bestaat**: Als het `VERSION` bestand ontbreekt, wordt deze geïnitialiseerd met versie `1.0.0`.

---

## Werking van het Script

### Stap 1: Valideren van Git en de Repository
Het script valideert:
- Of **Git** is geïnstalleerd.
- Of het script binnen een geldige **Git repository** wordt uitgevoerd.

```powershell
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Error "Git is not installed or not available in PATH. Please install Git and try again."
    exit 1
}

if (-not (Test-Path ".git")) {
    Write-Error "The .git directory is not found in the current working directory."
    exit 1
}
```

---

### Stap 2: Valideren van de Branch
Het script voert alleen uit op de **development** branch.

```powershell
$currentBranch = git rev-parse --abbrev-ref HEAD 2>$null
if ($currentBranch -ne "development") {
    Write-Host "The script runs only on 'development' branch. Current branch: $currentBranch. Exiting."
    exit 0
}
```

---

### Stap 3: Lezen en Valideren van de Huidige Versie
Als het `VERSION` bestand niet bestaat, initialiseert het script de versie met **1.0.0**.

```powershell
if (Test-Path $versionFile) {
    $currentVersion = Get-Content $versionFile -Raw
} else {
    Write-Output "1.0.0" > $versionFile
    $currentVersion = "1.0.0"
}
```

Het valideert ook het formaat van de versie (**MAJOR.MINOR.PATCH**).

---

### Stap 4: Bepalen van Versie-Increment
Het script leest het laatste commitbericht of PR-titel:
- **`add`** in het bericht verhoogt het **minor**-nummer en reset het **patch**-nummer.
- **`fix`** verhoogt het **patch**-nummer.

```powershell
if ($commitMessage -match "\badd\b") {
    $minor++
    $patch = 0
} elseif ($commitMessage -match "\bfix\b") {
    $patch++
}
```

De nieuwe versie wordt geconstrueerd als `$major.$minor.$patch`.

---

### Stap 5: Updaten van VERSION en package.json
Het script:
1. Werkt het `VERSION` bestand bij.
2. Past de versie aan in de Angular `package.json`.

```powershell
Set-Content -Path $versionFile -Value $newVersion

$packageJson = Get-Content $packageJsonPath -Raw | ConvertFrom-Json
$packageJson.version = $newVersion
$formattedJson = $packageJson | ConvertTo-Json -Depth 10
Set-Content -Path $packageJsonPath -Value $formattedJson
```

---

### Stap 6: Committen, Taggen en Pushen
Het script commit en pusht de wijzigingen inclusief een Git-tag.

```powershell
git add $versionFile
git add $packageJsonPath
git commit -m "chore(version): Bumped version to $newVersion [auto-version] [ci:skip]"
git tag "v$newVersion"
git push origin HEAD:$currentBranch
```

---

## Samenvatting van Belangrijkste Acties
1. **Validatie**: Controleert Git-installatie en de huidige branch.
2. **Versieverhoging**: Verhoogt versie op basis van trefwoorden `add` en `fix`.
3. **Update Bestanden**: Past de nieuwe versie toe op `VERSION` en `package.json`.
4. **Git-Acties**: Commit de wijzigingen, tagt de nieuwe versie en pusht alles naar de repository.

## Uitvoervoorbeeld
Een commit met `fix` in het bericht:
- Huidige versie: `1.0.1`
- Nieuwe versie: `1.0.2`

Een PR met `add` in de titel:
- Huidige versie: `1.0.1`
- Nieuwe versie: `1.1.0`

De pipeline logt de acties en de nieuwe versie:
```plaintext
Updated version to 1.1.0 in VERSION
Updated Angular app version to 1.1.0 in package.json
Successfully committed and tagged the new version: v1.1.0
```
