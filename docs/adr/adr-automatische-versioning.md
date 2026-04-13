## ADR: Automatische Versioning Methode met Moonlit Auto-Versioning

| **Name**            | Moonlit Auto-Versioning                                                   |
|---------------------|----------------------------------------------------------------------------|
| **Current version** | 1                                                                          |
| **Current status**  | ✅ **DECIDED**                                                             |
| **Problem/Issue**   | Het handmatig beheren van softwareversies leidt vaak tot fouten en inconsistenties. Automatisering is nodig om dit proces te stroomlijnen en een consistent versiebeheer te waarborgen. |
| **Decision**        | We hebben gekozen voor de Moonlit Auto-Versioning methode om automatische versiebeheer te implementeren op basis van semantische versiebeheerprincipes (MAJOR.MINOR.PATCH). |
| **Alternatives**    | 1. **Semver met Semantic Release**: Gestandaardiseerde versiebeheer met changelogs en CI-integratie, maar vereist extra tooling en configuratie. <br> 2. **Git Tags en Handmatige Incrementen**: Simpel en direct, maar foutgevoelig en inefficiënt voor grotere teams. <br> 3. **Custom CI/CD Pipelines**: Flexibel en schaalbaar, maar vereist aanzienlijke ontwikkeltijd en onderhoud. |
| **Arguments**       | 1. **Automatische versie-incrementen**: Moonlit Auto-Versioning verhoogt versienummers op basis van trefwoorden zoals "add" en "fix" in commitberichten en pull requests.<br> 2. **Efficiëntie**: Minimaliseert handmatige taken en voorkomt menselijke fouten. <br> 3. **Synchronisatie**: Beheert consistentie tussen bestanden zoals `VERSION` en `package.json`. <br> 4. **Integratie**: Eenvoudig te integreren met bestaande Git-workflows en CI/CD-tools. <br> 5. **Configuratie**: Beperkte leercurve en snel inzetbaar binnen het team. |

---