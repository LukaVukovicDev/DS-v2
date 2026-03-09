# CoworkingApp.GUI

WPF GUI projekat za tvoj backend `CoworkingApp.BusinessLogic`.

## Šta je urađeno
- učitavanje `config.txt` preko `ConfigReader`
- korišćenje `CoworkingFacade` kao jedine ulazne tačke prema backend-u
- pregled i CRUD za:
  - korisnike
  - lokacije
  - tipove članstva
  - resurse
  - rezervacije
- filtriranje korisnika po:
  - tekstu pretrage
  - lokaciji
  - tipu članstva
  - statusu
- pregled rezervacija:
  - za izabranog korisnika
  - za izabrani dan i lokaciju
- observer osvežavanje nakon kreiranja/izmene/otkazivanja rezervacije
- rezervacije se kreiraju preko `ReservationBuilder`
- `try/catch` za prikaz validacionih grešaka iz backend-a

## Kako da povežeš projekat
1. Dodaj ovaj GUI projekat u solution.
2. Desni klik na GUI projekat -> Add Reference / Project Reference.
3. Označi `CoworkingApp.BusinessLogic`.
4. Postavi `config.txt` da bude pored `.exe` fajla.
5. Pokreni GUI projekat kao startup projekat.

## Napomene
Backend koji si poslao **nema implementirane** sledeće stavke, pa ni GUI ne može pošteno da ih završi bez dodatnog backenda/baze:
- autentikaciju administratora sa hashovanim lozinkama
- automatski CSV eksport izveštaja

Za te dve stavke morao bi da dodaš nove tabele, modele, repozitorijume i facade metode.
