# Update-Values Script Documentatie

## Auteur
| Versie | Auteur       | Datum      | Opmerking                   |
|--------|--------------|------------|-----------------------------|
| 1.0.0  | Osama Halabi | 2024-11-17 | Update Values  documenteren |

Dit document beschrijft het **update-values.ps1** script dat wordt gebruikt om de versietag van verschillende services bij te werken in Kubernetes `values.yaml` bestanden.

---

## Doel van het Script
Het script voert de volgende acties uit:
1. **Validatie van de inputparameters**: Controleert of de opgegeven omgeving en versie geldig zijn.
2. **Dynamisch bijwerken** van de `image.tag` waarde voor verschillende services in het juiste `values.yaml` bestand.
3. **Ondersteuning voor omgevingen**:
    - **dev**: Gebruikt `values.yaml`.
    - **prod**: Gebruikt `values-prod.yaml`.

---

## Parameters
Het script accepteert de volgende parameters:

| Parameter  | Type      | Verplicht | Beschrijving                           |
|------------|-----------|-----------|---------------------------------------|
| `-Version` | String    | Ja        | De versie die wordt toegepast.        |
| `-Env`     | String    | Ja        | Doelomgeving: `dev` of `prod`.        |

---

## Bestandspadvalidatie
Op basis van de `Env` parameter selecteert het script het correcte bestandspad:

```powershell
switch ($Env) {
    'dev'  { $ValuesFilePath = "../agrifirm/values.yaml" }
    'prod' { $ValuesFilePath = "../agrifirm/values-prod.yaml" }
    default {
        Write-Error "Error: Invalid value for Env. Please specify 'dev' or 'prod'."
        exit 1
    }
}
```

Als het bestand niet bestaat of de versieparameter niet is meegegeven, wordt het script afgebroken met een foutmelding.


## Werking van het Script
### Stap 1: Itereren over Services
Voor elke service wordt het volgende commando uitgevoerd:

```powershell
yq eval '.{0}.image.tag = {1}' -i "{2}" -f $service, $escapedVersion, $ValuesFilePath
```

- **`yq eval`**: Past de nieuwe versie toe op de juiste sleutel in het YAML-bestand.
- **`-i`**: Voert de wijzigingen inline uit.

De variabele `$escapedVersion` zorgt ervoor dat de versie correct wordt geformatteerd.

### Stap 2: Uitvoeren van yq-commando
Het commando wordt uitgevoerd met `Invoke-Expression`:

```powershell
try {
    Invoke-Expression $yqCommand
} catch {
    Write-Error "Failed to execute yq for service: $service. Error: $_"
}
```

Als een fout optreedt tijdens het bijwerken, wordt een foutmelding gegenereerd en het script vervolgt met de volgende service.

### Stap 3: Loggen van Acties
Na succesvolle uitvoering wordt een logbericht weergegeven:

```powershell
Write-Host "Updated $ValuesFilePath with new version: $Version for services: $($services -join ', ')"
```

---

## Voorbeeld van Uitvoering
Het volgende commando voert het script uit voor de `dev` omgeving met versie `1.2.3`:

```bash
PowerShell -File update-values.ps1 -Version "1.2.3" -Env "dev"
```

### Verwachte Uitvoer
```plaintext
Running: yq eval '.webapp.image.tag = "1.2.3"' -i "../agrifirm/values.yaml"
Running: yq eval '.basalration.image.tag = "1.2.3"' -i "../agrifirm/values.yaml"
Running: yq eval '.importration.image.tag = "1.2.3"' -i "../agrifirm/values.yaml"
Updated ../agrifirm/values.yaml with new version: 1.2.3 for services: webapp, basalration, importration
```

---

## Foutmeldingen
Het script genereert foutmeldingen in de volgende situaties:
1. **Ongeldige omgeving**:
   ```plaintext
   Error: Invalid value for Env. Please specify 'dev' or 'prod'.
   ```
2. **Ontbrekend bestand**:
   ```plaintext
   Error: File not found at ../agrifirm/values.yaml.
   ```
3. **Ontbrekende versieparameter**:
   ```plaintext
   Error: Version parameter is required.
   ```

---

## Samenvatting
Het **update-values.ps1** script automatiseert het bijwerken van versietags voor services in Kubernetes YAML-bestanden. Het biedt ondersteuning voor zowel **dev** als **prod** omgevingen en maakt gebruik van het hulpprogramma **yq** voor efficiënte wijzigingen.

---

## Auteurstabel
| Versie | Auteur      | Datum         | Opmerking                 |
|--------|-------------|---------------|---------------------------|
| 1.0.0  | Groep 5     | 2024-06-08    | Eerste versie van document|
