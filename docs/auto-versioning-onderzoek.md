# Onderzoek: Automatische Versioning van een Applicatie

## Inleiding

Het beheren van softwareversies is cruciaal voor elke ontwikkelcyclus. Automatische versioning biedt een oplossing voor de uitdagingen die handmatige versiebeheer met zich meebrengt, zoals fouten en inconsistenties. Dit onderzoek richt zich op automatische versioning, onderzoekt alternatieve methodes en belicht de voor- en nadelen van elke methode. Tot slot wordt een conclusie getrokken op basis van de bevindingen (Fowler, 2019).

---

## Automatische Versioning: Moonlit Auto-Versioning

De Moonlit Auto-Versioning-methode automatiseert het proces van versiebeheer op basis van commitberichten en pull request-titels. Het verhoogt versienummers volgens een vastgelegde semantische structuur (MAJOR.MINOR.PATCH) afhankelijk van specifieke keywords zoals "add" en "fix" (Semantic Versioning, n.d.).

### Voordelen
- **Consistentie**: Automatische updates volgen vooraf gedefinieerde regels.
- **Efficiëntie**: Minder tijdrovend dan handmatig beheer.
- **Integratie**: Synchroniseert versies tussen bestanden zoals `VERSION` en `package.json`.

### Nadelen
- **Complexiteit**: Vereist een nauwkeurig geschreven script.
- **Beperkingen**: Afhankelijk van een specifieke workflow (bijvoorbeeld keywords in commits).

---

## Alternatieve Methodes

### 1. **Semver met Semantic Release**
Semantic Release gebruikt een gestandaardiseerde set tools om versienummers te genereren op basis van semantische versiebeheerregels (Semantic Versioning, n.d.).

#### Voordelen
- **Changelogs**: Automatisch gegenereerde changelogs.
- **Breed ondersteund**: Ondersteunt CI/CD-tools zoals Jenkins en GitHub Actions (Jenkins, n.d.; GitHub Actions, n.d.).
- **Configuratie-opties**: Flexibel aanpasbaar aan projectbehoeften.

#### Nadelen
- **Leercurve**: Vereist begrip van semantische versiebeheer.
- **Afhankelijkheid**: Vereist extra tooling en configuratie.

### 2. **Git Tags en Handmatige Incrementen**
Een eenvoudige methode waarbij ontwikkelaars handmatig tags toevoegen aan commits.

#### Voordelen
- **Eenvoud**: Geen extra tooling nodig.
- **Controle**: Ontwikkelaars kunnen specifieke versies kiezen.

#### Nadelen
- **Foutgevoelig**: Handmatig beheer kan leiden tot inconsistenties.
- **Inefficiënt**: Tijdrovend voor grotere teams.

### 3. **Custom CI/CD Pipelines**
Ontwikkelaars kunnen CI/CD-tools zoals Jenkins, GitHub Actions of GitLab CI gebruiken om een op maat gemaakte versiebeheeroplossing te implementeren (GitLab CI/CD, n.d.).

#### Voordelen
- **Flexibiliteit**: Volledig aanpasbaar.
- **Integratie**: Kan naadloos worden geïntegreerd met andere processen.

#### Nadelen
- **Complexiteit**: Kost veel tijd om op te zetten.
- **Onderhoud**: Vereist voortdurende aanpassingen.

---

## Vergelijking van Methodes

| Methode                 | Voordelen                          | Nadelen                          |
|-------------------------|-------------------------------------|-----------------------------------|
| Moonlit Auto-Versioning | Consistent, efficiënt              | Complex script, workflowgebonden |
| Semantic Release        | Automatische changelogs, flexibel  | Leercurve, afhankelijkheden      |
| Git Tags                | Eenvoudig, volledig controleerbaar | Foutgevoelig, inefficiënt        |
| Custom CI/CD Pipelines  | Flexibel, diep geïntegreerd        | Complex, onderhoudsintensief     |

---

## Conclusie

Automatische versioning biedt aanzienlijke voordelen in termen van efficiëntie en consistentie, vooral in grotere projecten met meerdere teamleden (Fowler, 2019). De keuze voor een methode hangt af van de specifieke behoeften van het team:

- **Voor gestandaardiseerde workflows** is Semantic Release ideaal vanwege de changelog-functionaliteit.
- **Voor eenvoudige projecten** kan Git Tags voldoende zijn, mits er zorgvuldige controle is.
- **Voor maximale flexibiliteit** zijn custom CI/CD-pipelines een sterke keuze, hoewel ze meer inspanning vereisen.

De Moonlit Auto-Versioning-methode is een uitstekende balans tussen eenvoud en functionaliteit, mits de workflow van het team overeenkomt met de configuratie van het script. Voor toekomstig onderzoek wordt aanbevolen om hybride benaderingen te overwegen die de voordelen van verschillende methodes combineren, zoals het integreren van Semantic Release met custom pipelines.

---

## Referenties

- Fowler, M. (2019). *Continuous Delivery: Reliable Software Releases through Build, Test, and Deployment Automation*. Addison-Wesley Professional.
- Semantic Versioning. (n.d.). *Semantic Versioning 2.0.0*. Retrieved from https://semver.org/
- Jenkins. (n.d.). *Jenkins Documentation*. Retrieved from https://www.jenkins.io/doc/
- GitHub Actions. (n.d.). *GitHub Actions Documentation*. Retrieved from https://docs.github.com/en/actions
- GitLab CI/CD. (n.d.). *GitLab Continuous Integration and Delivery*. Retrieved from https://docs.gitlab.com/ee/ci/

---

## Auteur
| Versie | Auteur       | Datum      | Opmerking             |
|--------|--------------|------------|-----------------------|
| 1.0.0  | Osama Halabi | 2024-11-20 | Automatische Versioning onderzoek |