# Rantsoenwijzer HANMinorGroep5

## Way of working

### Afspraken
- Werkdagen van 09:00 tot 17:00.
- DSU om 09:30.

### Branches
- De development branch is voor het toevoegen van de nieuwe features 
- De main branch is voor de stabiele versie
- De release branche is voor staging

### Sprintbord
- Wanneer pull request afgekeurd wordt, wordt de taak naar "needs work" geplaatst. 
- Reviewer verplaatst taak naar "Closed" Of "Needs work".

### Code review
- Code wordt binnen 2 uur gereviewd.
- Feedback wordt gemeld in de "discussion".

### Pull requests
- Voeg de suffix "add" toe aan de pull request titel bij het toevoegen van een feature.
- Voeg de suffix "docs" toe aan de pull request titel als de pull request gaat over documentatie.
- Voeg de suffix "fix" toe aan de pull request titel als er een bug of iets dergelijke is opgelost.

## Definition of Done

### Codekwaliteit
- Officiële styling guide van .NET is toegepast op de code.
- Code is gereviewd door ten minste één teamlid.

### Tests
- Unit tests zijn geschreven en hebben een dekking van minimaal 80%.
- Geen openstaande kritieke bugs of fouten die de functionaliteit beïnvloeden.
- Integratietests worden automatisch uitgevoerd door pipeline.
- Het resultaat van End-to-end tests worden vastgelegd in het testrapport.

### Functionaliteit
- Alle acceptatiecriteria van de user story zijn behaald.
- Functionaliteit is gedemonstreerd aan stakeholders of product owners en goedgekeurd.

### Gebruikerservaring
- Front-end is ontwikkeld volgens de wireframes.
- Functionaliteit is getest in de benodigde browsers en/of apparaten.
- Gebruikersfeedback (indien beschikbaar) is verwerkt.

### Documentatie
- Release notes zijn opgesteld voorafgaand aan de release.
- De benodigde technische documentatie is bijgewerkt.

### Deployable
- Alle code is gemerged naar de hoofdbranche zonder conflicts.
- Build- en deploy-scripts zijn getest en werken correct.
- Het item is deployable en bevat geen blokkades voor release.
- Afbeeldingen zijn geoptimaliseerd.

