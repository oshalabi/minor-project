# Testplan voor een Microservices Applicatie

**Backend Microservices:** C#  
**Frontend:** Angular

---

## 1. Inleiding
Dit testplan beschrijft de strategieën, tools en procedures die worden toegepast voor het testen van een microservices-applicatie. Het doel is ervoor te zorgen dat:
- Alle functionaliteiten correct werken.
- De integratie tussen de frontend en backend naadloos verloopt.
- De applicatie voldoet aan de functionele en niet-functionele eisen.

### Doelen:
- **Backend**: Uitgebreide geautomatiseerde tests voor stabiliteit en betrouwbaarheid.
- **Frontend**: Handmatige tests gevolgd door (indien tijd beschikbaar) geautomatiseerde tests.

---

## 2. Teststrategie

### Testmethodologie
- **Unit Tests**: Testen van individuele componenten in isolatie.
- **Integratietests**: Verificatie van interacties tussen verschillende modules.
- **End-to-End Tests**: Volledige tests van gebruikersflows en systeemgedrag.

---

## 3. Te Testen Componenten

### 3.1 Wat wordt getest?

#### Backend
- Public methods van services en controllers (exclusief getters/setters).
- Samenwerking tussen microservices via integratietesten.

#### Frontend
- Handmatige tests gericht op:
    - Gebruiksvriendelijkheid.
    - Functionele validatie (zoals formulierverwerking en foutmeldingen).
- Optioneel: Geautomatiseerde tests van Angular-componenten en gebruikersflows.

### 3.2 Wat wordt niet getest?
- **Backend**:
    - Private methods.
    - Getters en setters.
    - Data Transfer Objects (DTO's) en entities.
- **Frontend**:
    - Design-validaties (zoals kleurconsistentie).
    - Browserspecifieke functionaliteiten (wordt als stabiel aangenomen).

---

## 4. Testtools

### Backend
- **xUnit**: Voor unit- en integratietests.
- **Moq**: Voor het mocken van services tijdens tests.

### Frontend
- **Handmatige tests**: Initiële focus op gebruiksvriendelijkheid en functionaliteit.
- **Geautomatiseerde tests (optioneel)**:
    - Cypress: End-to-end tests.
    - Karma & Jasmine: Voor Angular-componenten en unittests.

---

## 5. Testscenario's

### 5.1 Backend

#### Unit Test Scenario
**Doel:** Valideren van individuele functionaliteiten.  
**Testtemplate:**
```csharp
public class ServiceUnitTest
{
    private Mock<IRepository> _repositoryMock;
    private MyService _sut;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepository>();
        _sut = new MyService(_repositoryMock.Object);
    }

    [Test]
    public void GetEntity_ReturnsExpectedEntity()
    {
        // Arrange
        var expectedEntity = new Entity { Id = 1, Name = "Test" };
        _repositoryMock.Setup(repo => repo.GetEntity(1)).Returns(expectedEntity);

        // Act
        var result = _sut.GetEntity(1);

        // Assert
        Assert.AreEqual(expectedEntity, result);
        _repositoryMock.Verify(repo => repo.GetEntity(1), Times.Once);
    }
}
```
## Integratietesten

### Doel
Testen van de samenwerking tussen verschillende backend-componenten om te valideren dat gegevensstromen en logica correct functioneren.

### Algemene Testtemplate
**Naam:** [Naam van de test]  
**Beschrijving:** [Korte beschrijving van het doel van de test]

**Voorwaarden:**
1. [Voorwaarde 1]
2. [Voorwaarde 2]

**Testscript:**
```csharp
[Test]
public void [TestNaam]()
{
    // Arrange
    // [Voorbereidende stappen voor de test]

    // Act
    // [Uitvoeren van de functie of methode die getest wordt]

    // Assert
    // [Validatie van de verwachte resultaten]
}
```

Verwachte Resultaten:
1. [Verwachting 1]
2. [Verwachting 2]

## Frontend Teststrategie

### Handmatige Tests (Eerste Prioriteit)

#### Gebruiksvriendelijke Tests
De handmatige tests richten zich op het verifiëren van de gebruiksvriendelijkheid en functionaliteit van de frontend.

**Doelstellingen:**
- Valideer dat formulieren correct functioneren.
- Controleer dat foutmeldingen en validaties worden weergegeven zoals verwacht.
- Bevestig dat de frontend correcte gegevens naar de backend stuurt en de responses correct verwerkt.

**Stappenplan voor Handmatige Tests:**
1. Navigeer naar de pagina die getest moet worden.
2. Vul een formulier in:
    - Gebruik zowel correcte als incorrecte gegevens.
3. Controleer:
    - Of foutmeldingen verschijnen bij ongeldige invoer.
    - Of validatieregels correct worden toegepast.
4. Verifieer dat de verzonden gegevens correct worden verwerkt door de backend.

---

### Geautomatiseerde Tests (Indien Tijd Beschikbaar)

#### Unittests voor Angular-componenten
Unittests worden geschreven om de werking van individuele Angular-componenten te valideren. Hierbij worden services gemockt om externe afhankelijkheden te isoleren.

**Voorbeeldtest:** Initialisatie van een Angular-component en ophalen van data.

```javascript
import { TestBed } from '@angular/core/testing';
import { MyComponent } from './my.component';
import { MyService } from './my.service';
import { of } from 'rxjs';

describe('MyComponent', () => {
  let component: MyComponent;
  let service: jasmine.SpyObj<MyService>;

  beforeEach(() => {
    // Mock de service
    const serviceSpy = jasmine.createSpyObj('MyService', ['getData']);

    // Configureer het testbed
    TestBed.configureTestingModule({
      declarations: [MyComponent],
      providers: [{ provide: MyService, useValue: serviceSpy }],
    });

    // Injecteer de gemockte service
    service = TestBed.inject(MyService) as jasmine.SpyObj<MyService>;
    component = TestBed.createComponent(MyComponent).componentInstance;
  });

  it('should fetch data on init', () => {
    // Arrange
    const mockData = ['item1', 'item2'];
    service.getData.and.returnValue(of(mockData));

    // Act
    component.ngOnInit();

    // Assert
    expect(component.items).toEqual(mockData);
  });
});
```
## Testscope

### Backend
- Alle functies en logica.
- Samenwerking tussen microservices.

### Frontend
- Handmatige tests van formulieren, validaties en gegevensverwerking.
- Optioneel: Geautomatiseerde tests voor Angular-componenten.

## Conclusie
Dit testplan zorgt voor een gestructureerde aanpak, waarbij in eerste instantie handmatige tests voor de frontend worden uitgevoerd en geautomatiseerde tests voor de backend. Indien er tijd over is, worden geautomatiseerde tests voor de frontend toegevoegd. Hiermee wordt de functionaliteit en betrouwbaarheid van de applicatie gewaarborgd.