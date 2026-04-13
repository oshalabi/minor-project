# Eigen bijdrage Joris Wittenberg

[//]: # (Als deliverable voor de individuele bijdrage in het beroepsproduct maak een eigen markdown bestand `<mijn-voornaam>.md` in je repo aan met tekst inclusief linkjes naar code en documentaties bestanden, pull requests, commit diffs. Maak hierin de volgende kopjes met een invulling.)

[//]: # ()
[//]: # (Je schrapt verder deze tekst en vervangt alle andere template zaken, zodat alleen de kopjes over blijven. **NB: Aanwezigheid van template teksten na inleveren ziet de beoordelaar als een teken dat je documentatie nog niet af is, en hij/zij deze dus niet kan of hoeft te beoordelen**.)

[//]: # ()
[//]: # (Je begin hier onder het hoofdkopje met een samenvatting van je bijdrage zoals je die hieronder uitwerkt. Best aan het einde schrijven. Zorg voor een soft landing van de beoordelaar, maar dat deze ook direct een beeld krijgt. Je hoeft geen heel verslag te schrijven. De kopjes kunnen dan wat korter met wat bullet lijst met links voor 2 tot 4 zaken en 1 of 2 inleidende zinnen erboven. Een iets uitgebreidere eind conclusie schrijf je onder het laatste kopje.)


## 1. Code/platform bijdrage

Competenties: *DevOps-1 Continuous Delivery*

In de rol van developer heb ik het volgende bijgedragen:
* [ImportRation](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/7398?_a=files) microservice: Ik heb ervoor gezorgd dat het mogelijk is om via de ImportRation microservice exceldatasheet uit te lezen.
  Met bijbehorende [unit-tests](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/4f1ceb85fe450ebc1f1257ad2b5a9f1af08830b5?refName=refs%2Fheads%2Findividuele-bijdragen-joris)


* [Layout](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/4cb0d3b238f308402888713c5d2b3eaab380f5d8?refName=refs%2Fheads%2FcollapseAbleComp): grotendeels van de styling en pagina opbouw heb ik op me genomen. Dit komt eigenlijk terug in bijna elke commit.

[//]: # (Beschrijf hier kort je bijdrage vanuit je rol, developer &#40;Dev&#41; of infrastructure specialist &#40;Ops&#41;. Als Developer beschrijf en geef je links van minimaal 2 en maximaal 4 grootste bijdrages qua code functionaliteiten of non-functionele requirements. Idealiter werk je TDD &#40;dus ook commit van tests en bijbehorende code tegelijk&#41;, maar je kunt ook linken naar geschreven automatische tests &#40;unit tests, acceptance tests &#40;BDD&#41;, integratie tests, end to end tests, performance/load tests, etc.&#41;. Als Opser geef je je minimaal 2 maximaal 4 belangrijkste bijdragen aan het opzetten van het Kubernetes platform, achterliggende netwerk infrastructuur of configuration management &#40;MT&#41; buiten Kubernetes &#40;en punt 2&#41;.)

## 2. Bijdrage app configuratie/containers/kubernetes

Competenties: *DevOps-2 Orchestration, Containerization*
* [ImportRation opzet](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/790626d67dcd4a4ff2a22988c43f3b8e470e6206?refName=refs/heads/%235656-Importeren-rantsoen&path=/src/docker-compose.yml&_a=contents): In deze commit is te zien dat ik de basis opzet voor de ImportRation Containers.

[//]: # (Beschrijf en geef hier links naar je minimaal 2 en maximaal 4 grootste bijdragen qua configuratie, of bijdrage qua 12factor app of container Dockerfiles en/of .yml bestanden of vergelijkbare config &#40;rondom containerization en orchestration&#41;.)

## 3. Bijdrage versiebeheer, CI/CD pipeline en/of monitoring

Competenties: *DevOps-1 - Continuous Delivery*, *DevOps-3 GitOps*, *DevOps-5 - SlackOps*

* [Opzetten pipeline](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/326befaa43445271d7f1b01026dbdc009a488358?refName=refs%2Fheads%2Fdevelopment):
In de eerste week van het project hebben we samen met de groep in Veenendaal de pipeline grotendeels opgezet.

* Voor de laatste techreview hebben we samen een release pipeline gemaakt voor het opzetten van een nieuw environment.

[//]: # (Beschrijf hier en geef links naar je bijdragen aan het opzetten en verder automatiseren van delivery pipeline, GitOps toepassing en/of het opzetten van monitoring, toevoegen van metrics en custom metrics en rapportages.)

[//]: # ()
[//]: # (NB Het gebruik van *versiebeheer* &#40;&#40;e.g. git&#41;&#41; hoort bij je standaardtaken en deze hoef je onder dit punt NIET te beschrijven, het gaat hier vooral om documenteren van processtandaarden, zoals toepassen van een pull model.)

# Onderzoek: Hoe review je code? Mijn ervaring met code reviews

Competenties: *Nieuwsgierige houding*

<div style="border: 1px solid #ccc; padding: 10px; border-radius: 5px;">

## Inleiding
Toen ik begon met code reviews, vond ik het een uitdaging om te bepalen of iets echt goed of fout was. Samen met mijn groepsgenoten werken we aan projecten waarin het reviewen van elkaars code een belangrijk onderdeel is. Ik merkte dat ik vaak twijfelde: *"Is dit de juiste manier?"* of *"Moet ik hier iets van zeggen?"* Dit onderzoek is voortgekomen uit die vraag: hoe beoordeel je code op een gestructureerde manier?

## Wat is een code review?
Een code review is een proces waarin de wijzigingen van een groepsgenoot of teamlid worden beoordeeld voordat deze worden samengevoegd met de hoofdcodetak. Het doel is om:
- **Fouten op te sporen** voordat ze live gaan.
- **De codekwaliteit te waarborgen** door leesbaarheid en onderhoudbaarheid te verbeteren.
- **Kennisdeling te bevorderen**, zodat iedereen leert van elkaars werk.

## Mijn uitdaging: Is iets goed of fout?
Ik vond het moeilijk om te beoordelen of code "goed" genoeg was. Soms lijkt code te werken, maar dat betekent niet dat het de beste oplossing is. Of ik wist niet zeker of een bepaald patroon correct was, omdat ik zelf nog lerende ben. Hierdoor voelde ik me soms onzeker tijdens het reviewproces.

## Hoe review je code? Een gestructureerde aanpak
Om deze onzekerheid te overwinnen, ben ik op zoek gegaan naar manieren om gestructureerd en objectief code te reviewen. Hier zijn de stappen die mij hebben geholpen:

### 1. Begin met de context
- Lees de beschrijving van de wijziging: wat is het probleem dat wordt opgelost?
- Vraag jezelf af: *"Snap ik waarom deze oplossing is gekozen?"* Als de context onduidelijk is, vraag om meer uitleg.

### 2. Focus eerst op functionaliteit
- Werkt de code zoals bedoeld? Dit kun je testen door de code lokaal te draaien of de bijgevoegde tests te bekijken.
- Controleer of de oplossing logisch en volledig is: zijn er edge cases die niet worden behandeld?

### 3. Beoordeel de leesbaarheid
- Is de code makkelijk te begrijpen voor anderen in de groep?
- Zijn variabelen en functies duidelijk genoemd? Korte en duidelijke namen helpen om de bedoeling van de code te begrijpen.

### 4. Gebruik checklists
Checklists hebben me geholpen om mijn beoordelingen gestructureerd te houden, zoals:
- Volgt de code de afgesproken stijlrichtlijnen?
- Zijn er tests toegevoegd of aangepast?
- Zijn er mogelijke problemen met beveiliging of performance?

### 5. Vraag om verduidelijking in plaats van direct te oordelen
Als ik twijfel over een oplossing, probeer ik een vraag te stellen in plaats van direct feedback te geven. Bijvoorbeeld: *"Waarom heb je gekozen voor deze aanpak?"* Dit maakt de review minder over goed of fout en meer over samenwerken en leren.

### 6. Vertrouw op tools
Automatische tools zoals linters, code-analyzers en testpakketten helpen om technische fouten te vinden. Zo kan ik me richten op de logica en leesbaarheid van de code.

## Wat heb ik geleerd?
Door een gestructureerde aanpak te volgen, ben ik beter geworden in het beoordelen van code. Ik heb geleerd dat een code review niet altijd draait om het vinden van fouten, maar ook om samen te zoeken naar verbeteringen. Daarnaast besefte ik dat het stellen van vragen vaak meer waarde toevoegt dan het direct oordelen over goed of fout.

## Conclusie
Code review is een belangrijk proces, zelfs binnen kleine groepen. Hoewel ik het in het begin lastig vond om te beoordelen of code goed of fout was, heeft een gestructureerde aanpak mij meer zelfvertrouwen gegeven. Door te focussen op functionaliteit, leesbaarheid en samenwerking kan ik nu beter bijdragen aan de kwaliteit van onze projecten.
</div>

## 5. Bijdrage code review/kwaliteit anderen en security

Competenties: *DevOps-7 - Attitude*, *DevOps-4 DevSecOps*

- [PR Calculatie krachtvoer](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/pullrequest/8921?_a=files&path=/src/WebApp/src/)
## 6. Bijdrage documentatie

Competenties: *DevOps-6 Onderzoek*
- [Wireframe](../docs/wireframes/wf_importeer_lijst.png): Voordat ik begon aan de [userstory:5631 (Importeren voedingssoorten)](https://dev.azure.com/ISMinor/HANMinorGroep5/_workitems/edit/5643/) heb ik een wireframe gemaakt.
- [ADR RabbitMQ](https://dev.azure.com/ISMinor/HANMinorGroep5/_git/HANMinorGroep5/commit/1fb7efd371e918a65d2cb85b64ae1d2bc0c2ac90?refName=refs%2Fheads%2Findividuele-bijdragen-joris&path=%2Fdocs%2Fadr%2Fadr-rabbitmq.md&_a=contents):
De keuze waarom we voor RabbitMQ zijn gegaan als message broker leg ik uit in de [ADR RabbitMQ](../docs/adr/adr-rabbitmq.md).

- Als groep hebben we allemaal ons steentje bijgedragen bij het uitwerken van de userstories.

[//]: # (Zet hier een links naar en geef beschrijving van je C4 diagram of diagrammen, README of andere markdown bestanden, ADR's of andere documentatie. Bij andere markdown bestanden of doumentatie kun je denken aan eigen proces documentatie, zoals code standaarden, commit- of branchingconventies. Tot slot ook user stories en acceptatiecriteria &#40;hopelijk verwerkt in gitlab issues en vertaalt naar `.feature` files&#41; en evt. noemen en verwijzen naar handmatige test scripts/documenten.)

## 7. Bijdrage Agile werken, groepsproces, communicatie opdrachtgever en soft skills

Competenties: *DevOps-1 - Continuous Delivery*, *Agile*

- Tijdens sprintreview 3 heb ik het initiatief genomen en de prestatie grotendeels geleidt.
- Ik heb een retrospective geleidt aan de hand van een [Miroboard](https://miro.com/app/board/uXjVL4VzFdQ=/)
- Ik heb zo goed als elke scrum ceremonie bijgewoond en toegevoegde waarde geleverd.

## 8. Leerervaringen

Competenties: *DevOps-7 - Attitude*

### Tops:
- Verantwoordelijkheid, Dit is een top die ik eigenlijk aan het hele team kan geven. Iedereen heeft zich vol voor de groep gegeven en alles eraan gedaan om tot een mooi eindproduct te komen.
- Behulpzaamheid, We stonden allemaal direct voor elkaar klaar wanneer iemand een vraag had of iets onduidelijk was.
### Tips:
- Thuiswerken moet efficiënter. Ik heb afgelopen project gemerkt dat ik thuiswerken heel erg prettig vind, echter heeft dit soms tot momenten geleid waar ik toch makkelijk afgeleid was.
- Eerder bespreekbaar maken wanneer ik ergens moeite mee heb. Tijdens heb project heb ik heel lang gewacht met aankaarten dat ik het moeilijk vind om code te reviewen.

### Wat ik meeneem naar afstudeerstage
 * Sterktes om voort te zetten:
   - Verantwoordelijkheid: Het gehele team heeft zich volledig ingezet voor het gezamenlijke doel. Deze mentaliteit van betrokkenheid en toewijding wil ik behouden en stimuleren.
   - Behulpzaamheid: De openheid om elkaar direct te helpen en vragen te beantwoorden heeft bijgedragen aan een prettige en productieve samenwerking.

* Lessen voor verbetering:
  - Efficiënter thuiswerken: Ik wil bewuster omgaan met mijn thuiswerksituatie om afleiding te minimaliseren en productiever te zijn.
  - Tijdig zaken bespreekbaar maken: Ik neem mezelf voor om eerder aan te geven wanneer ik ergens moeite mee heb, zoals bijvoorbeeld het reviewen van code, zodat ik sneller ondersteuning kan krijgen en kan groeien.

## 9. Conclusie & feedback

Competenties: *DevOps-7 - Attitude*

De grootste uitdaging voor mij tijdens dit project was toch echt wel het domein. Normaal gesproken kan ik me goed verplaatsen in de ogen van de opdrachtgever, bij dit project was dit niet het geval. Dit heeft in het begin voor enige opstoppingen gezorgd en zelfs dat we op de helft nog structuurwijzigingen moesten doorvoeren.
Ik vind dat we op gebied van dev-ops principes steekjes hier en daar hebben laten liggen. Dit komt doordat we te snel aan de slag gingen en meteen wilde beginnen met code kloppen.
